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
using System.Windows.Shapes;
using DAL;
using CommonTools.Views;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        ServiceHost host;

        public ShellView()
        {
            InitializeComponent();


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Session.User == null)
                {
                    var login = new Logon();
                    login.ShowDialog();

                    if (Session.User.rights == "admin" || Session.User.rights == "suCRM")
                    {
                        this.MainMenu.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.MainMenu.Visibility = Visibility.Hidden;

                    }
                }
            }



            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex, true);
            }


            //if (CommonTools.Tools.HelperTools.GetConfigEntry("WCFHostStarten") == "1")
            //{
            //    Uri baseAddress = new Uri("http://localhost:8083/SteinbachWCFLocal");
            //    try
            //    {


            //        host = new ServiceHost(typeof(SteinbachWCFLocalService.SteinbachService), baseAddress);
            //        //host = new ServiceHost(typeof(SteinbachWCFLocalService.SteinbachService));

            //        // Enable metadata publishing.
            //        ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            //        smb.HttpGetEnabled = true;
            //        smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            //        host.Description.Behaviors.Add(smb);

            //        // Open the ServiceHost to start listening for messages. Since
            //        // no endpoints are explicitly configured, the runtime will create
            //        // one endpoint per base address for each service contract implemented
            //        // by the service.
            //        host.Open();

            //        Console.WriteLine("The service is ready at {0}", baseAddress);
            //        Console.WriteLine("Press <Enter> to stop the service.");
            //        Console.ReadLine();

            //        // Close the ServiceHost.
            //        // host.Close();
            //    }
            //    catch (Exception ex)
            //    {

            //        DAL.Tools.LoggingTool.addDatabaseLogging("0", 0, "Fehler beim Start des WCF Service", CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));
            //    }




            //    DAL.Tools.LoggingTool.addDatabaseLogging("0", 0, "WCF Service gestartet auf", baseAddress.ToString());
            //}


        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (host != null)
                {

                    host.Close();
                }
            }
            catch (Exception)
            {

                
            }


        }

      

    




    }
}
