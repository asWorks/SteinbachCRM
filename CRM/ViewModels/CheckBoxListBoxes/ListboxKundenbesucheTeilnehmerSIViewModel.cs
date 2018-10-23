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
   
    public class ListboxKundenbesucheTeilnehmerSIViewModel:INotifyPropertyChanged 
    {
        SteinbachEntities db;
        ObservableCollection<person> _selectedNames = new ObservableCollection<person>();
        ObservableCollection<Kundenbesuche_TeilnehmerSI> fk;
        Firmen_Kundenbesuch CurrentBesuch;

        List<person> _AvailableNames;

        public event Action<bool> DataChanged;

        private void OnDataChanged(bool arg)
        {
            if (DataChanged != null)
            {
                DataChanged(arg);
            }

        }

        public ListboxKundenbesucheTeilnehmerSIViewModel(Firmen_Kundenbesuch besuch, SteinbachEntities db)
        {
            _selectedNames.CollectionChanged += (sender, e) => UpdateCollection(sender, e);
            this.db = db;
            CurrentBesuch = besuch;
            _AvailableNames = db.personen.Where(n=>n.ListeKontakteAktiv == 1).ToList();



        }

        void UpdateCollection(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems != null && e.NewItems.Count > 0)
                        {
                            foreach (person item in e.NewItems)
                            {
                                if (fk.Where(k => k.id_TeilnemerSI == item.id).Count() == 0)
                                {
                                    var k = new Kundenbesuche_TeilnehmerSI();
                                    k.id_TeilnemerSI = item.id;
                                    k.Firmen_Kundenbesuche = CurrentBesuch;


                                    fk.Add(k);
                                    db.AddToKundenbesuche_TeilnehmerSI(k);
                                    OnDataChanged(true);

                                }

                            }
                        }

                        break;
                    }

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        if (e.OldItems != null && e.OldItems.Count > 0)
                        {
                            foreach (person item in e.OldItems)
                            {
                                if (fk.Where(k => k.id_TeilnemerSI == item.id).Count() == 1)
                                {
                                    var k = fk.Where(ka => ka.id_TeilnemerSI == item.id).SingleOrDefault();
                                    fk.Remove(k);
                                    db.DeleteObject(k);
                                    OnDataChanged(true);
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

            fk = new ObservableCollection<Kundenbesuche_TeilnehmerSI>(db.Kundenbesuche_TeilnehmerSI.Where(k => k.id_besuch == CurrentBesuch.id));
            if (fk.Count > 0)
            {
                SelectedNames.Clear();
                foreach (var item in fk)
                {

                    SelectedNames.Add(item.person);
                }
                OnPropertyChanged("SelectedNames");
            }

        }



        public IEnumerable<person> AvailableNames
        {
            get
            {

                return _AvailableNames;
            }
        }




        public ObservableCollection<person> SelectedNames
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
