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
    [Export(typeof(IListboxTeilnehmerSI))]
    public class ListboxTeilnehmerSIViewModel : Screen, IListboxTeilnehmerSI
    {
        SteinbachEntities db;
        ObservableCollection<person> _selectedNames = new ObservableCollection<person>();
        ObservableCollection<Termine_TeilnehmerSI> fk;
        CRMTermine CurrentTermin;

        List<person> _AvailableNames;

        public ListboxTeilnehmerSIViewModel(CRMTermine termin, SteinbachEntities db)
        {
            _selectedNames.CollectionChanged += (sender, e) => UpdateCollection(sender, e);
            this.db = db;
            CurrentTermin = termin;
            _AvailableNames = db.personen.ToList();



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
                                if (fk.Where(k => k.id_Teilnehmer == item.id).Count() == 0)
                                {
                                    var k = new Termine_TeilnehmerSI();
                                    k.id_Teilnehmer = item.id;
                                    k.CRMTermine = CurrentTermin;

                                    fk.Add(k);
                                    db.AddToTermine_TeilnehmerSI(k);
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
                            foreach (person item in e.OldItems)
                            {
                                if (fk.Where(k => k.id_Teilnehmer == item.id).Count() == 1)
                                {

                                    var k = fk.Where(ka => ka.id_Teilnehmer == item.id).SingleOrDefault();
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

            if (CurrentTermin != null)
            {
                 fk = new ObservableCollection<Termine_TeilnehmerSI>(db.Termine_TeilnehmerSI.Where(k => k.id_Termin == CurrentTermin.id));
                if (fk.Count > 0)
                {
                    SelectedNames.Clear();
                    foreach (var item in fk)
                    {
                        SelectedNames.Add(item.person);
                    }
                    NotifyOfPropertyChange(() => SelectedNames);
                }
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




    }
}
