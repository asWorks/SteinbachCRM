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
using System.Diagnostics;
using System.Windows.Threading;
using System.IO;


namespace asWorksUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ProgName;
        string[] arg;
        StringBuilder sb;

        DispatcherTimer timer;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                timer = new DispatcherTimer();
                timer.Tick += new EventHandler(timer_Tick);
                timer.Interval = new TimeSpan(0, 0, 0, 0, 2000);
                MessageBox.Show("Falscher Konstruktor.");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        void timer_Tick(object sender, EventArgs e)
        {
            DoUpdate();
            timer.Stop();
        }

        public MainWindow(string Argument)
        {
            try
            {
                InitializeComponent();
                timer = new DispatcherTimer();
                timer.Tick += new EventHandler(timer_Tick);
                timer.Interval = new TimeSpan(0, 0, 0, 0, 2000);
                            
                if (Argument != null)
                {
                    arg = Argument.Split(',');
                    ProgName = arg[2].Replace(".exe", "");
                }
                else
                {
                    MessageBox.Show("Keine Argumente.");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }



        }







        private void DoUpdate()
        {
            try
            {
                DirectoryInfo Installed = new DirectoryInfo(arg[0]);
                DirectoryInfo Update = new DirectoryInfo(arg[1]);

                foreach (var item in Installed.GetFiles())
                {
                    item.Delete();
                }
                foreach (var item in Update.GetFiles())
                {
                    item.CopyTo(System.IO.Path.Combine(Installed.FullName, item.Name));
                }

                btnCancel.Content = "Schließen";
                RestartApplication();
                CloseProcess("asWorksUpdater");

                //  CloseUpdater();


            }
            catch (Exception ex)
            {

                sb.Clear();
                sb.AppendLine("Fehler beim Updatevorgang");
                sb.AppendLine();
                sb.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    sb.AppendLine(ex.InnerException.Message);
                }


                Message.Text = sb.ToString();
            }

        }



        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

            CloseProcess(arg[2]);
            timer.Start();


        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            sb = new StringBuilder();
            sb.Clear();
            sb.AppendLine();

            try
            {
                if (arg.Length == 3)
                {
                    if (CommonTools.Tools.ConfigEntry<int>.GetConfigEntry("UpdatePfadeAnzeigen", "0", "Schalter zum anzeugen der Updatepfade") != 0)
                    {

                        sb.AppendLine("Verwendete Einstellungen");
                        sb.AppendLine(arg[0]);
                        sb.AppendLine(arg[1]);
                        sb.AppendLine(arg[2]);
                        sb.AppendLine();
                    }


                }
                else
                {
                    sb.AppendLine("Fehler !");
                    sb.AppendLine();
                    sb.AppendLine("Keine Komandozeilen Argumente gefunden");
                    sb.AppendLine("Update kann nicht ausgeführt werden");
                    sb.AppendLine();
                    btnUpdate.IsEnabled = false;
                    btnCancel.Content = "Schließen";

                }

            }
            catch (Exception ex)
            {

                Message.Text = sb.ToString();
            }


            sb.AppendLine(string.Format("Es ist ein neues Update für {0} verfügbar.", ProgName));
            sb.AppendLine("Soll das Update jetzt intalliert werden?");
            sb.AppendLine();
            sb.AppendLine(ProgName + " wird zu diesem Zweck beendet und nach Abschluß");
            sb.AppendLine("neu gestartet.");

            Message.Text = sb.ToString();


        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void RestartApplication()
        {
            try
            {
                Process P = new Process();
                P.StartInfo.FileName = System.IO.Path.Combine(arg[0], arg[2]);
                P.Start();
            }
            catch (Exception)
            {

                sb.Clear();
                sb.Append("Fehler beim Neustart von ");
                sb.AppendLine(arg[2]);

                Message.Text = sb.ToString();
            }


        }

        private void CloseProcess(string pName)
        {
            try
            {
                if (pName.ToLower().Contains(".exe"))
                {
                    pName = pName.Replace(".exe", "");
                }

                if (Process.GetProcessesByName(pName).Length > 0)
                {
                    var process = Process.GetProcessesByName(pName);
                    foreach (var item in process)
                    {

                        bool res = item.CloseMainWindow();
                        if (res == false)
                        {
                            MessageBox.Show(string.Format("Prozess {0} konnte nicht beendet werden.", pName));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                sb.Clear();
                sb.AppendLine("Fehler beim Schließen des Updateprogrammes.");
                Message.Text = sb.ToString();

            }


        }

    }
}
