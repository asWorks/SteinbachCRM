using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteinbachCRM.Events
{

    
    public class SelectUIEvent
    {
        public enum EnumActivateModule
        {
            Termin,
            Kunde,
            Home,
            Uebersicht,
            ShutDown,
            None

        }
        public bool MainSelectionButtonsEnabled { get; private set; }
        public EnumActivateModule ActivateModule { get; private set; }
    

        public SelectUIEvent(bool ButtonsEnabled,EnumActivateModule ActivateModule)
        {
            this.MainSelectionButtonsEnabled = ButtonsEnabled;
            this.ActivateModule = ActivateModule;
           
        }

    }
}
