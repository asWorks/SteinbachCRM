using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteinbachCRM.Tools
{
    public class TransferTimespan
    {
        public DateTime VonDatum { get; set; }
        public DateTime BisDatum { get; set; }

        public TransferTimespan(DateTime VonDatum,DateTime BisDatum)
        {
            this.VonDatum = VonDatum;
            this.BisDatum = BisDatum;

        }

    }
}
