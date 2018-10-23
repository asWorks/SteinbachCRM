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
    public partial class SI_MailingSelectionView : UserControl
    {
        //   ViewModels.SI_EventsViewModel vModel;
        CollectionViewSource FirmenViewSource;
        CollectionViewSource FirmenFullViewSource;
        SteinbachEntities db;

        public SI_MailingSelectionView()
        {
            InitializeComponent();
            db = new SteinbachEntities();



        }

        private void LoadData()
        {

            var vModel = (ViewModels.SI_MailingSelectionViewModel)this.DataContext;
            vModel.ChangeVisibility += vModel_ChangeVisibility;
            ListboxSelectedEventsVM.DataContext = vModel.ListboxSelectedEventsVM;
            ListboxSelectedKategorienVM.DataContext = vModel.ListboxSelectedKategorienVM;
            ListboxSelectedTypFirmaEigenschaftenVM.DataContext = vModel.ListboxSelectedTypFirmaEigenschaftenVM;
            vModel.ThisDispatcher = this.Dispatcher;




        }

        void vModel_ChangeVisibility(string arg1, bool arg2)
        {

            string[] columns = arg1.Split(',');

            foreach (string item in columns)
            {
                int buf = 0;
                if (int.TryParse(item, out buf))
                {

                    if (arg2)
                    {
                        this.KontakteGrig.Columns[buf].Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        this.KontakteGrig.Columns[buf].Visibility = System.Windows.Visibility.Visible;
                    }
                }

            }



        }





        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
          
        }




    }
}
