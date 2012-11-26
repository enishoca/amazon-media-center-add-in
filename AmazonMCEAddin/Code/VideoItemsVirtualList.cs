using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.MediaCenter;
using Microsoft.MediaCenter.Hosting;
using Microsoft.MediaCenter.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;


namespace AmazonMCEAddin
{
    public class VideoItemsVirtualList : VirtualList
    {
        private string m_Query;
        
        private int batchSize = 30;
        private Dictionary<int, object> _pendingItemRequest = new Dictionary<int, object>();
        private Dictionary<int, object> _retrievedItem = new Dictionary<int, object>();
        private const string TempPictureFileExtension = "amzv";

        public VideoItemsVirtualList()
        {
            //this is arbitrary, but the list prevents you from going beyond the end anyway.
            Count = 100000;
            VisualReleaseBehavior = ReleaseBehavior.Dispose;
            EnableSlowDataRequests = true;
        }
        //The query that is passed is actually a http get query string
        //Changing the query actually forces the query to run - this is usually bound in mcml to the current context's query
        //so that changing the current context changes the query and re-runs the get.
        public string Query
        {
            get { return m_Query; }
            set
            {
                m_Query = value;
                FirePropertyChanged("Query");
            }
        }

        
        /// <summary>
        /// Create a new video item and pass it the relevant info to get the data
        /// </summary>
        /// <param name="index">the index of the item being requested</param>
        /// <param name="callback"></param>
        protected override void OnRequestItem(int index, ItemRequestCallback callback)
        {
            //if there is no query, then this is pointless anyway
            if (Query == "")
            { return; }
            
            //since we are batching up requests, see if this batch has already been requested.
            //If it hasn't been requested, we need to request the data.

            //if we can't find this item in the retrieved items list, 
            if(!_retrievedItem.ContainsKey(index))
            {
                //figure out the correct batch request and make it
                int startIndex = (int)Math.Floor((decimal)(index / batchSize)) * batchSize;
                GetVideoData(Query, startIndex, batchSize);
            }

            //in theory, we cannot be here without having got data first
            //so if the 
            if (_retrievedItem.ContainsKey(index))
            {
                //create a new video item
                VideoItem v = new VideoItem(this, index);

                //get the data from the retrieval and parse
                v.processNodeData((JObject)_retrievedItem[index]);

                //now free up the memory by removing the retrieved info
                _retrievedItem.Remove(index);

                //now let the list know
                callback(this, index, v);
            }
        }

 

        /// <summary>
        /// This function sets up the background request for the image
        /// </summary>
        /// <param name="index"></param>
        protected override void OnRequestSlowData(int index)
        {
            //if this request has already been made once
            if (_pendingItemRequest.ContainsKey(index))
            {
                //cancel the request
                return; 

            }
            //If this virtual list does not have a proper query
            if (Query == "")
            {
                //don't try to get an image
                return;
            }
            //add this index to the pendingItemRequest list so that we know this has already been requested
            _pendingItemRequest[index] = index;

            //create a new slowdata Request item.
            SlowDataItem slowData = new SlowDataItem();

            //set the slowdata index to match index requested
            slowData.Index = index;

            //Read the path of the image url from the relevant video item.
            slowData.PicturePath = ((VideoItem)this[slowData.Index]).Format.CoverArtLargeUri;

            //schedule this process to run on a worker thread
            Microsoft.MediaCenter.UI.Application.DeferredInvokeOnWorkerThread(GetVideoImage, ProcessSlowData, slowData);
        }


        /// <summary>
        /// This function makes a web request for the data, and stores it in the requestedItem dictionary, 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="startIndex"></param>
        /// <param name="batchSize"></param>
        private void GetVideoData(string query, int startIndex, int batchSize)
        {
            //need to batch up the data requests and get e.g. 10 at a time
            

            //First go get the actual data
            AmazonRequester req = new AmazonRequester();
            //string query = Query + "&StartIndex=" + index.ToString();
            string currentQuery = Query.Replace("HideNum=T", "HideNum=F");
            currentQuery += "&NumberOfResults=" + batchSize + "&StartIndex=" + startIndex;
            string data = req.ExecuteQuery(currentQuery);

            //now create a json parser and read in the data
            JsonTextReader reader = new JsonTextReader(new StringReader(data));
            JObject results = JObject.Parse(data);

            //set a counter
            int currentIndex = startIndex;

            //loop through the titles, and pass the content to relevant video item
            foreach (JObject node in results["message"]["body"]["titles"])
            {
                //store the relevant data as a node in retrieved items - they will then be pulled
                //by the next OnRequestItem and removed from _retrievedItem
                _retrievedItem[currentIndex] = node;
                currentIndex++;
            }
        }
        /// <summary>
        /// This function goes and gets the image
        /// </summary>
        /// <param name="args"></param>
        private static void GetVideoImage(object args)
        {
            // Heavy operation: get video information from amazon.
            ThreadPriority priority = Thread.CurrentThread.Priority;
            try
            {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;

                SlowDataItem slowData = (SlowDataItem)args;
                if (slowData.PicturePath == "")
                { return; }
                try
                {
                    
                    //actually go and get the image.
                    slowData.TitleImage = new Image(slowData.PicturePath);
                }
                catch (Exception e)
                {
                    //not sure what we can do in this case
                }
            }
            finally
            {
                Thread.CurrentThread.Priority = priority;
            }
        }

        /// <summary>
        /// This function updates the relevant video item with the retrieved image.
        /// </summary>
        /// <param name="args"></param>
        private void ProcessSlowData(object args)
        {
            SlowDataItem slowData = (SlowDataItem)args;


            _pendingItemRequest.Remove(slowData.Index);

            if (IsDisposed || !IsItemAvailable(slowData.Index))
            {
                return;
            }
            //go and get the relevant video item
            VideoItem v = (VideoItem)this[slowData.Index];
            //set the image to be the retrieved image
            v.Image = slowData.TitleImage;
        }
        

        /// <summary>
        /// a struct to hold info about image requests being made.
        /// </summary>
        private class SlowDataItem
        {
            public int Index;
            public Image TitleImage;
            public string PicturePath = "";
        }


    }
}
