using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Web;
using System.Diagnostics;

//This needs some cleanup

namespace AmazonMCEAddin
{
    class AmazonVideoRequest
    {
        
        
        /// <summary>
        /// requests data optionally using the amazon cookies
        /// </summary>
        /// <param name="url"></param>
        /// <param name="useCookie"></param>
        /// <returns></returns>
        public static string getURL(string url, bool useCookie = false)
        {
            WebClientWithCookies client;
            if (useCookie)
            {
                var formatter = new BinaryFormatter();
                CookieContainer cc = null;
                using (Stream s = File.OpenRead(cookiePath()))
                    cc = (CookieContainer)formatter.Deserialize(s);

                client = new WebClientWithCookies(cc);
            }
            else
            {
                client = new WebClientWithCookies(new CookieContainer());
            }

            //generate a signature for this URL
            string sig = generate_signature(url);

            //add this signature as a header
            client.Headers.Add("x-android-sign", sig);

            //get the data
            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            return reader.ReadToEnd();
        }
        //Generates a url with the base parameters populated.
        /*legal modes are :
        'catalog/GetCategoryList'
        'catalog/Browse'
        'catalog/Search'
        'catalog/GetSearchSuggestions'
        'catalog/GetASINDetails'
        'catalog/GetSimilarities'

        'catalog/GetStreamingUrls'
        'catalog/GetStreamingTrailerUrls'
        'catalog/GetContentUrls'

        'library/GetLibrary'
        'library/Purchase'
        'library/GetRecentPurchases'

        'link/LinkDevice'
        'link/UnlinkDevice'
        'link/RegisterClient'
        'licensing/Release'

        'usage/UpdateStream'
        'usage/ReportLogEvent'
        'usage/ReportEvent'
        'usage/GetServerConfig'
         * */
        public static string generateUrl(string mode)
        {
            string deviceID = Resources.DeviceID;
            string deviceTypeID = Resources.DeviceTypeID;
            string firmware = Resources.Firmware;
            string format = "json";
            string parameters = "?firmware=" + firmware + "&deviceTypeID=" + deviceTypeID + "&deviceID=" + deviceID + "&format=" + format;
            return "https://atv-ext.amazon.com/cdp/" + mode + parameters;

        }
        /// <summary>
        ///Deletes the amazon cookie
        /// </summary>
        public static void clearLoginCookie()
        {
            File.Delete(cookiePath());
        }
        public static string generateSearchUrl(string keywords, int maxItemCount = 24, int startItem = 0)
        {
            string encodedKeywords = HttpUtility.UrlEncode(keywords);
            string parameters = "&searchString=" + encodedKeywords + "&OfferGroups=B0043YVHMY&Detailed=T&IncludeAll=T&SuppressBlackedoutEST=T&version=2&HideNum=F&NumberOfResults=" + maxItemCount + "&StartIndex=" + startItem;
            return generateUrl("catalog/Search") + parameters;
        }
        /// <summary>
        /// Get the full details on a specific video
        /// </summary>
        /// <param name="asin">amazon product id</param>
        /// <returns>string of the video details</returns>
        public static string getVideoDetails(string asin)
        {
            string parameters = "&asinList=" + asin + "&NumberOfResults=1&IncludeAll=T&playbackInformationRequired=true&version=2";
            string url = generateUrl("catalog/GetASINDetails") + parameters;
            return getURL(url, true);
        }


        //I don't think this is used any more, but I need to check
        public static string searchPrime(string keywords, int maxItemCount = 24, int startItem = 0)
        {
            return getURL(generateSearchUrl(keywords, maxItemCount, startItem), true);
        }
        public static bool checkIfHaveLoginCookie()
        {
            return File.Exists(cookiePath());
        }
        public static string ExecuteQuery(string query)
        {
            return getURL(query, true);
        }
        public static string getVideoItemsWithQuery(string query, int maxItems = 24, int startItem = 0)
        {
            string url = generateUrl("catalog/Browse") + "&version=2&HideNum=F&NumberOfResults=" + maxItems + "&StartIndex=" + startItem + "&" + query;
            return getURL(url, true);
        }

        public static string GenerateVirtualBrowseUrlTemplate()
        {
            return generateUrl("catalog/Browse") + "&version=2&HideNum=F&NumberOfResults=1";
        }
        //legacy
        public static string GenerateBrowseUrlTemplate(int maxItems = 24, int startItem = 0)
        {
            return generateUrl("catalog/Browse") + "&version=2&HideNum=F&NumberOfResults=" + maxItems + "&StartIndex=" + startItem + "&";
        }
        public static string getPrime(int maxItemCount, int startItem = 0)
        {

            string browse_parms = "&OfferGroups=B0043YVHMY&NumberOfResults=" + maxItemCount + "&StartIndex=" + startItem + "&ContentType=Movie&OrderBy=SalesRank&HighDef=F&playbackInformationRequired=false&OrderBy=SalesRank&SuppressBlackedoutEST=T&HideNum=F&Detailed=T&AID=1&IncludeNonWeb=T&version=2";
            string url = generateUrl("catalog/Browse") + browse_parms;
            return getURL(url);
        }
        public static string getCategories()
        {
            string browse_parms = "&version=1";
            string url = generateUrl("catalog/GetCategoryList") + browse_parms;
            return getURL(url);
        }

