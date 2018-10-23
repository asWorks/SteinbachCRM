using System;
using System.Linq;
using System.ComponentModel.Composition;
using SteinbachCRM.ViewModels.Interfaces;
using System.Windows;
using Caliburn.Micro;
using DAL;
using System.Collections.ObjectModel;
using C1.WPF.DataGrid;
using System.Windows.Data;
using SteinbachCRM.Events;
using System.Diagnostics;


namespace SteinbachCRM.ViewModels
{
    [Export(typeof(FirmenPersonenListeViewModel))]
    public class FirmenPersonenListeViewModel : Screen, IFirmenPersonenListe, IHandle<KundeChangedEvent>,IDataState
    {
        SteinbachEntities db;
        IEventAggregator _events;

        //private Visibility _HideUselessColumns;

        //public Visibility HideUselessColumns
        //{
        //    get { return _HideUselessColumns; }
        //    set
        //    {
        //        _HideUselessColumns = value;
        //        NotifyOfPropertyChange(() => HideUselessColumns);

        //    }
        //}


        private Firmen_Personen _CurrentPerson;
        private firma _CurrentFirma;
        private ObservableCollection<Firmen_Personen> _Personenliste;
        private ListCollectionView _Personlist;
        private ObservableCollection<Personen_Telefon> _Telefonnummern;
        private ObservableCollection<Personen_Mailadressen> _Mailadressen;


        public Firmen_Personen CurrentPerson
        {
            get { return _CurrentPerson; }
            set
            {
                _CurrentPerson = value;
                NotifyOfPropertyChange(() => CurrentPerson);
            }
        }


        public ListCollectionView Personlist
        {
            get
            {
                if (_Personlist == null)
                {
                    _Personlist = new ListCollectionView(_Personenliste);
                }
                return _Personlist;

            }
            set
            {

                _Personlist = value;
                NotifyOfPropertyChange(() => Personlist);
            }
        }



        public ObservableCollection<Firmen_Personen> Personenliste
        {
            get { return _Personenliste; }
            set
            {
                _Personenliste = value;
                NotifyOfPropertyChange(() => Personenliste);
            }
        }




        public ObservableCollection<Personen_Telefon> Telefonnummern
        {
            get { return _Telefonnummern; }
            set
            {
                _Telefonnummern = value;
                NotifyOfPropertyChange(() => Telefonnummern);
            }
        }

        public ObservableCollection<Personen_Mailadressen> Mailadressen
        {
            get { return _Mailadressen; }
            set
            {
                _Mailadressen = value;
                NotifyOfPropertyChange(() => Mailadressen);

            }
        }


        public firma CurrentFirma
        {
            get { return _CurrentFirma; }
            set
            {
                _CurrentFirma = value;
                NotifyOfPropertyChange(() => CurrentFirma);
            }
        }

        [ImportingConstructor]
        public FirmenPersonenListeViewModel(IEventAggregator events)
        {
            _events = events;
            events.Subscribe(this);

        }

        private void LoadFirma(int id)
        {
            this.db = new SteinbachEntities();
            CurrentFirma = db.firmen.Where(f => f.id == id).SingleOrDefault();

            //CurrentFirma = Firma;
            Personenliste = new ObservableCollection<Firmen_Personen>(CurrentFirma.Firmen_Personen);

        }


        public FirmenPersonenListeViewModel(SteinbachEntities db, firma Firma)
        {
            this.db = db;
            CurrentFirma = Firma;
            Personenliste = new ObservableCollection<Firmen_Personen>(CurrentFirma.Firmen_Personen);

        }


        public FirmenPersonenListeViewModel(int id_Firma)
        {
            db = new SteinbachEntities();

            if (id_Firma == 0)
            {
                CurrentFirma = CommonTools.EntitiesActions.FirmaActions.GetNewFirma(db);

            }
            else
            {
                CurrentFirma = db.firmen.Where(f => f.id == id_Firma).SingleOrDefault();
            }
          
            Personenliste = new ObservableCollection<Firmen_Personen>(CurrentFirma.Firmen_Personen);

        }






        public void CurrentPerson_SelectionChanged(FrameworkElement C1Grid)
        {
            var grid = (C1DataGrid)C1Grid;
            var person = (Firmen_Personen)grid.SelectedItem;
            CurrentPerson = person;
            if (CurrentPerson != null)
            {
                Telefonnummern = new ObservableCollection<Personen_Telefon>(person.Personen_Telefon);
                Mailadressen = new ObservableCollection<Personen_Mailadressen>(person.Personen_Mailadressen);
            }


        }


        public void AddPerson()
        {
            var pers = new Firmen_Personen();
            pers.Betreuer = DAL.Session.User.benutzername;
            pers.Newsletter = 0;
            pers.Weihnachtskarte = 0;
            pers.Messeeinladung = 0;
            CurrentFirma.Firmen_Personen.Add(pers);
            db.AddToFirmen_Personen(pers);
            Personenliste.Add(pers);
            CurrentPerson = pers;



        }



