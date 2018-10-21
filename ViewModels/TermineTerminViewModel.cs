using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SteinbachCRM.ViewModels.Interfaces;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using DAL;
using System.Windows.Threading;
using System.Windows.Controls;
using SteinbachCRM.Events;
using CommonTools.ObjectFactories;
using System.Windows;


namespace SteinbachCRM.ViewModels
{
    [Export(typeof(TermineTerminViewModel))]
    public class TermineTerminViewModel : Screen, ITermineTermin, IDataState, IHandle<TerminEvent>,IDataMethods
    {

        #region "Declarations"
        private readonly IEventAggregator _RaiseEvents;
        SteinbachEntities db;
        private DispatcherTimer timer;
        public event Action<string> OnNotifyUINewAppointment;


        #endregion

        #region "PrivatePropertiesParts"


        private CRMTermine _CurrentTermin;

        private ListboxTeilnehmerExternViewModel _ListboxTeilnehmerExtern;
        private ListboxTeilnehmerSIViewModel _ListboxTeilnehmerSI;

        #endregion

        #region "Properties"
        //private string _AppointmentType;

        //public string AppointmentType
        //{
        //    get { return _AppointmentType; }
        //    set
        //    {
        //        _AppointmentType = value;
        //        NotifyOfPropertyChange(() => AppointmentType);
        //    }
        //}

        //private C1.WPF.DateTimeEditors.C1DateTimePickerEditMode _DateTimePickerFormat;

        //public C1.WPF.DateTimeEditors.C1DateTimePickerEditMode DateTimePickerFormat
        //{
        //    get { return _DateTimePickerFormat; }
        //    set
        //    {
        //        _DateTimePickerFormat = value;
        //        NotifyOfPropertyChange(() => DateTimePickerFormat); 
        //    }
        //}

        //private int _isWholeday;

        //public int isWholeDay
        //{
        //    get { return _isWholeday; }
        //    set 
        //    {

        //        _isWholeday = value;
        //        DateTimePickerFormat = _isWholeday == 1 ? C1.WPF.DateTimeEditors.C1DateTimePickerEditMode.Date : C1.WPF.DateTimeEditors.C1DateTimePickerEditMode.DateTime;

        //        NotifyOfPropertyChange(() => isWholeDay);
        //    }
        //}


        //public bool isDirty
        //{
        //    get { return false; }
        //}

        public ListboxTeilnehmerSIViewModel ListboxTeilnehmerSI
        {
            get { return _ListboxTeilnehmerSI; }
            set
            {
                _ListboxTeilnehmerSI = value;
                NotifyOfPropertyChange(() => ListboxTeilnehmerSI);
            }
        }

        public ListboxTeilnehmerExternViewModel ListboxTeilnehmerExtern
        {
            get { return _ListboxTeilnehmerExtern; }
            set
            {
                _ListboxTeilnehmerExtern = value;
                NotifyOfPropertyChange(() => ListboxTeilnehmerExtern);
            }
        }
        public CRMTermine CurrentTermin
        {
            get { return _CurrentTermin; }
            set
            {
                _CurrentTermin = value;
                //AppointmentType = value.AppointmentType;
                //DateTimePickerFormat = CurrentTermin.Ganztaegig == 1 ? C1.WPF.DateTimeEditors.C1DateTimePickerEditMode.Date : C1.WPF.DateTimeEditors.C1DateTimePickerEditMode.DateTime;
                NotifyOfPropertyChange(() => CurrentTermin);
            }
        }

        public DateTime? TerminVonOld { get; set; }
        public DateTime? TerminBisOld { get; set; }
        public DateTime? TerminErinnerungOld { get; set; }

        #endregion

        #region "Constrcuctors"

        /// <summary>
        /// Von PopupViewModels genutzt im View First Modus
        /// </summary>
        /// <param name="termin">Gewähltes Terminobjekt </param>
        /// <param name="db">ObjectContext </param>
        public TermineTerminViewModel(CRMTermine termin, SteinbachEntities db)
        {

            CurrentTermin = termin;
            this.db = db;
            PopulateListboxes(CurrentTermin, db);

        }

        [ImportingConstructor]
        public TermineTerminViewModel(IEventAggregator events)
        {
            events.Subscribe(this);
            _RaiseEvents = events;
        }

        #endregion

        #region "Functions"

        //public void RejectChanges()
        //{


