using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace AmazonMCEAddin
{
    class CategoryStructureSetup
    {
        //This sets up an initial menu structure, and pulls the newest category information
        //then uses recurse to go through all the sub categories
        public static Category getCategoryStructure()
        {
            Category cat_root = new Category("Home", "", null, 0);
            
            Category cat_movies = new Category("Movies", "", cat_root, 0);
            cat_root.List.Add(cat_movies);
            Category cat_tv = new Category("TV", "", cat_root, 1);
            cat_root.List.Add(cat_tv);
            Category cat_search = new Category("Search", "", cat_root, 2);
            cat_root.List.Add(cat_search);
            Category cat_logout = new Category("Sign-out", "", cat_root, 3);
            cat_root.List.Add(cat_logout);

            cat_root.bindListToChoice();

            string categoryData = AmazonVideoRequest.getCategories();

            JsonTextReader reader = new JsonTextReader(new StringReader(categoryData));

            JObject categories = JObject.Parse(categoryData);
            //int catID = 0;

            //We will only focus on movies at the moment.
            //This is amazon prime movies
            recurse(categories["message"]["body"]["categories"][2]["categories"][2], cat_movies);

            //for when we want tv
            recurse(categories["message"]["body"]["categories"][3]["categories"][2], cat_tv);
            return cat_root;
        }

        private static void recurse(JToken category, Category parent)
        {
            string browseUrl = AmazonVideoRequest.GenerateVirtualBrowseUrlTemplate() + "&";
            int index = 0;
            parent.hasChildren = (category["categories"] != null);
            if (parent.hasChildren)
            {
                //to prevent loading of dummy content, I am resetting query to blank for any parents.
                parent.Query = "";
                foreach (JToken subcategory in category["categories"])
                {
                    string junk2 = subcategory.ToString();
                    Category subcatobj = new Category((string)subcategory["title"], browseUrl + (string)subcategory["query"], parent, index);
                    subcatobj.CatDescription = (string)subcategory["description"];
                    parent.List.Add(subcatobj);
                    parent.hasChildren = true;
                    index++;
                    recurse(subcategory, subcatobj);
                }
                parent.bindListToChoice();
            }
        }
    }
}
