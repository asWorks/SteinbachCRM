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

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for KommunikationView.xaml
    /// </summary>
    public partial class KommunikationView : UserControl
    {
        public KommunikationView()
        {
            InitializeComponent();
        }

        private void KommuniKation_DeletingRows(object sender, C1.WPF.DataGrid.DataGridDeletingRowsEventArgs e)
        {

        }
    }
}
