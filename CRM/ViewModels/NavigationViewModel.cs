using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using SteinbachCRM.ViewModels.Interfaces;
using System.ComponentModel.Composition;


namespace SteinbachCRM.ViewModels
{
    [Export(typeof(INavigation))]
    class NavigationViewModel : Conductor<Object>, INavigation,IDataState
    {
        private bool _isDirty;

        public bool isDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                NotifyOfPropertyChange(() => isDirty);
            
            }
        }
        

        public void ShowKundenDaten()
        {
           // ActivateItem(new KundendatenViewModel());
        }

    }
}
