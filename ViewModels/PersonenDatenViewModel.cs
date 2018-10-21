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
using C1.WPF.DataGrid;
using SteinbachCRM.Events;
using System.Text.RegularExpressions;
using AutoMapper;
using System.Diagnostics;
using System.Windows.Threading;


namespace SteinbachCRM.ViewModels
{
    [Export(typeof(PersonenDatenViewModel))]
    public class PersonenDatenViewModel : Screen, IPersonenDaten, IHandle<KundeChangedEvent>, IDataState
    {
        bool isLoop = false;
        SteinbachEntities db;
        IEventAggregator _events;
        public DispatcherTimer timer;

        private List<Firmen_TelefonViewModel> DeletedPhoneNumbers;

        #region "Properties Private Part"

        private ObservableCollection<Firmen_Personen> _PersonenAuswahl;

        private ObservableCollection<Firmen_Personen> _ContentCombobox;

        private bool _TelefonnummernExpanded;
        private bool _PersonenExpanded;
        private bool _MailadressenExpanded;
        private Firmen_Personen _CurrentPerson;
        private Firmen_PersonenViewModel firmen_PersonenViewModel;
        private firma _CurrentFirma;
        private ObservableCollection<Firmen_Personen> _Personenliste;
        private ObservableCollection<Personen_Telefon> _Telefonnummern;
        private ObservableCollection<Personen_Mailadressen> _Mailadressen;


        #endregion

        #region "Properties"

    


        public bool MailadressenExpanded
        {
            get { return _MailadressenExpanded; }
            set
            {
                _MailadressenExpanded = value;
                NotifyOfPropertyChange(() => MailadressenExpanded);
            }
        }

        public bool PersonenExpanded
        {
            get { return _PersonenExpanded; }
            set
            {
                _PersonenExpanded = value;
                NotifyOfPropertyChange(() => PersonenExpanded);

            }
        }

        public bool TelefonnummernExpanded
        {
            get { return _TelefonnummernExpanded; }
            set
            {
                _TelefonnummernExpanded = value;
            }
        }


        public Firmen_Personen CurrentPerson
        {
            get { return _CurrentPerson; }
            set
            {
                _CurrentPerson = value;
                NotifyOfPropertyChange(() => CurrentPerson);
            }
        }

        public ObservableCollection<Firmen_Personen> PersonenAuswahl
        {
            get
            {
                if (_PersonenAuswahl == null)
                {
                    _PersonenAuswahl = new ObservableCollection<Firmen_Personen>();
                }
                return _PersonenAuswahl;
            }
            set
            {
                _PersonenAuswahl = value;
                NotifyOfPropertyChange(() => PersonenAuswahl);
            }
        }


        private ObservableCollection<Firmen_TelefonViewModel> _VMFirmenTelefon;
        public ObservableCollection<Firmen_TelefonViewModel> VMFirmenTelefon
        {
            get { return _VMFirmenTelefon; }
            set
            {
                if (value != _VMFirmenTelefon)
                {
                    _VMFirmenTelefon = value;
                    NotifyOfPropertyChange(() => VMFirmenTelefon);
                    // isDirty = true;
                }
            }
        }

        private Firmen_TelefonViewModelCollection _VM_FT_Colection;
        public Firmen_TelefonViewModelCollection VM_FT_Colection
        {
            get { return _VM_FT_Colection; }
            set
            {
                if (value != _VM_FT_Colection)
                {
                    _VM_FT_Colection = value;
                    NotifyOfPropertyChange(() => VM_FT_Colection);
                    //isDirty = true;
                }
            }
        }




        private Firmen_TelefonViewModel _SelectedVMFirmenTelefon;
        public Firmen_TelefonViewModel SelectedVMFirmenTelefon
        {
            get { return _SelectedVMFirmenTelefon; }
            set
            {
                if (value != _SelectedVMFirmenTelefon)
                {
                    _SelectedVMFirmenTelefon = value;
                    NotifyOfPropertyChange(() => SelectedVMFirmenTelefon);
                    //isDirty = true;
                }
            }
        }






