using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using SteinbachCRM.ViewModels.Interfaces;
using System.Windows.Data;
using DAL;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows;
using SteinbachCRM.ObjectCollections;
using System.Windows.Controls;
using CommonTools.Tools;
using SteinbachCRM.Events;
using System.Diagnostics;
//using DalDBContext;

namespace SteinbachCRM.ViewModels
{
    [Export(typeof(KundendatenViewModel))]
    public class KundendatenViewModel : Screen, IKundendaten, IDataState, IHandle<SelectKundenDatenEnabled>
    {

        #region "Declarations"
        public event FirmaChanged EventFirmaChanged;
        public delegate void FirmaChanged();
        SteinbachEntities db;
        IEventAggregator _events;


        #endregion

        #region "PrivatePropertiesPart"

        private bool _SelectEnabled;

        private FirmenPersonenListeViewModel firmenPersonenListeViewModel;
        private PersonenDatenViewModel personenDatenViewModel;
        private FirmaDataViewModel firmenDatenViewModel;
        private ObservableCollection<firma> _AlleFirmen;
        private firma _CurrentFirmaData;
        private int _CurrentFirma;

        #endregion

        #region "Properties"

        public bool SelectEnabled
        {
            get { return _SelectEnabled; }
            set
            {
                _SelectEnabled = value;
                NotifyOfPropertyChange(() => SelectEnabled);
            }
        }
        private bool _FirmenDatenEnabled;

        public bool FirmenDatenEnabled
        {
            get { return _FirmenDatenEnabled; }
            set
            {
                _FirmenDatenEnabled = value;
                NotifyOfPropertyChange(() => FirmenDatenEnabled);

            }
        }



        public bool isDirty
        {

            get
            {
                //if (PersonenDatenViewModel.isDirty || this.FirmenPersonenListeViewModel.isDirty || this.FirmenxDatenViewModel.isDirty)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                //List<string> mod = ManageChanges.GetModifiedProperties(db);
                //Console.WriteLine(mod.ToString());
                return CommonTools.Tools.ManageChanges.isDirty(db) || PersonenDatenViewModel.isDirty;

            }

        }

        public firma CurrentFirmaData
        {
            get { return _CurrentFirmaData; }
            set
            {
                _CurrentFirmaData = value;
                NotifyOfPropertyChange(() => CurrentFirmaData);
            }
        }


        //public int CurrentFirma
        //{
        //    get { return _CurrentFirma; }
        //    set
        //    {
        //        _CurrentFirma = value;
        //        NotifyOfPropertyChange(() => CurrentFirma);
        //    }
        //}



        public FirmenPersonenListeViewModel FirmenPersonenListeViewModel
        {
            get { return firmenPersonenListeViewModel; }
            set
            {
                firmenPersonenListeViewModel = value;
                NotifyOfPropertyChange(() => FirmenPersonenListeViewModel);
            }
        }

        public PersonenDatenViewModel PersonenDatenViewModel
        {
            get { return personenDatenViewModel; }
            set
            {
                personenDatenViewModel = value;
                NotifyOfPropertyChange(() => PersonenDatenViewModel);
            }
        }

        public FirmaDataViewModel FirmenxDatenViewModel
        {
            get { return firmenDatenViewModel; }
            set
            {
                firmenDatenViewModel = value;
                NotifyOfPropertyChange(() => FirmenxDatenViewModel);
            }
        }

        private KommunikationViewModel _KommunikationVM;

        public KommunikationViewModel KommunikationVM
        {
            get { return _KommunikationVM; }
            set
            {
                _KommunikationVM = value;
                NotifyOfPropertyChange(() => KommunikationVM);
            }
        }

        private KundenbesuchListeViewModel _KundenbesuchVM;
        public KundenbesuchListeViewModel KundenbesuchVM
        {
            get { return _KundenbesuchVM; }
            set
            {
                if (value != _KundenbesuchVM)
                {
                    _KundenbesuchVM = value;
                    NotifyOfPropertyChange(() => KundenbesuchVM);

                }
            }
        }



        private firma _SelectedAlleFirmen;

        public firma SelectedAlleFirmen
        {
            get { return _SelectedAlleFirmen; }
            set
            {
                if (value != null)
                {
                    _SelectedAlleFirmen = value;
                    NotifyOfPropertyChange(() => SelectedAlleFirmen);
                }

            }
        }


        public ObservableCollection<firma> AlleFirmen
        {
            get { return _AlleFirmen; }
            set
            {
                _AlleFirmen = value;
                NotifyOfPropertyChange(() => AlleFirmen);
            }
        }

        private ObservableCollection<vw_PersonenUndFirmen> _AllePersonen;
        public ObservableCollection<vw_PersonenUndFirmen> AllePersonen
        {
            get { return _AllePersonen; }
            set
            {
                if (value != _AllePersonen)
                {
                    _AllePersonen = value;
                    NotifyOfPropertyChange(() => AllePersonen);

                }
            }
        }

