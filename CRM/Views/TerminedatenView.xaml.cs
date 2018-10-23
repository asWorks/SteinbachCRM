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
using DAL;
using SteinbachCRM.ObjectCollections;
using CommonTools.Tools;

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for KundendatenView.xaml
    /// </summary>
    public partial class TerminedatenView : UserControl
    {
        SteinbachEntities db;
        LinkViewsource<UserControl> Eintraege;

        public TerminedatenView()
        {
            InitializeComponent();
            db = new SteinbachEntities();

        }

     




       

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            //var query = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(g => g.Gruppe == "Land"), db);
            //Eintraege = new LinkViewsource<UserControl>(this, query, "PersonTyp");
            var vModel = (TerminedatenViewModel)this.DataContext;
            vModel.OnNotifyViewTerminChanged += new TerminedatenViewModel.DelegateTerminChanged(vModel_OnNotifyViewTerminChanged);

          

        }

        void vModel_OnNotifyViewTerminChanged()
        {
            this.tabTermineTermin.Focus();
            
        }

    
     

     





    }
}
