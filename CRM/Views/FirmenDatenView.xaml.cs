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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SteinbachCRM.ViewModels;
using SteinbachCRM.ObjectCollections;
using DAL;
using CommonTools.Tools;

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for FirmenDatenView.xaml
    /// </summary>
    public partial class FirmenDatenView : UserControl
    {

        private CollectionViewSource EintraegeLookupViewSource;
        private CollectionViewSource EintraegeQuelleLookupViewSource;
        private CollectionViewSource EintraegeTypLookupViewSource;
        private CollectionViewSource EintraegeGebietLookupViewSource;
        private CollectionViewSource EintraegeLandLookupViewSource;
        private CollectionViewSource AdressenLookupViewSource;
        LinkViewsource<UserControl> BetreuerFirmaLookup;
        LinkViewsource<UserControl> ListKategorienLookup;


        SteinbachEntities db;

        public FirmenDatenView()
        {
            InitializeComponent();
            db = new SteinbachEntities();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vModel = (FirmenDatenViewModel)this.DataContext;
            this.CurrentFirma.DataContext = vModel.CurrentFirma;
            //this.ListKategorien.DataContext = vModel;
            //this.ListboxKategorien.DataContext = vModel.ListboxKategorien;
            this.ListboxEigenschaften.DataContext = vModel.ListboxEigenschaften;

           // this.lbEmployees.DataContext = vModel;
           // this.textBlock1.DataContext = vModel;
            //this.CurrentAdresse.DataContext = vModel;


            EintraegeLookupViewSource = ((CollectionViewSource)(this.FindResource("EintraegeLookup")));
            EintraegeLookupViewSource.Source = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "Status"), db);

            EintraegeQuelleLookupViewSource = ((CollectionViewSource)(this.FindResource("EintraegeQuelleLookup")));
            EintraegeQuelleLookupViewSource.Source = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "Quelle"), db);

            EintraegeTypLookupViewSource = ((CollectionViewSource)(this.FindResource("EintraegeTypLookup")));
            EintraegeTypLookupViewSource.Source = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "TypFirma"), db);

            EintraegeGebietLookupViewSource = ((CollectionViewSource)(this.FindResource("EintraegeGebietLookup")));
            EintraegeGebietLookupViewSource.Source = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "Gebiet"), db);

            //EintraegeLandLookupViewSource = ((CollectionViewSource)(this.FindResource("EintraegeLandLookup")));
            //EintraegeLandLookupViewSource.Source = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "Land"), db);

            //AdressenLookupViewSource = ((CollectionViewSource)(this.FindResource("AdressenLookup")));
            //AdressenLookupViewSource.Source = new AdressenCollection(db.Firmen_Adressen.Where(i => i.id_Firma == 1563), db);

            //var query = new PersonCollection(db.personen.Where(g => g.ListeBetreuerFirmen == 1), db);
            //BetreuerFirmaLookup = new LinkViewsource<UserControl>(this, query, "BetreuerFirmaLookup");

            //var kat = new FirmenCollection(db.firmen.Where(f => f.istFirma == 1),db);
            //ListKategorienLookup = new LinkViewsource<UserControl>(this, kat,  "ListKategorienLookup");
          
         
            
            

        }

        private void CurrentAdressex_GotFocus(object sender, EventArgs e)
        {

            var item = (ListViewItem)sender;
            this.CurrentAdressex.SelectedItem = item.DataContext;


        }

        private void CurrentTelefon_GotFocus(object sender, EventArgs e)
        {

            var item = (ListViewItem)sender;
            this.CurrentTelefon.SelectedItem = item.DataContext;


        }

        private void CurrentMailadresse_GotFocus(object sender, EventArgs e)
        {

            var item = (ListViewItem)sender;
            this.CurrentMailadresse.SelectedItem = item.DataContext;

        }

        //private void ListKategorien_GotFocus(object sender, EventArgs e)
        //{

        //    var item = (ListViewItem)sender;
        //    this.ListKategorien.SelectedItem = item.DataContext;

        //}

        private void Speichern_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();

        }

     

      

     

     

       
     

    }
}
