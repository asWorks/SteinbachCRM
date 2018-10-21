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
   
    public class ListboxKundenbesucheVertreteneFirmenViewModel:INotifyPropertyChanged 
    {
        SteinbachEntities db;
        ObservableCollection<firma> _selectedNames = new ObservableCollection<firma>();
        ObservableCollection<Kundenbesuche_VertreteneFirmen> fk;
        Firmen_Kundenbesuch CurrentBesuch;
        List<firma> _AvailableNames;
        public event Action<bool> DataChanged;

        private void OnDataChanged(bool arg)
        {
            if (DataChanged != null)
            {
                DataChanged(arg);
            }

        }

        public ListboxKundenbesucheVertreteneFirmenViewModel(Firmen_Kundenbesuch besuch, SteinbachEntities db)
        {
            _selectedNames.CollectionChanged += (sender, e) => UpdateCollection(sender, e);
            this.db = db;
            CurrentBesuch = besuch ;
            _AvailableNames = db.firmen.Where(n=>n.istFirma==1).ToList();



        }

        void UpdateCollection(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems != null && e.NewItems.Count > 0)
                        {
                            foreach (firma item in e.NewItems)
                            {
                                if (fk.Where(k => k.id_VertreteneFirma == item.id).Count() == 0)
                                {
                                    var k = new Kundenbesuche_VertreteneFirmen();
                                    k.id_VertreteneFirma = item.id;
                                    k.Firmen_Kundenbesuche = CurrentBesuch; 


                                    fk.Add(k);
                                    db.AddToKundenbesuche_VertreteneFirmen(k);
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
                            foreach (firma item in e.OldItems)
                            {
                                if (fk.Where(k => k.id_VertreteneFirma == item.id).Count() == 1)
                                {
                                    var k = fk.Where(ka => ka.id_VertreteneFirma == item.id).SingleOrDefault();
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

            fk = new ObservableCollection<Kundenbesuche_VertreteneFirmen>(db.Kundenbesuche_VertreteneFirmen.Where(k => k.id_besuch == CurrentBesuch.id));
            if (fk.Count > 0)
            {
                SelectedNames.Clear();
                foreach (var item in fk)
                {

                    SelectedNames.Add(item.firma);
                }
                OnPropertyChanged("SelectedNames");
            }

        }



        public IEnumerable<firma> AvailableNames
        {
            get
            {

                return _AvailableNames;
            }
        }




        public ObservableCollection<firma> SelectedNames
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