        public ObservableCollection<Firmen_Personen> ContentCombobox
        {
            get { return _ContentCombobox; }
            set
            {
                _ContentCombobox = value;
                NotifyOfPropertyChange(() => ContentCombobox);
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

        public Firmen_PersonenViewModel Firmen_PersonenViewModel
        {
            get { return firmen_PersonenViewModel; }
            set
            {
                firmen_PersonenViewModel = value;
                NotifyOfPropertyChange(() => Firmen_PersonenViewModel);
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


        #endregion

        #region "Constructors"

        [ImportingConstructor]
        public PersonenDatenViewModel(IEventAggregator events)
        {
            events.Subscribe(this);
            _events = events;
        }

        private void LoadFirma(int id)
        {
            this.db = new SteinbachEntities();
            CurrentFirma = db.firmen.Where(f => f.id == id).SingleOrDefault();

            Personenliste = new ObservableCollection<Firmen_Personen>(CurrentFirma.Firmen_Personen.OrderBy(o => o.Nachname));
            ContentCombobox = new ObservableCollection<Firmen_Personen>(CurrentFirma.Firmen_Personen.OrderBy(o => o.Nachname));
           // VMFirmenTelefon = new ObservableCollection<Firmen_TelefonViewModel>();
            //Mapper.CreateMap<Personen_Telefon, Firmen_TelefonViewModel>();
            MailadressenExpanded = true;
            TelefonnummernExpanded = true;
            PersonenExpanded = true;

        }

        public PersonenDatenViewModel(SteinbachEntities db, firma Firma)
        {
            this.db = db;
            CurrentFirma = Firma;

            Personenliste = new ObservableCollection<Firmen_Personen>(CurrentFirma.Firmen_Personen.OrderBy(o => o.Nachname));
            ContentCombobox = new ObservableCollection<Firmen_Personen>(CurrentFirma.Firmen_Personen.OrderBy(o => o.Nachname));
          //  VMFirmenTelefon = new ObservableCollection<Firmen_TelefonViewModel>();
            VM_FT_Colection = new Firmen_TelefonViewModelCollection();
           

          

            // Mapper.CreateMap<Personen_Telefon, Firmen_TelefonViewModel>();

            MailadressenExpanded = true;
            TelefonnummernExpanded = true;
            PersonenExpanded = true;

            
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);

        }

        public PersonenDatenViewModel(int id_Firma)
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

            Personenliste = new ObservableCollection<Firmen_Personen>(CurrentFirma.Firmen_Personen.OrderBy(o => o.Nachname));
            ContentCombobox = new ObservableCollection<Firmen_Personen>(CurrentFirma.Firmen_Personen.OrderBy(o => o.Nachname));


            MailadressenExpanded = true;
            TelefonnummernExpanded = true;
            PersonenExpanded = true;

        }

        #endregion

        #region "Functions"

        public void Save()
        {
            try
            {

                VM_FT_Colection.Save();


                //foreach (var item in VMFirmenTelefon)
                //{
                //    item.SaveChanges(db);
                //}



                //foreach (var item in DeletedPhoneNumbers)
                //{
                //    var pers = Firmen_TelefonViewModel.GetPersonTelefon(db, item);
                //    db.DeleteObject(pers);
                //    DeletedPhoneNumbers.Remove(item);
                //}


                //  db.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex, true);
            }
        }


        bool LoadPerson(Firmen_Personen pers)
        {
            CurrentPerson = pers;



            if (pers != null)
            {
                LoadCheckBoxListBoxes();

                if (VM_FT_Colection != null)
                {
                    if (VM_FT_Colection.isDirty)
                    {
                        MessageBoxResult res = MessageBox.Show("Änderungen speichern ?", "", MessageBoxButton.YesNoCancel);

                        switch (res)
                        {
                            case MessageBoxResult.Cancel:
                                {
                                    return false;
                                    //break;
                                }

                            case MessageBoxResult.No:
                                {

                                    LoadPerson_Execute(pers);
                                    return true;
                                   // break;
                                }

                            case MessageBoxResult.Yes:
                                {

                                    VM_FT_Colection.Save();
                                    LoadPerson_Execute(pers);
                                    return true;

                                    //break;
                                }

                            default:
                                return true;
                                //break;
                        }


                    }
                    else
                    {
                        LoadPerson_Execute(pers);
                        return true;
                    }

                  
                }
               
             

            }
            return false;
        }

        private void LoadCheckBoxListBoxes()
        {
           CurrentPerson.ListboxPersonenKategorienVM = new DAL.ViewModels.ListboxPersonenKategorienViewModel(CurrentPerson, db);
           CurrentPerson.ListboxPersonenEventsVM = new DAL.ViewModels.ListboxPersonenEventsViewModel(CurrentPerson, db); 

            
            timer.Start();
        }

        private void LoadPerson_Execute(Firmen_Personen pers)
        {
            //  CurrentPerson = db.Firmen_Personen.Where(p => p.id == pers.id).SingleOrDefault();

            PersonenAuswahl = new ObservableCollection<Firmen_Personen>(db.Firmen_Personen.Where(p => p.id == pers.id));
            Telefonnummern = new ObservableCollection<Personen_Telefon>(pers.Personen_Telefon);

            Mailadressen = new ObservableCollection<Personen_Mailadressen>(pers.Personen_Mailadressen);
            VM_FT_Colection = new Firmen_TelefonViewModelCollection(pers.Personen_Telefon, db);

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
           CurrentPerson.ListboxPersonenKategorienVM.AddSelectedNames();
           CurrentPerson.ListboxPersonenEventsVM.AddSelectedNames();

            timer.Stop();
        }

        public void MakePhoneCall(FrameworkElement dc)
        {

            try
            {
                var vm = (Firmen_TelefonViewModel)dc.DataContext;

                var x = Firmen_TelefonViewModel.GetPersonTelefon(db, vm);
                //var buf = CommonTools.Tools.GetRegex.GetPhoneNumber(x.Dialnumber);
                //MessageBox.Show(buf);

                if (x.Typ != 16)  //Fax
                {
                    TapiLib.CommonCalls.MakeDialerCall(x.Dialnumber, "Steinbach CRM", x.Firmen_Personen.Fullname, "Anruf aus CRM initiiert");
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



        public void CurrentPerson_SelectionChanged(FrameworkElement ListView)
        {
            //var view = (ListView)ListView;
            //var person = (Firmen_Personen)view.SelectedItem;
            //LoadPerson(person);

            //CurrentPerson = person;
            //if (CurrentPerson != null)
            //{
            //    Telefonnummern = new ObservableCollection<Personen_Telefon>(person.Personen_Telefon);
            //    Mailadressen = new ObservableCollection<Personen_Mailadressen>(person.Personen_Mailadressen);
            //}


        }












        public void ComboboxPersonen_SelectionChanged(FrameworkElement ComboBox)
        {
          
            if (isLoop == true)
            {
                isLoop = false;
                return; 
            }

            var pers = CurrentPerson;
            var box = (ComboBox)ComboBox;
            var person = (Firmen_Personen)box.SelectedItem;

            if (LoadPerson(person)== false)
            {
                isLoop = true;
                box.SelectedItem = pers;


            }


           


        }

        #endregion


        #region "Add"

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



        public void AddTelefonNummer()
        {
            if (CurrentPerson != null)
            {
                //  var tel = new Personen_Telefon();

                //  //CurrentPerson.Personen_Telefon.Add(tel);

                //  //db.AddToPersonen_Telefon(tel);
                //  //Telefonnummern.Add(tel);

                //  Firmen_TelefonViewModel vm = new Firmen_TelefonViewModel();
                //  vm.id_person = CurrentPerson.id;

                ////  Mapper.Map<Personen_Telefon, Firmen_TelefonViewModel>(tel, vm);
                // // VMFirmenTelefon.Add(vm);
                //  VM_FT_Colection.Insert(0, vm);
                // VM_FT_Colection.AddItem(CurrentPerson.id);
                VM_FT_Colection.AddItem(CurrentPerson);

            }


        }


        public void AddPerson()
        {
            if (CurrentPerson != null)
            {
                PersonenAuswahl.Remove(CurrentPerson);
            }

            var pers = new Firmen_Personen();
            pers.Betreuer = DAL.Session.User.benutzername;
            pers.Newsletter = 0;
            pers.Weihnachtskarte = 0;
            pers.Messeeinladung = 0;
            pers.Nachname = "neu . . . ";
            pers.ErstKontakt = Session.User.id;
            pers.created = DateTime.Now;

            using (var se = new SteinbachEntities())
            {
                se.AddToFirmen_Personen(pers);
                se.SaveChanges();
            }



            Firmen_Personen Person = db.Firmen_Personen.Where(n => n.id == pers.id).SingleOrDefault();
            if (Person != null)
            {
                CurrentFirma.Firmen_Personen.Add(Person);
                PersonenAuswahl.Add(Person);
                LoadPerson(Person);
            }



            //CurrentPerson = pers;
            //Telefonnummern = new ObservableCollection<Personen_Telefon>(pers.Personen_Telefon);
            //Mailadressen = new ObservableCollection<Personen_Mailadressen>(pers.Personen_Mailadressen);




        }

        #endregion

        #region "Delete"
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
                    //db.SaveChanges();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));

            }

        }


        public void DeleteTelefonnummer(FrameworkElement dc)
        {

            try
            {

                Firmen_TelefonViewModel vmx = (Firmen_TelefonViewModel)dc.DataContext;

                if (MessageBox.Show(string.Format("Telefonnummer {0} {1} wirklich endgültig löschen ?", vmx.Vorwahl, vmx.Rufnummer), "Sicherheitsabfrage",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    //  if (DeletedPhoneNumbers == null)
                    //  {
                    //      DeletedPhoneNumbers = new List<Firmen_TelefonViewModel>();
                    //  }

                    ////  Personen_Telefon x = Firmen_TelefonViewModel.GetPersonTelefon(db, vmx);

                    // // db.DeleteObject(x);
                    // // Telefonnummern.Remove(x);
                    //  DeletedPhoneNumbers.Add(vmx);
                    //  VMFirmenTelefon.Remove(vmx);
                    //  // db.SaveChanges();

                    _VM_FT_Colection.DeleteItem(vmx);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));

            }

        }

        public void DeletePerson(FrameworkElement dc)
        {

            try
            {

                var x = (Firmen_Personen)dc.DataContext;
                if (MessageBox.Show(string.Format("Adresse {0} {1} wirklich endgültig löschen ?", x.Vorname, x.Nachname), "Sicherheitsabfrage",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    var pList = x.Personen_Telefon.ToList();
                    foreach (var pp in pList)
                    {
                        db.DeleteObject(pp);
                        Telefonnummern.Remove(pp);
                        VM_FT_Colection.Clear();
                    }

                    var maList = x.Personen_Mailadressen.ToList();
                    foreach (var maa in maList)
                    {
                        db.DeleteObject(maa);
                        Mailadressen.Remove(maa);

                    }

                    var crmMailList = x.CRMEmails.ToList();
                    foreach (var cm in crmMailList)
                    {
                        cm.id_Kontakt = null;
                       // db.DeleteObject(cm);

                    }

                    var TermineTeilnehmer = x.Termin_TeilnehmerExtern.ToList();
                    foreach (var TT in TermineTeilnehmer)
                    {
                        db.DeleteObject(TT);
                    }

                    var KundenbesucheTeilnehmerExtern = x.Kundenbesuche_TeilnehmerExtern.ToList();
                    foreach (var KTE in KundenbesucheTeilnehmerExtern)
                    {
                        db.DeleteObject(KTE);
                    }

                    var KundenbesucheKontakt = x.Firmen_Kundenbesuche.ToList();
                    foreach (var fkb in KundenbesucheKontakt)
                    {
                        fkb.id_kontakt = null;
                       // db.DeleteObject(fkb);
                    }

                    db.DeleteObject(x);

                    PersonenAuswahl.Remove(x);
                    Personenliste.Remove(x);
                    CurrentPerson = null;
                    Telefonnummern = new ObservableCollection<Personen_Telefon>();
                    Mailadressen = new ObservableCollection<Personen_Mailadressen>();
                    VM_FT_Colection = new Firmen_TelefonViewModelCollection();
                    //db.SaveChanges();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));

            }

        }

        #endregion







        public void Handle(KundeChangedEvent message)
        {
            LoadFirma(message.id);
        }

        public bool isDirty
        {

            get
            {
                bool dirty = false;
                //bool vmDirty = false;
                //foreach (var item in VMFirmenTelefon)
                //{
                //    if (item.isDirty)
                //    {
                //        vmDirty = true;
                //    }
                //}

                if (VM_FT_Colection != null)
                {
                    dirty = CommonTools.Tools.ManageChanges.isDirty(db) || VM_FT_Colection.isDirty;
                }
                else
                {
                    dirty = CommonTools.Tools.ManageChanges.isDirty(db);
                }



                return dirty;

            }
        }
    }
}
