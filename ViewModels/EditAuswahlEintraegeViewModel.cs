using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SteinbachCRM.ViewModels.Interfaces;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;
using DAL;
using System.Windows;

namespace SteinbachCRM.ViewModels
{
    [Export(typeof(IEditAuswahlEintraege))]
    class EditAuswahlEintraegeViewModel : Screen, IEditAuswahlEintraege, IDataState
    {
        SteinbachEntities db;

        public EditAuswahlEintraegeViewModel()
        {
            db = new SteinbachEntities();
            KategorienListe = new ObservableCollection<AuswahlEintraegeGruppen>(db.AuswahlEintraegeGruppen);
        }


        private AuswahlEintraegeGruppen _CurrentKategorie;

        public AuswahlEintraegeGruppen CurrentKategorie
        {
            get { return _CurrentKategorie; }
            set
            {
                _CurrentKategorie = value;
                NotifyOfPropertyChange(() => CurrentKategorie);
            }
        }


        private ObservableCollection<AuswahlEintraege> _EintraegeListe;

        public ObservableCollection<AuswahlEintraege> EintraegeListe
        {
            get { return _EintraegeListe; }
            set
            {
                _EintraegeListe = value;
                NotifyOfPropertyChange(() => EintraegeListe);
            }
        }


        private ObservableCollection<AuswahlEintraegeGruppen> _KategorienListe;

        public ObservableCollection<AuswahlEintraegeGruppen> KategorienListe
        {
            get { return _KategorienListe; }
            set
            {
                _KategorienListe = value;
                NotifyOfPropertyChange(() => KategorienListe);

            }
        }







        public void GridGruppen_SelectionChanged(C1.WPF.DataGrid.C1DataGrid grid)
        {
            try
            {
                var item = (AuswahlEintraegeGruppen)grid.SelectedItem;
                _CurrentKategorie = item;
                EintraegeListe = new ObservableCollection<AuswahlEintraege>(db.AuswahlEintraege.Where(e => e.id_Gruppe == item.id));
            }
            catch (Exception)
            {


            }


        }


        public void AddGruppe()
        {
            if (KategorienListe != null)
            {
                var Eintrag = new AuswahlEintraegeGruppen();
                Eintrag.Gruppe = "NG" + DateTime.Now.ToString();
               
                KategorienListe.Add(Eintrag);
                db.AddToAuswahlEintraegeGruppen(Eintrag);

            } 

        }

        public void AddEintrag()
        {
            if (EintraegeListe != null && CurrentKategorie != null)
            {
                var Eintrag = new AuswahlEintraege();
                Eintrag.AuswahlEintraegeGruppen = CurrentKategorie;
                Eintrag.Gruppe = CurrentKategorie.Gruppe;
                EintraegeListe.Add(Eintrag);
                db.AddToAuswahlEintraege(Eintrag);


            }



        }
        //C1.WPF.DataGrid.C1DataGrid grid
        //RoutedEventArgs e
        public void GridEintraege_DeleteEintrag(RoutedEventArgs e)
        {
            //try
            //{
            //    var grid = (C1.WPF.DataGrid.C1DataGrid)e.Source;
            //    var item = (AuswahlEintraegeGruppen)grid.SelectedItem;
            //    var eintrag = db.AuswahlEintraege.Where(id => id.id == item.id).SingleOrDefault();

            //    if (MessageBox.Show(string.Format("Eintrag {0} wirklich endgültig löschen ?", eintrag.Eintrag), "Sicherheitsabfrage",
            //       MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //    {
            //        EintraegeListe.Remove(eintrag);
            //        db.DeleteObject(eintrag);
            //        db.SaveChanges();

            //    }




            //}
            //catch (Exception)
            //{


            //}

        }


        //public void DeleteEintrag()
        //{
        //    Console.WriteLine("DeleteEintrag");
        //}

        public void GridEintraege_DeletingRows(C1.WPF.DataGrid.DataGridDeletingRowsEventArgs e)
        {

            if (e.DeletedRows.Count() == 1)
            {
                var adr = (AuswahlEintraege)e.DeletedRows[0].DataItem;
                if (MessageBox.Show(string.Format("Eintrag {0} wirklich endgültig löschen ?", adr.Eintrag), "Sicherheitsabfrage",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    db.DeleteObject(adr);

                }
                else
                {
                    e.Cancel = true;
                }
            }

        }

        public void SaveChanges()
        {
            db.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
        }


        public bool isDirty
        {
            get { return false; }
        }
    }


}
