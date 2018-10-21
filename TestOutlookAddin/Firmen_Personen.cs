using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestOutlookAddin
{
    public partial class Firmen_Personen
    {
        private string _Fullname;

        public string Fullname
        {
            get
            { return string.Format("{0}, {1} ", Nachname,Vorname) ;}
            
        }
        
    }
}
