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

        public Size size { set; get; }
        private static string NAMESPACE_PREFIX = "x";
        private static string CONTENT_SEASON = "SEASON";
        private static string CONTENT_MOVIE = "MOVIE";

        public VideoItem()
        {
        }
        public VideoItem(JObject node)
        {
            m_Command = new Command();
            //try to use HD unless no HD available.
            int selectedFormat = 0;
            int hdFormat = -1;
            int sdFormat = -1;
            int counter = 0;
            Size movie_size = new Size(173, 248);
            Size tv_size = new Size(294, 248);

            foreach (JObject format in node["formats"])
            {
                if ((string)format["videoFormatType"] == "HD") hdFormat = counter;
                if ((string)format["videoFormatType"] == "SD") sdFormat = counter;
                counter++;
            }
            selectedFormat = (hdFormat != -1) ? hdFormat : sdFormat;

            string test = node.ToString();
            m_Title = (string)node["title"];
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
                case "SERIES":
                    size = tv_size;
                    m_ChildTitleQuery = (string)node["childTitles"][0]["feedUrl"];
                    break;
                case "EPISODE":
                    size = tv_size;
                    break;
            }
            m_ASIN = (string)node["titleId"];
            string m_ImageURL = (string)node["formats"][selectedFormat]["images"][1]["uri"];
            m_Image = new Image(m_ImageURL);
            m_Price = "Free!";

        }
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
        public Image Image { get { return m_Image; } }
        public String Price { get { return m_Price; } }
        public String Synopsis { get { return m_Synopsis; } }
        public String ASIN { get { return m_ASIN; } }
        public String RegulatoryRating { get { return m_RegulatoryRating; } }
        public String Runtime { get { return m_runtime; } }
        public String ContentType { get { return m_contentType; } }

        public Application Application
        {
            get { return Application.Current; }
        }


        public Command LaunchViewer()
        {
            return m_Command;
        }
        public void LaunchVideoViewer()
        {

            string flashVars = AmazonVideoRequest.getFlashVars(m_ASIN);
            string htmlexpath = Application.Current.viewerPath + "?ASIN=" + m_ASIN + flashVars;
            Application.Current.MediaCenterEnvironment.NavigateToPage(PageId.ExtensibilityUrl, htmlexpath);
        }
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
