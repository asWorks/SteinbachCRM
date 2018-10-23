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
using CommonTools.Tools;
using DAL;
using SteinbachCRM.ObjectCollections;

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for TermineGesamtView.xaml
    /// </summary>
    public partial class TermineTerminView : UserControl
    {

        SteinbachEntities db;

        LinkBindingListViewsource<UserControl, BindingListCollectionView> EintraegeAktionenlookup;
        LinkBindingListViewsource<UserControl, BindingListCollectionView> EintraegeStatuslookup;
        LinkBindingListViewsource<UserControl, BindingListCollectionView> EintraegeErinnerunglookup;
        LinkBindingListViewsource<UserControl, BindingListCollectionView> EintraegePrioritaetlookup;
        LinkBindingListViewsource<UserControl, BindingListCollectionView> EintraegeSI_Userlookup;
        LinkBindingListViewsource<UserControl, BindingListCollectionView> Firmenlookup;
        LinkBindingListViewsource<UserControl, BindingListCollectionView> EintraegeGebietlookup;
        LinkBindingListViewsource<UserControl, BindingListCollectionView> EintraegeTeamlookup;

        public TermineTerminView()
        {
            InitializeComponent();
            db = new SteinbachEntities();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vModel = (TermineTerminViewModel)this.DataContext;
            this.CurrentTermin.DataContext = vModel.CurrentTermin;
            this.ListboxTeilnehmerSI.DataContext = vModel.ListboxTeilnehmerSI;
            this.ListboxKontakte.DataContext = vModel.ListboxTeilnehmerExtern;
            vModel.OnNotifyUINewAppointment += vModel_OnNotifyUINewAppointment;
           // this.dtpTerminVon.DataContext = vModel;


            EintraegeAktionenlookup = new LinkBindingListViewsource<UserControl, BindingListCollectionView>(this, db.AuswahlEintraege.Where(a => a.Gruppe == "TerminAktion"), "EintraegeAktionenLookup");
            EintraegeStatuslookup = new LinkBindingListViewsource<UserControl, BindingListCollectionView>(this, db.AuswahlEintraege.Where(a => a.Gruppe == "TerminStatus"), "EintraegeStatusLookup");
            EintraegeErinnerunglookup = new LinkBindingListViewsource<UserControl, BindingListCollectionView>(this, db.AuswahlEintraege.Where(a => a.Gruppe == "TypErinnerungDauer"), "EintraegeErinnerungLookup");
            EintraegePrioritaetlookup = new LinkBindingListViewsource<UserControl, BindingListCollectionView>(this, db.AuswahlEintraege.Where(a => a.Gruppe == "TerminPrioritaet"), "EintraegePrioritaetLookup");
            EintraegeSI_Userlookup = new LinkBindingListViewsource<UserControl, BindingListCollectionView>(this, db.personen, "SI_UserLookup");
            EintraegeGebietlookup = new LinkBindingListViewsource<UserControl, BindingListCollectionView>(this, db.AuswahlEintraege.Where(a => a.Gruppe == "Gebiet"), "EintraegeGebietLookup");
            EintraegeTeamlookup = new LinkBindingListViewsource<UserControl, BindingListCollectionView>(this, db.AuswahlEintraege.Where(a => a.Gruppe == "TerminTeam"), "EintraegeTeamLookup");

            Firmenlookup = new LinkBindingListViewsource<UserControl, BindingListCollectionView>(this, db.firmen.Where(a => a.IstKunde == 1), "FirmenLookup");
            //this.dtpErinnerungDatum.EditMode = C1.WPF.DateTimeEditors.C1DateTimePickerEditMode.Date;
        }

        void vModel_OnNotifyUINewAppointment(string obj)
        {

            switch (obj)
            {
                case "Termin":
                    {
                        dtpErinnerungDatum.Visibility = Visibility.Collapsed;
                        txtErinerungDatum.Visibility = Visibility.Collapsed;
                        cboErinnerung.Visibility = Visibility.Visible;
                        
                        
                        break;
                    }

                case "Aufgabe":
                    {
                        dtpErinnerungDatum.Visibility = Visibility.Visible;
                        txtErinerungDatum.Visibility = Visibility.Visible;
                        cboErinnerung.Visibility = Visibility.Collapsed;


                        break;
                    }
                default:
                    break;
            }
        }

        private void ChkErinnerungGesendet_ThisCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            //if (MessageBox.Show("Termin wirklich deaktivieren ?","",MessageBoxButton.YesNo) == MessageBoxResult.No)
            //{
            //    ChkErinnerungGesendet.CheckBoxChecked = true;
            //}
        }

        private void ChkErinnerungGesendet_ThisCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            
            //if (MessageBox.Show("Termin wirklich deaktivieren ?", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
            //{
            //    ChkErinnerungGesendet.CheckBoxChecked = true;
            //}
        }

        private void LabelAndFilteredComboBox_onfcbChanged(object sender, CommonTools.UserControls.FilteredComboBoxChangedEventArgs e)
        {
            LoadFirmen(e.filter);
        }

        private void LoadFirmen(string filter)
        {
            try
            {
                if (filter == string.Empty)
                {
                    //var query = db.firmen.Where(f => f.IstKunde == 1).OrderBy(f=>f.istFirma);
                    var query = from f in db.firmen
                                where f.IstKunde == 1
                                orderby f.name
                                select f;

                    Firmenlookup.ViewSource.Source = new FirmenCollection(query, db);
                }
                else
                {
                    var query = db.firmen.Where(f => f.name.Contains(filter) && f.IstKunde == 1).OrderBy(f => f.name);
                    Firmenlookup.ViewSource.Source = new FirmenCollection(query, db);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));
            }


        }
     
    }
}