        private vw_PersonenUndFirmen _SelectedAllePersonen;
        public vw_PersonenUndFirmen SelectedAllePersonen
        {
            get { return _SelectedAllePersonen; }
            set
            {
                if (value != _SelectedAllePersonen)
                {
                    _SelectedAllePersonen = value;
                    if (value != null)
                    {
                        firma f = db.firmen.Where(n => n.id == value.id).SingleOrDefault();
                        if (f != null)
                        {
                            TryChangingFirma(f);
                        }
                    }

                  
                    
                    NotifyOfPropertyChange(() => SelectedAllePersonen);

                }
            }
        }


        #endregion

        #region "Constructors"

        public KundendatenViewModel(SteinbachEntities db, firma Firma)
        {

            firmenDatenViewModel = new FirmaDataViewModel(db, Firma);
            PersonenDatenViewModel = new PersonenDatenViewModel(db, Firma);
            FirmenPersonenListeViewModel = new FirmenPersonenListeViewModel(db, Firma);
            KommunikationVM = new KommunikationViewModel(Firma.id);
            KundenbesuchVM = new KundenbesuchListeViewModel(Firma.id);

            this.db = db;
            LoadFirmen(string.Empty);

        }

        [ImportingConstructor]
        public KundendatenViewModel(FirmaDataViewModel fdViewmodel, FirmenPersonenListeViewModel fplViewmodel, PersonenDatenViewModel pdViewModel,
           KundenbesuchListeViewModel kbesuchViewModel, IEventAggregator events)
        {
            FirmenPersonenListeViewModel = fplViewmodel;
            PersonenDatenViewModel = pdViewModel;
            FirmenxDatenViewModel = fdViewmodel;
            KundenbesuchVM = kbesuchViewModel;
            events.Subscribe(this);
            _events = events;

            //FirmenxDatenViewModel = new FirmenDatenViewModel(db, CurrentFirmaData);
            //PersonenDatenViewModel = new PersonenDatenViewModel(db, CurrentFirmaData);
            //FirmenPersonenListeViewModel = new FirmenPersonenListeViewModel(db, CurrentFirmaData);

            db = new SteinbachEntities();
            SelectEnabled = true;
            FirmenDatenEnabled = true;
            LoadFirmen(string.Empty);

            // PersonenDatenViewModel pdViewModel,
            //FirmenPersonenListeViewModel fplViewmodel,

        }

        #endregion

        #region "EventCalls"

        public void FirmenOnFcb_SelectionChanged(SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count == 1)
            {
                var f = (firma)e.AddedItems[0];

                TryChangingFirma(f);

            }


        }

        private void TryChangingFirma(firma f)
        {
            if (isDirty)
            {
                MessageBoxResult res = MessageBox.Show("Änderungen speichern ?", "Sicherheitsabfrage", MessageBoxButton.YesNoCancel);

                switch (res)
                {
                    case MessageBoxResult.Cancel:
                        {
                            break;
                        }

                    case MessageBoxResult.No:
                        {
                            ReloadFirma(f.id);
                            break;
                        }

                    case MessageBoxResult.Yes:
                        {
                            SaveData();
                            SelectCompany(f);
                            break;
                        }

                    default:
                        break;
                }
            }
            else
            {
                SelectCompany(f);
            }
        }

