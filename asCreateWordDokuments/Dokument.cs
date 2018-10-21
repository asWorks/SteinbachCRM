using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asCreateWordDokuments.Pages;

namespace asCreateWordDokuments
{
    public class Dokument
    {
        public List<Page> Sheets { get; set; }
        public string DocumentsPath { get; set; }
        public string DocumentTypePath { get; set; }
        public string DocumentTemplate { get; set; }
        public string FilesPath { get; set; }
        public string DocType { get; set; }
        public string RechnungsNummer { get; set; }
        public bool Speichern { get; set; }
        public bool SofortDruck { get; set; }
        public int AnzahlDruckKopien { get; set; }
        public bool isVisible { get; set; }

        public Dokument()
        {
            Sheets = new List<Pages.Page>();
            Speichern = Convert.ToBoolean( CommonTools.Tools.ConfigEntry<int>.GetConfigEntry("DokumenteSpeichern", "1"));
            SofortDruck = Convert.ToBoolean(CommonTools.Tools.ConfigEntry<int>.GetConfigEntry("SofortDruck", "0"));
            isVisible = Convert.ToBoolean(CommonTools.Tools.ConfigEntry<int>.GetConfigEntry("WordDocumentVisible", "1"));

            AnzahlDruckKopien = 1;
            
        }

        public void AddSheet(Pages.Page NewSheet)
        {
            Sheets.Add(NewSheet);
        }

        public void reset()
        {
            Sheets.Clear();
        }

        public void BuildPaths()
        {
             
            string root = CommonTools.Tools.ConfigEntry<string>.GetConfigEntry("RootDokumente",@"E:\test\Dokumente");
            DocumentsPath = String.Format("{0}{1}\\Dokumente", root, DocType);
            DocumentTypePath = String.Format("{0}{1}", root, DocType);
            FilesPath = String.Format("{0}{1}\\Files", root, DocType);
            var dir = new System.IO.DirectoryInfo(DocumentTypePath);
            var fi = dir.GetFiles("*.dotx");
            if (fi.Count() > 0)
            {
                System.IO.FileInfo f = fi[0];
                DocumentTemplate = f.FullName;
            }
            else
            {
                DocumentTemplate = string.Empty;

            }


        }
    }
}
