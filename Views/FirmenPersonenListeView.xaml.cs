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
using CommonTools.Tools;
using DAL;
using SteinbachCRM.ObjectCollections;
using System.Collections.ObjectModel;

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for FirmenPersonenNeuView.xaml
    /// </summary>
    public partial class FirmenPersonenListeView : UserControl
    {
        LinkViewsource<UserControl> TypTelefonLookup;
        LinkViewsource<UserControl> TypMailadresseLookup;
        LinkViewsource<UserControl> AnredeLookup;
        LinkViewsource<UserControl> GebietLookup;
        LinkViewsource<UserControl> BetreuerLookup;
        LinkViewsource<UserControl> StandortLookup;

        //LinkViewsource<UserControl> PersonenLookup;


        SteinbachEntities db;


        public FirmenPersonenListeView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            db = new SteinbachEntities();
            var vModel = (ViewModels.FirmenPersonenListeViewModel)this.DataContext;
            int firmenID = 0;
            if (vModel.CurrentFirma != null)
            {
                firmenID = vModel.CurrentFirma.id;
            }


            var TypTelefon = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(x => x.Gruppe == "TypTelefon"), db);
            TypTelefonLookup = new LinkViewsource<UserControl>(this, TypTelefon, "EintraegeTypTelefonLookup");

            var TypMailadresse = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(x => x.Gruppe == "TypMailadresse"), db);
            TypMailadresseLookup = new LinkViewsource<UserControl>(this, TypMailadresse, "EintraegeTypMailadresseLookup");

            var anrede = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(x => x.Gruppe == "Anrede"), db);
            AnredeLookup = new LinkViewsource<UserControl>(this, anrede, "EintraegeAnredeLookup");

            var gebiet = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(x => x.Gruppe == "Gebiet"), db);
            GebietLookup = new LinkViewsource<UserControl>(this, gebiet, "EintraegeGebietLookup");

            var standort = new ObservableCollection<Firmen_Adressen>(db.Firmen_Adressen.Where(x => x.id_Firma == firmenID));
            StandortLookup = new LinkViewsource<UserControl>(this, standort, "EintraegeStandortLookup");


            var betreuer = new PersonCollection(db.personen.Where(g => g.ListeKontakteAktiv == 1), db);
            BetreuerLookup = new LinkViewsource<UserControl>(this, betreuer, "EintraegeBetreuerLookup");



            //var Personen = new Firmen_PersonenCollection(db.Firmen_Personen.Where(p => p.id_Firma == 14), db);
            //PersonenLookup = new LinkViewsource<UserControl>(this, Personen, "PersonenListeCVS");


        }

        //private void HideColumns_Click(object sender, RoutedEventArgs e)
        //{
        //    Visibility temp;
        //    if (this.txtVorname2.Visibility == Visibility.Collapsed)
        //    {
        //        temp = Visibility.Visible;
        //    }
        //    else
        //    {
        //        temp = Visibility.Collapsed;
        //    }


        //    txtVorname2.Visibility = temp;
        //    txtTitel.Visibility = temp;
        //    txtNamenszusatz.Visibility = temp;
        //    txtBetreuer.Visibility = temp;
        //    MemoMail.Visibility = temp;
        //    MemoTelefon.Visibility = temp;


        //}


    }
}
