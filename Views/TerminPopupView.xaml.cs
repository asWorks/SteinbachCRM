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
using SteinbachCRM.ViewModels;

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for TerminPopupView.xaml
    /// </summary>
    public partial class TerminPopupView : Window
    {
        int ID;


        public TerminPopupView()
        {
            InitializeComponent();
        }

        public TerminPopupView(int id)
        {
            InitializeComponent();
            ID = id;
          


            TerminPopupViewModel vModel = new TerminPopupViewModel(ID);
            Caliburn.Micro.ViewModelBinder.Bind(vModel, this, null);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //TerminPopupViewModel vModel = new TerminPopupViewModel(ID);
            //Caliburn.Micro.ViewModelBinder.Bind(vModel, this, null);
        }

    }
}
