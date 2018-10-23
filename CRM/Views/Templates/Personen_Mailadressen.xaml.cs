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
    /// Interaction logic for Firmen_Adressen.xaml
    /// </summary>
    public partial class Personen_Mailadressen : UserControl
    {
        CollectionViewSource EintraegeTypMailadresseViewSource;
        private CollectionViewSource StandortLookupViewSource;
        //LinkViewsource<UserControl> Eintraege;
        SteinbachEntities db;

        public Personen_Mailadressen()
        {
            InitializeComponent();
            db = new SteinbachEntities();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

          
            EintraegeTypMailadresseViewSource = ((CollectionViewSource)(this.FindResource("EintraegeTypMailadresseLookup")));
            var query = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(g => g.Gruppe == "TypMailadresse"), db);
            EintraegeTypMailadresseViewSource.Source = query;
            //StandortLookupViewSource = ((CollectionViewSource)(this.FindResource("StandortLookup")));
           // StandortLookupViewSource.Source = new ObservableCollection<DAL.Firmen_Adressen>(db.Firmen_Adressen.Where(i => i.id_Firma == vModel.id_Firma));
            //Eintraege = new LinkViewsource<UserControl>(this, query, "EintraegeTypMailadresseLookup");

            //if (DataContext.GetType() == typeof(DAL.Firmen_Mailadressen))
            //{
            //    var vModel = (DAL.Firmen_Mailadressen)this.DataContext;
            //    StandortLookupViewSource.Source = new ObservableCollection<DAL.Firmen_Adressen>(db.Firmen_Adressen.Where(i => i.id_Firma == vModel.id_Firma));
            //}
            //else
            //{
            //    var vModel = (DAL.Personen_Mailadressen)this.DataContext;
            //    //StandortLookupViewSource.Source = new ObservableCollection<DAL.Firmen_Adressen>(db.Firmen_Adressen.Where(i => i.id_Firma == vModel.id_Firma));
            //}

        }











    }
}
