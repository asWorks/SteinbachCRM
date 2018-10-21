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
    /// Interaction logic for EditAuswahlEintraege.xaml
    /// </summary>
    public partial class EditAuswahlEintraegeView : UserControl
    {
        public EditAuswahlEintraegeView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //var vmodel = (EditAuswahlEintraegeViewModel)this.DataContext;
            //this.GridGruppen.DataContext = vmodel.KategorienListe;
            //this.GridEintraege.DataContext = vmodel.EintraegeListe;

        }

        private void DeleteEintrag_Click(object sender, RoutedEventArgs e)
        {

        }

     
    }
}
