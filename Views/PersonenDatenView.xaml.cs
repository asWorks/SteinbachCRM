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

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for FirmenDatenView.xaml
    /// </summary>
    public partial class PersonenDatenView : UserControl
    {

      
        private CollectionViewSource EintraegeAnredeLookupViewSource;
        private CollectionViewSource EintraegeBetreuerLookupViewSource;
        private CollectionViewSource EintraegeGebietLookupViewSource;
      


        SteinbachEntities db;

        public PersonenDatenView()
        {
            InitializeComponent();
            db = new SteinbachEntities();
           

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vModel = (PersonenDatenViewModel)this.DataContext;
            //this.CurrentPerson.DataContext = vModel.CurrentPerson;

            //this.Personenliste.DataContext = vModel.Personenliste;
            //this.CurrentAdresse.DataContext = vModel.CurrentAdresse;

            ObservableCollection<AuswahlEintraege>  ae = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "Gebiet"), db);
            Console.WriteLine(ae.Count());
            //EintraegeLookupViewSource = ((CollectionViewSource)(this.FindResource("EintraegeLookup")));
            //EintraegeLookupViewSource.Source = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "Status"), db);

            EintraegeAnredeLookupViewSource = ((CollectionViewSource)(this.FindResource("EintraegeAnredeLookup")));
            EintraegeAnredeLookupViewSource.Source = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "Anrede"), db);

            EintraegeBetreuerLookupViewSource = ((CollectionViewSource)(this.FindResource("EintraegeBetreuerLookup")));
            EintraegeBetreuerLookupViewSource.Source = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "Betreuer"), db);

            EintraegeGebietLookupViewSource = ((CollectionViewSource)(this.FindResource("EintraegeGebietLookup")));
            EintraegeGebietLookupViewSource.Source = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(i => i.Gruppe == "Gebiet"), db);

           

          
        }


        private void Personenliste_GotFocus(object sender, EventArgs e)
        {

            //var item = (ListViewItem)sender;
            //this.Personenliste.SelectedItem = item.DataContext;


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

     



    }
}
