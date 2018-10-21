using System;
using System.Collections.Generic;

using System.Collections.ObjectModel;
using DAL;
using DAL.Tools;
using System.Data.Metadata.Edm;





namespace SteinbachCRM.ObjectCollections
{
    public class GenericObservableCollection<T> : ObservableCollection<T>
    {

        private SteinbachEntities _context;
        string EntitySetName;
      
        public SteinbachEntities Context
        {
            get { return _context; }
        }



        public GenericObservableCollection(IEnumerable<T> Query, SteinbachEntities context)
            :base(Query)
        {
            try
            {
                _context = context;
            }
            catch (Exception ex)
            {

                LoggingTool.LogExeption(ex, "FirmaCollection", "Constructor");
                //System.Windows.MessageBox.Show("Beim Initialisieren von vwBrunvollAuftragsbestand ist ein Fehler aufgetreten.");
            }



        }


        public GenericObservableCollection(IEnumerable<T> query, SteinbachEntities context , string entitySetName)
            : base(query)
        {
            try
            {
                _context = context;
                EntitySetName = entitySetName;
            }
            catch (Exception ex)
            {

                LoggingTool.LogExeption(ex, "FirmaCollection", "Constructor");
                //System.Windows.MessageBox.Show("Beim Initialisieren von vwBrunvollAuftragsbestand ist ein Fehler aufgetreten.");
            }



        }

        protected override void InsertItem(int index, T item)
        {

          // item.Logs.AssociationChanged += new System.ComponentModel.CollectionChangeEventHandler(Logs_AssociationChanged);
            this.Context.AddObject(EntitySetName,item);
            
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {

            try
            {
                T project = this[index];
               
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

