using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using SteinbachCRM.Models;
using System.Collections.ObjectModel;
using DAL;
using System.Runtime.InteropServices;
using System.Windows;
using SteinbachCRM.Views;

namespace SteinbachCRM.ViewModels
{
    [Export(typeof(KommunikationViewModel))]
    public class KommunikationViewModel : Screen
    {

        #region "Declaration"
        SteinbachEntities db;
        int id_firma = 0;
        bool bDoneInitializing = false;
        #endregion


        #region "Properties"
        private ObservableCollection<KommunikationModel> _KommuniKation;

        public ObservableCollection<KommunikationModel> KommuniKation
        {
            get { return _KommuniKation; }
            set
            {
                _KommuniKation = value;
                NotifyOfPropertyChange(() => KommuniKation);
            }
        }


        private KommunikationModel _SelectedKommunikation;

        public KommunikationModel SelectedKommunikation
        {
            get { return _SelectedKommunikation; }
            set
            {
                _SelectedKommunikation = value;
                NotifyOfPropertyChange(() => SelectedKommunikation);
            }
        }

        private DateTime _VonDatum;

        public DateTime VonDatum
        {
            get { return _VonDatum; }
            set
            {
                _VonDatum = value;
                if (BisDatum < value)
                {
                    BisDatum = value.AddDays(7);
                }
                NotifyOfPropertyChange(() => VonDatum);
                FillCommunication(id_firma);

            }
        }

        private DateTime _BisDatum;

        public DateTime BisDatum
        {
            get { return _BisDatum; }
            set
            {
                _BisDatum = value;
                if (VonDatum >= value)
                {
                    VonDatum = value.AddDays(-7);
                }
                NotifyOfPropertyChange(() => BisDatum);
                FillCommunication(id_firma);
            }
        }

        private TextWrapping _ColumnTextWrapping;

        public TextWrapping ColumnTextWrapping
        {
            get { return _ColumnTextWrapping; }
            set
            {
                _ColumnTextWrapping = value;
                NotifyOfPropertyChange(() => ColumnTextWrapping);
            }
        }

        private bool _MultiLine;

        public bool MultiLine
        {
            get { return _MultiLine; }
            set
            {
                _MultiLine = value;
                NotifyOfPropertyChange(() => MultiLine);
                if (value)
                {
                    ColumnTextWrapping = TextWrapping.WrapWithOverflow;
                }
                else
                {
                    ColumnTextWrapping = TextWrapping.NoWrap;
                }
            }
        }



        #endregion


        #region "Constructor"
        public KommunikationViewModel(int id_Firma)
        {

            KommuniKation = new ObservableCollection<KommunikationModel>();

            int lookback = int.Parse(CommonTools.Tools.HelperTools.GetConfigEntry("CommunicationLookBack"));
            int lookahead = int.Parse(CommonTools.Tools.HelperTools.GetConfigEntry("CommunicationLookAhead"));

            VonDatum = CommonTools.Tools.DateTools.GetDateWithCustomHour(DateTime.Now.AddDays(lookback * -1), 0, 0, 0);
            BisDatum = CommonTools.Tools.DateTools.GetDateWithCustomHour(DateTime.Now.AddDays(lookahead), 0, 0, 0);

            MultiLine = CommonTools.Tools.HelperTools.GetConfigEntry("CommunicationMultiLine", "0") == "0" ? false : true;
            id_firma = id_Firma;
            bDoneInitializing = true;
            FillCommunication(id_Firma);


        }

