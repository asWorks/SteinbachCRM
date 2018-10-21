using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SteinbachCRM.ViewModels.Interfaces;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using DAL;
using SteinbachCRM.Tools;


namespace SteinbachCRM.ViewModels
{
    [Export(typeof(IStandardPopup))]
    public class StandardPopupViewModel : Screen, IStandardPopup, IDataMethods
    {
        public event System.Action DoRejectChanges; 

        public void onDoRejectChanges()
        {
            if (DoRejectChanges != null)
            {
                DoRejectChanges();
            }

        }

        public enum EnumPopupType
        {
            Termin,
            Scheduler,
            Email,
            Kundenbesuch,
            Events,
            SelectEvents
        }

        SteinbachEntities db;

        private object _CurrentObject;

        public object CurrentObject
        {
            get { return _CurrentObject; }
            set
            {
                _CurrentObject = value;
                NotifyOfPropertyChange(() => CurrentObject);

            }
        }





        public StandardPopupViewModel(int id, EnumPopupType Type)
        {
            Init(id, Type, null);

        }


        public StandardPopupViewModel(int id, EnumPopupType Type, object AddionalData)
        {
            Init(id, Type, AddionalData);
        }

        public StandardPopupViewModel( EnumPopupType Type, object AddionalData)
        {
            Init(Type, AddionalData);
        }


        private void Init(EnumPopupType Type, object AddionalData)
        {
            db = new SteinbachEntities();
            switch (Type)
            {
                case EnumPopupType.Termin:
                    {
                       

                        break;
                    }
                case EnumPopupType.Scheduler:
                    {
                        if (AddionalData != null)
                        {
                           
                        }
                        else
                        {
                            //CurrentObject = new SchedulerViewModel();
                        }

                        break;
                    }

                case EnumPopupType.Email:
                    {
                        //CurrentObject = new MailViewerViewModel(id);

                        break;
                    }

                case EnumPopupType.Kundenbesuch:
                    {

                        if (AddionalData != null)
                        {
                            var kb = (Firmen_Kundenbesuch)AddionalData;
                            var buf = new KundenbesuchViewModel(kb);
                            buf.DoRejectChanges += () => onDoRejectChanges();
                            CurrentObject = buf;
                            

                        }
                      
                       
                        break;
                    }

                case EnumPopupType.Events:
                    {
                        CurrentObject = new SI_EventsViewModel();

                        break;
                    }


                default:
                    break;
            }

        }



        private void Init(int id, EnumPopupType Type, object AddionalData)
        {
            db = new SteinbachEntities();
            switch (Type)
            {
                case EnumPopupType.Termin:
                    {
                        CRMTermine termin = db.CRMTermine.Where(t => t.id == id).SingleOrDefault();
                        CurrentObject = new TermineTerminViewModel(termin, db);

                        break;
                    }
                case EnumPopupType.Scheduler:
                    {
                        if (AddionalData != null)
                        {
                            var tsp = (TransferTimespan)AddionalData;
                            CurrentObject = new SchedulerViewModel(tsp.VonDatum, tsp.BisDatum);
                        }
                        else
                        {
                            CurrentObject = new SchedulerViewModel();
                        }

                        break;
                    }

                case EnumPopupType.Email:
                    {
                        CurrentObject = new MailViewerViewModel(id);

                        break;
                    }

                case EnumPopupType.Kundenbesuch:
                    {
                        var buf = new KundenbesuchViewModel(id);
                        buf.DoRejectChanges += () => onDoRejectChanges();
                        CurrentObject = buf;
                        break;
                    }

                case EnumPopupType.Events:
                    {
                        var buf = new SI_EventsViewModel();
                        buf.DoRejectChanges += () => onDoRejectChanges();
                        CurrentObject = buf;

                        break;
                    }

                case EnumPopupType.SelectEvents:
                    {
                        var buf = new SI_MailingSelectionViewModel();
                        buf.DoRejectChanges += () => onDoRejectChanges();
                        CurrentObject = buf;

                        break;
                    }


                default:
                    break;
            }


        }



        public bool isDirty
        {
            get
            {
                try
                {
                    if (CurrentObject is IDataMethods)
                    {
                        IDataMethods co = (IDataMethods)CurrentObject;
                        return co.isDirty;
                    }

                    return false;
                }
                catch (Exception)
                {
                    return false;

                }

            }
            set
            {

            }
        }

        public bool SaveChanges()
        {
            IDataMethods co = (IDataMethods)CurrentObject;
            return co.SaveChanges();
        }

        public bool RejectChanges()
        {
            IDataMethods co = (IDataMethods)CurrentObject;
            return co.RejectChanges();
        }
    }
}
