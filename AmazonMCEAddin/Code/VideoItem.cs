using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Image m_Image;
        //private string m_ImageUrl;
        private string m_Price;
        private string m_ASIN;
        private AmazonRating amazonRating;
        private Image starsRatingImage;
        private static Image starsFullImage = new Image("resx://AmazonMCEAddin/AmazonMCEAddin.Resources/stars_full");
        private static Image starsHalfImage = new Image("resx://AmazonMCEAddin/AmazonMCEAddin.Resources/stars_half");
        private string m_Genres;
        private string m_RegulatoryRating;
        private string m_Director;
        private string m_StarringCast;
        private string m_StudioOrNetwork;
        private string m_runtime;
        private Format selectedFormat;
        private DateTime m_firstAiringDate;
        private string m_contentType;
        private string m_ChildTitleQuery;
        private VideoItems m_ChildTitleItems;
        private bool m_trailerAvailable;

        private string m_ItemQuery;
        private int m_ItemIndex;
        
        public Size size { set; get; }
        /// <summary>
        /// New constructor for virtual list.
        /// </summary>
        /// <param name="vlist"></param>
        /// <param name="Index"></param>
        public VideoItem(IModelItemOwner owner, int Index)
            : base(owner)        
        {
            m_ItemIndex = Index;
        }

        public VideoItem()
        {
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
            //Debug.Print(node.ToString());

            m_Command = new Command();
            //try to use HD unless no HD available.
            Format sdFormat = null;
            Format hdFormat = null;
            //set up default sizes for images
            Size movie_size = new Size(188, 250);
            Size tv_size = new Size(334, 250);
            
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
            starsRatingImage = (amazonRating.Rating % 1 == 0) ? starsFullImage : starsHalfImage;
            m_RegulatoryRating = (string)node["regulatoryRating"];
            m_trailerAvailable = (bool)node["trailerAvailable"];
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
            if (releaseOrFirstAiringDate != null)
            {
                if (releaseOrFirstAiringDate["valueFormatted"] != null)
                {
                    m_firstAiringDate = (DateTime)node["releaseOrFirstAiringDate"]["valueFormatted"];
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

            //todo: fix this to work properly with the right image url
            //m_ImageUrl =(string)node["formats"][selectedFormat]["images"][1]["uri"];;
            //m_Image = new Image(m_ImageURL);
            m_Price = "Free!";
            FirePropertyChanged("size");
            FirePropertyChanged("Title");
            FirePropertyChanged("Synopsis");
        }

        public void processDetailData(JObject node)
        {
            Debug.Print(node.ToString());

            Format sdFormat = null;
            Format hdFormat = null;
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

            m_Genres = "";
            foreach (JValue genre in node["genres"])
            {
                if (m_Genres.Length > 0)
                {
                    m_Genres += ", ";
                }
                m_Genres += (string)genre;
            }
            m_Director = node["director"] != null ? (string)node["director"] : "";
            m_StarringCast = node["starringCast"] != null ? (string)node["starringCast"] : "";
            m_StudioOrNetwork = node["studioOrNetwork"] != null ? (string)node["studioOrNetwork"] : "";
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

        public Format Format { get { return selectedFormat; } }

        public Image Image { get { return m_Image; } set { m_Image = value; FirePropertyChanged("Image"); } }

        //public string ImageUrl { get { return m_ImageUrl; } }

        public String Price { get { return m_Price; } }

        public String Synopsis { get { return m_Synopsis; } }

        public String ASIN { get { return m_ASIN; } }

        public AmazonRating AmazonRating { get { return amazonRating; } }

        public Image StarsRatingImage { get { return starsRatingImage; } }

        public float StarsRatingImageStartOffset { get { return (5 - (float)Math.Ceiling(amazonRating.Rating)) * 26; } }

        public float StarsRatingImageEndOffset { get { return -(float)Math.Ceiling(amazonRating.Rating) * 26; } }

        public Inset StarsRatingImageMargin { get { return new Inset(-(int)((5 - (float)Math.Ceiling(amazonRating.Rating)) * 26), 0, -(int)Math.Ceiling(amazonRating.Rating) * 26, 0); } }

        public String Genres { get { return m_Genres; } }

        public String RegulatoryRating { get { return m_RegulatoryRating; } }

        public String Director { get { return m_Director; } }

        public String StarringCast { get { return m_StarringCast; } }

        public String StudioOrNetwork { get { return m_StudioOrNetwork; } }

        public String Runtime { get { return m_runtime; } }

        public String FirstAiringYear { get { return (m_firstAiringDate.Equals(DateTime.MinValue)) ? "" : m_firstAiringDate.ToString("yyyy"); } }

        public String FirstAiringDate { get { return (m_firstAiringDate.Equals(DateTime.MinValue)) ? "" : m_firstAiringDate.ToString("MMMM d, yyyy"); } }

        public bool TrailerAvailable { get { return m_trailerAvailable && !Format.SubscriptionAvailable; } }

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

        public void LaunchTrailerViewer()
        {
            string flashVars = AmazonVideoRequest.getFlashVars(m_ASIN);
            string htmlexpath = Application.Current.viewerPath + "?ASIN=" + m_ASIN + flashVars;
            Application.Current.MediaCenterEnvironment.NavigateToPage(PageId.ExtensibilityUrl, htmlexpath);
        }

        //This navigates to the relevant next step if this item is selected in VideoGallery.mcml
        //It generally just goes to VideoDetails unless it is a season.
        public void ViewDetails()
        {
            // Request additional metadata and process
            string detailData = AmazonVideoRequest.getVideoDetails(m_ASIN);
            JObject detailResults = JObject.Parse(detailData);
            processDetailData((JObject)detailResults["message"]["body"]["titles"][0]);

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
