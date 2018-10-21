using System;
using System.Linq;
using SteinbachCRM.ViewModels.Interfaces;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using DAL;
using System.Collections.ObjectModel;
using System.Windows;
using SteinbachCRM.Events;
using CommonTools.Tools;
using SteinbachCRM.Views;




namespace SteinbachCRM.ViewModels
{


    [Export(typeof(TermineListeViewModel))]
    public class TermineListeViewModel : Screen, ITermineListe, IDataState, IHandle<TerminSavedEvent>
    {
        private readonly IEventAggregator _events;
        //public event Action<int> onTerminChanged;
        SteinbachEntities db;

        private ObservableCollection<AuswahlEintraege> _ActionLookUp;

        public ObservableCollection<AuswahlEintraege> ActionLookUp
        {
            get { return _ActionLookUp; }
            set
            {
                _ActionLookUp = value;
                NotifyOfPropertyChange(() => ActionLookUp);
            }
        }

        private ObservableCollection<CRMTermine> _TermineListe;

        public ObservableCollection<CRMTermine> TermineListe
        {
            get { return _TermineListe; }
            set
            {
                _TermineListe = value;
                NotifyOfPropertyChange(() => TermineListe);
            }
        }

        private DateTime _DatumVon;

        public DateTime DatumVon
        {
            get { return _DatumVon; }
            set
            {
                _DatumVon = value;
                NotifyOfPropertyChange(() => DatumVon);
                if (DatumBis < value)
                {
                    DatumBis = value.AddDays(7);
                }
                else
                {

                    RefreshTermine();
                }

            }
        }

        private void RefreshTermine()
        {
            DateTime von = DateTools.GetDateWithCustomHour(DatumVon, 00, 00, 0);
            DateTime bis = DateTools.GetDateWithCustomHour(DatumBis, 23, 59, 00);
            LoadTermine(von, bis);
        }

        private DateTime _DatumBis;

        public DateTime DatumBis
        {
            get { return _DatumBis; }
            set
            {
                _DatumBis = value;
                NotifyOfPropertyChange(() => DatumBis);

                if (DatumVon > value)
                {
                    DatumVon = value.AddDays(-7);
                }
                else
                {
                    RefreshTermine();
                  
                }

            }
        }

        private bool _TemineIsChecked;

        public bool TermineIsChecked
        {
            get { return _TemineIsChecked; }
            set
            {
                _TemineIsChecked = value;
                NotifyOfPropertyChange(() => TermineIsChecked);
                RefreshTermine();
            }
        }
        

        [ImportingConstructor]
        public TermineListeViewModel(IEventAggregator Events)
        {
            _events = Events;
            Events.Subscribe(this);
            db = new SteinbachEntities();
            ActionLookUp = new ObservableCollection<AuswahlEintraege>(db.AuswahlEintraege.Where(t => t.Gruppe == "TerminAktion"));
            TermineIsChecked = true;
            int back = ConfigEntry<int>.GetConfigEntry("AppointmentListLookback", "0", "Rückblick Termine in Tagen");
            int ahead = ConfigEntry<int>.GetConfigEntry("AppointmentListLookahead", "14", "Vorschau Termine in Tagen");


            DatumVon = DateTime.Now.AddDays(back);
            DatumBis = DateTime.Now.AddDays(ahead);
            DateTime von = DateTools.GetDateWithCustomHour(DatumVon, 00, 00, 0);
            DateTime bis = DateTools.GetDateWithCustomHour(DatumBis, 23, 59, 00);
           
            LoadTermine(von, bis);

            // LoadTermine();




        }

