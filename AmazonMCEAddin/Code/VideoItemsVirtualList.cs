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
        private Dictionary<int, object> _pendingItemRequest = new Dictionary<int, object>();
        private const string TempPictureFileExtension = "amzv";

        public VideoItemsVirtualList()
        {
            Count = 100000;
            VisualReleaseBehavior = ReleaseBehavior.Dispose;
            EnableSlowDataRequests = true;
        }
        /// <summary>
        /// Create a new video item and pass it the relevant info to get the data
        /// </summary>
        /// <param name="index">the index of the item being requested</param>
        /// <param name="callback"></param>
        protected override void OnRequestItem(int index, ItemRequestCallback callback)
        {
            Trace.WriteLine("Making new request for index: " + index.ToString());
            //Set the query to the right index to start
            string query = Query + "&StartIndex=" + index.ToString();
            //When an item is requested, create a placehlder for it.
            VideoItem v = new VideoItem(this, index, query);
            //now let the list know
            callback(this, index, v);
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
        /// 
        /// </summary>
        /// <param name="index"></param>
        protected override void OnRequestSlowData(int index)
        {
            Trace.WriteLine("Request for index: " + index.ToString());
            if (_pendingItemRequest.ContainsKey(index))
            {
                Trace.WriteLine("duplicate request for index: " + index.ToString());
                return; 

            }
            if (Query == "")
            {
                Trace.WriteLine("no query specified");
                return;
            }
            _pendingItemRequest[index] = index;
            SlowDataItem slowData = new SlowDataItem();
            slowData.Index = index;
            slowData.Query = ((VideoItem)this[slowData.Index]).Query;
            Microsoft.MediaCenter.UI.Application.DeferredInvokeOnWorkerThread(GetVideoInformation, ProcessSlowData, slowData);
        }


        private static void GetVideoInformation(object args)
        {
            //
            // Heavy operation: Build our randomly-generated snow flake image.
            //

            ThreadPriority priority = Thread.CurrentThread.Priority;
            try
            {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;

                SlowDataItem slowData = (SlowDataItem)args;

                if (slowData.Query == "")
                {
                    Trace.WriteLine("We should never get here");
                    return;
                }
                Trace.WriteLine("Getting video information for index: " + slowData.Index.ToString());

                //string data = AmazonVideoRequest.ExecuteQuery(slowData.Query);
                AmazonRequester req = new AmazonRequester();
                string data = req.ExecuteQuery(slowData.Query);
                //req.dispose();

                JsonTextReader reader = new JsonTextReader(new StringReader(data));

                JObject results = JObject.Parse(data);
                //need to check that we got a valid result
                try
                {

                    slowData.Node = (JObject)results["message"]["body"]["titles"][0];
                    slowData.PicturePath = (string)slowData.Node["formats"][0]["images"][1]["uri"];
                    slowData.TitleImage = new Image(slowData.PicturePath);
                }
                catch (Exception e)
                {
                    //not sure what we can do in this case
                }
                //get image

            }
            finally
            {
                Thread.CurrentThread.Priority = priority;
            }
        }

        private void ProcessSlowData(object args)
        {
            SlowDataItem slowData = (SlowDataItem)args;

            //
            // Remove tracking for pending picture acquires.
            //

            _pendingItemRequest.Remove(slowData.Index);


            //
            // If the VirtualList has been disposed before this callback is received,
            // or if the data item specified by the index has been thrown away, then
            // clean up the picture file.
            //
            // Note that we need to check IsItemAvailable() instead of just calling
            // the indexer and checking for null.  This is because doing that query 
            // can cause the item to be faulted in after it'd already been thrown 
            // away due to going offscreen.
            //

            if (IsDisposed || !IsItemAvailable(slowData.Index))
            {
                //if (result.PicturePath != null)
                //{
                //    File.Delete(result.PicturePath);
                //}

                return;
            }

            if (slowData.Query == "")
            {
                Trace.WriteLine("We should also never get here");
                return;
            }
            VideoItem v = (VideoItem)this[slowData.Index];
            Trace.WriteLine("About to write data for video item - current title: " + v.Title);
            v.processNodeData(slowData.Node);
            Trace.WriteLine("Processing returned data for index: " + slowData.Index.ToString() + ", Title: " + v.Title);
            v.Image = slowData.TitleImage;
        }
        

        /// <summary>
        /// a struct to hold info about image requests being made.
        /// </summary>
        private class SlowDataItem
        {
            public int Index;
            public string Query = "";
            public JObject Node;
            public Image TitleImage;
            public string PicturePath = "";
        }


    }
}
