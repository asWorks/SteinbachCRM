using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using SteinbachCRM.ViewModels.Interfaces;
using Caliburn.Micro;
using DAL;
using System.Collections.ObjectModel;

namespace SteinbachCRM.ViewModels
{
     [Export(typeof(Firmen_PersonenViewModel))]
	public class Firmen_PersonenViewModel : Screen,IFirmen_Personen
	{
		 SteinbachEntities db;
         IEventAggregator _events;
		private firma _SelectedFirma;

		public firma SelectedFirma
		{
			get { return _SelectedFirma; }
			set
			{
				_SelectedFirma = value;
				NotifyOfPropertyChange(() => SelectedFirma);
			}
		}

		private Firmen_Personen _SelectedPerson;

	public Firmen_Personen SelectedPerson
	{
		get { return _SelectedPerson;}
		set
		{
			_SelectedPerson = value;
		NotifyOfPropertyChange(()=>SelectedPerson);
		}
	}
	
		 
		[ImportingConstructor]
		 public Firmen_PersonenViewModel(IEventAggregator events)
		{
            events.Subscribe(this);
            _events = events;


		}

		 public Firmen_PersonenViewModel(SteinbachEntities db, firma Firma)
		 {
			 SelectedFirma = Firma;
			 this.db = db;

		 }

         //public Firmen_PersonenViewModel(int id_Firma)
         //{
         //    SelectedFirma = Firma;
         //    this.db = db;

         //}
	 
		

	}
}
