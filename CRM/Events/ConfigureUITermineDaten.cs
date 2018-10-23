using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteinbachCRM.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigureUITermineDaten
    {
           public int SelectTab { get;private set; }
           public string ActivatedTabs { get;private set; }
/// <summary>
/// Einstellen der Oberfläche von TermineDatenView
/// </summary>
/// <param name="SelectTab">Gibt den zu selektierenden Tab an zb. 2 = Tab 2 aktivieren</param>
/// <param name="ActivatedTabs">Tabs en-oder disablen zB. '010' Tab 1 und 3 disabled - tab 2 enabled</param>
        public ConfigureUITermineDaten(int SelectTab,string ActivatedTabs)
           {
               this.SelectTab = SelectTab;
               this.ActivatedTabs = ActivatedTabs;

           }
    }
}
