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
            Category cat_root = new Category("", "", null, 0);

            Category cat_home = new Category("Home", "", cat_root, 0);
            cat_root.List.Add(cat_home);
            Category cat_prime = null;
            if (Resources.PrimeOnly.Equals("false"))
            {
                cat_prime = new Category("Prime Instant Video", "", cat_root, 1);
                cat_root.List.Add(cat_prime);
            }
            Category cat_movies = new Category("Movies", "", cat_root, 2);
            cat_root.List.Add(cat_movies);
            Category cat_tv = new Category("TV Shows", "", cat_root, 3);
            cat_root.List.Add(cat_tv);
            Category cat_search = new Category("Search", "", cat_root, 4);
            cat_root.List.Add(cat_search);
            Category cat_logout = new Category("Sign-out", "", cat_root, 5);
            cat_root.List.Add(cat_logout);

            cat_root.bindListToChoice();

            string categoryData = AmazonVideoRequest.getCategories();
            JsonTextReader reader = new JsonTextReader(new StringReader(categoryData));
            JObject categories = JObject.Parse(categoryData);
            //System.Diagnostics.Debug.Print(categories.ToString());

            //int catID = 0;

            cat_home.hasChildren = true;
            Category watchList = new Category("Your Watchlist", "", cat_home, 0);
            cat_home.List.Add(watchList);
            Category yourVideoLibrary = new Category("Your Video Library", AmazonVideoRequest.getLibraryRequest(), cat_home, 1);
            cat_home.List.Add(yourVideoLibrary);
            Category recentlyWatched = new Category("Recently Watched", "", cat_home, 2);
            cat_home.List.Add(recentlyWatched);
            Category yourTvShows = new Category("Your TV Shows", "", cat_home, 3);
            cat_home.List.Add(yourTvShows);
            cat_home.bindListToChoice();

            int subCategoryIndex = 2;
            // Load 'Prime Instant Video' root category
            if (Resources.PrimeOnly.Equals("false"))
            {
                subCategoryIndex = 1;
                recurse(categories["message"]["body"]["categories"][1]["categories"][subCategoryIndex], cat_prime);
            }

            // Load 'Movies' root category
            recurse(categories["message"]["body"]["categories"][2]["categories"][subCategoryIndex], cat_movies);

            // Load 'TV Shows' root category
            recurse(categories["message"]["body"]["categories"][3]["categories"][subCategoryIndex], cat_tv);
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
