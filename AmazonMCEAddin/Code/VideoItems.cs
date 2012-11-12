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

namespace AmazonMCEAddin
{
    public class VideoItems: ModelItem
    {
        private string m_Query;
        protected Choice m_Choice;
        public Choice ListContent { get { return m_Choice; } }
        ArrayListDataSet List;


        public VideoItems()
        {
            m_Choice = new Choice();
            List = new ArrayListDataSet();
            string junk = "test";
            //workaround to add list without crashing
            //this is a known issue - if you bind an empty list to a choice, it crashes.
            List.Add(junk);
            m_Choice.Options = List;
            List.Clear();


        
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
                if (m_Query == "")
                {
                    m_Choice.Options.Clear();
                    FirePropertyChanged("ListContent");

                }
                else
                {
                    ExecuteQuery(m_Query);
                }
                FirePropertyChanged("Query");
            }
        }

        //This function actually executes the query set above, and loops through the dataset, initializing each new item
        //with a separate json node.
        private void ExecuteQuery(string query, int maxItems = 24, int startItem = 0, bool search = false)
        {
            string data;
            data = AmazonVideoRequest.ExecuteQuery(query);

            JsonTextReader reader = new JsonTextReader(new StringReader(data));

            JObject titles = JObject.Parse(data);
            m_Choice.Options.Clear();
            foreach (JObject node in titles["message"]["body"]["titles"])
            {
                m_Choice.Options.Add(new VideoItem(node));

            }
            FirePropertyChanged("ListContent");
        }
        //I don't think this is used any more, but I need to check
        public VideoItems(string query, int maxItems=24, int startItem=0, bool search=false)
        {
            m_Choice = new Choice();
            List = new ArrayListDataSet();
            string data;
            //if (search)
            //{
            //    data = AmazonVideoRequest.searchPrime(query, maxItems, startItem);
            //}
            //else
            //{
            //    data = AmazonVideoRequest.getVideoItemsWithQuery(query, maxItems, startItem);
            //}
            data = AmazonVideoRequest.ExecuteQuery(query);
            JsonTextReader reader = new JsonTextReader(new StringReader(data));

            JObject titles = JObject.Parse(data);
            foreach (JObject node in titles["message"]["body"]["titles"])
            {
                List.Add(new VideoItem(node));

            }
            m_Choice.Options = List;
            
        }
            
     }
}
