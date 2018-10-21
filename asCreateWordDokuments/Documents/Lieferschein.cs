using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace asCreateWordDokuments.Documents
{
    public class Lieferschein:Dokument
    {
        public  Lieferschein():base()
        {
            DocType = @"\Lieferscheine";
            BuildPaths();
            

        }


    }
}
