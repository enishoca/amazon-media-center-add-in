using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.MediaCenter;
using Microsoft.MediaCenter.Hosting;
using Microsoft.MediaCenter.UI;
using System.Xml;
using System.Xml.XPath;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace AmazonMCEAddin
{
    public struct AmazonRating
    {
        public int Count { get; set; }

        public float Rating { get; set; }
    }

    public class VideoItem : ModelItem
    {
        private Command m_Command;
        private string m_Title;
        private string m_Synopsis;
        private string m_ASIN;
        private AmazonRating amazonRating;
        private string m_RegulatoryRating;
        private string m_runtime;
        private Format selectedFormat;
        private string m_firstAiringDate;
        private string m_contentType;
        private string m_ChildTitleQuery;
        private VideoItems m_ChildTitleItems;

        public Size size { set; get; }

        public VideoItem()
        {
        }

        //Initializes a new video item with a json node.
        public VideoItem(JObject node)
        {
            m_Command = new Command();
            //try to use HD unless no HD available.
            Format sdFormat = null;
            Format hdFormat = null;
            //set up default sizes for images
            Size movie_size = new Size(173, 248);
            Size tv_size = new Size(294, 248);

            //not all titles have HD, so we loop through available options and pick HD if we can.
            foreach (JObject format in node["formats"])
            {
                switch ((string)format["videoFormatType"])
                {
                    case "HD":
                        hdFormat = new Format(format);
                        break;
                    case "SD":
                        sdFormat = new Format(format);
                        break;
                }
            }
            selectedFormat = (hdFormat != null) ? hdFormat : sdFormat;

            m_Title = (string)node["title"];
            m_Synopsis = (string)node["synopsis"];
            amazonRating = new AmazonRating();
            amazonRating.Count = (int)node["amazonRating"]["count"];
            amazonRating.Rating = (float)node["amazonRating"]["rating"];
            m_RegulatoryRating = (string)node["regulatoryRating"];
            m_contentType = (string)node["contentType"];
            JObject runtime = (JObject)node["runtime"];
            m_runtime = "";
            if (runtime != null)
            {
                if (runtime["valueFormatted"] != null)
                {
                    m_runtime = (string)node["runtime"]["valueFormatted"];
                }
            }
            JObject releaseOrFirstAiringDate = (JObject)node["releaseOrFirstAiringDate"];
            m_firstAiringDate = "";
            if (releaseOrFirstAiringDate != null)
            {
                if (releaseOrFirstAiringDate["valueFormatted"] != null)
                {
                    m_firstAiringDate = ((DateTime)node["releaseOrFirstAiringDate"]["valueFormatted"]).ToString("MMMM d, yyyy");
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

        public Format Format { get { return selectedFormat; } }

        public String Synopsis { get { return m_Synopsis; } }

        public String ASIN { get { return m_ASIN; } }

        public AmazonRating AmazonRating { get { return amazonRating; } }

        public String RegulatoryRating { get { return m_RegulatoryRating; } }

        public String Runtime { get { return m_runtime; } }

        public String FirstAiringDate { get { return m_firstAiringDate; } }

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
