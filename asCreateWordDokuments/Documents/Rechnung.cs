using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace asCreateWordDokuments.Documents
{
    public class Rechnung:Dokument
    {
        public  Rechnung():base()
        {
            DocType = @"\Rechnungen";
            BuildPaths();
            

        }


    }
}
