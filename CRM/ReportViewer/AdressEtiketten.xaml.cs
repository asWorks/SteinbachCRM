using CommonTools.Tools;
using GrapeCity.ActiveReports.Document;
using SteinbachCRM.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SteinbachCRM.ReportViewer
{
    /// <summary>
    /// Interaction logic for AfressEtiketten.xaml
    /// </summary>
    public partial class AdressEtiketten : Window
    {
        IEnumerable<Models.SelectedAdressesModel> AdressLabels;

        public AdressEtiketten(IEnumerable<Models.SelectedAdressesModel> SelectedLabels)
        {
            AdressLabels = SelectedLabels;
            viewer1 = new GrapeCity.ActiveReports.Viewer.Wpf.Viewer();
            InitializeComponent();
            ShowLabels();
        }



        private void ShowLabels()
        {

            PageDocument pageDocument = null;

            pageDocument = ActiveReportsTools<int>.GetDocument(StaticMethods.GetPath(@"Etiketten\AdressEtikett.rdlx"));
            pageDocument.LocateDataSource += pageDocument_LocateDataSource;
            viewer1.LoadDocument(pageDocument);

        }

        void pageDocument_LocateDataSource(object sender, GrapeCity.ActiveReports.LocateDataSourceEventArgs args)
        {
            string AnredeWennLeer = CommonTools.Tools.ConfigEntry<string>.GetConfigEntry("AnredeWennLeer", "Herrn/Frau", "Anrede in Sertienbrief oder Adressetikett");
            //GetAnredeWennLeer(AnredeWennLeer,p.Anrede)
            var query = from p in AdressLabels
                        select new
                               {
                                   id = p.id,
                                   Firmenname = p.Firmenname,
                                   Nachname = p.Nachname,
                                   Vorname = p.Vorname,
                                   Vorname2 = p.Vorname2,
                                   Anrede = p.Anrede,
                                   Anrede1=p.Anrede1,
                                   Titel = p.Titel,
                                   Land = p.Land,
                                   Ort = p.Ort,
                                   Straße = p.Straße,
                                   PLZ = p.PLZ
                               };
      
            args.Data = query.ToList();
        }

        // Anrede = GetAnredeWennLeer(AnredeWennLeer,p.Anrede)
        private string GetAnredeWennLeer(string AnredeWennLeer, string item)
        {


            string Anrede = string.Empty;
            if (string.IsNullOrEmpty(item) || item.Trim() == "")
            {
                Anrede = AnredeWennLeer;
            }
            else
            {
                if (item.Contains("Herr"))
                {
                    Anrede = item.Replace("Herr", "Herrn");
                }
                else
                {
                    Anrede = item;
                }
            }
            return Anrede;
        }
    }
}
