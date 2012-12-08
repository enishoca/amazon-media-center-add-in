using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.MediaCenter;
using Microsoft.MediaCenter.Hosting;
using Microsoft.MediaCenter.UI;
using System.Xml;
using System.Xml.XPath;
using System;
using Microsoft.Win32;

namespace AmazonMCEAddin
{
    public class Application : ModelItem
    {
        private static Application singleApplicationInstance;
        private AddInHost host;
        private HistoryOrientedPageSession session;
        private Category currentContext;

        //Added global search string to allow us to come back from video detail to same search as before.
        public string GlobalSearchString { get; set; }

        public int MinimumSearchTextLength { get; set; }
        public string viewerPath;
        public string m_FooterLine1;
        public string m_FooterLine2;
        private string m_LoginProcessStatus = "";
        bool postLogonRun = false;

        public Application()
            : this(null, null)
        {
        }

 

        public Application(HistoryOrientedPageSession session, AddInHost host)
        {
            this.session = session;
            this.host = host;
            singleApplicationInstance = this;
        }
        public void initializeApplication()
        {
            //store the path to the viewer in the global variable
            viewerPath = getViewerPath();
        }
        //work around to get reg key from 32-bit location due to installer being 32-bit.
        private string getViewerPath()
        {
            string CompanyName = Resources.CompanyName;
            string Path = "";
            try{
                Path = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\" + CompanyName + @"\AmazonMCEAddin", "ViewerPath", "");
            }
            catch(Exception e)
            {
            }
            if(Path == null)
                try
                {
                    Path = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\" + CompanyName + @"\AmazonMCEAddin", "ViewerPath", "");
                }
            catch (Exception e)
                {
                displayErrorMessageAndQuit("Unable to find registry entry for viewer path - please reinstall");

            }
            return Path;

        }
        public VideoItemsVirtualList getVideoItemsVirtualList
        {
            get
            {

                VideoItemsVirtualList vlist = new VideoItemsVirtualList();
                vlist.Query = Current.CurrentContext.Query;
                return vlist;
            }
        }

        private void displayErrorMessageAndQuit(string message)
        {
            //todo: write an error display handler
            //PageSession.Current.Close();
            //return;

        }
        public string LoginProcessStatus
        {
            get
            {
                return m_LoginProcessStatus;
            }
            set 
            {
                m_LoginProcessStatus = value;
                FirePropertyChanged("LoginProcessStatus");
            }
        }
        //This clears the cookie file and takes the user back to the login page.
        public void Logout()
        {
            AmazonVideoRequest.clearLoginCookie();
            GoToLoginPage();
        }
        //This property is read by the menu.mcml file to populate the footer text - this usually displays either
        // the video title or the category name
        public string FooterLine1
        {
            get { return m_FooterLine1; }
            set
            {
                m_FooterLine1 = value;
                FirePropertyChanged("FooterLine1");
            }
        }
        //This property is read by the menu.mcml file to populate the footer text - this usually displays either
        // the video synopsis or the category description
        public string FooterLine2
        {
            get { return m_FooterLine2; }
            set
            {
                m_FooterLine2 = value;
                FirePropertyChanged("FooterLine2");
            }
        }
        //This function generates the query and passes it to the current content, which is expected to be "search".  This will
        //then bind the query to the video items, which will initiate the search
        public void DoSearch(string SearchText)
        {
            if(SearchText.Length >= Convert.ToInt32(Resources.MinimumSearchTextLength))
            {
                CurrentContext.Query = AmazonVideoRequest.generateSearchUrl(SearchText);
            }
            else
            {
                CurrentContext.Query = "";
            }
        }
        //This sends the username and password to amazon using the helper functions (which then create local cookies etc)
        public void DoLogin(string username, string password)
        {
            LoginProcessStatus = "Processing";
            if (username == "" | password == "")
            {
                LoginProcessStatus = "Required";
                return;
            }
            if (AmazonVideoRequest.login(username, password))
            {
                GoToMainMenu();
                return;
            }
            else
            {
                LoginProcessStatus = "Error";
                return;
                //TODO: Need to put something in here to let user know it didn't work.
            }
        }
        //After logim go and get the category structure
        //TODO: Change this over to use cached data, and maybe check in the background for newer data later
        //      this could make launch faster etc.
        public void PostLogin()
        {
            
            Category rootCat = CategoryStructureSetup.getCategoryStructure();
            currentContext = (Category)rootCat.ListContent.Options[0];
        }
        //Returns the currently selected category
        //All the navigation is driven by this - you can access sub-categories by looking at CurrentContext.ListItems.Options
        //and you can change the current category by simply assigning a new category here.
        public Category CurrentContext
        {
            get { return currentContext; }
            set
            {
                currentContext = value;
                FooterLine1 = "";
                FooterLine2 = "";
                FirePropertyChanged("CurrentContext");

            }
        }
        //checks to see if there is a cookie
        //NOTE: This doesn't check to see if the login is actually valid.
        private bool LoggedIn()
        {
            return AmazonVideoRequest.checkIfHaveLoginCookie();
        }