        public void LoadTermine(DateTime VDate, DateTime bDate)
        {

            db.Refresh(System.Data.Objects.RefreshMode.StoreWins, db.CRMTermine);
            if (TermineIsChecked)
            {
                //TermineListe = new ObservableCollection<CRMTermine>(db.CRMTermine.Where(t => t.TerminVon >= VDate && t.TerminVon <= bDate && t.AppointmentType == "Termin").OrderBy(t => t.TerminVon));
                TermineListe = new ObservableCollection<CRMTermine>(db.CRMTermine.Where(t => t.TerminBis >= VDate && t.TerminVon <= bDate && t.AppointmentType == "Termin").OrderBy(t => t.TerminVon));
            }
            else
            {
                TermineListe = new ObservableCollection<CRMTermine>(db.CRMTermine.Where(t => t.TerminBis >= VDate && t.TerminVon <= bDate && t.AppointmentType == "Aufgabe" && t.Angelegt == DAL.Session.User.id).OrderBy(t => t.TerminVon));
            }
            

        }

        public void Termin_SelectionChanged(C1.WPF.DataGrid.C1DataGrid grid)
        {
        }



        public void TerminNeu()
        {
            _events.Publish(new TerminEvent(0, null, "Termin"));
            _events.Publish(new ConfigureUITermineDaten(1, "01"));
            _events.Publish(new SelectUIEvent(false, SelectUIEvent.EnumActivateModule.None));
        }

        public void AufgabeNeu()
        {
            _events.Publish(new TerminEvent(0, null, "Aufgabe"));
            _events.Publish(new ConfigureUITermineDaten(1, "01"));
            _events.Publish(new SelectUIEvent(false, SelectUIEvent.EnumActivateModule.None));
        }


        public void goBack()
        {
            _events.Publish(new SelectUIEvent(true, SelectUIEvent.EnumActivateModule.Home));
        }

        public void Test111()
        {
            DatumVon = new DateTime(2014, 01, 01, 01, 01, 01);
        }

        public void TerminListeMouseDoubleClick(C1.WPF.DataGrid.C1DataGrid grid)
        {
            try
            {

                var t = (CRMTermine)grid.SelectedItem;
                _events.Publish(new TerminEvent(t.id, db, t.AppointmentType));
                _events.Publish(new ConfigureUITermineDaten(1, "01"));
                _events.Publish(new SelectUIEvent(false, SelectUIEvent.EnumActivateModule.None));

            }
            catch (Exception)
            {


            }

        }

        public bool isDirty
        {
            get { return false; }
        }


        public void Termine_DeletingRows(C1.WPF.DataGrid.DataGridDeletingRowsEventArgs e)
        {

          
           
            if (e.DeletedRows.Count() > 0)
            {
                try
                {
                    if (MessageBox.Show(string.Format("{0} ausgewählte Termine endgültig löschen ?", e.DeletedRows.Count()), "Sicherheitsabfrage",
                                     MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        foreach (var item in e.DeletedRows)
                        {
                           
                            
                                            var adr = (CRMTermine)item.DataItem;
                                            CommonTools.ObjectFactories.TerminFactory.DeleteTermine(adr.id);

           
                        }

                        db.SaveChanges();


                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                catch (Exception ex)
                {

                    CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex, true);
                }




            }


        }

        public void DatumVonChanged(C1.WPF.DateTimeEditors.NullablePropertyChangedEventArgs<DateTime> e)
        {
            //var von = Tools.DateTools.GetDateWithCustomHour((DateTime)e.NewValue,06, 0, 0);


            //TermineListe = new ObservableCollection<CRMTermine>(db.CRMTermine.Where(t => t.TerminVon >= von).OrderBy(t => t.TerminVon));

        }

        public void DatumBisChanged(C1.WPF.DateTimeEditors.NullablePropertyChangedEventArgs<DateTime> e)
        {
            //var bis = Tools.DateTools.GetDateWithCustomHour((DateTime)e.NewValue, 06, 0, 0);
        }

        public void Handle(TerminSavedEvent message)
        {
            DateTime von = DateTools.GetDateWithCustomHour(DatumVon, 00, 00, 0);
            DateTime bis = DateTools.GetDateWithCustomHour(DatumBis, 23, 59, 00);
            LoadTermine(von, bis);
        }

        public void ShowSchedule()
        {
            var schedule = new StandardPopupView(2, StandardPopupViewModel.EnumPopupType.Scheduler, "Übersicht :", 850, 1200,DatumVon,DatumBis);
            schedule.ShowDialog();

        }
    }
}
