using System;
using System.Collections;
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
using DAL;

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for SI_EventsView.xaml
    /// </summary>
    public partial class SI_EventsView : UserControl
    {
        ViewModels.SI_EventsViewModel vModel;
        CollectionViewSource FirmenViewSource;
        CollectionViewSource FirmenFullViewSource;
        SteinbachEntities db;

        public SI_EventsView()
        {
            InitializeComponent();
            db = new SteinbachEntities();



        }

        private void LoadData()
        {

            //vModel = (ViewModels.SI_EventsViewModel)this.DataContext;

            FirmenViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("FirmenViewSource")));
            //FirmenViewSource.Source = vModel.Firmen;

            FirmenFullViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("FirmenFullViewSource")));
            //FirmenFullViewSource.Source = vModel.FirmenFull;

           
                FirmenFullViewSource.Source = db.firmen.Where(n=>n.IstKunde == 1).OrderByDescending(n=>n.KdNr);
          



        }

        private IEnumerable fcbFirmen_OnFCBChangedFunc(DataTypes.FilteredComboBoxChangedEventArgs arg)
        {
            int res = 0;
            if (int.TryParse(arg.filter, out res) == true)
            {


                return null;
                // return db.lagerlisten.Where(n => n.artikelnr.Contains(arg.filter));

            }
            else
            {
                //  return db.lagerlisten.Where(n => n.bezeichnung.Contains(arg.filter));
            }

            return null;
        }

      

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private IEnumerable fcbFirma_OnFCBChangedFunc(DataTypes.FilteredComboBoxChangedEventArgs arg)
        {
           
                if (arg.filter != string.Empty)
                {
                    return db.firmen.Where(n => n.name.Contains(arg.filter) && n.IstKunde == 1);
                }
                else
                {
                    return  db.firmen.Where(n=>n.name.StartsWith("XXX"));
                }

           
        }

        private void DeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            
            Button btn = sender as Button;
            int index = (btn.Parent as C1.WPF.DataGrid.DataGridCellPresenter).Row.Index;
            this.EventsGrid.SelectedIndex = index;
        }


    }
}
