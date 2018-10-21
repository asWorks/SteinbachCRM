using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using DAL;
using System.Collections.ObjectModel;
using SteinbachCRM.Models;
using SteinbachCRM.Views;
using System.Windows;

namespace SteinbachCRM.ViewModels
{
    [Export(typeof(KundenbesuchListeViewModel))]
    public class KundenbesuchListeViewModel : Screen
    {

        #region "Additional Properties"
        private DateTime _VonDatum;
        public DateTime VonDatum
        {
            get { return _VonDatum; }
            set
            {
                if (value != _VonDatum)
                {
                    _VonDatum = value;
                    NotifyOfPropertyChange(() => VonDatum);
                    LoadData();
                    // isDirty = true;
                }
            }
        }

        private DateTime _BisDatum;
        public DateTime BisDatum
        {
            get { return _BisDatum; }
            set
            {
                if (value != _BisDatum)
                {
                    _BisDatum = value;
                    NotifyOfPropertyChange(() => BisDatum);
                    // isDirty = true;
                }
            }
        }

        public int CurrentID { get; set; }

        private bool _showAllCustomers;
        public bool showAllCustomers
        {
            get { return _showAllCustomers; }
            set
            {
                if (value != _showAllCustomers)
                {
                    _showAllCustomers = value;
                    LoadData();
                    NotifyOfPropertyChange(() => showAllCustomers);
                    // isDirty = true;
                }
            }
        }

        private bool _noDateSelection;
        public bool noDateSelection
        {
            get { return _noDateSelection; }
            set
            {
                if (value != _noDateSelection)
                {
                    _noDateSelection = value;
                    LoadData();
                    NotifyOfPropertyChange(() => noDateSelection);
                    // isDirty = true;
                }
            }
        }




        #endregion



        #region "Collections"
        private ObservableCollection<KundenbesucheListe> _Kundenbesuche;
        public ObservableCollection<KundenbesucheListe> Kundenbesuche
        {
            get { return _Kundenbesuche; }
            set
            {
                if (value != _Kundenbesuche)
                {
                    _Kundenbesuche = value;
                    NotifyOfPropertyChange(() => Kundenbesuche);
                    //isDirty = true;


                }
            }
        }

        private KundenbesucheListe _SelectedKundenbesuche;
        public KundenbesucheListe SelectedKundenbesuche
        {
            get { return _SelectedKundenbesuche; }
            set
            {
                if (value != _SelectedKundenbesuche)
                {
                    _SelectedKundenbesuche = value;
                    NotifyOfPropertyChange(() => SelectedKundenbesuche);
                    // isDirty = true;
                }
            }
        }


        #endregion

        #region "Constructors"
        public KundenbesuchListeViewModel()
        {
            showAllCustomers = false;
            noDateSelection = false;
        }

        public KundenbesuchListeViewModel(int FirmaID)
        {

            CurrentID = FirmaID;
            string lookback = CommonTools.Tools.HelperTools.GetConfigEntry("KundenbesucheDatumVonLookBack", "90", "Zeitraum für den Kundenbesuche rückwirkend vorbelegt werden");

            VonDatum = DateTime.Now.AddDays(double.Parse(lookback) * -1);
            BisDatum = DateTime.Now;

            LoadData();


        }



        #endregion

        #region "Functions"

        private void LoadData()
        {
            Kundenbesuche = new ObservableCollection<KundenbesucheListe>();
            using (var db = new SteinbachEntities())
            {

                List<Firmen_Kundenbesuch> besuche;

                if (showAllCustomers && !noDateSelection)
                {
                    besuche = db.Firmen_Kundenbesuche.Where(f => f.id_firma >= 1 && f.id_firma <= 999999 && f.datum_von >= VonDatum).ToList();
                }
                else if (!showAllCustomers && !noDateSelection)
                {
                    besuche = db.Firmen_Kundenbesuche.Where(f => f.id_firma == CurrentID && f.datum_von >= VonDatum).ToList();
                }
                else if (!showAllCustomers && noDateSelection)
                {
                    besuche = db.Firmen_Kundenbesuche.Where(f => f.id_firma == CurrentID).ToList();
                }
                else if (showAllCustomers && noDateSelection)
                {
                    besuche = db.Firmen_Kundenbesuche.Where(f => f.id_firma >= 1 && f.id_firma <= 999999).ToList();
                }
                else
                {
                    besuche = db.Firmen_Kundenbesuche.Where(f => f.id_firma == CurrentID && f.datum_von >= VonDatum).ToList();
                }




                foreach (var item in besuche)
                {
                    //string buf_idSIMitarbeiter = db.personen.Where(p => p.id == item.id_siperson).SingleOrDefault() == null
                    //    ? ""
                    //    : db.personen.Where(p => p.id == item.id_siperson).SingleOrDefault().benutzername;

                    string buf_idSIMitarbeiter = GetListeSIMitarbeiter(db, item.id );

                    string buf_idKunde = db.firmen.Where(p => p.id == item.id_firma).SingleOrDefault() == null
                        ? ""
                        : db.firmen.Where(p => p.id == item.id_firma).SingleOrDefault().name;

                    string buf_idFirma = db.firmen.Where(p => p.id == item.id_vertretenefirma).SingleOrDefault() == null
                        ? ""
                        : db.firmen.Where(p => p.id == item.id_vertretenefirma).SingleOrDefault().name;


                    string buf_idKontakt = db.Firmen_Personen.Where(p => p.id == item.id_kontakt).SingleOrDefault() == null
                        ? ""
                        : buf_idKontakt = db.Firmen_Personen.Where(p => p.id == item.id_kontakt).SingleOrDefault().Fullname;


                    var kb = new KundenbesucheListe
                    {
                        //datum_von = (DateTime)item.datum_von,
                        //datum_bis = (DateTime)item.datum_bis,
                        kmgefahren = item.kmgefahren == null ? 0 : (double)item.kmgefahren,
                        kfzkenzeichen = item.kfzkennzeichen,
                        besuchsthema = item.besuchsthema,
                        id_siperson = buf_idSIMitarbeiter,
                        id_kontakt = buf_idKontakt,
                        id_firma = buf_idKunde,
                        id_vertretenefirma = buf_idFirma,
                        id = item.id,
                        projektnummer = item.projektnummer


                    };

                    if (item.datum_von != null)
                    {
                        kb.datum_von = item.datum_von;
                    }

                    if (item.datum_bis != null)
                    {
                        kb.datum_bis = item.datum_bis;
                    }



                    Kundenbesuche.Add(kb);

                }

            }

        }

