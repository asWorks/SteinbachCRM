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
using mshtml;
using System.IO;


namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for MailViewerView.xaml
    /// </summary>
    public partial class MailViewerView : UserControl
    {

        FileStream fs = null;

        public MailViewerView()
        {
            InitializeComponent();

        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                var vModel = (ViewModels.MailViewerViewModel)this.DataContext;
                switch (vModel.BodyFormat)
                {
                    case "HTML":
                        {
                            string filename = Environment.GetEnvironmentVariable("LOCALAPPDATA");
                            filename += @"\temp.html";

                            var utf = Encoding.UTF8;
                            FileStream fsOpen = File.Open(filename, FileMode.Create);

                            StreamWriter sr = new StreamWriter(fsOpen, utf);
                            sr.Write(vModel.HTMLText);
                            sr.Flush();
                            sr.Close();
                            sr.Dispose();
                            sr = null;
                            fsOpen.Close();
                            fsOpen.Dispose();


                            //FileStream fs = File.Open(filename, FileMode.Open);
                            fs = File.Open(filename, FileMode.Open);

                            Browser.NavigateToStream(fs);
                            this.Browser.Visibility = Visibility.Visible;
                            this.RTFTextBox.Visibility = Visibility.Collapsed;
                            break;
                        }
                    case "RichText":
                        {
                            byte[] rtfBytes = BitConverter.GetBytes((long)vModel.RTFText);


                            this.RTFTextBox.Visibility = Visibility.Visible;
                            this.Browser.Visibility = Visibility.Collapsed;
                            RTFTextBox.AppendText(System.Text.Encoding.Default.GetString(rtfBytes));
                            break;
                        }

                    case "PLAIN":
                        {

                            Browser.NavigateToString(vModel.Text);
                            this.Browser.Visibility = Visibility.Visible;
                            this.RTFTextBox.Visibility = Visibility.Collapsed;
                            break;
                        }


                    default:
                        break;
                }
            }

            catch (IOException ex)
            {
                
                CommonTools.Tools.ErrorMethods.HandleStandardError(ex, "Temopäre HTMLDatei ist in Bearbeitung"); 

            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.HandleStandardError(ex); 
            }

            
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            fs.Close();
            fs.Dispose();
            fs = null;

        }
    }
}
