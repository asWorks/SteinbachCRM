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
using SteinbachCRM.ViewModels;

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for TermineListe.xaml
    /// </summary>
    public partial class TermineListeView : UserControl
    {
        SteinbachEntities db;
        LinkBindingListViewsource<UserControl, BindingListCollectionView> Firmenlookup;
        LinkBindingListViewsource<UserControl, BindingListCollectionView> Actionlookup;
        LinkBindingListViewsource<UserControl, BindingListCollectionView> Angelegtlookup;
        public TermineListeView()
        {
            InitializeComponent();
            db = new SteinbachEntities();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vModel = (TermineListeViewModel)this.DataContext;
            

            Firmenlookup = new LinkBindingListViewsource<UserControl, BindingListCollectionView>(this, db.firmen.Where(a => a.IstKunde == 1), "FirmenLookup");
            Actionlookup = new LinkBindingListViewsource<UserControl, BindingListCollectionView>(this, db.AuswahlEintraege.Where(a => a.Gruppe == "TerminAktion"), "ActionLookup");
            Angelegtlookup = new LinkBindingListViewsource<UserControl, BindingListCollectionView>(this, db.personen, "AngelegtLookup");
           
            //DatumVon.DateTime = DateTime.Now;
            //DatumBis.DateTime = DatumVon.DateTime.Value.AddDays(7);
        }

        private void DatumVon_DateTimeChanged(object sender, C1.WPF.DateTimeEditors.NullablePropertyChangedEventArgs<DateTime> e)
        {

           
            //if (DatumBis.DateTime < DatumVon.DateTime)
            //{
            //     DatumBis.DateTime = DatumVon.DateTime.Value.AddDays(7);
            //}
        }

     
    }
}
