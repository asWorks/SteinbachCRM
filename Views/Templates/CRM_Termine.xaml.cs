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

namespace SteinbachCRM.Templates
{
    /// <summary>
    /// Interaction logic for FirmenDatenView.xaml
    /// </summary>
    public partial class CRM_Termine : UserControl
    {

      
        private CollectionViewSource EintraegeAnredeLookupViewSource;
        private CollectionViewSource SI_UserLookupViewSource;
        private CollectionViewSource EintraegeGebietLookupViewSource;
      


        SteinbachEntities db;

        public CRM_Termine()
        {
            InitializeComponent();
            db = new SteinbachEntities();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //var vModel = (PersonenDatenViewModel)this.DataContext;
            //this.CurrentPerson.DataContext = vModel.CurrentPerson;
            ////this.CurrentAdresse.DataContext = vModel.CurrentAdresse;

            ObservableCollection<AuswahlEintraege>  ae = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "Gebiet"), db);
            Console.WriteLine(ae.Count());
            //EintraegeLookupViewSource = ((CollectionViewSource)(this.FindResource("EintraegeLookup")));
            //EintraegeLookupViewSource.Source = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "Status"), db);

            EintraegeAnredeLookupViewSource = ((CollectionViewSource)(this.FindResource("EintraegeAnredeLookup")));
            EintraegeAnredeLookupViewSource.Source = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "Anrede"), db);

            SI_UserLookupViewSource = ((CollectionViewSource)(this.FindResource("SI_UserLookup")));
            SI_UserLookupViewSource.Source = new PersonCollection(db.personen.Where(i => i.ListeKontakteAktiv == 1), db);

            EintraegeGebietLookupViewSource = ((CollectionViewSource)(this.FindResource("EintraegeGebietLookup")));
            EintraegeGebietLookupViewSource.Source = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "Gebiet"), db);

           

          
        }
    }
}
