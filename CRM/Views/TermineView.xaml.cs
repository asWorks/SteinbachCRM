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
using System.Collections.ObjectModel;
using CommonTools.Tools;

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for FirmenDatenView.xaml
    /// </summary>
    public partial class TermineView : UserControl
    {
        //<CollectionViewSource x:Key="EintraegeAktionenLookup" />
        //<CollectionViewSource x:Key="EintraegeStatusLookup" />
        //<CollectionViewSource x:Key="EintraegeErinnerungLookup" />
        //<CollectionViewSource x:Key="EintraegePrioritaetLookup" />
        //<CollectionViewSource x:Key="SI_UserLookup" />
        //<CollectionViewSource x:Key="FirmenLookup" />
        //<CollectionViewSource x:Key="EintraegeGebietLookup" />
        //<CollectionViewSource x:Key="EintraegeTeamLookup" />
      
        //private CollectionViewSource EintraegeAnredeLookupViewSource;
        //private CollectionViewSource SI_UserLookupViewSource;
        //private CollectionViewSource EintraegeGebietLookupViewSource;
        LinkBindingListViewsource<UserControl,BindingListCollectionView> EintraegeAktionenlookup;
        LinkBindingListViewsource<UserControl,BindingListCollectionView> EintraegeStatuslookup;
        LinkBindingListViewsource<UserControl,BindingListCollectionView> EintraegeErinnerunglookup;
        LinkBindingListViewsource<UserControl,BindingListCollectionView> EintraegePrioritaetlookup;
        LinkBindingListViewsource<UserControl,BindingListCollectionView> EintraegeSI_Userlookup;
        LinkBindingListViewsource<UserControl,BindingListCollectionView> Firmenlookup;
        LinkBindingListViewsource<UserControl,BindingListCollectionView> EintraegeGebietlookup;
        LinkBindingListViewsource<UserControl,BindingListCollectionView> EintraegeTeamlookup;


        SteinbachEntities db;

        public TermineView()
        {
            InitializeComponent();
            db = new SteinbachEntities();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vModel = (TermineViewModel)this.DataContext;
            this.CurrentTermin.DataContext = vModel.CurrentTermin;
            this.ListboxTeilnehmerSI.DataContext = vModel.ListboxTeilnehmerSI;
            this.ListboxKontakte.DataContext = vModel.ListboxTeilnehmerExtern;

            ////this.CurrentAdresse.DataContext = vModel.CurrentAdresse;


            EintraegeAktionenlookup = new LinkBindingListViewsource<UserControl,BindingListCollectionView>(this, db.AuswahlEintraege.Where(a => a.Gruppe == "TerminAktion"), "EintraegeAktionenLookup");
            EintraegeStatuslookup = new LinkBindingListViewsource<UserControl,BindingListCollectionView>(this, db.AuswahlEintraege.Where(a => a.Gruppe == "TerminStatus"), "EintraegeStatusLookup");
            EintraegeErinnerunglookup = new LinkBindingListViewsource<UserControl,BindingListCollectionView>(this, db.AuswahlEintraege.Where(a => a.Gruppe == "TerminErinnerung"), "EintraegeErinnerungLookup");
            EintraegePrioritaetlookup = new LinkBindingListViewsource<UserControl,BindingListCollectionView>(this, db.AuswahlEintraege.Where(a => a.Gruppe == "TerminPrioritaet"), "EintraegePrioritaetLookup");
            EintraegeSI_Userlookup = new LinkBindingListViewsource<UserControl,BindingListCollectionView>(this, db.personen, "SI_UserLookup");
            EintraegeGebietlookup = new LinkBindingListViewsource<UserControl,BindingListCollectionView>(this, db.AuswahlEintraege.Where(a => a.Gruppe == "Gebiet"), "EintraegeGebietLookup");
            EintraegeTeamlookup = new LinkBindingListViewsource<UserControl,BindingListCollectionView>(this, db.AuswahlEintraege.Where(a => a.Gruppe == "TerminTeam"), "EintraegeTeamLookup");

            Firmenlookup = new LinkBindingListViewsource<UserControl, BindingListCollectionView>(this, db.firmen.Where(a => a.IstKunde == 1), "FirmenLookup");

           

          
        }
    }
}
