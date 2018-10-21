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
using SteinbachCRM.ObjectCollections;
using System.Collections;
using DAL;
using System.Collections.ObjectModel;

namespace SteinbachCRM.Templates
{
    /// <summary>
    /// Interaction logic for Kunden_Adressen.xaml
    /// </summary>
    public partial class Firmen_Telefon : UserControl
    {

        LinkViewsource<UserControl> Eintraege;
        private CollectionViewSource StandortLookupViewSource;

        SteinbachEntities db;

        public Firmen_Telefon()
        {
            InitializeComponent();
            db = new SteinbachEntities();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vModel = (DAL.Firmen_Telefon)this.DataContext;
            var query = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(g => g.Gruppe == "TypTelefon"), db);

            Eintraege = new LinkViewsource<UserControl>(this, query, "EintraegeTelefonTypLookup");
            StandortLookupViewSource = ((CollectionViewSource)(this.FindResource("StandortLookup")));
            StandortLookupViewSource.Source = new ObservableCollection<DAL.Firmen_Adressen>(db.Firmen_Adressen.Where(i => i.id_Firma == vModel.id_Firma));
            
        }











    }
}
