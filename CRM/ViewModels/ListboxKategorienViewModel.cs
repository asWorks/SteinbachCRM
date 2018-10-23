using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Input;
using DAL;
using SteinbachCRM.ViewModels.Interfaces;
using System.ComponentModel.Composition;
using Caliburn.Micro;


namespace SteinbachCRM.ViewModels
{
    [Export(typeof(IListboxKategorien))]
    public class ListboxKategorienViewModel : Screen, IListboxKategorien
    {
        SteinbachEntities db;
        ObservableCollection<firma> _selectedNames = new ObservableCollection<firma>();
        ObservableCollection<Firmen_Kategorien> fk;
        firma CurrentFirma;
     
        List<firma> _AvailableNames;

        public ListboxKategorienViewModel(firma Firma,SteinbachEntities db)
        {
            _selectedNames.CollectionChanged += (sender, e) => UpdateCollection(sender, e);
            this.db = db;
            CurrentFirma = Firma;
            _AvailableNames = db.firmen.Where(f => f.istFirma == 1).ToList();



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
                                if (fk.Where(k => k.id_Kategorie == item.id).Count() == 0)
                                {
                                    var k = new Firmen_Kategorien();
                                    k.id_Kategorie = item.id;
                                    k.firma = CurrentFirma;
                                    

                                    fk.Add(k);
                                    db.AddToFirmen_Kategorien(k);
                               //     db.SaveChanges();

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
                                if (fk.Where(k => k.id_Kategorie == item.id).Count() == 1)
                                {
                                    var k = fk.Where(ka => ka.id_Kategorie == item.id).SingleOrDefault();
                                    fk.Remove(k);
                                    db.DeleteObject(k);
                                   // db.SaveChanges();
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

            fk = new ObservableCollection<Firmen_Kategorien>(db.Firmen_Kategorien.Where(k => k.id_Firma == CurrentFirma.id));
            if (fk.Count > 0)
            {
                SelectedNames.Clear();
                foreach (var item in fk)
                {
                    
                    SelectedNames.Add(item.Kategorie);
                }
                NotifyOfPropertyChange(() => SelectedNames);
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




    }
}
