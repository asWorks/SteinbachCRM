using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using SteinbachCRM.ViewModels.Interfaces;
using System.ComponentModel.Composition;
using System.Windows.Media;
using System.Windows.Controls;
using DAL;
using CommonTools.Views;
using System.Windows.Threading;
using SteinbachCRM.Views;
using SteinbachCRM.ObjectCollections;
using System.Collections.ObjectModel;
using DAL;
using System.Windows;
using CommonTools.Tools;
using SteinbachCRM.Events;
using AutoMapper;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;




namespace SteinbachCRM.ViewModels
{
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : Conductor<Object>, IShell, IHandle<SelectUIEvent>
    {
        #region "Declaration"

        //   SteinbachEntities db;
        KundendatenViewModel kdViewModel;
        TerminedatenViewModel tViewModel;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        object ActiveControl;

        private readonly IEventAggregator _events;
        //bool testflag;

        #endregion

        #region "Properties"
        public bool MayEndApp { get; set; }

        private bool _ButtonsEnabled;

        public bool ButtonsEnabled
        {
            get { return _ButtonsEnabled; }
            set
            {
                _ButtonsEnabled = value;
                NotifyOfPropertyChange(() => ButtonsEnabled);
            }
        }

        private bool _AdminEnabled;

        public bool AdminEnabled
        {
            get { return _AdminEnabled; }
            set
            {
                _AdminEnabled = value;
                NotifyOfPropertyChange(() => AdminEnabled);
            }
        }


        #endregion

        #region "Constructors"
        [ImportingConstructor]
        public ShellViewModel(TerminedatenViewModel TermineDatenVM, KundendatenViewModel kdViewModel, IEventAggregator Events)
        {
            Events.Subscribe(this);
            this._events = Events;
            this.TermineDatenVM = TermineDatenVM;
            this.kdViewModel = kdViewModel;
            ActiveControl = (NavigationViewModel)new NavigationViewModel();
            ActivateItem(ActiveControl);
            ButtonsEnabled = true;
            AdminEnabled = false;
            MayEndApp = true;
            this.DisplayName = "Steinbach CRM";

            
         
            if (CommonTools.Tools.ConfigEntry<int>.GetConfigEntry("UpdateCRMAktiv", "0", " 0 = inaktiv - Alles andere = aktiv") != 0)
            {

               string installed = CommonTools.Tools.HelperTools.GetConfigEntry("InstallpathCRM", @"X:\Backup\Steinbach\27.05.2013\18-35-33\SteinbachCRM\bin\Release\SteinbachCRM.exe");
               string update = CommonTools.Tools.HelperTools.GetConfigEntry("UpdatepathCRM", @"E:\Projects\Steinbach\SteinbachCRM\bin\Release\SteinbachCRM.exe");
              
                
                string version = "";
                if (version == "xxyxc")
                {
                    MessageBox.Show("");
                }


                CommonTools.Tools.HelperTools.CheckUpdate(installed, update, "SteinbachCRM.exe");
            }






        }
        #endregion

        #region "Menue"

        public void mnuManageEntries()
        {
            DoActivateItem(new EditAuswahlEintraegeViewModel());
        }

        public void ShowNavigation()
        {

            DoActivateItem(new NavigationViewModel());

        }


        public void ShowKundendaten()
        {
            ManageChanges.SaveChangesEnum result = ManageChanges.SaveChangesEnum.noResult;

            if (kdViewModel != null)
            {
                //result = kdViewModel.SaveChanges();
            }

            // if (result != CommonTools.Tools.ManageChanges.SaveChangesEnum.Cancel)
            {
                DeactivateItem(kdViewModel, true);
                ActivateItem(kdViewModel);
                //DoActivateItem(kdViewModel, true);
                //_events.Publish(new SelectUIEvent(false, SelectUIEvent.EnumActivateModule.Kunde));
            }

            CheckAdmin();



        }

        public void ShowTermine()
        {

            ActivateItem(TermineDatenVM);
            // DoActivateItem(TermineDatenVM);
            //// _events.Publish(new ConfigureUITermineDaten(0, "10"));
            // ButtonsEnabled = false;
            // AdminEnabled = false;


        }

        public void ShowDirektTermine()
        {


            var schedule = new StandardPopupView(2, StandardPopupViewModel.EnumPopupType.Scheduler, "Übersicht :", 850, 1200);
            schedule.ShowDialog();



        }

        public void ShowStandardPopup()
        {

            DoActivateItem(new EditAuswahlEintraegeViewModel());
            //var Termin = new StandardPopupView(2, StandardPopupViewModel.EnumPopupType.Termin, "Termin Nr. 2 :", 550, 880);
            //Termin.ShowDialog();
        }


        public void showBesuchsberichte()
        {
            // MailViewerViewModel mv = new MailViewerViewModel(6);
            String[] size = CommonTools.Tools.HelperTools.GetConfigEntry("PopUpSizeKundenbesuch", "800,2000").Split(',');
            var besuch = new StandardPopupView(0, StandardPopupViewModel.EnumPopupType.Kundenbesuch, "Kundenbesuch", double.Parse(size[0]), double.Parse(size[1]));
            besuch.ShowDialog();

        }

        public void ManageEvents()
        {
            String[] size = CommonTools.Tools.HelperTools.GetConfigEntry("PopUpSizeEvents", "600,1300").Split(',');
            var events = new StandardPopupView(0, StandardPopupViewModel.EnumPopupType.Events, "Events", double.Parse(size[0]), double.Parse(size[1]));
            events.ShowDialog();
        }

        public void SelectEvents()
        {
            String[] size = CommonTools.Tools.HelperTools.GetConfigEntry("PopUpSizeSelectedEvents", "850,1500").Split(',');
            var events = new StandardPopupView(0, StandardPopupViewModel.EnumPopupType.SelectEvents, "Events", double.Parse(size[0]), double.Parse(size[1]));
            events.ShowDialog();
        }


        #endregion

        #region "Events"




        public void Handle(SelectUIEvent message)
        {

            MayEndApp = ButtonsEnabled = message.MainSelectionButtonsEnabled;

            if (message.ActivateModule == SelectUIEvent.EnumActivateModule.Home)
            {
                DoActivateItem(new NavigationViewModel(), true);
            }
        }


        public void ShellView_Closing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = kdViewModel.isDirty || TermineDatenVM.isDirty;

        }




        #endregion

        #region "Imported ViewModels"
        private TerminedatenViewModel _TermineDatenVM;

        public TerminedatenViewModel TermineDatenVM
        {
            get { return _TermineDatenVM; }
            set
            {
                _TermineDatenVM = value;
                NotifyOfPropertyChange(() => TermineDatenVM);

            }
        }



        #endregion

        #region "HelperFunctions"

        private void DoActivateItem(Object viewModel)
        {


            DoActivateItem(viewModel, false);


        }


        private void DoActivateItem(Object viewModel, bool bIgnoreDatastate)
        {
            var ac = (IDataState)ActiveControl;
            if (ac != null)
            {

                if (ac.isDirty == false || bIgnoreDatastate == true)
                {
                    DeactivateItem(ac, true);
                    ActiveControl = viewModel;
                    ActivateItem(ActiveControl);

                }


            }
        }

        void CheckAdmin()
        {
            if (DAL.Session.User.rights != null)
            {
                if (DAL.Session.User.rights.ToLower() == "admin" || DAL.Session.User.rights.ToLower() == "su")
                {
                    AdminEnabled = true;
                }
                else
                {
                    AdminEnabled = false;
                }
            }
            else
            {
                AdminEnabled = false;
            }


        }


        #endregion


    }
}