        //    if (MessageBox.Show("Bearbeitung wirklich abbrechen ?", "Sicherheitsabfrage", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //    {
        //        db.Dispose();
        //        db = null;
        //        if (_RaiseEvents != null)
        //        {
        //            _RaiseEvents.Publish(new SelectUIEvent(true, SelectUIEvent.EnumActivateModule.Kunde));
        //            _RaiseEvents.Publish(new ConfigureUITermineDaten(0, "10"));
        //        }


        //    }


        //    //if (CommonTools.Tools.ManageChanges.isDirty(db))
        //    //{
        //    //    List<string> mod = CommonTools.Tools.ManageChanges.GetModifiedProperties(db);

        //    //    if (MessageBox.Show("Änderungen speichern ?", "Sicherheitsabfrage", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //    //    {
        //    //        db.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
        //    //        _RaiseEvents.Publish(new SelectUIEvent(true, SelectUIEvent.EnumActivateModule.Kunde));
        //    //        _RaiseEvents.Publish(new ConfigureUITermineDaten(0, "10"));
        //    //        _RaiseEvents.Publish(new TerminSavedEvent(CurrentTermin.id));
        //    //    }
        //    //    else
        //    //    {
        //    //        db.Dispose();
        //    //        db = null;
        //    //        _RaiseEvents.Publish(new SelectUIEvent(true, SelectUIEvent.EnumActivateModule.Kunde));
        //    //        _RaiseEvents.Publish(new ConfigureUITermineDaten(0, "10"));
        //    //    }
        //    //}

        //    //else
        //    //{
        //    //    db.Dispose();
        //    //    db = null;
        //    //    _RaiseEvents.Publish(new SelectUIEvent(true, SelectUIEvent.EnumActivateModule.Kunde));
        //    //    _RaiseEvents.Publish(new ConfigureUITermineDaten(0, "10"));
        //    //}


        //}

        //public void SaveChanges()
        //{

        //    db.SaveChanges();
        //    if (_RaiseEvents != null)
        //    {
        //        _RaiseEvents.Publish(new SelectUIEvent(true, SelectUIEvent.EnumActivateModule.Kunde));
        //        _RaiseEvents.Publish(new ConfigureUITermineDaten(0, "10"));
        //        _RaiseEvents.Publish(new TerminSavedEvent(CurrentTermin.id));
        //    }


        //}


        /// <summary>
        /// Beide Listboxen Teilnehmer füllen - Mehrfachauswahl deshalb kein Binding 
        /// </summary>
        /// <param name="termin"></param>
        /// <param name="db"></param>
        private void PopulateListboxes(CRMTermine termin, SteinbachEntities db)
        {
            ListboxTeilnehmerSI = new ListboxTeilnehmerSIViewModel(termin, db);
            ListboxTeilnehmerExtern = new ListboxTeilnehmerExternViewModel(termin, db);
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();

        }

        void NotifyUINewAppointment(string type)
        {
            if (OnNotifyUINewAppointment != null)
            {
                OnNotifyUINewAppointment(type);
            }

        }

        private bool isReminderEnabled()
        {
            return CurrentTermin.Erinnerung == 1 ? true : false;

        }

        #endregion

        #region "Events"


        public void TerminVonDateTimeChanged(C1.WPF.DateTimeEditors.NullablePropertyChangedEventArgs<DateTime> e)
        {
            if (TerminVonOld != null)
            {

                DateTime Neu = (DateTime)e.NewValue;
                DateTime Alt = (DateTime)TerminVonOld;
                TimeSpan tsVon = Neu - Alt;
                if (CurrentTermin.ErinerungDatum != null)
                {

                    CurrentTermin.ErinerungDatum = ((DateTime)CurrentTermin.ErinerungDatum).Add(tsVon);
                }

                CurrentTermin.TerminBis = ((DateTime)CurrentTermin.TerminBis).Add(tsVon);


            }

            TimeSpan ts = (TimeSpan)(CurrentTermin.TerminBis - CurrentTermin.TerminVon);
            CurrentTermin.TerminDauer = ts.Ticks;

            TerminVonOld = CurrentTermin.TerminVon;

        }

        public void TerminBisDateTimeChanged(C1.WPF.DateTimeEditors.NullablePropertyChangedEventArgs<DateTime> e)
        {

            if (TerminBisOld != null)
            {
                DateTime dt = (DateTime)e.NewValue;
                if (CurrentTermin.TerminVon >= dt)
                {
                    MessageBox.Show("Terminende kann nicht vor Terminbegin liegen.", "Plausibilität");

                    CurrentTermin.TerminBis = TerminBisOld;
                }
            }

            TimeSpan ts = (TimeSpan)(CurrentTermin.TerminBis - CurrentTermin.TerminVon);
            CurrentTermin.TerminDauer = ts.Ticks;
            TerminBisOld = CurrentTermin.TerminBis;

        }

