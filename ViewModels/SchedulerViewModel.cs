using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using SteinbachCRM.ViewModels.Interfaces;
using Caliburn.Micro;

namespace SteinbachCRM.ViewModels
{
     [Export(typeof(IScheduler))]
    public class SchedulerViewModel :Screen,IScheduler,IDataState
    {

        public bool isDirty
        {
            get { return false; }
        }

        private DateTime _VonDatum;

        public DateTime VonDatum
        {
            get { return _VonDatum; }
            set
            {
                _VonDatum = value;
                NotifyOfPropertyChange(() => VonDatum);
 
            }
        }

        private DateTime _BisDatum;

        public DateTime BisDatum
        {
            get { return _BisDatum; }
            set
            {
                _BisDatum = value;
                NotifyOfPropertyChange(() => BisDatum);
            }
        }

    
         public SchedulerViewModel()
        {
            this.VonDatum = DateTime.Now;
            this.BisDatum = DateTime.Now.AddDays(30);

        }

         public SchedulerViewModel(DateTime VonDatum,DateTime BisDatum)
        {
            this.VonDatum = VonDatum;
            this.BisDatum = BisDatum;

        }
     
     }

 	
}
