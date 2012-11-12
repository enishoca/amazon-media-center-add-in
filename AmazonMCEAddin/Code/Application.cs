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
        public int MinimumSearchTextLength { get; set; }
        public string viewerPath;
        public string m_FooterLine1;
        public string m_FooterLine2;
        private string m_LoginProcessStatus = "";
        bool postLogonRun = false;

        private int currentCategoryIndex = 0;
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

            string CompanyName = Resources.CompanyName;
            viewerPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\" + CompanyName + @"\AmazonMCEAddin", "ViewerPath", "");

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
        public void Logout()
        {
            AmazonVideoRequest.clearLoginCookie();
            GoToLoginPage();
        }
        public string FooterLine1
        {
            get { return m_FooterLine1; }
            set
            {
                m_FooterLine1 = value;
                FirePropertyChanged("FooterLine1");
            }
        }
        public string FooterLine2
        {
            get { return m_FooterLine2; }
            set
            {
                m_FooterLine2 = value;
                FirePropertyChanged("FooterLine2");
            }
        }
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
        public void PostLogin()
        {
            
            Category rootCat = CategoryStructureSetup.getCategoryStructure();
            currentContext = (Category)rootCat.ListContent.Options[0];
        }
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
        public int currentCategory
        {
            get {return currentCategoryIndex;}
            set {
                currentCategoryIndex = value;
            }
        }
        void command_Invoked(object sender, EventArgs e)
        {

        }
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
    }
}