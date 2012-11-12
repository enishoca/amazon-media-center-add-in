using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.MediaCenter;
using Microsoft.MediaCenter.Hosting;
using Microsoft.MediaCenter.UI;

namespace AmazonMCEAddin
{

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
        public string Query
        {
            get { return mQuery; }
            set
            {
                mQuery = value;
                FirePropertyChanged("Query");
            }
        }
        public bool SetFocus
        {
            get { return mSetFocus; }
            set
            {
                mSetFocus = value;
                FirePropertyChanged("SetFocus");
            }
        }


        public void GoToCategory()
        {
            switch (Name)
            {
                case "Sign-out":
                    Application.Logout();
                    break;
                default:
                    //int ind = this.Index;
                    //int currnt = Application.Current.CurrentContext.ListContent.ChosenIndex;
                    //Application.Current.CurrentContext.ListContent.ChosenIndex = this.Index;
                    //if(this.ListContent.Options != null) this.ListContent.ChosenIndex = 0;

                    //Change the context to show the new menu etc
                    Application.Current.CurrentContext = this;

                    //Now focus on this item in the menu. not working because list isn't reloaded yet.
                    //((Category)this.Parent.ListContent.Options[this.Index]).SetFocus = true;

                    //Application.Current.CurrentContext.Parent.ListContent.ChosenIndex = this.Index;
                    break;

            }
        }
        

    }
}
