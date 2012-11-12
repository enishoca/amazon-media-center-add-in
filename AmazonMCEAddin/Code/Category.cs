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
        public Category Parent {get;set;}
        protected Choice m_Choice;
        public Choice ListContent { get { return m_Choice; } }
        public ArrayListDataSet List {get;set;}
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
        

    }
}