        //I don't think this is used any more, but I need to check
        public static List<Category> getPrimeCategories()
        {
            List<Category> cats= new List<Category>();

            string data = AmazonVideoRequest.getCategories();
            JsonTextReader reader = new JsonTextReader(new StringReader(data));

            JObject categories = JObject.Parse(data);

            //Get Prime Movie Categories
            foreach (JObject category in categories["message"]["body"]["categories"][1]["categories"][1]["categories"][2]["categories"])
            {
                cats.Add(new Category((string)category["title"], (string)category["query"], null,0));
            }
            //Get Prime TV Categories
            foreach (JObject category in categories["message"]["body"]["categories"][1]["categories"][1]["categories"][3]["categories"])
            {
                cats.Add(new Category((string)category["title"], (string)category["query"], null,0));
            }
            return cats;

        }
        //This is passed to the viewer to use the correct session and remote ip address etc taht amazon are expecting
        public static string getFlashVars(string ASIN)
        {
            string baseurl = "http://www.amazon.com/gp/product/";
            string url = baseurl + ASIN;
            string page = getURL(url, true);

            string pattern = @"'flashVars', '(.*?)' \+ new";
            //string pattern = "'flashVars', '(.*?)' + new Date().getTime()+ '(.*?)'";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            //put all the matches into a MatchCollection
            MatchCollection matches = reg.Matches(page);


            string flashvars = matches[0].ToString().Replace("'flashVars', '", "").Replace("' + new", "");
            return flashvars;
        }
        public static string cookiePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Resources.CookieFileName);
        }

        public static bool login(string username, string password)
        {
            BrowserSession b = new BrowserSession();
            b.Get("https://www.amazon.com/gp/flex/sign-out.html");
            b.FormElements["email"] = username;
            b.FormElements["password"] = password;
            b.FormElements["action"] = "sign-in";
            string response = b.Post("https://www.amazon.com/gp/flex/sign-in/select.html");
            string error_message = "The e-mail address and password you entered do not match any accounts on record.";
            if (response.Contains(error_message))
            {
                //Something went wrong
                return false;
            }
            else
            {
                //all good.
                CookieContainer cc = new CookieContainer();
                cc.Add(b.Cookies);
                var formatter = new BinaryFormatter();
                using (Stream s = File.Create(cookiePath()))
                    formatter.Serialize(s, cc);
                return true;
            }


        }

        #region "helper functions"

        private static string generate_signature(string url)
        {
            string result = "";
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            string HexKey = Resources.AndroidSignatureHexKey;
            byte[] keyBytes = StringToByteArray(HexKey);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyBytes);
            byte[] messageBytes = encoding.GetBytes(url);
            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            result = Convert.ToBase64String(hashmessage);
            return result;

        }
        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        #endregion

    }

    //this is a test to see if the static nature of the above calls is interfering with getting multiple requests etc.
    class AmazonRequester
    {
        
        public AmazonRequester()
        {
        }
        public string ExecuteQuery(string url, bool useCookie = true)
        {
            WebClientWithCookies client;
            if (useCookie)
            {
                var formatter = new BinaryFormatter();
                CookieContainer cc = null;
                using (Stream s = File.OpenRead(AmazonVideoRequest.cookiePath()))
                    cc = (CookieContainer)formatter.Deserialize(s);

                client = new WebClientWithCookies(cc);
            }
            else
            {
                client = new WebClientWithCookies(new CookieContainer());
            }

            //generate a signature for this URL
            string sig = generate_signature(url);

            //add this signature as a header
            client.Headers.Add("x-android-sign", sig);

            //get the data
            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            string tmp = reader.ReadToEnd();
            Trace.WriteLine("********Query: " + url + "\r\n***********" + tmp);
            return tmp;
        }

        #region "helper functions"

        private static string generate_signature(string url)
        {
            string result = "";
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            string HexKey = Resources.AndroidSignatureHexKey;
            byte[] keyBytes = StringToByteArray(HexKey);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyBytes);
            byte[] messageBytes = encoding.GetBytes(url);
            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            result = Convert.ToBase64String(hashmessage);
            return result;

        }
        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        #endregion


     }
}
