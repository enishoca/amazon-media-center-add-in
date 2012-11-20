using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.MediaCenter;
using Microsoft.MediaCenter.Hosting;
using Microsoft.MediaCenter.UI;
using System.IO;    
using System.Xml;
using System.Xml.XPath;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace AmazonMCEAddin
{
    public class VideoItem : ModelItem
    {
        private Command m_Command;
        private string m_Title;
        private string m_Synopsis;
        private Image m_Image;
        private string m_Price;
        private string m_ASIN;
        private string m_RegulatoryRating;
        private string m_runtime;
        private string m_contentType;
        private string m_ChildTitleQuery;
        private VideoItems m_ChildTitleItems;

        private string m_ItemQuery;
        private int m_ItemIndex;
        
        public Size size { set; get; }
        private static string NAMESPACE_PREFIX = "x";
        private static string CONTENT_SEASON = "SEASON";
        private static string CONTENT_MOVIE = "MOVIE";

        /// <summary>
        /// New constructor for virtual list.
        /// </summary>
        /// <param name="vlist"></param>
        /// <param name="Index"></param>
        public VideoItem(IModelItemOwner owner, int Index, string query)
            :
            base(owner)
        {
            m_ItemIndex = Index;
            m_ItemQuery = query;
            //GetItemByIndex(query, Index);
        }

        
        public VideoItem()
        {
        }
        //This is junk - needed to move out of the view item into a separate process.
        public void GetItemByIndex()
        {
            if (m_ItemQuery == "")
            {
                return;
            }
            m_ItemQuery += "&NumberOfResults=1&StartIndex=" + m_ItemIndex.ToString();
            string data = AmazonVideoRequest.ExecuteQuery(m_ItemQuery);

            JsonTextReader reader = new JsonTextReader(new StringReader(data));

            JObject results = JObject.Parse(data);
            //need to check that we got a valid result
            JObject node = (JObject)results["message"]["body"]["titles"][0];
            processNodeData(node);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (m_Image != null)
            {
                m_Image.Dispose();
            }
        }

        //Initializes a new video item with a json node.
        public VideoItem(JObject node)
        {
            processNodeData(node);
        }
        public void processNodeData(JObject node)
        {
            m_Command = new Command();
            //try to use HD unless no HD available.
            int selectedFormat = 0;
            int hdFormat = -1;
            int sdFormat = -1;
            int counter = 0;
            //set up default sizes for images
            Size movie_size = new Size(173, 248);
            Size tv_size = new Size(294, 248);

            
            //not all titles have HD, so we loop through available options and pick HD if we can.
            foreach (JObject format in node["formats"])
            {
                if ((string)format["videoFormatType"] == "HD") hdFormat = counter;
                if ((string)format["videoFormatType"] == "SD") sdFormat = counter;
                counter++;
            }
            selectedFormat = (hdFormat != -1) ? hdFormat : sdFormat;

            string test = node.ToString();
            //TODO: REmove this test
            m_Title = m_ItemIndex.ToString() + ". " + (string)node["title"];

            //m_Title = (string)node["title"];
            m_Synopsis = (string)node["synopsis"];
            m_RegulatoryRating = (string)node["regulatoryRating"];
            m_contentType = (string)node["contentType"];
            JObject runtime = (JObject)node["runtime"];
            m_runtime = "";
            if (runtime != null)
            {
                if(runtime["valueFormatted"] != null)
                {
                m_runtime = (string)node["runtime"]["valueFormatted"];
                }
            }
            switch (m_contentType)
            {
                case "MOVIE":
                    size = movie_size;
                    break;
                case "SEASON":
                    size = tv_size;
                    string childquery = (string)node["childTitles"][0]["feedUrl"];
                    if (childquery.IndexOf("?") > 0)
                    {
                        //this must be a browse query, and therefore contains the full url but without the device keys.
                        //need to replace the prefix and use the querystring.
                        m_ChildTitleQuery = AmazonVideoRequest.generateUrl("catalog/Browse") + "&" + childquery.Split('?')[1];
                    }
                    else
                    {
                        //this must have come from a search query.
                        m_ChildTitleQuery = AmazonVideoRequest.generateUrl("catalog/Browse") + "&" + childquery + "&version=2";
                    }
                    break;
                    //So far, I have not yet seen Series or Episode, but I know they are options, so when we find one, we 
                    //will need to address it.
                case "SERIES":
                    size = tv_size;
                    m_ChildTitleQuery = (string)node["childTitles"][0]["feedUrl"];
                    break;
                case "EPISODE":
                    size = tv_size;
                    break;
            }

            m_ASIN = (string)node["titleId"];
            //string m_ImageURL = (string)node["formats"][selectedFormat]["images"][1]["uri"];
            //m_Image = new Image(m_ImageURL);
            m_Price = "Free!";
            FirePropertyChanged("size");
            FirePropertyChanged("Title");
            FirePropertyChanged("Synopsis"); 
        }
        public string Query
        {
            get
            {
                return m_ItemQuery;
            }
            set
            {
                m_ItemQuery = value;
            }
        }
        //When this item is a season, it will have a query property under childtitles
        //we need to be able to bind to this in DisplaySeasons.mcml, so we expose here as a property that is not initialized
        //at the start, but caches if it does get loaded.
        public VideoItems ChildTitles
        {
            get
            {
                if (m_ChildTitleItems == null)
                {
                    m_ChildTitleItems = new VideoItems();
                    m_ChildTitleItems.Query = m_ChildTitleQuery;
                }
                return m_ChildTitleItems;
            }
        }
        public String Title { get { return m_Title; } }
        public Image Image { get { return m_Image; } set { m_Image = value; FirePropertyChanged("Image"); } }
        public String Price { get { return m_Price; } }
        public String Synopsis { get { return m_Synopsis; } }
        public String ASIN { get { return m_ASIN; } }
        public String RegulatoryRating { get { return m_RegulatoryRating; } }
        public String Runtime { get { return m_runtime; } }
        public String ContentType { get { return m_contentType; } }

        //This is used in mcml to get back to the application from the current video item
        public Application Application
        {
            get { return Application.Current; }
        }

        //I don't think this is used anymore, but I need to check
        public Command LaunchViewer()
        {
            return m_Command;
        }

        //This navigates to the actual video viewer.
        //TODO: Move this into Application, and place the stub here to call it from Application.
        public void LaunchVideoViewer()
        {

            string flashVars = AmazonVideoRequest.getFlashVars(m_ASIN);
            string htmlexpath = Application.Current.viewerPath + "?ASIN=" + m_ASIN + flashVars;
            Application.Current.MediaCenterEnvironment.NavigateToPage(PageId.ExtensibilityUrl, htmlexpath);
        }

        //This navigates to the relevant next step if this item is selected in VideoGallery.mcml
        //It generally just goes to VideoDetails unless it is a season.
        public void ViewDetails()
        {
            switch (m_contentType)
            {
                case "MOVIE":
                    Application.Current.GoToVideoDetails(this);
                    break;
                case "SEASON":
                    Application.Current.GoToSeasonDetails(this);
                    break;
                case "SERIES":
                    Application.Current.GoToVideoDetails(this);
                    break;
                case "EPISODE":
                    Application.Current.GoToVideoDetails(this);
                    break;
            }
        }


    }
}
