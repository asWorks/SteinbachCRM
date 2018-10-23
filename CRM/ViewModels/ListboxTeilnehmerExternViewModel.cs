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
    public class ListboxTeilnehmerExternViewModel : Screen, IListboxTeilnehmerExtern
    {
        SteinbachEntities db;
        ObservableCollection<Firmen_Personen> _selectedNames = new ObservableCollection<Firmen_Personen>();
        ObservableCollection<Termin_TeilnehmerExtern> fk;
        CRMTermine CurrentTermin;

        //   List<Firmen_Personen> _AvailableNames;




        public ListboxTeilnehmerExternViewModel(CRMTermine termin, SteinbachEntities db)
        {
            _selectedNames.CollectionChanged += (sender, e) => UpdateCollection(sender, e);
            this.db = db;
            CurrentTermin = termin;
            if (termin != null && termin.id_firma != null && termin.id_firma != 0)
            {
                _AvailableNames = new ObservableCollection<Firmen_Personen>(db.Firmen_Personen.Where(f => f.id_Firma == termin.id_firma).ToList());
            }
            else
            {
                _AvailableNames = new ObservableCollection<Firmen_Personen>(db.Firmen_Personen.Where(f => f.id_Firma == 0).ToList());
            }
        }

        public void UpdateAvailableNames(int id)
        {

            AvailableNames = new ObservableCollection<Firmen_Personen>(db.Firmen_Personen.Where(f => f.id_Firma == id).ToList());
            //AddSelectedNames();
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
                                    if (fk.Where(k => k.id_Teilnehmer == item.id).Count() == 0)
                                    {
                                        var k = new Termin_TeilnehmerExtern();
                                        k.id_Teilnehmer = item.id;
                                        k.CRMTermine = CurrentTermin;

                                        fk.Add(k);
                                        db.AddToTermin_TeilnehmerExtern(k);
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
                                foreach (Firmen_Personen item in e.OldItems)
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

            if (CurrentTermin != null)
            {
                fk = new ObservableCollection<Termin_TeilnehmerExtern>(db.Termin_TeilnehmerExtern.Where(k => k.id_Termin == CurrentTermin.id));
                if (fk.Count > 0)
                {
                    SelectedNames.Clear();
                    foreach (var item in fk)
                    {
                        SelectedNames.Add(item.Firmen_Personen);
                    }
                    NotifyOfPropertyChange(() => SelectedNames);
                }
            }

                
           
        }



        //public IEnumerable<Firmen_Personen> AvailableNames
        //{
        //    get
        //    {

        //        return _AvailableNames;
        //    }
        //}




        public ObservableCollection<Firmen_Personen> SelectedNames
        {
            get
            {
                return _selectedNames;
            }
        }




    }
}
