using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteinbachCRM.Models
{
    public class KundenbesucheListe
    {
       
        public DateTime? datum_von { get; set; }
        public DateTime? datum_bis { get; set; }
        public string kfzkenzeichen { get; set; }
        public double kmgefahren { get; set; }
        public string id_firma { get; set; }
        public string id_kontakt { get; set; }
        public string id_siperson { get; set; }
        public string besuchsthema { get; set; }
        public string projektnummer { get; set; }
        public int id { get; set; }
        public string id_vertretenefirma { get; set; }

    }
}