        public void TerminErinnerungDateTimeChanged(C1.WPF.DateTimeEditors.NullablePropertyChangedEventArgs<DateTime> e)
        {
            if (TerminErinnerungOld != null)
            {
                DateTime dt = (DateTime)e.NewValue;
                DateTime dtNew = dt.AddMinutes(-15);

                if (CurrentTermin.TerminVon < dtNew)
                {
                    MessageBox.Show("Terminerinnerung muß mindestens 15 Minuten vor Terminbeginn liegen.", "Plausibilität");
                    CurrentTermin.ErinerungDatum = TerminErinnerungOld;
                }
            }

            TerminErinnerungOld = CurrentTermin.ErinerungDatum;
        }




        public void TerminFcbSelectionChanged(SelectionChangedEventArgs e)
        {
            var remind = (AuswahlEintraege)e.AddedItems[0];
            int ah = ((int)remind.ai_int) * -1;
            CurrentTermin.ErinerungDatum = ((DateTime)CurrentTermin.TerminVon).AddMinutes(ah);



        }


        public void Handle(TerminEvent message)
        {
            TerminVonOld = null;
            TerminBisOld = null;
            TerminErinnerungOld = null;


            db = new SteinbachEntities();
            if (message.TerminID > 0)
            {

                CurrentTermin = db.CRMTermine.Where(t => t.id == message.TerminID).SingleOrDefault();
                TerminVonOld = null;
                TerminBisOld = null;
                TerminErinnerungOld = null;
                NotifyUINewAppointment(CurrentTermin.AppointmentType);

            }
            else
            {

                CurrentTermin = EntitiesFactory.GetNewTermin(message.TerminTyp);
                NotifyUINewAppointment(CurrentTermin.AppointmentType);
                db.AddToCRMTermine(CurrentTermin);

            }


            PopulateListboxes(CurrentTermin, db);
        }




        /// <summary>
        /// Verzögertes Starten der Auswahl der selektierten Teilnehmer - direkter Aufruf in PopulateListboxes funktioniert nicht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            ListboxTeilnehmerSI.AddSelectedNames();
            ListboxTeilnehmerExtern.AddSelectedNames();

            timer.Stop();
        }

        /// <summary>
        /// Updaten der ListboxTeilehmerExtern nach Wechsel der Firma in der Listbox 
        /// </summary>
        /// <param name="e"></param>
        public void FirmaSelectionChanged(SelectionChangedEventArgs e)
        {
            var box = (ComboBox)e.Source;
            if (box.SelectedItem != null)
            {
                var f = (firma)box.SelectedItem;
                ListboxTeilnehmerExtern.UpdateAvailableNames(f.id);
            }

        }

        public void FirmenOnFilterChanged(CommonTools.UserControls.FilteredComboBoxChangedEventArgs e)
        {
            // wird bis auf weiteres in View - Codebehind gehandelt. Bis Klärung Combobox ItemsSource aus Viewmodel 

        }



        #endregion

        public bool isDirty
        {
            get
            {
                return CommonTools.Tools.ManageChanges.isDirty(db);
            }
            set
            {
                
            }
        }

        public bool SaveChanges()
        {
            db.SaveChanges();
            if (_RaiseEvents != null)
            {
                _RaiseEvents.Publish(new SelectUIEvent(true, SelectUIEvent.EnumActivateModule.Kunde));
                _RaiseEvents.Publish(new ConfigureUITermineDaten(0, "10"));
                _RaiseEvents.Publish(new TerminSavedEvent(CurrentTermin.id));
            }

            return true;
        }

        public bool RejectChanges()
        {
            if (MessageBox.Show("Bearbeitung wirklich abbrechen ?", "Sicherheitsabfrage", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.Dispose();
                db = null;
                if (_RaiseEvents != null)
                {
                    _RaiseEvents.Publish(new SelectUIEvent(true, SelectUIEvent.EnumActivateModule.Kunde));
                    _RaiseEvents.Publish(new ConfigureUITermineDaten(0, "10"));
                }
                return true;

            }

            return false;
        }

        public event System.Action DoRejectChanges;

    }

   

}
