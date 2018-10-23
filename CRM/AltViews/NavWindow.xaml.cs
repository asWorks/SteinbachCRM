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

namespace SteinbachCRM
{
    /// <summary>
    /// Interaction logic for NavWindow.xaml
    /// </summary>
    public partial class NavWindow : System.Windows.Navigation.NavigationWindow
    {
        private double plHeight;
        
        public NavWindow()
        {
            InitializeComponent();
        }

        private void NavigationWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            try
            {
                switch (NavigationService.Content.ToString())
                {
                    //case "ProjektDB.MainWindow":
                    //    {
                    //        var c = (MainWindow)NavigationService.Content;
                    //        e.Cancel = true;
                    //        if (c.NavigationService.CanGoBack)
                    //        {
                    //            //c.AreChangesHandled = true;
                    //            c.NavigationService.GoBack(); 
                    //        }
                             
                    //        //bool buf = c.CheckForChanges();
                    //        //if (c.CheckForChanges())
                    //        //    e.Cancel = true;
                    //        break;
                    //    }
                    default:
                        break;
                }
            }
            catch (Exception)
            {


            }



            //var x = MessageBox.Show("Fenster schließen?","Test",MessageBoxButton.YesNo);

            //if (x == MessageBoxResult.No)
            //    e.Cancel = true;


        }

        private void NavigationWindow_Navigating(object sender, NavigatingCancelEventArgs e)
        {
          



        }

        private void NavigationWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           
              

        }

        private void NavigationWindow_Navigated(object sender, NavigationEventArgs e)
        {
          
        }

    }
}
