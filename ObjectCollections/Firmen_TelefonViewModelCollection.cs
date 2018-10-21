using System;
using System.Collections.Generic;

using System.Collections.ObjectModel;
using DAL;
using DAL.Tools;
using SteinbachCRM.ViewModels;
using System.Data.Objects.DataClasses;
using AutoMapper;
using System.Windows;





namespace SteinbachCRM.ObjectCollections
{
    public class Firmen_TelefonViewModelCollection : ObservableCollection<Firmen_TelefonViewModel>
    {

        private List<Firmen_TelefonViewModel> Deleted;
        private SteinbachEntities _context;

        public SteinbachEntities Context
        {
            get { return _context; }
        }



        public Firmen_TelefonViewModelCollection()
            : base()
        {


        }


        public Firmen_TelefonViewModelCollection(EntityCollection<Personen_Telefon> Query, SteinbachEntities context)
            : base()
        {
            try
            {
                _context = context;

                foreach (var item in Query)
                {

                    

                    Firmen_TelefonViewModel vm = new Firmen_TelefonViewModel();
                    var e = (EntityObject)item;
                    vm.EntityState = e.EntityState;
                    Mapper.Map<Personen_Telefon, Firmen_TelefonViewModel>(item, vm);
                    vm.isDirty = false;
                    base.InsertItem(base.Items.Count, vm);



                }

            }
            catch (Exception ex)
            {

                LoggingTool.LogExeption(ex, "FirmaCollection", "Constructor");

            }


        }




        public void AddItem(Firmen_Personen person)
        {
            Firmen_TelefonViewModel vm = new Firmen_TelefonViewModel();
            vm.Firmen_Personen = person;
            base.InsertItem(base.Items.Count, vm);



        }




        public void AddItem(int Parent_ID)
        {
            Firmen_TelefonViewModel vm = new Firmen_TelefonViewModel();
            vm.id_Person = Parent_ID;
            base.InsertItem(base.Items.Count, vm);



        }



        public void DeleteItem(Firmen_TelefonViewModel vm)
        {
            if (Deleted == null)
            {
                Deleted = new List<Firmen_TelefonViewModel>();
            }
            Deleted.Add(vm);
            base.Remove(vm);

        }


        public void Save()
        {
            try
            {
                foreach (var item in base.Items)
                {
                    item.SaveChanges(Context);
                }

                if (Deleted != null)
                {
                    foreach (var item in Deleted.ToArray())
                    {
                        var pers = Firmen_TelefonViewModel.GetPersonTelefon(Context, item);
                        Context.DeleteObject(pers);
                        Deleted.Remove(item);
                    }

                    Deleted.Clear();
                    Deleted = null;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Fehler in Firmen_TelefonViewModelCollection " + ex.Message);
            }




        }

        public bool isDirty
        {

            get
            {
                bool vmDirty = false;
                foreach (var item in base.Items)
                {
                    if (item.isDirty)
                    {
                        vmDirty = true;
                    }
                }

                var dirty = vmDirty || Deleted != null;

                return dirty;

            }
        }



    }
}

