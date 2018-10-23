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
    [Export(typeof(IListboxEigenschaften))]
    public class ListboxEigenschaftenViewModel : Screen, IListboxEigenschaften
    {
        SteinbachEntities db;
        ObservableCollection<AuswahlEintraege> _selectedNames = new ObservableCollection<AuswahlEintraege>();
        ObservableCollection<Firma_Eigenschaften> fk;
        firma CurrentFirma;
     
        List<AuswahlEintraege> _AvailableNames;

        public ListboxEigenschaftenViewModel(firma Firma,SteinbachEntities db)
        {
            _selectedNames.CollectionChanged += (sender, e) => UpdateCollection(sender, e);
            this.db = db;
            CurrentFirma = Firma;
            _AvailableNames = db.AuswahlEintraege.Where(f => f.Gruppe == "TypFirmaEigenschaften").ToList();



        }

        void UpdateCollection(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {


            switch (e.Action)
            {

                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems != null && e.NewItems.Count > 0)
                        {
                            foreach (AuswahlEintraege item in e.NewItems)
                            {
                                if (fk.Where(k => k.id_Eigenschaft == item.id).Count() == 0)
                                {
                                    var k = new Firma_Eigenschaften();
                                    k.id_Eigenschaft = item.id;
                                    k.firma = CurrentFirma;
                                    

                                    fk.Add(k);
                                    db.AddToFirma_Eigenschaften(k);
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
                            foreach (AuswahlEintraege item in e.OldItems)
                            {
                                if (fk.Where(k => k.id_Eigenschaft == item.id).Count() == 1)
                                {
                                    var k = fk.Where(ka => ka.id_Eigenschaft == item.id).SingleOrDefault();
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

            fk = new ObservableCollection<Firma_Eigenschaften>(db.Firma_Eigenschaften.Where(k => k.id_Firma == CurrentFirma.id));
            if (fk.Count > 0)
            {
                SelectedNames.Clear();
                foreach (var item in fk)
                {
                    
                    SelectedNames.Add(item.AuswahlEintraege);
                }
                NotifyOfPropertyChange(() => SelectedNames);
            }

        }



        public IEnumerable<AuswahlEintraege> AvailableNames
        {
            get
            {
               
                return _AvailableNames;
            }
        }




        public ObservableCollection<AuswahlEintraege> SelectedNames
        {
            get
            {
                return _selectedNames;
            }
        }




    }
}