        private void SelectCompany(firma fa)
        {
            try
            {
               // var f = (firma)e.AddedItems[0];
                CurrentFirmaData = fa;

                // _events.Publish(new SelectKundenDatenEnabled(false));
                //_events.Publish(new KundeChangedEvent(f.id, "", f, db));
                EventFirmaChanged();
                LoadFirma();

                if (CurrentFirmaData != null)
                {
                    SelectedAlleFirmen = CurrentFirmaData;
                }
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex, true);
            }

        }

        public void PersonenonfcbChanged(CommonTools.UserControls.FilteredComboBoxChangedEventArgs e)
        {
            LoadPersonen(e.filter);
        }

        public void FirmenonfcbChanged(CommonTools.UserControls.FilteredComboBoxChangedEventArgs e)
        {
            LoadFirmen(e.filter);

        }

        #endregion

        #region "Commands"
        private void SwitchState(bool bEdit)
        {
            FirmenDatenEnabled = bEdit;
            SelectEnabled = !bEdit;

        }



        public void SaveData()
        {
            try
            {

                PersonenDatenViewModel.Save();

                //this.FirmenPersonenListeViewModel.Save();
                this.FirmenxDatenViewModel.Save();

                db.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
                if (CurrentFirmaData != null)
                {
                    SelectedAlleFirmen = CurrentFirmaData;
                }

                //SwitchState(false);
                //_events.Publish(new SelectUIEvent(true, SelectUIEvent.EnumActivateModule.None));
                // return res;
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex, true);
                // return null;
            }



        }


        public void NewCompany()
        {
            if (isDirty)
            {
                MessageBoxResult res = MessageBox.Show("Änderungen speichern ?", "Sicherheitsabfrage", MessageBoxButton.YesNoCancel);

                switch (res)
                {
                    case MessageBoxResult.Cancel:
                        {
                            break;
                        }

                    case MessageBoxResult.No:
                        {

                            ReloadFirma(0);

                            MakeNewCompany();
                            break;
                        }

                    case MessageBoxResult.Yes:
                        {
                            SaveData();
                            MakeNewCompany();
                            break;
                        }

                    default:
                        break;
                }
            }
            else
            {
                MakeNewCompany();
            }


        }

        private void MakeNewCompany()
        {
            CurrentFirmaData = CommonTools.EntitiesActions.FirmaActions.GetNewFirma(db);
            db.AddTofirmen(CurrentFirmaData);


            EventFirmaChanged();
            LoadFirma();
        }
        #endregion

        #region "Funktionen"




        public void DeleteCustomer()
        {

            CommonTools.ObjectFactories.KundeFactory.DeleteCustumer(CurrentFirmaData.id);
            ReloadFirma(0);

        }


        private void LoadFirmen(string filter)
        {


            try
            {
                if (filter == string.Empty)
                {

                    var query = from f in db.firmen
                                where f.IstKunde == 1
                                orderby f.name
                                select f;

                    AlleFirmen = new FirmenCollection(query, db);
                }
                else
                {
                    var query = db.firmen.Where(f => f.name.Contains(filter) && f.IstKunde == 1).OrderBy(f => f.name);
                    AlleFirmen = new FirmenCollection(query, db);


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));
            }

        }

        
        private void LoadPersonen(string filter)
        {
            
            try
            {
            
            if (filter == string.Empty)
                {

                  var query = from p in db.vw_PersonenUndFirmen
                               where p.istFirma == 1
                              orderby p.Nachname
                               select p;
  
                 AllePersonen = new ObservableCollection<vw_PersonenUndFirmen>(query);   

                }
                else
                {
                    
                    var query = from p in db.vw_PersonenUndFirmen
                               where p.IstKunde == 1 && (p.Nachname.Contains(filter) || p.Vorname.Contains(filter))
                               orderby p.Nachname
                               select p;

                    AllePersonen = new ObservableCollection<vw_PersonenUndFirmen>(query);   
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));
            }

        }


        private void LoadFirma()
        {


            FirmenxDatenViewModel = new FirmaDataViewModel(db, CurrentFirmaData);
            //FirmenxDatenViewModel = new FirmaDataViewModel(CurrentFirmaData.id);

            PersonenDatenViewModel = new PersonenDatenViewModel(db, CurrentFirmaData);
            //PersonenDatenViewModel = new PersonenDatenViewModel(id);


            FirmenPersonenListeViewModel = new FirmenPersonenListeViewModel(db, CurrentFirmaData);

            KommunikationVM = new KommunikationViewModel(CurrentFirmaData.id);


            KundenbesuchVM = new KundenbesuchListeViewModel(CurrentFirmaData.id);

            //FirmenPersonenListeViewModel = new FirmenPersonenListeViewModel(id);

            //    }
            //    else if (result == ManageChanges.SaveChangesEnum.AllDone)
            //    {
            //        FirmenxDatenViewModel = new FirmaDataViewModel(db, CurrentFirmaData);
            //        PersonenDatenViewModel = new PersonenDatenViewModel(db, CurrentFirmaData);
            //        FirmenPersonenListeViewModel = new FirmenPersonenListeViewModel(db, CurrentFirmaData);
            //    }
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));
            //}



        }

        public void UpdateFirma()
        {
            if (CurrentFirmaData != null)
            {
                db.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
                EventFirmaChanged();
                ReloadFirma(CurrentFirmaData.id);

            }

        }

        public ManageChanges.SaveChangesEnum SaveChanges()
        {
            return ManageChanges.SaveChanges(db);

        }


        #endregion



        public void Handle(SelectKundenDatenEnabled message)
        {
            SelectEnabled = message.isEnabled;
        }


        public override void CanClose(Action<bool> callback)
        {
            // base.CanClose(callback);

            if (isDirty)
            {
                MessageBoxResult res = MessageBox.Show("Änderungen speichern ?", "", MessageBoxButton.YesNoCancel);

                switch (res)
                {
                    case MessageBoxResult.Cancel:
                        {
                            callback(false);
                            break;
                        }

                    case MessageBoxResult.No:
                        {
                            callback(true);
                            ManageChanges.SetUnmodified(db);
                            break;
                        }

                    case MessageBoxResult.Yes:
                        {
                            SaveData();
                            callback(true);
                            break;
                        }

                    default:
                        break;
                }


            }
            else
            {
                callback(true);
            }


        }

        protected override void OnActivate()
        {
            base.OnActivate();
            ReloadFirma(0);
        }


        private void ReloadFirma(int id)
        {
            // db.Dispose();
            db = new SteinbachEntities();

            if (CurrentFirmaData != null)
            {

                if (id == 0)
                {
                    CurrentFirmaData = null;
                }
                else
                {
                    CurrentFirmaData = null;
                    CurrentFirmaData = db.firmen.Where(i => i.id == id).SingleOrDefault();
                    EventFirmaChanged();
                    LoadFirma();
                }


            }
            LoadFirmen(string.Empty);
            if (CurrentFirmaData != null)
            {
                SelectedAlleFirmen = CurrentFirmaData;
            }
        }


    }
}
