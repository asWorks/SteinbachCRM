using System;
using System.Linq;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using SteinbachCRM.ViewModels.Interfaces;
using DAL;
using System.Collections.ObjectModel;
using System.Windows;
using CommonTools.Tools;
using SteinbachCRM.Events;

namespace SteinbachCRM.ViewModels
{
    [Export(typeof(TerminedatenViewModel))]
    public class TerminedatenViewModel : Screen, ITerminedaten, IDataState, IHandle<ConfigureUITermineDaten>
    {

        #region "Declarations"
        public event DelegateTerminChanged OnNotifyViewTerminChanged;
        public delegate void DelegateTerminChanged();
        SteinbachEntities db;


        #endregion

        #region "Imported ViewModels"


        #endregion

        #region "UIProperties"
        private bool _TabTermineListeEnabled;

        public bool TabTermineListeEnabled
        {
            get { return _TabTermineListeEnabled; }
            set
            {
                _TabTermineListeEnabled = value;
                NotifyOfPropertyChange(() => TabTermineListeEnabled);
            }
        }

        private bool _TabTermineTerminEnabled;

        public bool TabTermineTerminEnabled
        {
            get { return _TabTermineTerminEnabled; }
            set
            {
                _TabTermineTerminEnabled = value;
                NotifyOfPropertyChange(() => TabTermineTerminEnabled);

            }
        }

        private int _TabControlSelectedIndex;

        public int TabControlSelectedIndex
        {
            get { return _TabControlSelectedIndex; }
            set
            {
                _TabControlSelectedIndex = value;
                NotifyOfPropertyChange(() => TabControlSelectedIndex);

            }
        }



        #endregion

        #region "PrivatePropertiesPart"

        private bool _SelectionEnabled;

        private TermineListeViewModel _TermineListeVM;

        private TermineTerminViewModel _TestTermineGesamt;


        private ObservableCollection<CRMTermine> _AlleTermine;

        #endregion

        #region "Properties"

        public bool SelectionEnabled
        {
            get { return _SelectionEnabled; }
            set
            {
                _SelectionEnabled = value;
                NotifyOfPropertyChange(() => SelectionEnabled);
            }
        }


        public TermineTerminViewModel TermineTerminVM
        {
            get { return _TestTermineGesamt; }
            set
            {
                _TestTermineGesamt = value;
                NotifyOfPropertyChange(() => TermineTerminVM);
            }
        }

