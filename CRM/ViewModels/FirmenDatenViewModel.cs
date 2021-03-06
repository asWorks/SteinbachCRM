﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SteinbachCRM.ViewModels.Interfaces;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using DAL;
using System.Collections.ObjectModel;
using SteinbachCRM.ObjectCollections;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SteinbachCRM.Events;


namespace SteinbachCRM.ViewModels
{
    [Export(typeof(FirmenDatenViewModel))]
    public class FirmenDatenViewModel : Screen, IFirmenDaten, IHandle<KundeChangedEvent>
    {

        #region "Declarations"

        IEventAggregator _events;
        public DispatcherTimer timer;
        SteinbachEntities db;

        #endregion

        #region "Properties"
        public bool isDirty { get; set; }
        private Int32 _id;
        public Int32 id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyOfPropertyChange(() => id);
                    isDirty = true;
                }
            }
        }

        private DateTime? _created;
        public DateTime? created
        {
            get { return _created; }
            set
            {
                if (value != _created)
                {
                    _created = value;
                    NotifyOfPropertyChange(() => created);
                    isDirty = true;
                }
            }
        }

        private String _name;
        public String name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyOfPropertyChange(() => name);
                    isDirty = true;
                }
            }
        }

        private String _kurzname;
        public String kurzname
        {
            get { return _kurzname; }
            set
            {
                if (value != _kurzname)
                {
                    _kurzname = value;
                    NotifyOfPropertyChange(() => kurzname);
                    isDirty = true;
                }
            }
        }

        private Int16? _istFirma;
        public Int16? istFirma
        {
            get { return _istFirma; }
            set
            {
                if (value != _istFirma)
                {
                    _istFirma = value;
                    NotifyOfPropertyChange(() => istFirma);
                    isDirty = true;
                }
            }
        }

        private Int16? _istVerarbeitet;
        public Int16? istVerarbeitet
        {
            get { return _istVerarbeitet; }
            set
            {
                if (value != _istVerarbeitet)
                {
                    _istVerarbeitet = value;
                    NotifyOfPropertyChange(() => istVerarbeitet);
                    isDirty = true;
                }
            }
        }

        private Int16? _IstGruppe;
        public Int16? IstGruppe
        {
            get { return _IstGruppe; }
            set
            {
                if (value != _IstGruppe)
                {
                    _IstGruppe = value;
                    NotifyOfPropertyChange(() => IstGruppe);
                    isDirty = true;
                }
            }
        }

        private Int16? _IstKunde;
        public Int16? IstKunde
        {
            get { return _IstKunde; }
            set
            {
                if (value != _IstKunde)
                {
                    _IstKunde = value;
                    NotifyOfPropertyChange(() => IstKunde);
                    isDirty = true;
                }
            }
        }

        private String _Kundennummer;
        public String Kundennummer
        {
            get { return _Kundennummer; }
            set
            {
                if (value != _Kundennummer)
                {
                    _Kundennummer = value;
                    NotifyOfPropertyChange(() => Kundennummer);
                    isDirty = true;
                }
            }
        }

        private Int32? _KdNr;
        public Int32? KdNr
        {
            get { return _KdNr; }
            set
            {
                if (value != _KdNr)
                {
                    _KdNr = value;
                    NotifyOfPropertyChange(() => KdNr);
                    isDirty = true;
                }
            }
        }

        private String _KundenTyp;
        public String KundenTyp
        {
            get { return _KundenTyp; }
            set
            {
                if (value != _KundenTyp)
                {
                    _KundenTyp = value;
                    NotifyOfPropertyChange(() => KundenTyp);
                    isDirty = true;
                }
            }
        }

        private Int32? _Typ;
        public Int32? Typ
        {
            get { return _Typ; }
            set
            {
                if (value != _Typ)
                {
                    _Typ = value;
                    NotifyOfPropertyChange(() => Typ);
                    isDirty = true;
                }
            }
        }

        private String _Webseite;
        public String Webseite
        {
            get { return _Webseite; }
            set
            {
                if (value != _Webseite)
                {
                    _Webseite = value;
                    NotifyOfPropertyChange(() => Webseite);
                    isDirty = true;
                }
            }
        }

        private Int32? _Quelle;
        public Int32? Quelle
        {
            get { return _Quelle; }
            set
            {
                if (value != _Quelle)
                {
                    _Quelle = value;
                    NotifyOfPropertyChange(() => Quelle);
                    isDirty = true;
                }
            }
        }

        private Int32? _Gebiet;
        public Int32? Gebiet
        {
            get { return _Gebiet; }
            set
            {
                if (value != _Gebiet)
                {
                    _Gebiet = value;
                    NotifyOfPropertyChange(() => Gebiet);
                    isDirty = true;
                }
            }
        }

        private Int32? _AngelegtVon;
        public Int32? AngelegtVon
        {
            get { return _AngelegtVon; }
            set
            {
                if (value != _AngelegtVon)
                {
                    _AngelegtVon = value;
                    NotifyOfPropertyChange(() => AngelegtVon);
                    isDirty = true;
                }
            }
        }

        private DateTime? _AngelegtAm;
        public DateTime? AngelegtAm
        {
            get { return _AngelegtAm; }
            set
            {
                if (value != _AngelegtAm)
                {
                    _AngelegtAm = value;
                    NotifyOfPropertyChange(() => AngelegtAm);
                    isDirty = true;
                }
            }
        }

        private Int32? _Zahlungsbedingungen;
        public Int32? Zahlungsbedingungen
        {
            get { return _Zahlungsbedingungen; }
            set
            {
                if (value != _Zahlungsbedingungen)
                {
                    _Zahlungsbedingungen = value;
                    NotifyOfPropertyChange(() => Zahlungsbedingungen);
                    isDirty = true;
                }
            }
        }

        private String _Name_Alt;
        public String Name_Alt
        {
            get { return _Name_Alt; }
            set
            {
                if (value != _Name_Alt)
                {
                    _Name_Alt = value;
                    NotifyOfPropertyChange(() => Name_Alt);
                    isDirty = true;
                }
            }
        }

        private String _Betreuer;
        public String Betreuer
        {
            get { return _Betreuer; }
            set
            {
                if (value != _Betreuer)
                {
                    _Betreuer = value;
                    NotifyOfPropertyChange(() => Betreuer);
                    isDirty = true;
                }
            }
        }

        private String _Status;
        public String Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    NotifyOfPropertyChange(() => Status);
                    isDirty = true;
                }
            }
        }

        #endregion




        #region  "Name Properties"
        //private string _name;

        //public string name
        //{
        //    get { return _name; }
        //    set
        //    {
        //        _name = value;
        //        CurrentFirma.name = value;
        //        NotifyOfPropertyChange(() => name);
        //    }
        //}
        
        #endregion



        #region "Private Property Variablen"
        private ListboxKategorienViewModel _ListboxKategorien;

        private ObservableCollection<Firmen_Kategorien> _Firmen_KategorienOC;

        //private ObservableCollection<StammFirmen_Kategorien> _Employees;
        private firma _CurrentFirma;
        private Firmen_Adressen _CurrentAdresse;
        private ObservableCollection<Firmen_Telefon> _Telefonnummern;
        private ObservableCollection<Firmen_Mailadressen> _Mailadressen;
        private ObservableCollection<Firmen_Adressen> _Adressen;

        #endregion

        #region "Properties Collections"

        public ListboxKategorienViewModel ListboxKategorien
        {
            get { return _ListboxKategorien; }
            set
            {
                _ListboxKategorien = value;
                NotifyOfPropertyChange(() => ListboxKategorien);
            }
        }

        private ListboxEigenschaftenViewModel _ListBoxEigenschaften;

        public ListboxEigenschaftenViewModel ListboxEigenschaften
        {
            get { return _ListBoxEigenschaften; }
            set
            {
                _ListBoxEigenschaften = value;
                NotifyOfPropertyChange(() => ListboxEigenschaften);
            }
        }



        public ObservableCollection<Firmen_Kategorien> Firmen_KategorienOC
        {
            get { return _Firmen_KategorienOC; }
            set
            {
                _Firmen_KategorienOC = value;
                NotifyOfPropertyChange(() => Firmen_KategorienOC);
            }
        }

        //public ObservableCollection<StammFirmen_Kategorien> Employees
        //{
        //    get { return _Employees; }
        //    set
        //    {
        //        _Employees = value;
        //        NotifyOfPropertyChange(() => Employees);
        //    }
        //}

        public ObservableCollection<Firmen_Telefon> Telefonnummern
        {
            get { return _Telefonnummern; }
            set
            {
                _Telefonnummern = value;
                NotifyOfPropertyChange(() => Telefonnummern);
            }
        }

        public Firmen_Adressen CurrentAdresse
        {
            get { return _CurrentAdresse; }
            set
            {
                _CurrentAdresse = value;
                NotifyOfPropertyChange(() => CurrentAdresse);

            }
        }

        public firma CurrentFirma
        {
            get { return _CurrentFirma; }
            set
            {
                _CurrentFirma = value;
                //SelectedEintraegeStatus = db.AuswahlEintraege.Where(e => e.Eintrag == CurrentFirma.status).SingleOrDefault();
                //name = value.name;
                NotifyOfPropertyChange(() => CurrentFirma);
            }
        }

        public ObservableCollection<Firmen_Adressen> Adressen
        {
            get { return _Adressen; }
            set
            {
                _Adressen = value;
                NotifyOfPropertyChange(() => Adressen);

            }
        }

        public ObservableCollection<Firmen_Mailadressen> Mailadressen
        {
            get { return _Mailadressen; }
            set
            {
                _Mailadressen = value;
                NotifyOfPropertyChange(() => Mailadressen);
               // CurrentFirma = db.firmen.Where(i => i.id == 19).SingleOrDefault();
            }
        }

        #endregion

        #region "Functions"



        public void Save()
        {
            db.SaveChanges();
        }





        public void AddNames()
        {
            ListboxKategorien.AddSelectedNames();
        }

        #endregion

        #region "Constructors"
        public FirmenDatenViewModel(SteinbachEntities db, firma Firma)
        {
            this.db = db;

            CurrentFirma = Firma;

            ListboxKategorien = new ListboxKategorienViewModel(CurrentFirma, db);
            ListboxEigenschaften = new ListboxEigenschaftenViewModel(CurrentFirma, db);

            Adressen = new ObservableCollection<Firmen_Adressen>(CurrentFirma.Firmen_Adressen);


            Telefonnummern = new ObservableCollection<Firmen_Telefon>(CurrentFirma.Firmen_Telefon);
            Mailadressen = new ObservableCollection<Firmen_Mailadressen>(CurrentFirma.Firmen_Mailadressen);
            //Employees = new ObservableCollection<StammFirmen_Kategorien>(db.StammFirmen_Kategorien);
            Firmen_KategorienOC = new ObservableCollection<Firmen_Kategorien>(db.Firmen_Kategorien.Where(k => k.id_Firma == CurrentFirma.id));
            this.db = db;
            this.db.SavingChanges += new EventHandler(db_SavingChanges);
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();


        }


        private void LoadFirma(int id)
        {

          

           // CurrentFirma = db.firmen.Where(i => i.id == id).SingleOrDefault();
        
            ListboxKategorien = new ListboxKategorienViewModel(CurrentFirma, db);
            ListboxEigenschaften = new ListboxEigenschaftenViewModel(CurrentFirma, db);

            Adressen = new ObservableCollection<Firmen_Adressen>(CurrentFirma.Firmen_Adressen);


            Telefonnummern = new ObservableCollection<Firmen_Telefon>(CurrentFirma.Firmen_Telefon);
            Mailadressen = new ObservableCollection<Firmen_Mailadressen>(CurrentFirma.Firmen_Mailadressen);
            //Employees = new ObservableCollection<StammFirmen_Kategorien>(db.StammFirmen_Kategorien);
            Firmen_KategorienOC = new ObservableCollection<Firmen_Kategorien>(db.Firmen_Kategorien.Where(k => k.id_Firma == CurrentFirma.id));
           // this.db = db;
            this.db.SavingChanges += new EventHandler(db_SavingChanges);
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();
        }



        [ImportingConstructor]
        public FirmenDatenViewModel(IEventAggregator events)
        {
            events.Subscribe(this);
            _events = events;
            //this.db = new SteinbachEntities();
        }
        #endregion

        #region "EventCalls"
        /// <summary>
        /// Verzögert den Aufruf der Funktion da ein direkter AUfruf im Constructor nicht funktioniert hat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            ListboxKategorien.AddSelectedNames();
            ListboxEigenschaften.AddSelectedNames();
            timer.Stop();
        }

        void db_SavingChanges(object sender, EventArgs e)
        {
            try
            {
                bool LogChanges = true;
                var om = this.db.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Modified | System.Data.EntityState.Added | System.Data.EntityState.Deleted);


                foreach (var o in om)
                {


                    if (o.Entity is firma)
                    {


                        var p = (firma)o.Entity;
                        if (p.EntityState != System.Data.EntityState.Deleted)
                        {
                            p.AngelegtAm = DateTime.Now;
                            p.AngelegtVon = Session.User.id;
                            LogChanges = true;

                        }
                        else
                        {
                            LogChanges = false;
                        }


                    }

                    if (LogChanges == true)
                    {

                        if (o.Entity is Firmen_Adressen)
                        {
                            var fa = (Firmen_Adressen)o.Entity;
                            if (fa.EntityState != System.Data.EntityState.Deleted)
                            {
                                var p = fa.firma;
                                p.AngelegtAm = DateTime.Now;
                                p.AngelegtVon = Session.User.id;
                            }
                        }

                        if (o.Entity is Firmen_Kategorien)
                        {
                            var fa = (Firmen_Kategorien)o.Entity;
                            if (fa.EntityState != System.Data.EntityState.Deleted)
                            {
                                var p = fa.firma;
                                p.AngelegtAm = DateTime.Now;
                                p.AngelegtVon = Session.User.id;
                            }
                        }

                        if (o.Entity is Firmen_Mailadressen)
                        {
                            var fa = (Firmen_Mailadressen)o.Entity;
                            if (fa.EntityState != System.Data.EntityState.Deleted)
                            {
                                var p = fa.firma;
                                p.AngelegtAm = DateTime.Now;
                                p.AngelegtVon = Session.User.id;
                            }
                        }

                        if (o.Entity is Firmen_Telefon)
                        {
                            var fa = (Firmen_Telefon)o.Entity;
                            if (fa.EntityState != System.Data.EntityState.Deleted)
                            {
                                var p = fa.firma;
                                p.AngelegtAm = DateTime.Now;
                                p.AngelegtVon = Session.User.id;
                            }
                        }



                        if (o.Entity is Firmen_Personen)
                        {

                            var fa = (Firmen_Personen)o.Entity;
                            if (fa.EntityState != System.Data.EntityState.Deleted)
                            {
                                fa.GeaendertAm = DateTime.Now;
                                fa.GeaendertVon = Session.User.id;



                                var p = fa.firma;
                                p.AngelegtAm = DateTime.Now;
                                p.AngelegtVon = Session.User.id;
                            }

                        }


                        if (o.Entity is Personen_Telefon)
                        {
                            var fa = (Personen_Telefon)o.Entity;
                            if (fa.EntityState != System.Data.EntityState.Deleted)
                            {
                                var p = fa.Firmen_Personen;
                                p.GeaendertAm = DateTime.Now;
                                p.GeaendertVon = Session.User.id;
                            }
                        }

                        if (o.Entity is Personen_Mailadressen)
                        {
                            var fa = (Personen_Mailadressen)o.Entity;
                            if (fa.EntityState != System.Data.EntityState.Deleted)
                            {
                                var p = fa.Firmen_Personen;
                                p.GeaendertAm = DateTime.Now;
                                p.GeaendertVon = Session.User.id;
                            }
                        }



                    }

                }
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);
            }

        }


        public void ListboxKategorie_Selectionchanged(SelectionChangedEventArgs e)
        {
            var query = db.Firmen_Kategorien.Where(f => f.id_Firma == CurrentFirma.id);
            if (e.AddedItems.Count > 0)
            {
                foreach (var item in e.AddedItems)
                {
                    //var kat = (StammFirmen_Kategorien)item;
                    var kategorie = new Firmen_Kategorien();
                    kategorie.id_Firma = CurrentFirma.id;
                    //kategorie.id_Kategorie = kat.id;
                    //kategorie.Kategoriename = kat.Kategoriename;
                    db.AddToFirmen_Kategorien(kategorie);
                }

            }

            if (e.RemovedItems.Count > 0)
            {
                foreach (var item in e.RemovedItems)
                {
                    //var kat = (StammFirmen_Kategorien)item;
                    var kategorie = new Firmen_Kategorien();
                    kategorie.id_Firma = CurrentFirma.id;
                    //kategorie.id_Kategorie = kat.id;
                    //kategorie.Kategoriename = kat.Kategoriename;
                    db.Firmen_Kategorien.DeleteObject(kategorie);
                }

            }

        }

        #endregion

        #region Adressen und Telefonnummern

        public void AddAdresse()
        {
            var adr = new Firmen_Adressen();
            adr.id_Firma = CurrentFirma.id;
            adr.Hauptadresse = 0;
            adr.TypGeschaeftlich = 0;
            adr.TypLieferadresse = 0;
            adr.TypRechnungsadresse = 0;
            db.AddToFirmen_Adressen(adr);
            Adressen.Add(adr);

        }

        public void AddKategorie()
        {
            var Kat = new Firmen_Kategorien();
            Kat.id_Firma = CurrentFirma.id;

            db.AddToFirmen_Kategorien(Kat);
            Firmen_KategorienOC.Add(Kat);


        }

        public void AddTelefonNummer()
        {
            var tel = new Firmen_Telefon();
            tel.id_Firma = CurrentFirma.id;
            db.AddToFirmen_Telefon(tel);
            Telefonnummern.Add(tel);



        }

        public void AddMailadresse()
        {
            var adr = new Firmen_Mailadressen();
            adr.id_Firma = CurrentFirma.id;
            db.AddToFirmen_Mailadressen(adr);
            Mailadressen.Add(adr);
        }


        public void CheckBoxHauptadresseChecked(FrameworkElement dc)
        {

            try
            {

                var x = (Firmen_Adressen)dc.DataContext;
                //foreach (var item in Adressen)
                //{
                //    if (item.id != x.id)
                //    {
                //        item.Hauptadresse = 0;
                //    }
                //}

                var query = db.Firmen_Adressen.Where(f => f.id_Firma == x.id_Firma && f.id != x.id);
                foreach (var item in query)
                {
                    item.Hauptadresse = 0;
                }

            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);
            }

        }


        public void DeleteKategorie(FrameworkElement dc)
        {

            try
            {
                // string buf = string.Empty;
                var x = (Firmen_Kategorien)dc.DataContext;
                var lookup = db.firmen.Where(f => f.id == x.id_Kategorie).SingleOrDefault();

                //var buf = lookup == null ? "Leere Kategorie" : lookup.name;


                if (MessageBox.Show(string.Format("Kategorie {0} wirklich endgültig löschen ?", lookup == null ? "Leere Kategorie" : lookup.name), "Sicherheitsabfrage",
                     MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    db.DeleteObject(x);
                    Firmen_KategorienOC.Remove(x);
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);

            }


        }

        public void DeleteAdress(FrameworkElement dc)
        {

            try
            {
                var x = (Firmen_Adressen)dc.DataContext;

                if (MessageBox.Show(string.Format("Adresse {0} {1} wirklich endgültig löschen ?", x.Straße, x.Ort), "Sicherheitsabfrage",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    db.DeleteObject(x);
                    Adressen.Remove(x);
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);
            }

        }
        public void DeleteTelefonnummer(FrameworkElement dc)
        {

            try
            {

                var x = (Firmen_Telefon)dc.DataContext;
                if (MessageBox.Show(string.Format("Telefonnummer {0} {1} wirklich endgültig löschen ?", x.Vorwahl, x.Rufnummer), "Sicherheitsabfrage",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    db.DeleteObject(x);
                    Telefonnummern.Remove(x);
                    db.SaveChanges();

                }

            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);
            }

        }
        public void DeleteMailadresse(FrameworkElement dc)
        {

            try
            {

                var x = (Firmen_Mailadressen)dc.DataContext;
                if (MessageBox.Show(string.Format("Adresse {0} {1} wirklich endgültig löschen ?", x.Typ, x.Mailadresse), "Sicherheitsabfrage",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    db.DeleteObject(x);
                    Mailadressen.Remove(x);
                    db.SaveChanges();

                }

            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);
            }

        }

        #endregion

       

       


        public void Handle(KundeChangedEvent message)
        {
            this.db = message.db;
            this.CurrentFirma = message.Firma;

            LoadFirma(message.id);
        }
    }
}
