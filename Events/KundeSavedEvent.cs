using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteinbachCRM.Events
{
    public class KundeSavedEvent
    {
        public int id { get; set; }
        public KundeSavedEvent(int id)
        {
            this.id = id;
        }
        
    }
}