        private void FillCommunication(int id_Firma)
        {
            KommuniKation.Clear();

            if (id_Firma == 0 || bDoneInitializing == false)
            {
                return;
            }

            try
            {
                using (var db = new SteinbachEntities())
                {
                    var termine = db.CRMTermine.Where(t => t.AppointmentType == "Aufgabe" && t.id_firma == id_Firma && t.TerminVon >= VonDatum && t.TerminVon <= BisDatum);
                    foreach (var item in termine)
                    {
                        var t = new KommunikationModel();
                        t.Type = "Termin";
                        t.Aktion = db.AuswahlEintraege.Where(a => a.id == item.Aktion).SingleOrDefault().Eintrag;
                        t.Benutzer = db.personen.Where(b => b.id == item.Angelegt).SingleOrDefault().benutzername;
                        t.DatumUhrzeit = (DateTime)item.TerminVon;
                        t.Betreff = item.Betreff;
                        if (item.Gebiet != null)
                        {
                            t.Gebiet = db.AuswahlEintraege.Where(a => a.id == item.Gebiet).SingleOrDefault().Eintrag;
                        }
                        var query = db.Termin_TeilnehmerExtern.Where(id => id.id_Termin == item.id);
                        if (query != null)
                        {
                            foreach (var tx in query)
                            {
                                var tnn = db.Firmen_Personen.Where(tn => tn.id == tx.id_Teilnehmer).SingleOrDefault();

                                t.ExterneTeilnehmerString += tnn.Fullname + ", ";
                            }
                        }


                        //  t.Status = db.AuswahlEintraege.Where(a => a.id == item.Status).SingleOrDefault().Eintrag;
                        t.ItemID = item.id;
                        if (item.Angelegt != null)
                        {
                            t.Benutzer = db.personen.Where(p => p.id == item.Angelegt).SingleOrDefault().benutzername;
                        }

                        //if (item.Status != null)
                        //{
                        //    //t.Status = db.AuswahlEintraege.Where(s => s.id == item.Status).SingleOrDefault().Eintrag;

                        //}

                        t.Status = CommonTools.Tools.HelperTools.GetAuswahlEintraegeEntry(item.Status);

                        KommuniKation.Add(t);


                    }

                    var mails = db.CRMEmails.Where(e => e.id_Firma == id_Firma && e.Datum >= VonDatum && e.Datum <= BisDatum);
                    foreach (var m in mails)
                    {
                        var ma = new KommunikationModel();
                        ma.Type = "Mail";
                        ma.ItemID = m.id;
                        ma.Betreff = m.Betreff;
                        ma.Aktion = GetEintrag(m.Aktion);
                        if (m.id_Firma != null && m.id_Firma != 0)
                        {
                            var g = db.firmen.Where(id => id.id == m.id_Firma).SingleOrDefault().Gebiet;
                            ma.Gebiet = GetEintrag(g);
                        }
                        ma.Status = GetEintrag(m.Status);
                        ma.Person = m.Firmen_Personen != null ? m.Firmen_Personen.Fullname : m.Absender;
                        ma.Benutzer = m.OutlookUsername != null ? m.OutlookUsername : "";

                        ma.DatumUhrzeit = (DateTime)m.Datum;
                        //  ma.Benutzer = db.personen.Where(b => b.id == m.id_PersonSI).SingleOrDefault().benutzername;
                        // ma.Person = m.Absender;
                        KommuniKation.Add(ma);
                    }

                }
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.HandleStandardError(ex, "Methode FillKommunication");
            }




        }

        #endregion


        public void KommuniKation_DoubleClick(C1.WPF.DataGrid.C1DataGrid grid)
        {
            try
            {
                var t = (Models.KommunikationModel)grid.SelectedItem;
                if (t.Type == "Termin")
                {
                    String[] size = CommonTools.Tools.HelperTools.GetConfigEntry("PopUpSizeTermin", "650,950").Split(',');
                    var Termin = new StandardPopupView(t.ItemID, StandardPopupViewModel.EnumPopupType.Termin, "Termin Nr. : " + t.ItemID, double.Parse(size[0]), double.Parse(size[1]));
                    Termin.ShowDialog();
                }
                else if (t.Type == "Mail")
                {
                    String[] size = CommonTools.Tools.HelperTools.GetConfigEntry("PopUpSizeMail", "800,1100").Split(',');
                    var Mail = new StandardPopupView(t.ItemID, StandardPopupViewModel.EnumPopupType.Email, "Email Nr. : " + t.ItemID, double.Parse(size[0]), double.Parse(size[1]));

                    Mail.ShowDialog();
                }
            }

            catch (System.IO.IOException ex)
            {

                CommonTools.Tools.ErrorMethods.HandleStandardError(ex, "Fehler beim Öffnen von Mailviewer");

            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.HandleStandardError(ex, "Fehler beim Öffnen von Mailviewer");
            }




        }



        private string GetEintrag(object id)
        {
            if (id == null)
            {
                return "";
            }

            int _ID = (int)id;
            using (var db = new SteinbachEntities())
            {
                var res = db.AuswahlEintraege.Where(i => i.id == _ID).SingleOrDefault();
                if (res == null)
                {
                    return string.Format("id {0} - Kein Eintrag", id.ToString());
                }
                return res.Eintrag;
            }

        }

        public void Kommunikation_DeletingRows(C1.WPF.DataGrid.DataGridDeletingRowsEventArgs e)
        {

            if (e.DeletedRows.Count() == 1)
            {
                var Com = (KommunikationModel)e.DeletedRows[0].DataItem;
                if (Com.Type == "Mail")
                {
                    using (var db = new SteinbachEntities())
                    {
                        var mail = db.CRMEmails.Where(m => m.id == Com.ItemID).SingleOrDefault();
                        if (MessageBox.Show(string.Format("Mail {0} wirklich endgültig löschen ?", mail.Betreff), "Sicherheitsabfrage",
                                            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            CommonTools.ObjectFactories.EmailFactory.DeleteEmail(mail.id);
                        }
                        else
                        {
                            e.Cancel = true;
                        }

                    }


                }
                else if (Com.Type == "Termin")
                {
                    using (var db = new SteinbachEntities())
                    {
                        var termin = db.CRMTermine.Where(m => m.id == Com.ItemID).SingleOrDefault();
                        if (MessageBox.Show(string.Format("Termin {0} wirklich endgültig löschen ?", termin.Betreff), "Sicherheitsabfrage",
                                            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            CommonTools.ObjectFactories.TerminFactory.DeleteTermine(termin.id);
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }

                }

            }
            else
            {
                e.Cancel = true;
            }
        }

    }



}

