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

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for FirmaView.xaml
    /// </summary>
    public partial class FirmaView : UserControl
    {
        public FirmaView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vModel = (FirmaViewModel)this.DataContext;
            this.spCurrentFirma.DataContext = vModel.CurrentFirma;
        }

        private void spCurrentFirma_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
