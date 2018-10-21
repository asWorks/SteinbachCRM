using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Input;
using DAL;
using System.ComponentModel.Composition;



namespace SteinbachCRM.ViewModels
{
   
    public class ListboxSelectedEventsViewModel:INotifyPropertyChanged 
    {
        SteinbachEntities db;
        ObservableCollection<SI_Events> _selectedNames = new ObservableCollection<SI_Events>();
        ObservableCollection<SI_SelectedEvents> fk;
        Firmen_Personen CurrentPerson;

        List<SI_Events> _AvailableNames;

        public ListboxSelectedEventsViewModel(Firmen_Personen person, SteinbachEntities db)
        {
            _selectedNames.CollectionChanged += (sender, e) => UpdateCollection(sender, e);
            this.db = db;
            CurrentPerson = person;
            _AvailableNames = db.SI_Events.Where(f => f.istAktiv == 1).ToList();



        }

        void UpdateCollection(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems != null && e.NewItems.Count > 0)
                        {
                            foreach (SI_Events item in e.NewItems)
                            {
                                if (fk.Where(k => k.id_Event == item.id).Count() == 0)
                                {
                                    var k = new SI_SelectedEvents();
                                    k.id_Event = item.id;
                                    //k.Firmen_Personen = CurrentPerson;


                                    fk.Add(k);
                                    db.AddToSI_SelectedEvents(k);
                                    db.SaveChanges();

                                }

                            }
                        }

                        break;
                    }

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        if (e.OldItems != null && e.OldItems.Count > 0)
                        {
                            foreach (SI_Events item in e.OldItems)
                            {
                                if (fk.Where(k => k.id_Event == item.id).Count() == 1)
                                {
                                    var k = fk.Where(ka => ka.id_Event == item.id).SingleOrDefault();
                                    fk.Remove(k);
                                    db.DeleteObject(k);
                                    db.SaveChanges();
                                }
                            }
                        }
                        break;
                    }


                default:
                    {
                        break;
                    }

            }

        }




        public void AddSelectedNames()
        {

            fk = new ObservableCollection<SI_SelectedEvents>(db.SI_SelectedEvents);
            if (fk.Count > 0)
            {
                SelectedNames.Clear();
                foreach (var item in fk)
                {

                    SelectedNames.Add(item.SI_Events);
                }
                OnPropertyChanged("SelectedNames");
            }

        }



        public IEnumerable<SI_Events> AvailableNames
        {
            get
            {

                return _AvailableNames;
            }
        }




        public ObservableCollection<SI_Events> SelectedNames
        {
            get
            {
                return _selectedNames;
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


    }
}