        public void AddTelefonNummer()
        {
            if (CurrentPerson != null)
            {
                var tel = new Personen_Telefon();
                CurrentPerson.Personen_Telefon.Add(tel);

                db.AddToPersonen_Telefon(tel);
                Telefonnummern.Add(tel);

            }


        }

        public void AddMailadresse()
        {
            if (CurrentPerson != null)
            {

                var adr = new Personen_Mailadressen();
                CurrentPerson.Personen_Mailadressen.Add(adr);

                db.AddToPersonen_Mailadressen(adr);
                Mailadressen.Add(adr);

            }

        }


        public void Mailadressen_DeletingRows(C1.WPF.DataGrid.DataGridDeletingRowsEventArgs e)
        {

            if (e.DeletedRows.Count() == 1)
            {
                var adr = (Personen_Mailadressen)e.DeletedRows[0].DataItem;
                if (MessageBox.Show(string.Format("Adresse {0} {1} wirklich endgültig löschen ?", adr.Typ, adr.Mailadresse), "Sicherheitsabfrage",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    db.DeleteObject(adr);

                }
                else
                {
                    e.Cancel = true;
                }
            }

        }

        public void Telefonnummern_DeletingRows(C1.WPF.DataGrid.DataGridDeletingRowsEventArgs e)
        {

            if (e.DeletedRows.Count() == 1)
            {
                var tel = (Personen_Telefon)e.DeletedRows[0].DataItem;
                if (MessageBox.Show(string.Format("Telefonnummer {0} {1} wirklich endgültig löschen ?", tel.Vorwahl, tel.Rufnummer), "Sicherheitsabfrage",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    db.DeleteObject(tel);

                }
                else
                {
                    e.Cancel = true;
                }
            }

        }

        public void Personen_DeletingRows(C1.WPF.DataGrid.DataGridDeletingRowsEventArgs e)
        {

            if (e.DeletedRows.Count() == 1)
            {
                var pers = (Firmen_Personen)e.DeletedRows[0].DataItem;
                if (MessageBox.Show(string.Format("Person {0} {1} wirklich endgültig löschen ?", pers.Vorname, pers.Nachname), "Sicherheitsabfrage",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {


                    var ptList = pers.Personen_Telefon.ToList();
                    foreach (var pp in ptList)
                    {
                        db.DeleteObject(pp);
                        Telefonnummern.Remove(pp);
                    }

                    var maList = pers.Personen_Mailadressen.ToList();
                    foreach (var maa in maList)
                    {
                        db.DeleteObject(maa);
                        Mailadressen.Remove(maa);

                    }

                    db.DeleteObject(pers);

                }
                else
                {
                    e.Cancel = true;
                }
            }

        }


        public void DeleteTelefonnummer(FrameworkElement dc)
        {

            try
            {

                var x = (Personen_Telefon)dc.DataContext;
                if (MessageBox.Show(string.Format("Telefonnummer {0} {1} wirklich endgültig löschen ?", x.Vorwahl, x.Rufnummer), "Sicherheitsabfrage",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    db.DeleteObject(x);
                    Telefonnummern.Remove(x);
                   // db.SaveChanges();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));

            }

        }
        public void DeleteMailadresse(FrameworkElement dc)
        {

            try
            {

                var x = (Personen_Mailadressen)dc.DataContext;
                if (MessageBox.Show(string.Format("Adresse {0} {1} wirklich endgültig löschen ?", x.Typ, x.Mailadresse), "Sicherheitsabfrage",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    db.DeleteObject(x);
                    Mailadressen.Remove(x);
                   // db.SaveChanges();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));

            }

        }



        public void MakePhoneCall(FrameworkElement dc)
        {

            try
            {
                var vm = (Personen_Telefon)dc.DataContext;




                if (vm.Typ != 16)  //Fax
                {
                    TapiLib.CommonCalls.MakeDialerCall(vm.Dialnumber, "Steinbach CRM", vm.Firmen_Personen.Fullname, "Anruf aus CRM initiiert");
                }



            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);
            }

        }


        public void OpenOutlook(FrameworkElement dc)
        {
            try
            {
                var vm = (Personen_Mailadressen)dc.DataContext;
                if (vm != null)
                {
                    if (vm.Mailadresse != null && vm.Mailadresse != string.Empty)
                    {
                        ProcessStartInfo psi = new ProcessStartInfo("MailTo:" + vm.Mailadresse);
                        psi.UseShellExecute = true;
                        Process.Start(psi);
                    }
                }


            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage("Fehler beim Aufrufen des Mailclients.");
            }


        }



        public void Save()
        {
            try
            {
                db.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex, true);
            }

        }


        public void Handle(KundeChangedEvent message)
        {
            LoadFirma(message.id);
        }



        public bool isDirty
        {
            get { return CommonTools.Tools.ManageChanges.isDirty(db); }
        }
    }
}
