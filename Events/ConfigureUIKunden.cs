using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteinbachCRM.Events
{
    public class ConfigureUIKunden
    {
        public int SelectTab { get; private set; }
        public string ActivatedTabs { get; private set; }

        public ConfigureUIKunden(int SelectTab,string ActivatedTabs)
        {
            this.SelectTab = SelectTab;
            this.ActivatedTabs = ActivatedTabs;

        }
    }
}
