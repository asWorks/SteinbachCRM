using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DAL;
using CommonTools.Views;
using CommonTools.Extensions;


using System.Windows.Media;
using SteinbachCRM.Views;

namespace SteinbachCRM
{
    /// <summary>
    /// Interaction logic for Navigation.xaml
    /// </summary>
    public partial class Navigation : Page
    {

        #region "Declarations"

        private SteinbachEntities db;
        private ListCollectionView ProjekteView;
        private CollectionViewSource ProjekteViewSource;
        public DispatcherTimer timer;
        int imagesCount;
        int n = 4;
        //   bool[] tviState;

        #endregion


        #region "Page Load"

        public Navigation()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Session.User == null)
            {
                var login = new Logon();
                login.ShowDialog();







                db = new SteinbachEntities();


                MainTreeView.SetTreeViewExpandedState("Gruppe");

            }

        }








        #endregion


        #region "Menue Selection"


        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

            var tv = (TreeView)sender;
            var tvi = (TreeViewItem)tv.SelectedItem;
            Cursor = System.Windows.Input.Cursors.Wait;


            MainTreeView.GetTreeViewExpandedState("Gruppe");

            if (tvi != null && tvi.Tag != null)

                switch (tvi.Tag.ToString())
                {
                    case "Kundendaten":
                        {
                            var p = new Kundendaten();
                            NavigationService.Navigate(p);

                            break;
                        }

                    default:
                        Cursor = System.Windows.Input.Cursors.Arrow;
                        if (tvi.Tag.ToString() != "Gruppe")
                            MessageBox.Show("Diese Ansicht wurde nocht nicht implementiert");
                        break;
                }

            Cursor = System.Windows.Input.Cursors.Arrow;


        }


    }

        #endregion

}

