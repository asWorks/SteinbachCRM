using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteinbachCRM.Events
{
    public class SelectKundenDatenEnabled
    {
        public bool isEnabled { get; set; }

        public SelectKundenDatenEnabled(bool isEnabled)
        {
            this.isEnabled = isEnabled;
        }
    }
}