        private string GetListeSIMitarbeiter(SteinbachEntities Context, int BesuchID)
        {
            //string buf_idSIMitarbeiter = db.personen.Where(p => p.id == item.id_siperson).SingleOrDefault() == null
            //            ? ""
            //            : db.personen.Where(p => p.id == item.id_siperson).SingleOrDefault().benutzername;
            string buf = string.Empty;
            var query = from m in Context.Kundenbesuche_TeilnehmerSI
                        join p in Context.personen on m.id_TeilnemerSI equals p.id
                        where m.id_besuch == BesuchID
                        select p;

            if (query.Any())
            {

                foreach (var item in query)
                {
                    buf += item.kuerzel + " ,";
                }
            }

            return buf;
        }


        public void BesuchNeu()
        {
            try
            {
                String[] size = CommonTools.Tools.HelperTools.GetConfigEntry("PopUpSizeKundenbesuch", "800,2000").Split(',');
                Firmen_Kundenbesuch besuch = CommonTools.ObjectFactories.KundenbesuchsBerichteFactory.GetNewBesuch(CurrentID);
                var besuchView = new StandardPopupView(StandardPopupViewModel.EnumPopupType.Kundenbesuch, "Kundenbesuch", double.Parse(size[0]), double.Parse(size[1]), besuch);
                besuchView.ShowDialog();
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.HandleStandardError(ex, "Fehler bei Neuerstellung eines Besuchsberichtes");
            }


        }


        #endregion

        #region "Events"

        public void Kundenbesuche_DoubleClick(C1.WPF.DataGrid.C1DataGrid grid)
        {
            try
            {
                var t = (Models.KundenbesucheListe)grid.SelectedItem;
                if (t != null)
                {
                    int buf = t.id;
                    String[] size = CommonTools.Tools.HelperTools.GetConfigEntry("PopUpSizeKundenbesuch", "800,2000").Split(',');
                    var kbView = new StandardPopupView(t.id, StandardPopupViewModel.EnumPopupType.Kundenbesuch, "Kundenbesuch", double.Parse(size[0]), double.Parse(size[1]));
                    kbView.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                CommonTools.Tools.ErrorMethods.HandleStandardError(ex, "Fehler beim Anzeigen von Besuchsberichten");
            }


        }

        public void Kundenbesuche_DeletingRows(C1.WPF.DataGrid.DataGridDeletingRowsEventArgs e)
        {

            try
            {
                var Rights = "admin suCRM";
                if (!Rights.Contains(Session.User.rights))
                {
                    MessageBox.Show("Sie haben nicht die Berechtigung zum Löschen des Berichtes");
                    e.Cancel = true;
                    return;
                }

                if (e.DeletedRows.Count() == 1)
                {
                    var Com = (KundenbesucheListe)e.DeletedRows[0].DataItem;

                    using (var db = new SteinbachEntities())
                    {
                        var besuch = db.Firmen_Kundenbesuche.Where(m => m.id == Com.id).SingleOrDefault();
                        if (MessageBox.Show(string.Format("Besuchsbericht {0} wirklich endgültig löschen ?", besuch.besuchsthema), "Sicherheitsabfrage",
                                            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            CommonTools.ObjectFactories.KundenbesuchsBerichteFactory.DeleteBesuchsbericht(besuch.id);
                        }
                        else
                        {
                            e.Cancel = true;
                        }

                    }




                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.HandleStandardError(ex, "Fehler beim Löschen von Besuchsberichten");
            }


        }
        #endregion


    }

}