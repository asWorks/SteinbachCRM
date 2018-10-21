using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asCreateWordDokuments.Pages;

namespace asCreateWordDokuments
{
    public class CreateDoc : IDisposable
    {

        // Objekt von Missing mit NULL Wert
        private Object oMissing; // = System.Reflection.Missing.Value;

        // Objekt von TRUE und FALSE
        //private static Object oTrue = true;
        //private static Object oFalse = false;

        // Objekt erstellen von Word und von einem Dokument
        private Microsoft.Office.Interop.Word.Application oWord;
        Microsoft.Office.Interop.Word.Document oWordDoc;
        private int DruckPause = 0;
        bool disposed = false;
        public event Action<string> DoMessage;



        public CreateDoc()
        {
            try
            {
                oMissing = System.Reflection.Missing.Value;
                oWord = new Microsoft.Office.Interop.Word.Application();
                //  DruckPause = Session.PauseVorDruckMS;
            }
            catch (Exception)
            {
                throw;
            }

        }




        public bool run(Dokument dokument)
        {
            try
            {
                int i = 1;
                //Microsoft.Office.Interop.Word.Document oWordDoc = new Microsoft.Office.Interop.Word.Document();
                oWordDoc = new Microsoft.Office.Interop.Word.Document();
                string WordFilename = String.Format("{0}\\{1}.doc", dokument.DocumentsPath, dokument.RechnungsNummer);
                oWord.Visible = dokument.isVisible;
                Object oTemplatePath = null;
                if (dokument.DocumentTemplate != string.Empty)
                {
                    oTemplatePath = dokument.DocumentTemplate;
                }
                else
                {
                    DoMessage("Es wurde keine Dokumentenvorlage gefunden");
                }


                oWordDoc = oWord.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing);
                var objSelection = oWord.Selection;



                foreach (Page s in dokument.Sheets)
                {

                    objSelection.TypeText(s.Text);
                    if (i < dokument.Sheets.Count)
                    {
                        objSelection.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);

                    }

                    i++;
                }

                if (dokument.Speichern)
                {
                    oWordDoc.SaveAs(WordFilename);
                }

                System.Threading.Thread.Sleep(DruckPause);

                if (dokument.SofortDruck)
                {


                    DoMessage("Drucken Dokument :" + dokument.RechnungsNummer);
                    oWordDoc.PrintOut(oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing
                                       , dokument.AnzahlDruckKopien, oMissing, oMissing, oMissing, oMissing
                                       , oMissing, oMissing, oMissing, oMissing, oMissing);


                }


                //oWordDoc.Close();
                //oWordDoc = null;
                return true;

            }

            catch (Exception)
            {
                return false;
                throw;
            }


        }

        public void Close()
        {

            Dispose();
        }

        #region "Disposed"


        public void Dispose()
        {
            Dispose(true);
            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            try
            {
                if (!this.disposed)
                {
                    // If disposing equals true, dispose all managed 
                    // and unmanaged resources.
                    if (disposing)
                    {
                        // Dispose managed resources.


                    }
                    oWordDoc.Close();
                    oWordDoc = null;
                    oWord.Quit(true);
                    oWord = null;
                    // Release unmanaged resources. If disposing is false, 
                    // only the following code is executed.

                    // Note that this is not thread safe.
                    // Another thread could start disposing the object
                    // after the managed resources are disposed,
                    // but before the disposed flag is set to true.
                    // If thread safety is necessary, it must be
                    // implemented by the client.

                }
                disposed = true;
            }
            catch (Exception)
            {

                
            }


        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method 
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~CreateDoc()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion



    }
}
