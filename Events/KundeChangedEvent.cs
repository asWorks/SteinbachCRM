using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;

namespace SteinbachCRM.Events
{
    public class KundeChangedEvent
    {
        public int id { get; set; }
        public string addionalInfo { get; set; }
        public firma Firma { get; set; }
        public SteinbachEntities db { get; set; }

        public KundeChangedEvent(int id,string addionalInfo,firma Firma,SteinbachEntities db)
        {
            this.id = id;
            this.addionalInfo = addionalInfo;
            this.db = db;
            this.Firma = Firma;

        }
    }
}
