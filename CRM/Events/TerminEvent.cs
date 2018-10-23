using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;


namespace SteinbachCRM.Events
{
    public class TerminEvent
    {
     

        public int TerminID { get;private set; }
        public SteinbachEntities db { get; private set; }
        public string TerminTyp { get; private set; }
        public TerminEvent(int _TerminId,SteinbachEntities _db,string TerminTyp)
        {
            TerminID = _TerminId;
            db = _db;
            this.TerminTyp = TerminTyp;

        }
    }
}