        public static Application Current
        {
            get
            {
                return singleApplicationInstance;
            }
        }

        public MediaCenterEnvironment MediaCenterEnvironment
        {
            get
            {
                if (host == null) return null;
                return host.MediaCenterEnvironment;
            }
        }


        #region Page Navigation

        //Show the user the login page
        public void GoToLoginPage()
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties["Application"] = this;

            if (session != null)
            {
                
                session.GoToPage("resx://AmazonMCEAddin/AmazonMCEAddin.Resources/Login", properties);
            }
            else
            {
                Debug.WriteLine("GoToLoginPage");
            }
        }
        //Show the user the search page
        public void GoToSearchPage()
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties["Application"] = this;

            if (session != null)
            {
                session.GoToPage("resx://AmazonMCEAddin/AmazonMCEAddin.Resources/Search", properties);
            }
            else
            {
                Debug.WriteLine("GoToSearchPage");
            }
        }
        //Go to the main screen.
        public void GoToMainMenu()
        {
            if (!postLogonRun)
            {
                PostLogin();
                postLogonRun = true;
            }

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties["Application"] = this;

            if (session != null)
            {
                session.GoToPage("resx://AmazonMCEAddin/AmazonMCEAddin.Resources/Menu", properties);
            }
            else
            {
                Debug.WriteLine("GoToMainMenu");
            }
        }
        //Shows the details page, from where you can start watching a video
        public void GoToVideoDetails(VideoItem videoItem)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties["Application"] = this;
            properties["VideoItem"] = videoItem;

            if (session != null)
            {
                session.GoToPage("resx://AmazonMCEAddin/AmazonMCEAddin.Resources/VideoDetails", properties);
            }
            else
            {
                Debug.WriteLine("GoToVideoDetails");
            }
        }
        //Shows a page with a listing of the episodes in a season.
        public void GoToSeasonDetails(VideoItem videoItem)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties["Application"] = this;
            properties["VideoItem"] = videoItem;

            if (session != null)
            {
                session.GoToPage("resx://AmazonMCEAddin/AmazonMCEAddin.Resources/SeasonDetails", properties);
            }
            else
            {
                Debug.WriteLine("GoToSeasonDetails");
            }
        }

        //Step back up the category hierarchy - if at the top node, exit the application
        public void MoveBack()
        {
            if (!LoggedIn())
            {
                PageSession.Current.Close();
                return;
            }
            if (CurrentContext.Parent.Parent != null)
            {
                //move back
                CurrentContext = CurrentContext.Parent;
            }
            else
            {
                //quit
                PageSession.Current.Close();
                return;
            }
        }
        #endregion

        public void StartAmazonMCEAddin()
        {
            //This is presumably the place to load up initial content.
            initializeApplication();

            if (session != null)
            {
                //viewerPath = (new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath + @"\web\amazonviewer.htm";
                //First of all, check if we have already logged in.
                if (!LoggedIn())
                {
                    GoToLoginPage();
                }
                else
                {
                    GoToMainMenu();
                }
            }
            else
            {
                Debug.WriteLine("StartAmazonMCEAddin");
            }
        }
    }
}