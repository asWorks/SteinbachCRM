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
using CommonTools.Extensions;
namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView_Alt : UserControl
    {
        public ShellView_Alt()
        {
            InitializeComponent();
        }


        //private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{

        //    //var tv = (TreeView)sender;
        //    //var tvi = (TreeViewItem)tv.SelectedItem;
        //    //Cursor = System.Windows.Input.Cursors.Wait;


        //    //MainTreeView.GetTreeViewExpandedState("Gruppe");

        //    //if (tvi != null && tvi.Tag != null)

        //    //    switch (tvi.Tag.ToString())
        //    //    {
        //    //        case "Kundendaten":
        //    //            {
        //    //                var p = new Kundendaten();
        //    //                //NavigationService.Navigate(p);

        //    //                break;
        //    //            }

        //    //        default:
        //    //            Cursor = System.Windows.Input.Cursors.Arrow;
        //    //            if (tvi.Tag.ToString() != "Gruppe")
        //    //                MessageBox.Show("Diese Ansicht wurde nocht nicht implementiert");
        //    //            break;
        //    //    }

        //    Cursor = System.Windows.Input.Cursors.Arrow;


        //}

     
    }
}