        public TermineListeViewModel TermineListeVM
        {
            get { return _TermineListeVM; }
            set
            {
                _TermineListeVM = value;
                NotifyOfPropertyChange(() => TermineListeVM);
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



        public bool isDirty
        {
            get
            {
                //return ManageChanges.isDirty(db);
                return TermineTerminVM.isDirty;

            }

        }



        public ObservableCollection<CRMTermine> AlleTermine
        {
            get { return _AlleTermine; }
            set
            {
                _AlleTermine = value;
                NotifyOfPropertyChange(() => AlleTermine);
            }
        }
        #endregion

        #region "Constructors"

        public TerminedatenViewModel(SteinbachEntities db, CRMTermine termin)
        {
            //this.db = db;
            //LoadTermine(string.Empty);
            //TermineTerminVM = new TermineTerminViewModel();
            //TermineListeVM = new TermineListeViewModel();
            //TermineListeVM.onTerminChanged += new Action<int>(TermineListeVM_onTerminChanged);
            MessageBox.Show("Should not be called -Constructor TerminedatenViewModel");
        }


        public TerminedatenViewModel()
        {
            //db = new SteinbachEntities();
            //LoadTermine(string.Empty);
            //TermineTerminVM = new TermineTerminViewModel(null, db);
            //TermineListeVM = new TermineListeViewModel();
            //TermineListeVM.onTerminChanged += new Action<int>(TermineListeVM_onTerminChanged);
            MessageBox.Show("Should not be called -Constructor TerminedatenViewModel");
        }

        [ImportingConstructor]
        public TerminedatenViewModel(TermineTerminViewModel TermineTerminVm, TermineListeViewModel TermineListeVm, IEventAggregator Events)
        {
            TermineListeVM = TermineListeVm;
            TermineTerminVM = TermineTerminVm;
            //TermineListeVM.onTerminChanged += new Action<int>(TermineListeVM_onTerminChanged);
            SelectionEnabled = true;
            Events.Subscribe(this);
            //TabControlSelectedIndex = 0;
            //TabTermineTerminEnabled = true;
            //TabTermineListeEnabled = false;

        }

        #endregion

        #region "EventCalls"

        //void TermineListeVM_onTerminChanged(int obj)
        //{




        //    //if (CommonTools.Tools.ManageChanges.SaveChanges(db, MessageBoxButton.OKCancel) == ManageChanges.SaveChangesEnum.AllDone)
        //    //{
        //        CurrentTermin = db.CRMTermine.Where(t => t.id == obj).SingleOrDefault();
        //        TermineTerminVM = new TermineTerminViewModel(CurrentTermin, db);
        //        TerminChanged();


        //    //}



        //}

        //public void FirmenOnFcb_SelectionChanged(SelectionChangedEventArgs e)
        //{
        //    if (e.AddedItems.Count == 1)
        //    {
        //        var f = (CRMTermine)e.AddedItems[0];
        //        CurrentTermin = f;
        //        OnTerminChanged();
        //        LoadTermin();

        //    }
        //    else
        //    {

        //    }

        //}



        public void FirmenonfcbChanged(CommonTools.UserControls.FilteredComboBoxChangedEventArgs e)
        {
            LoadTermine(e.filter);


        }

        #endregion

        #region "Commands"
        public void SaveData()
        {
            db.SaveChanges();


        }

        public void TabTerminListeGotFocus(RoutedEventArgs e)
        {

            //var mc = new CommonTools.Tools.ManageChanges(db);
            //if (mc.isDirty())
            //{
            //    var mp = mc.ModifiedProperties;
            //}

            //if (CommonTools.Tools.ManageChanges.SaveChanges(db, MessageBoxButton.OKCancel) != ManageChanges.SaveChangesEnum.AllDone)
            //{
            //    NotifyViewTerminChanged();
            //}
        }

        public void NewCompany()
        {
            CRMTermine t = new CRMTermine();

            db.AddToCRMTermine(t);

            _CurrentTermin = t;
            NotifyViewTerminChanged();
            LoadTermin();

        }
        #endregion

        #region "Funktionen"

        private void LoadTermine(string filter)
        {
            try
            {
                if (filter == string.Empty)
                {

                    var query = from f in (db).CRMTermine
                                select f;

                    AlleTermine = new ObservableCollection<CRMTermine>(query);
                }
                else
                {
                    var query = db.CRMTermine.OrderBy(f => f.id);
                    AlleTermine = new ObservableCollection<CRMTermine>(query);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));
            }


        }


        private void NotifyViewTerminChanged()
        {
            if (OnNotifyViewTerminChanged != null)
            {
                OnNotifyViewTerminChanged();
            }
        }


        private void LoadTermin()
        {

            try
            {
                var mc = new ManageChanges(db);
                var result = mc.SaveChanges(MessageBoxButton.OKCancel);

                if (result == ManageChanges.SaveChangesEnum.Cancel)
                {

                }
                else if (result == ManageChanges.SaveChangesEnum.No)
                {

                    //CRM_Termine = new TermineViewModel(db,CurrentTermin);


                }
                else if (result == ManageChanges.SaveChangesEnum.AllDone)
                {
                    //CRM_Termine = new TermineViewModel(db,CurrentTermin);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));
            }



        }

        public void UpdateTermin()
        {
            if (CurrentTermin != null)
            {

                NotifyViewTerminChanged();
                LoadTermin();
            }

        }

        public ManageChanges.SaveChangesEnum SaveChanges()
        {
            return ManageChanges.SaveChanges(db);

        }


        #endregion

        
        public void Handle(ConfigureUITermineDaten message)
        {
            TabControlSelectedIndex = message.SelectTab;
            TabTermineListeEnabled = message.ActivatedTabs.Substring(0, 1) == "0" ? false : true;
            TabTermineTerminEnabled = message.ActivatedTabs.Substring(1, 1) == "0" ? false : true;


        }
    }
}
