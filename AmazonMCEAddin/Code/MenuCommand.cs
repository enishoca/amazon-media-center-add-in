using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.MediaCenter.UI;

namespace AmazonMCEAddin
{
    public class MenuCommand : Command
    {
        private object data;

        public new object Data
        {
            get { return data; }
            set
            {
                if (data != value)
                {
                    data = value;
                    FirePropertyChanged("Data");
                }
            }
        }

        public MenuCommand()
            : base()
        {
        }

        public MenuCommand(IModelItemOwner owner)
            : base(owner)
        {
        }
    }
}
