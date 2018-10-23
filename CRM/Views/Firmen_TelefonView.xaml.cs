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

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for Firmen_TelefonView.xaml
    /// </summary>
    public partial class Firmen_TelefonView : UserControl
    {

        LinkViewsource<UserControl> Eintraege;
        private CollectionViewSource StandortLookupViewSource;
        SteinbachEntities db;



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            

            var query = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(g => g.Gruppe == "TypTelefon"), db);

            Eintraege = new LinkViewsource<UserControl>(this, query, "EintraegeTelefonTypLookup");
            StandortLookupViewSource = ((CollectionViewSource)(this.FindResource("StandortLookup")));
            if (DataContext.GetType() == typeof(Firmen_Telefon))
            {
                var vModel = (DAL.Firmen_Telefon)this.DataContext;
                StandortLookupViewSource.Source = new ObservableCollection<DAL.Firmen_Adressen>(db.Firmen_Adressen.Where(i => i.id_Firma == vModel.id_Firma));
            }
            else
            {
                var vModel = (ViewModels.Firmen_TelefonViewModel)this.DataContext;
                StandortLookupViewSource.Source = new ObservableCollection<DAL.Firmen_Adressen>(db.Firmen_Adressen.Where(i => i.id_Firma == vModel.id_Firma));
            }



            // StandortLookupViewSource.Source = new ObservableCollection<DAL.Firmen_Adressen>(db.Firmen_Adressen.Where(i => i.id_Firma == vModel.id_Firma));

        }


        public Firmen_TelefonView()
        {
            InitializeComponent();
            db = new SteinbachEntities();

        }
    }
}
