using System;
using System.Collections.Generic;

using System.Collections.ObjectModel;
using DAL;
using DAL.Tools;
using SteinbachCRM.ViewModels;





namespace SteinbachCRM.ObjectCollections
{
    public class Firmen_TelefonCollection : ObservableCollection<Firmen_Telefon>
    {

        private SteinbachEntities _context;
      
        public SteinbachEntities Context
        {
            get { return _context; }
        }



        public Firmen_TelefonCollection(IEnumerable<Firmen_Telefon> Query, SteinbachEntities context)
            :base(Query)
        {
            try
            {
                _context = context;
            }
            catch (Exception ex)
            {

                LoggingTool.LogExeption(ex, "FirmaCollection", "Constructor");
               
            }



        }

        protected override void InsertItem(int index, Firmen_Telefon item)
        {

           // item.Logs.AssociationChanged += new System.ComponentModel.CollectionChangeEventHandler(Logs_AssociationChanged);
            
            this.Context.AddToFirmen_Telefon(item);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {

            try
            {
                Firmen_Telefon project = this[index];
               
                this.Context.DeleteObject(this[index]);
                base.RemoveItem(index);
            }
            catch (Exception ex)
            {

                string Message;
                Message = ex.Message;
                if (ex.InnerException != null)
                {
                    Message += '\n';
                    Message += ex.InnerException.Message;

                }
                throw new Exception(Message);
            }

        }


    }
}

