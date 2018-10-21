using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace asWorksUpdater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public string command { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length == 1)
            {
                 command = e.Args[0];
            }

            var f = new MainWindow(command);
            f.Show();

            base.OnStartup(e);
        }

        public string GetCommand()
        {
            return command;

        }
    }
}
