using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteinbachCRM.Events
{
    public class TerminSavedEvent
    {
        public int TerminId { get; private set; }
        public TerminSavedEvent(int TerminId )
        {
            this.TerminId = TerminId;

        }
    }
}
