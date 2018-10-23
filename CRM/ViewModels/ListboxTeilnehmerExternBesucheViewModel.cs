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
    [Export(typeof(IListboxTeilnehmerExtern))]
    public class ListboxTeilnehmerExternBesucheViewModel : Screen, IListboxTeilnehmerExtern
    {
        SteinbachEntities db;
        ObservableCollection<Firmen_Personen> _selectedNames = new ObservableCollection<Firmen_Personen>();
        ObservableCollection<Kundenbesuch_TeilnehmerExtern> fk;
        Firmen_Kundenbesuch CurrentBesuch;
        ObservableCollection<Firmen_Personen> Buffer;
        public event Action<bool> DataChanged;

        private void OnDataChanged(bool arg)
        {
            if (DataChanged != null)
            {
                DataChanged(arg);
            }

        }


        public ListboxTeilnehmerExternBesucheViewModel(SteinbachEntities db, Firmen_Kundenbesuch besuch)
        {
            this.db = db;   

            CurrentBesuch = besuch;
            int firmaID = besuch.id_firma == null ? 0 : (int)besuch.id_firma;
            _selectedNames.CollectionChanged += (sender, e) => UpdateCollection(sender, e);
            _AvailableNames = new ObservableCollection<Firmen_Personen>(db.Firmen_Personen.Where(f => f.id_Firma == firmaID).ToList());

        }

        public void UpdateAvailableNames(int id)
        {

            AvailableNames = new ObservableCollection<Firmen_Personen>(db.Firmen_Personen.Where(f => f.id_Firma == id).ToList());
           
        }


        void UpdateCollection(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            try
            {

                switch (e.Action)
                {

                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        {
                            if (e.NewItems != null && e.NewItems.Count > 0)
                            {
                                foreach (Firmen_Personen item in e.NewItems)
                                {
                                    if (fk.Where(k => k.id_TeilnehmerExtern == item.id).Count() == 0)
                                    {
                                        var k = new Kundenbesuch_TeilnehmerExtern();
                                        k.id_TeilnehmerExtern = item.id;
                                       
                                        k.Firmen_Kundenbesuche = CurrentBesuch;
                                        fk.Add(k);
                                      
                                        db.AddToKundenbesuche_TeilnehmerExtern(k);
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
                                foreach (Firmen_Personen item in e.OldItems)
                                {
                                    if (fk.Where(k => k.id_TeilnehmerExtern == item.id).Count() == 1)
                                    {

                                        var k = fk.Where(ka => ka.id_TeilnehmerExtern == item.id).SingleOrDefault();
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
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);
            }

        }

        private ObservableCollection<Firmen_Personen> _AvailableNames;

        public ObservableCollection<Firmen_Personen> AvailableNames
        {
            get { return _AvailableNames; }
            set
            {
                _AvailableNames = value;
                NotifyOfPropertyChange(() => AvailableNames);
            }
        }



        public void AddSelectedNames()
        {
            AddSelectedNames(false);
        }



        public void AddSelectedNames(bool CutAvailableNames)
        {

            if (CurrentBesuch != null)
            {
                fk = new ObservableCollection<Kundenbesuch_TeilnehmerExtern>(db.Kundenbesuche_TeilnehmerExtern.Where(k => k.id_besuch == CurrentBesuch.id));
                if (fk.Count > 0)
                {
                    SelectedNames.Clear();
                    foreach (var item in fk)
                    {
                        SelectedNames.Add(item.Firmen_Personen);
                    }
                    NotifyOfPropertyChange(() => SelectedNames);
                }

                if (CutAvailableNames)
                {
                     AvailableNames = SelectedNames;
                }
               
            }



        }






        public ObservableCollection<Firmen_Personen> SelectedNames
        {
            get
            {
                return _selectedNames;
            }
        }




    }
}
