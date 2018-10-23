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
    
    public class MultipleListboxViewModelBase<TSource,TTarget> : Screen, IListboxKategorien
    {
        //SteinbachEntities db;
        //ObservableCollection<TSource> _selectedNames = new ObservableCollection<TSource>();
        //ObservableCollection<TTarget> fk;
        //TSource CurrentFirma;
        //Action<ObservableCollection<TTarget>, TSource> NewCollectionCheck;
        //Action<ObservableCollection<TTarget>, TSource> OldCollectionCheck;

        //List<TSource> _AvailableNames;

        //public MultipleListboxViewModelBase(TSource Firma, SteinbachEntities db, Func<List<TSource>> GetAvailableNames, Action<ObservableCollection<TTarget>, TSource> CheckNewCollection, Action<ObservableCollection<TTarget>, TSource> CheckOldCollection)
        //{
        //    NewCollectionCheck = CheckNewCollection;
        //    _selectedNames.CollectionChanged += (sender, e) => UpdateCollection(sender, e);
        //    this.db = db;
        //    CurrentFirma = Firma;
        //    //_AvailableNames = db.firmen.Where(f => f.istFirma == 1).ToList();
        //    _AvailableNames = GetAvailableNames();
        //}

        //void UpdateCollection(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    switch (e.Action)
        //    {
        //        case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
        //            {
        //                if (e.NewItems != null && e.NewItems.Count > 0)
        //                {
        //                    foreach (TSource item in e.NewItems)
        //                    {

        //                        NewCollectionCheck(fk, item);
        //                        //if (fk.Where(k => k.id_Kategorie == item.id).Count() == 0)
        //                        //{
        //                        //    var k = new Firmen_Kategorien();
        //                        //    k.id_Kategorie = item.id;
        //                        //    k.firma = CurrentFirma;


        //                        //    fk.Add(k);
        //                        //    db.AddToFirmen_Kategorien(k);
        //                        //    //     db.SaveChanges();

        //                        //}

        //                    }
        //                }

        //                break;
        //            }

        //        case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
        //            {
        //                if (e.OldItems != null && e.OldItems.Count > 0)
        //                {
        //                    foreach (TSource item in e.OldItems)
        //                    {
        //                        OldCollectionCheck(fk, item);
        //                        //if (fk.Where(k => k.id_Kategorie == item.id).Count() == 1)
        //                        //{
        //                        //    var k = fk.Where(ka => ka.id_Kategorie == item.id).SingleOrDefault();
        //                        //    fk.Remove(k);
        //                        //    db.DeleteObject(k);
        //                        //    // db.SaveChanges();
        //                        //}
        //                    }
        //                }
        //                break;
        //            }


        //        default:
        //            {
        //                break;
        //            }

        //    }

        //}




        //public void AddSelectedNames(Func<List<TTarget>> FKListe,string Name)
        //{

        //   fk = new ObservableCollection<TTarget>(FKListe());



        //   if (fk.Count > 0)
        //   {
        //       SelectedNames.Clear();
        //       foreach (var item in fk)
        //       {
   
        //           SelectedNames.Add(item.);
        //       }
        //       NotifyOfPropertyChange(() => SelectedNames);
        //   }

        //}



        //public IEnumerable<TSource> AvailableNames
        //{
        //    get
        //    {

        //        return _AvailableNames;
        //    }
        //}




        //public ObservableCollection<TSource> SelectedNames
        //{
        //    get
        //    {
        //        return _selectedNames;
        //    }
        //}




    }
}
