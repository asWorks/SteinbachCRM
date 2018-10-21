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
    /// Interaction logic for KundenbesuchView.xaml
    /// </summary>
    public partial class KundenbesuchView : UserControl
    {
        public KundenbesuchView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vModel = (KundenbesuchViewModel)this.DataContext;
            
            this.ListBoxTeilnehmerExternBesucheVM.DataContext = vModel.ListBoxTeilnehmerExternBesucheVM;
            this.ListboxKundenbesucheVertreteneFirmenVM.DataContext = vModel.ListboxKundenbesucheVertreteneFirmenVM;
            this.ListboxKundenbesucheTeilnehmerSIVM.DataContext = vModel.ListboxKundenbesucheTeilnehmerSIVM;
          
           // Console.WriteLine( vModel.kfzkennzeichen);
        }
    }
}
