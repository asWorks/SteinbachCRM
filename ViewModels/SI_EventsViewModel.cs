using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using DAL;
using System.Collections.ObjectModel;
using SteinbachCRM.ViewModels.Interfaces;
using System.Windows;

namespace SteinbachCRM.ViewModels
{
    public class SI_EventsViewModel : PropertyChangedBase, IDataMethods
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

        private ObservableCollection<SI_Events> _events;
        public ObservableCollection<SI_Events> events
        {
            get { return _events; }
            set
            {
                if (value != _events)
                {
                    _events = value;
                    NotifyOfPropertyChange(() => events);
                   // isDirty = true;
                }
            }
        }

        private SI_Events _Selectedevents;
        public SI_Events Selectedevents
        {
            get { return _Selectedevents; }
            set
            {
                if (value != _Selectedevents)
                {
                    _Selectedevents = value;
                    NotifyOfPropertyChange(() => Selectedevents);
                  //  isDirty = true;
                }
            }
        }

     


        #endregion

        #region "Constructors"
        public SI_EventsViewModel()
        {
            db = new SteinbachEntities();
            events = new ObservableCollection<SI_Events>(db.SI_Events);
           // isDirty = false;
          



        }
        #endregion

        #region "Functions"
        public void AddEvent()
        {
            var ev = new SI_Events();
            ev.istAktiv = 1;
            db.AddToSI_Events(ev);
            events.Add(ev);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        #endregion
        #region "Events"
        public void DeletePositionEvents(Views.SI_EventsView window)
        {
             try
            {
                var grid = window.EventsGrid;
                var pos = (SI_Events)grid.SelectedItem;
                if (pos != null)
                {
                    if (MessageBox.Show(string.Format("Position {0} wirklich löschen?", pos.Bezeichnung), "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            foreach (var item in pos.Personen_Events.ToList())
                            {
                                db.DeleteObject(item);
                            }
                           
                            db.DeleteObject(pos);
                        }
                        catch (Exception)
                        {
                        }


                        events.Remove(pos);
                    }



                }
            }
            catch (Exception ex )
            {
                CommonTools.Tools.ErrorMethods.HandleStandardError(ex, "Fehler beim Löschen von Events");
               
            }
        }
       
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
