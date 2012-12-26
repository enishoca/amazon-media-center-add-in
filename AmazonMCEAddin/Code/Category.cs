using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.MediaCenter;
using Microsoft.MediaCenter.Hosting;
using Microsoft.MediaCenter.UI;

namespace AmazonMCEAddin
{
    //This represents a specific menu item in the overall structure, and is largely derived from amazon's category structure.
    public sealed class Category : ModelItem
    {
        private bool mSetFocus;
        public string Name { get; set; }
        public string CatDescription { get; set; }
        public string mQuery;
        public bool hasChildren { get; set; }
        public Category Parent { get; set; }
        protected Choice m_Choice;
        public Choice ListContent { get { return m_Choice; } }
        public ArrayListDataSet List { get; set; }
        public int Index { get; set; }

        public Category()
        {
            Name = "";
            mQuery = "";
            Index = 0;
            mSetFocus = false;
            m_Choice = new Choice();
            List = new ArrayListDataSet();
        }

        public string ContextTitle
        {
            get
            {
                if (Parent != null)
                {
                    return Parent.ContextTitle + " > " + Name;
                }
                else
                {
                    return Name;
                }
            }
        }

        public Application Application
        {
            get { return Application.Current; }
        }

        public Category(string sName, string sQuery, Category oParent, int iIndex)
        {
            Name = sName;
            mQuery = sQuery;
            Parent = oParent;
            Index = iIndex;
            m_Choice = new Choice();
            List = new ArrayListDataSet();
        }

        //REVIEW:  This can potentially be removed
        public void bindListToChoice()
        {
            //This may well crash.
            m_Choice.Options = List;
        }

        //This is generally used for reading the category query that was set by Amazon
        //but in the case of a search, we set the query here, which fires a change and the bound videogallery refreshes with the new content
        public string Query
        {
            get { return mQuery; }
            set
            {
                mQuery = value;
                FirePropertyChanged("Query");
            }
        }

        //show this category as being the selected category
        public bool SetFocus
        {
            get { return mSetFocus; }
            set
            {
                mSetFocus = value;
                FirePropertyChanged("SetFocus");
            }
        }

        //Set this current category to be the new context
        public void GoToCategory()
        {
            switch (Name)
            {
                case "Sign-out":
                    Application.Logout();
                    break;
                default:
                    Application.Current.CurrentContext = this;
                    break;
            }
        }

        public Image Image
        {
            get
            {
                // FIXME: Ugly amount of hard-coding here.

                //http://g-ecx.images-amazon.com/images/G/01/digital/video/ps3/movies_top_prime.jpg
                //http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/prime_popular.jpg
                //http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/movies_genres_romance.jpg

                switch (Parent.Name)
                {
                    case "Movies":
                        switch (Name)
                        {
                            case "Popular Movies":
                                return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/movies_popular.jpg");
                            case "Editor's Picks":
                                return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/movies_editorspicks.jpg");
                            case "All Genres":
                                return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/movies_genres.jpg");
                            case "For The Kids":
                                return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/movies_genres_kids.jpg");
                        }
                        break;
                    case "TV":
                        switch (Name)
                        {
                            case "Popular TV Shows":
                                return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/tv_popular.jpg");
                            case "Editor's Picks":
                                return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/tv_editorspicks.jpg");
                            case "All Genres":
                                return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/tv_genres.jpg");
                            case "For The Kids":
                                return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/tv_genres_kids.jpg");
                            case "TV Channels":
                                //return new Image("http://g-ecx.images-amazon.com/images/G/01/digital/video/ps3/tv_tvchannels.jpg");
                                return new Image("http://g-ecx.images-amazon.com/images/G/01/digital/video/ps3/prime_tvchannels.jpg");
                        }
                        break;
                    case "Editor's Picks":
                        if (Parent.Parent.Name.Equals("Movies"))
                        {
                            switch (Name)
                            {
                                case "From Page to Screen":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/movies_editorspicks_page_to_screen.jpg");
                                case "History Repeats Itself":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/movies_editorspicks_history.jpg");
                            }
                        }
                        else
                        {
                            switch (Name)
                            {
                                case "British Invasion":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/tv_editorspicks_british_invasion.jpg");
                            }
                        }
                        break;
                    case "All Genres":
                        if (Parent.Parent.Name.Equals("Movies"))
                        {
                            switch (Name)
                            {
                                case "Drama":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/movies_genres_drama.jpg");
                                case "Horror":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/movies_genres_horror.jpg");
                                case "International":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/movies_genres_international.jpg");
                                case "Kids & Family":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/movies_genres_kids.jpg");
                                case "Sci-Fi & Fantasy":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/movies_genres_scifi.jpg");
                            }
                        }
                        else
                        {
                            switch (Name)
                            {
                                case "Action & Adventure":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/tv_genres_action.jpg");
                                case "Animation":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/tv_genres_animation.jpg");
                                case "Comedy":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/tv_genres_comedy.jpg");
                                case "Drama":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/tv_genres_drama.jpg");
                                case "Kids & Family":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/tv_genres_kids.jpg");
                                case "Sci-Fi & Fantasy":
                                    return new Image("http://g-ec2.images-amazon.com/images/G/01/digital/video/ps3/tv_genres_scifi.jpg");
                            }
                        }
                        break;
                    case "TV Channels":
                        string channel = Name.Replace(' ', '_').ToLower();
                        return new Image("http://g-ecx.images-amazon.com/images/G/01/digital/video/ps3/tv_tvchannels_" + channel + ".jpg");
                        //return new Image("http://g-ecx.images-amazon.com/images/G/01/digital/video/ps3/prime_tvchannels_" + channel + ".jpg");
                }

                return new Image("http://g-ecx.images-amazon.com/images/G/01/AIV/ps3/v1.2.1-1mtsp/assets/home/empty-library.png");
            }
        }
    }
}
