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
using DAL;
using SteinbachCRM.ObjectCollections;
using CommonTools.Tools;
using System.Data.Entity;

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for KundendatenView.xaml
    /// </summary>
    public partial class KundendatenView : UserControl
    {
        SteinbachEntities db;
        LinkViewsource<UserControl> Eintraege;

        public KundendatenView()
        {
            InitializeComponent();
            db = new SteinbachEntities();

        }

        void vModel_EventFirmaChanged()
        {
            tabFirmendaten.Focus();

        }




        //private void tabFirmendaten_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    Console.WriteLine("LostFocus");
        //}

        //private void tabFirmendaten_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    Console.WriteLine("GotFocus");
        //}

        //private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    Console.WriteLine("SelectionChanged");
        //}

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            var query = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(g => g.Gruppe == "Land"), db);
            Eintraege = new LinkViewsource<UserControl>(this, query, "PersonTyp");
            var vModel = (KundendatenViewModel)this.DataContext;
            vModel.EventFirmaChanged += () => vModel_EventFirmaChanged();
            this.tabFirmendaten.DataContext = vModel;

          // new KundendatenViewModel.FirmaChanged(vModel_EventFirmaChanged);

        }

     

        //private void SelectedAlleFirmen_KeyDown(object sender, KeyEventArgs e)
        //{
        //    using (var db = new SteinbachEntities())
        //    {
        //        if (e.Key == Key.Return)
        //        {
        //            SelectedAlleFirmen.ItemsSource = db.firmen.Where(f=>f.name.Contains(SelectedAlleFirmen.Text) && f.IstKunde == 1);
                                          
        //            SelectedAlleFirmen.DisplayMemberPath = "name";
        //            SelectedAlleFirmen.SelectedValue = "id";
        //            BindingExpression exp = this.SelectedAlleFirmen.GetBindingExpression(ComboBox.SelectedItemProperty);
        //            exp.UpdateSource();
        //            SelectedAlleFirmen.IsDropDownOpen = true;

        //        }
        //    }
        //}

        //private void SelectedAlleFirmen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void SelectedAlleFirmen_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{

        //}

        //private void SelectedAlleFirmen_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    //if (e.Key != Key.Return)
        //    //{
        //    //    e.Handled = true; 
        //    //}
        //}

        //private void SelectedAlleFirmen_PreviewKeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key != Key.Return)
        //    {
        //        e.Handled = true;
        //    }
        //}

     

        //private void DataGridHyperlinkColumn_Click(object sender, C1.WPF.DataGrid.DataGridRowEventArgs e)
        //{
        //    Console.WriteLine("Hyperlink_Clicked");
        //}

        //private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Kundendaten unloaded");
        //}






    }
}
