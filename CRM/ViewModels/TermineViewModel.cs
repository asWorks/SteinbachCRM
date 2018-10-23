using System;
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


namespace SteinbachCRM.ViewModels
{
    [Export(typeof(ICRM_Termine))]
    class TermineViewModel : Screen, ICRM_Termine
    {

        #region "Declarations"


        public DispatcherTimer timer;
        SteinbachEntities db;

        #endregion

        #region "Private Property Variablen"
    

     
        private ListboxKategorienViewModel _ListboxKategorien;





        #endregion

        #region "Properties"

        private string _TestContent;

        public string TestContent
        {
            get { return "Test"; }
            set
            {
                _TestContent = value;
                NotifyOfPropertyChange(() => TestContent);
            }
        }
        

        public ListboxKategorienViewModel ListboxKategorien
        {
            get { return _ListboxKategorien; }
            set
            {
                _ListboxKategorien = value;
                NotifyOfPropertyChange(() => ListboxKategorien);
            }
        }

        private ListboxTeilnehmerExternViewModel _ListboxTeilnehmerExtern;

        public ListboxTeilnehmerExternViewModel ListboxTeilnehmerExtern
        {
            get { return _ListboxTeilnehmerExtern; }
            set
            {
                _ListboxTeilnehmerExtern = value;
                NotifyOfPropertyChange(() => ListboxTeilnehmerExtern);
            }
        }
            private ListboxTeilnehmerSIViewModel _ListboxTeilnehmerSI;

        public ListboxTeilnehmerSIViewModel ListboxTeilnehmerSI
        {
            get { return _ListboxTeilnehmerSI; }
            set
            {
                _ListboxTeilnehmerSI = value;
                NotifyOfPropertyChange(()=>ListboxTeilnehmerSI);
            }
        }
        
   private CRMTermine _CurrentTermin;
        public CRMTermine CurrentTermin
        {
            get { return _CurrentTermin; }
            set
            {
                _CurrentTermin = value;
                NotifyOfPropertyChange(() => CurrentTermin);
            }
        }
        






        #endregion

        #region "Functions"



        //public void SaveChanges()
        //{
        //    db.SaveChanges();
        //}





        public void AddNames()
        {
            ListboxKategorien.AddSelectedNames();
        }

        #endregion

        #region "Constructors"
        public TermineViewModel(SteinbachEntities db, CRMTermine termin)
        {

            TestContent = "Constructor 1 Called:";
            this.db = db;
            CurrentTermin = termin;
            ListboxTeilnehmerSI = new ListboxTeilnehmerSIViewModel(termin, db);
            ListboxTeilnehmerExtern = new ListboxTeilnehmerExternViewModel(termin, db);

            this.db.SavingChanges += new EventHandler(db_SavingChanges);
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();



        }



        public TermineViewModel()
        {

            TestContent = "Constructor 2 Called:";
            db = new SteinbachEntities();
            CRMTermine termin = new CRMTermine();
            termin = db.CRMTermine.Where(t => t.id == 5).SingleOrDefault();
            ListboxTeilnehmerSI = new ListboxTeilnehmerSIViewModel(termin, db);
            ListboxTeilnehmerExtern = new ListboxTeilnehmerExternViewModel(termin, db);

            // db.AddToCRMTermine(termin);
            CurrentTermin = termin;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();


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
            ListboxTeilnehmerSI.AddSelectedNames();
            ListboxTeilnehmerExtern.AddSelectedNames();
            
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


        public void FirmaSelectionChanged(SelectionChangedEventArgs e)
        {
            var box = (ComboBox)e.Source;
            var f = (firma)box.SelectedItem;
            ListboxTeilnehmerExtern.UpdateAvailableNames(f.id);

 
        }



    

        bool isDirty
        {
            get
            {
                return false;
            }
            set
            {
               
            }
        }

        bool SaveChanges()
        {
            db.SaveChanges();
            return true;
        }

        public bool RejectChanges()
        {
            return true;
        }
    }
}
        #endregion

    

     