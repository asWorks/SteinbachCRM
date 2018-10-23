using System.Linq;
using DAL;
using Caliburn.Micro;
using SteinbachCRM.ViewModels.Interfaces;
using System.ComponentModel.Composition;

namespace SteinbachCRM.ViewModels
{
    [Export (typeof(IFirma))]
    class FirmaViewModel : Screen, IFirma,IDataState
    {
        private SteinbachEntities db;
        private firma _CurrentFirma;



        public firma CurrentFirma
        {
            get { return _CurrentFirma; }
            set
            {
                _CurrentFirma = value;
                NotifyOfPropertyChange(() => CurrentFirma);

            }
        }

               

        public FirmaViewModel()
        {
            db = new SteinbachEntities();
            CurrentFirma = db.firmen.Where(f => f.id == 23).SingleOrDefault();
            isDirty = true;
            //name = "asWorks";
        }


        public void Save()
        {
            db.SaveChanges();
            isDirty = false;
        }



        private bool _isDirty;

        public bool isDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                NotifyOfPropertyChange(()=>isDirty);
            }
        }
        
    }
}
