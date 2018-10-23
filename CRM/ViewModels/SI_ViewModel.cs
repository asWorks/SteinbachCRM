using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using DAL;
using System.Collections.ObjectModel;
using SteinbachCRM.ViewModels.Interfaces;

namespace SteinbachCRM.ViewModels
{
    public class SI_MailingSelectionView : PropertyChangedBase, IDataMethods
    {

        #region "Properties"
        SteinbachEntities db;

        private bool _isDirty;
        public bool isDirty
        {
            get
            {
                //return _isDirty;

                return CommonTools.Tools.ManageChanges.isDirty(db);
            }
            set
            {
                if (value != _isDirty)
                {
                    _isDirty = value;
                    NotifyOfPropertyChange(() => isDirty);

                }
            }
        }
        #endregion


        #region "ObservableCollection"


        private ObservableCollection<vwSelectedEventsUndKategorien> _SelectedEvents;
        public ObservableCollection<vwSelectedEventsUndKategorien> SelectedEvents
        {
            get { return _SelectedEvents; }
            set
            {
                if (value != _SelectedEvents)
                {
                    _SelectedEvents = value;
                    NotifyOfPropertyChange(() => SelectedEvents);
                    isDirty = true;
                }
            }
        }

        private vwSelectedEventsUndKategorien _SelectedSelectedEvents;
        public vwSelectedEventsUndKategorien SelectedSelectedEvents
        {
            get { return _SelectedSelectedEvents; }
            set
            {
                if (value != _SelectedSelectedEvents)
                {
                    _SelectedSelectedEvents = value;
                    NotifyOfPropertyChange(() => SelectedSelectedEvents);
                    isDirty = true;
                }
            }
        }




        #endregion

        #region "Constructors"
        public SI_MailingSelectionView()
        {
            db = new SteinbachEntities();
            SelectedEvents = new ObservableCollection<vwSelectedEventsUndKategorien>(db.vwSelectedEventsUndKategoriens);
           // isDirty = false;
          



        }
        #endregion

        #region "Functions"
        //public void AddEvent()
        //{
        //    var ev = new SI_Events();
        //    db.AddToSI_Events(ev);
        //    events.Add(ev);
        //}

        public void Save()
        {
            db.SaveChanges();
        }

        #endregion
        #region "Events"

       
        #endregion




        public bool SaveChanges()
        {
            Save();
           // isDirty = false;
            return true;
        }

        public bool RejectChanges()
        {
           // isDirty = false;
            return true;
        }

        public event System.Action DoRejectChanges;
    }
}
