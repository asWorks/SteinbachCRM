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

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for Kundendaten.xaml
    /// </summary>
    public partial class Kundendaten : Page
    {
        LinkViewsourcePage FirmaVS;
        LinkBindingListViewsourcePage PersonVS;
        LinkBindingListViewsourcePage TelefonVS;
        LinkBindingListViewsourcePage TelefonFirmenVS;
        SteinbachEntities db;
        Firmen_Personen CurrentPerson;


        public Kundendaten()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            db = new SteinbachEntities();

            var so = new ObjectCollections.FirmenCollection(db.firmen.Where(f => f.id == 23), db);
            FirmaVS = new LinkViewsourcePage(this, so, "FirmaViewSource");
            TelefonVS = new LinkBindingListViewsourcePage(this, "PhoneViewSource");
            PersonVS = new LinkBindingListViewsourcePage(this, "PersonViewSource");
            TelefonFirmenVS = new LinkBindingListViewsourcePage(this, "PhoneFirmenViewSource");


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }

        private void c1GridUnterartikel_DeletingRows(object sender, C1.WPF.DataGrid.DataGridDeletingRowsEventArgs e)
        {
            if (MessageBox.Show(string.Format("Möchten Sie die Bewegung {0} wirklich löschen ?", string.Join(",", e.DeletedRows.Select(row => (row.DataItem as Firmen_Telefon).id).ToArray())), "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                try
                {
                    foreach (var item in e.DeletedRows.Select(row => (row.DataItem as Firmen_Telefon)).ToArray())
                    {


                    }
                }
                catch (Exception)
                {


                }


            };
        }

        

        private void c1GridUnterartikel_RowsDeleted(object sender, C1.WPF.DataGrid.DataGridRowsDeletedEventArgs e)
        {
            try
            {
                foreach (var item in e.DeletedRows.Select(row => (row.DataItem as Firmen_Telefon)).ToArray())
                {
                    Firmen_Telefon k = db.Firmen_Telefon.Where(p => p.id == item.id).Single();

                    db.DeleteObject(k);
                    db.SaveChanges();

                }
            }
            catch (Exception)
            {


            }

        }

        private void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            var p = (Firmen_Personen)PersonVS.View.AddNew();
            //var f = db.firmen.Where(t => t.id == 23).SingleOrDefault();
            //p.firma.Add(f);
            //p.Nachname = "Neu . . "; 
            PersonVS.View.CommitNew();
        }

        private void AddPersonFirma_Click(object sender, RoutedEventArgs e)
        {
            TelefonFirmenVS.View.AddNew();
            TelefonFirmenVS.View.CommitNew();
        }

        private void c1GridPersonen_SelectionChanged(object sender, C1.WPF.DataGrid.DataGridSelectionChangedEventArgs e)
        {
            CurrentPerson = (Firmen_Personen)PersonVS.View.CurrentItem;

        }

        private void c1GridUnterartikel_SelectionChanged(object sender, C1.WPF.DataGrid.DataGridSelectionChangedEventArgs e)
        {
            //var Telefon = (Firmen_Telefon)TelefonVS.View.CurrentItem;

            //MessageBox.Show(Telefon.Telefonnummer);


        }

        private void AddPersonTelefon_Click(object sender, RoutedEventArgs e)
        {
            //c1GridUnterartikel.BeginNewRow();
            Firmen_Telefon kt = (Firmen_Telefon)TelefonVS.View.AddNew();
            
            //TelefonVS.View.CommitNew();
            //var Liste = new List<Firmen_Personen>();
            //foreach (var item in kt.Firmen_Personen)
            //{
            //    Liste.Add(item);
            //}

            //foreach (var item in Liste)
            //{
            //    kt.Firmen_Personen.Remove(item);
            //}

            //kt.Firmen_Personen.Add(CurrentPerson);

        }
    }
}
