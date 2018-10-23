using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using SteinbachCRM.ViewModels.Interfaces;
using Caliburn.Micro;
using DAL;

namespace SteinbachCRM.ViewModels
{
    [Export(typeof(ITerminPopup))]
    class TerminPopupViewModel :Screen,ITerminPopup
    {
        SteinbachEntities db;

        private TermineTerminViewModel _TerminView;


        public TermineTerminViewModel TerminView
        {
            get { return _TerminView; }
            set
            {
                _TerminView = value;
                NotifyOfPropertyChange(() => TerminView);
            }
        }
        


        public TerminPopupViewModel(int id)
        {
            db = new SteinbachEntities();
            
                var termin = db.CRMTermine.Where(t => t.id == id).SingleOrDefault();
                TerminView = new TermineTerminViewModel(termin, db);
               // ActivateItem(TerminView);
            


        }


        public TerminPopupViewModel()
        {
           

        }

    }
}
