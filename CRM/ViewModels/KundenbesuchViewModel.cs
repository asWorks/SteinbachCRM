using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using SteinbachCRM.ViewModels.Interfaces;
using System.Collections.ObjectModel;
using DAL;
using AutoMapper;
using System.Windows.Threading;
using SteinbachCRM.ObjectCollections;


namespace SteinbachCRM.ViewModels
{
    [Export(typeof(KundenbesuchViewModel))]
    public class KundenbesuchViewModel : Screen, IDataMethods
    {


        #region "Declaration"
        private DispatcherTimer timer;
        private DispatcherTimer timer1;
        Firmen_Kundenbesuch besuch;
        SteinbachEntities db;
        #endregion

        #region "Constructors"

        public KundenbesuchViewModel(Firmen_Kundenbesuch Besuch)
        {
            try
            {

                db = new SteinbachEntities();
                besuch = Besuch;



                Mapper.CreateMap<Firmen_Kundenbesuch, KundenbesuchViewModel>()
                    .ForMember(dest => dest.SI_Firma, opt => opt.Ignore())
                    .ForMember(dest => dest.SI_Person, opt => opt.Ignore());

                //Mapper.AssertConfigurationIsValid(); 
                Mapper.Map<Firmen_Kundenbesuch, KundenbesuchViewModel>(besuch, this);

                Firmen = new ObservableCollection<firma>(db.firmen.Where(f => f.IstKunde == 1));
                SelectedFirmen = db.firmen.Where(f => f.id == id_firma).SingleOrDefault();


                SI_Person = new ObservableCollection<person>(db.personen);
                SelectedSI_Person = db.personen.Where(p => p.id == id_siperson).SingleOrDefault();

                SI_Firma = new ObservableCollection<firma>(db.firmen.Where(f => f.istFirma == 1));
                SelectedSI_Firma = db.firmen.Where(f => f.id == id_vertretenefirma).SingleOrDefault();
                // ListBoxTeilnehmerExternBesucheVM = new ListboxTeilnehmerExternBesucheViewModel(id);


               // PopulateListboxes(besuch);
                // PopulateListboxes(id);
                PopulateListboxes(besuch);
                ReduceAvailablenames = true;
               // ChkBoxAvailablenamesEnabled = true;
                PopulateListboxTeilnehmerExtern();
                ChkBoxAvailablenamesEnabled = false;

               

                isDirty = false;

                //}
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);
            }

        }






        public KundenbesuchViewModel(int besuchID)
        {
            try
            {
                id = besuchID;

                db = new SteinbachEntities();
                if (besuchID == 0)
                {
                    besuch = CommonTools.ObjectFactories.KundenbesuchsBerichteFactory.GetNewBesuch();
                }
                else
                {
                    besuch = db.Firmen_Kundenbesuche.Where(k => k.id == besuchID).SingleOrDefault();

                }




                Mapper.CreateMap<Firmen_Kundenbesuch, KundenbesuchViewModel>()
                    .ForMember(dest => dest.SI_Firma, opt => opt.Ignore())
                    .ForMember(dest => dest.SI_Person, opt => opt.Ignore());

                //Mapper.AssertConfigurationIsValid(); 
                Mapper.Map<Firmen_Kundenbesuch, KundenbesuchViewModel>(besuch, this);

                Firmen = new ObservableCollection<firma>(db.firmen.Where(f => f.IstKunde == 1));
                SelectedFirmen = db.firmen.Where(f => f.id == id_firma).SingleOrDefault();


                SI_Person = new ObservableCollection<person>(db.personen);
                SelectedSI_Person = db.personen.Where(p => p.id == id_siperson).SingleOrDefault();

                SI_Firma = new ObservableCollection<firma>(db.firmen.Where(f => f.istFirma == 1));
                SelectedSI_Firma = db.firmen.Where(f => f.id == id_vertretenefirma).SingleOrDefault();
                // ListBoxTeilnehmerExternBesucheVM = new ListboxTeilnehmerExternBesucheViewModel(id);

                PopulateListboxes(besuch);
                // PopulateListboxes(id);
                ChkBoxAvailablenamesEnabled = true;
                isDirty = false;

                //}
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);
            }

        }
        #endregion

        #region "UI"
        private bool _ReduceAvailablenames;
        public bool ReduceAvailablenames
        {
            get { return _ReduceAvailablenames; }
            set
            {
                if (value != _ReduceAvailablenames)
                {
                    _ReduceAvailablenames = value;
                    if (value == true)
                    {
                        if (ListBoxTeilnehmerExternBesucheVM != null)
                        {
                            ListBoxTeilnehmerExternBesucheVM.UpdateAvailableNames((int)id_firma);
                            ChkBoxAvailablenamesEnabled = false;
                        }

                    }

                    else
                    {
                        if (ListBoxTeilnehmerExternBesucheVM != null)
                        {
                            ListBoxTeilnehmerExternBesucheVM.AddSelectedNames(true);
                        }
                    }
                    NotifyOfPropertyChange(() => ReduceAvailablenames);

                }
            }
        }

        private bool _ChkBoxAvailablenamesEnabled;
        public bool ChkBoxAvailablenamesEnabled
        {
            get { return _ChkBoxAvailablenamesEnabled; }
            set
            {
                if (value != _ChkBoxAvailablenamesEnabled)
                {
                    _ChkBoxAvailablenamesEnabled = value;
                    NotifyOfPropertyChange(() => ChkBoxAvailablenamesEnabled);
                }
            }
        }





        #endregion


        #region "Properties"
        private bool _isDirty;

        public bool isDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                NotifyOfPropertyChange(() => isDirty);

            }
        }



        private Int32 _id;
        public Int32 id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyOfPropertyChange(() => id);
                    isDirty = true;
                }
            }
        }

        private Int32? _id_firma;
        public Int32? id_firma
        {
            get { return _id_firma; }
            set
            {
                if (value != _id_firma)
                {
                    _id_firma = value;
                    NotifyOfPropertyChange(() => id_firma);
                    isDirty = true;
                }
            }
        }

        private Int32? _id_kontakt;
        public Int32? id_kontakt
        {
            get { return _id_kontakt; }
            set
            {
                if (value != _id_kontakt)
                {
                    _id_kontakt = value;
                    NotifyOfPropertyChange(() => id_kontakt);
                    isDirty = true;
                }
            }
        }

        private String _kfzkennzeichen;
        public String kfzkennzeichen
        {
            get { return _kfzkennzeichen; }
            set
            {
                if (value != _kfzkennzeichen)
                {
                    _kfzkennzeichen = value;
                    NotifyOfPropertyChange(() => kfzkennzeichen);
                    isDirty = true;
                }
            }
        }

        private Decimal? _kmgefahren;
        public Decimal? kmgefahren
        {
            get { return _kmgefahren; }
            set
            {
                if (value != _kmgefahren)
                {
                    _kmgefahren = value;
                    NotifyOfPropertyChange(() => kmgefahren);
                    isDirty = true;
                }
            }
        }

        private String _position;
        public String position
        {
            get { return _position; }
            set
            {
                if (value != _position)
                {
                    _position = value;
                    NotifyOfPropertyChange(() => position);
                    isDirty = true;
                }
            }
        }

        private DateTime? _datum_von;
        public DateTime? datum_von
        {
            get { return _datum_von; }
            set
            {
                if (value != _datum_von)
                {
                    _datum_von = value;
                    datum_bis = value;
                    NotifyOfPropertyChange(() => datum_von);
                    isDirty = true;
                }
            }
        }

        private DateTime? _datum_bis;
        public DateTime? datum_bis
        {
            get { return _datum_bis; }
            set
            {
                if (value != _datum_bis)
                {
                    _datum_bis = value;
                    NotifyOfPropertyChange(() => datum_bis);
                    isDirty = true;
                }
            }
        }

        private Int32? _id_siperson;
        public Int32? id_siperson
        {
            get { return _id_siperson; }
            set
            {
                if (value != _id_siperson)
                {
                    _id_siperson = value;
                    NotifyOfPropertyChange(() => id_siperson);
                    isDirty = true;
                }
            }
        }

        private Int32? _id_vertretenefirma;
        public Int32? id_vertretenefirma
        {
            get { return _id_vertretenefirma; }
            set
            {
                if (value != _id_vertretenefirma)
                {
                    _id_vertretenefirma = value;
                    NotifyOfPropertyChange(() => id_vertretenefirma);
                    isDirty = true;
                }
            }
        }

        private String _kurzbericht;
        public String kurzbericht
        {
            get { return _kurzbericht; }
            set
            {
                if (value != _kurzbericht)
                {
                    _kurzbericht = value;
                    NotifyOfPropertyChange(() => kurzbericht);
                    isDirty = true;
                }
            }
        }

        private String _produkte;
        public String produkte
        {
            get { return _produkte; }
            set
            {
                if (value != _produkte)
                {
                    _produkte = value;
                    NotifyOfPropertyChange(() => produkte);
                    isDirty = true;
                }
            }
        }

        private String _besuchsthema;
        public String besuchsthema
        {
            get { return _besuchsthema; }
            set
            {
                if (value != _besuchsthema)
                {
                    _besuchsthema = value;
                    NotifyOfPropertyChange(() => besuchsthema);
                    isDirty = true;
                }
            }
        }

        private String _todo;
        public String todo
        {
            get { return _todo; }
            set
            {
                if (value != _todo)
                {
                    _todo = value;
                    NotifyOfPropertyChange(() => todo);
                    isDirty = true;
                }
            }
        }

        private String _projektnummer;
        public String projektnummer
        {
            get { return _projektnummer; }
            set
            {
                if (value != _projektnummer)
                {
                    _projektnummer = value;
                    NotifyOfPropertyChange(() => projektnummer);
                    isDirty = true;
                }
            }
        }

        private Int32? _id_projekt;
        public Int32? id_projekt
        {
            get { return _id_projekt; }
            set
            {
                if (value != _id_projekt)
                {
                    _id_projekt = value;
                    NotifyOfPropertyChange(() => id_projekt);
                    isDirty = true;
                }
            }
        }

        #endregion
        #region "Collections"

        private ListboxTeilnehmerExternBesucheViewModel _ListBoxTeilnehmerExternBesucheVM;
        public ListboxTeilnehmerExternBesucheViewModel ListBoxTeilnehmerExternBesucheVM
        {
            get { return _ListBoxTeilnehmerExternBesucheVM; }
            set
            {
                if (value != _ListBoxTeilnehmerExternBesucheVM)
                {
                    _ListBoxTeilnehmerExternBesucheVM = value;
                    NotifyOfPropertyChange(() => ListBoxTeilnehmerExternBesucheVM);
                    isDirty = true;
                }
            }
        }

        private ListboxKundenbesucheVertreteneFirmenViewModel _ListboxKundenbesucheVertreteneFirmenVM;
        public ListboxKundenbesucheVertreteneFirmenViewModel ListboxKundenbesucheVertreteneFirmenVM
        {
            get { return _ListboxKundenbesucheVertreteneFirmenVM; }
            set
            {
                if (value != _ListboxKundenbesucheVertreteneFirmenVM)
                {
                    _ListboxKundenbesucheVertreteneFirmenVM = value;
                    NotifyOfPropertyChange(() => ListboxKundenbesucheVertreteneFirmenVM);
                    isDirty = true;
                }
            }
        }

        private ListboxKundenbesucheTeilnehmerSIViewModel _ListboxKundenbesucheTeilnehmerSIVM;
        public ListboxKundenbesucheTeilnehmerSIViewModel ListboxKundenbesucheTeilnehmerSIVM
        {
            get { return _ListboxKundenbesucheTeilnehmerSIVM; }
            set
            {
                if (value != _ListboxKundenbesucheTeilnehmerSIVM)
                {
                    _ListboxKundenbesucheTeilnehmerSIVM = value;
                    NotifyOfPropertyChange(() => ListboxKundenbesucheTeilnehmerSIVM);
                    isDirty = true;
                }
            }
        }











        private ObservableCollection<firma> _Firmen;
        public ObservableCollection<firma> Firmen
        {
            get { return _Firmen; }
            set
            {
                if (value != _Firmen)
                {
                    _Firmen = value;
                    NotifyOfPropertyChange(() => Firmen);
                    isDirty = true;
                }
            }
        }

        private firma _SelectedFirmen;
        public firma SelectedFirmen
        {
            get { return _SelectedFirmen; }
            set
            {
                if (value != _SelectedFirmen)
                {
                    if (value != null)
                    {
                        id_firma = value.id;
                        FillKontakte();

                        if (ListBoxTeilnehmerExternBesucheVM != null)
                        {
                            ListBoxTeilnehmerExternBesucheVM.UpdateAvailableNames((int)id_firma);
                        }

                    }
                    _SelectedFirmen = value;


                    NotifyOfPropertyChange(() => SelectedFirmen);
                    isDirty = true;
                }
            }
        }


        private ObservableCollection<Firmen_Personen> _Kontakte;
        public ObservableCollection<Firmen_Personen> Kontakte
        {
            get { return _Kontakte; }
            set
            {
                if (value != _Kontakte)
                {
                    _Kontakte = value;
                    NotifyOfPropertyChange(() => Kontakte);
                    isDirty = true;
                }
            }
        }

        private Firmen_Personen _SelectedKontakte;
        public Firmen_Personen SelectedKontakte
        {
            get { return _SelectedKontakte; }
            set
            {
                if (value != null)
                {
                    if (value != _SelectedKontakte)
                    {
                        _SelectedKontakte = value;
                        id_kontakt = value.id;
                        NotifyOfPropertyChange(() => SelectedKontakte);
                        isDirty = true;
                    }
                }

            }
        }



        private ObservableCollection<person> _SI_Person;
        public ObservableCollection<person> SI_Person
        {
            get { return _SI_Person; }
            set
            {
                if (value != _SI_Person)
                {
                    _SI_Person = value;
                    NotifyOfPropertyChange(() => SI_Person);
                    isDirty = true;
                }
            }
        }

        private person _SelectedSI_Person;
        public person SelectedSI_Person
        {
            get { return _SelectedSI_Person; }
            set
            {
                if (value != _SelectedSI_Person)
                {
                    _SelectedSI_Person = value;
                    id_siperson = value.id;
                    NotifyOfPropertyChange(() => SelectedSI_Person);
                    isDirty = true;
                }
            }
        }




        private ObservableCollection<firma> _SI_Firma;
        public ObservableCollection<firma> SI_Firma
        {
            get { return _SI_Firma; }
            set
            {
                if (value != _SI_Firma)
                {
                    _SI_Firma = value;
                    NotifyOfPropertyChange(() => SI_Firma);
                    isDirty = true;
                }
            }
        }

        private firma _SelectedSI_Firma;
        public firma SelectedSI_Firma
        {
            get { return _SelectedSI_Firma; }
            set
            {
                if (value != _SelectedSI_Firma)
                {
                    _SelectedSI_Firma = value;
                    id_vertretenefirma = value.id;
                    NotifyOfPropertyChange(() => SelectedSI_Firma);
                    isDirty = true;
                }
            }
        }




        #endregion


        #region "Functions"

        public void onDoRejectChanges()
        {
            if (DoRejectChanges != null)
            {
                DoRejectChanges();
            }

        }



        private void PopulateListboxes(Firmen_Kundenbesuch Besuch)
        {
            ListBoxTeilnehmerExternBesucheVM = new ListboxTeilnehmerExternBesucheViewModel(db, besuch);
            ListboxKundenbesucheVertreteneFirmenVM = new ListboxKundenbesucheVertreteneFirmenViewModel(besuch, db);
            ListboxKundenbesucheTeilnehmerSIVM = new ListboxKundenbesucheTeilnehmerSIViewModel(besuch, db);

            ListBoxTeilnehmerExternBesucheVM.DataChanged += new Action<bool>(ListBoxTeilnehmerExternBesucheVM_DataChanged);
            ListboxKundenbesucheVertreteneFirmenVM.DataChanged += ListboxKundenbesucheVertreteneFirmenVM_DataChanged;
            ListboxKundenbesucheTeilnehmerSIVM.DataChanged += ListboxKundenbesucheTeilnehmerSIVM_DataChanged;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();

        }

        private void PopulateListboxTeilnehmerExtern()
        {
            timer1 = new DispatcherTimer();
            timer1.Tick += timer1_Tick;
            timer1.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer1.Start();

        }

        void timer1_Tick(object sender, EventArgs e)
        {

            if (ListBoxTeilnehmerExternBesucheVM != null)
            {
                ListBoxTeilnehmerExternBesucheVM.UpdateAvailableNames((int)id_firma);

            }
            timer1.Stop();

        }

        void ListboxKundenbesucheTeilnehmerSIVM_DataChanged(bool obj)
        {
            isDirty = true;
        }

        void ListboxKundenbesucheVertreteneFirmenVM_DataChanged(bool obj)
        {
            isDirty = true;
        }

        void ListBoxTeilnehmerExternBesucheVM_DataChanged(bool obj)
        {
            isDirty = true;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            ListBoxTeilnehmerExternBesucheVM.AddSelectedNames(true);
            ListboxKundenbesucheVertreteneFirmenVM.AddSelectedNames();
            ListboxKundenbesucheTeilnehmerSIVM.AddSelectedNames();
            timer.Stop();
        }

        public void FillKontakte()
        {
            try
            {
                //using (var db = new SteinbachEntities())
                //{
                Kontakte = new ObservableCollection<Firmen_Personen>(db.Firmen_Personen.Where(f => f.id_Firma == id_firma));
                SelectedKontakte = db.Firmen_Personen.Where(p => p.id == id_kontakt).SingleOrDefault();

                //}
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);
            }

        }


        public void SaveData()
        {
            try
            {
                //using (var db = new SteinbachEntities())
                //{


                //Firmen_Kundenbesuch besuch;
                //if (id == 0)
                //{
                //    besuch = new Firmen_Kundenbesuch();
                //    db.AddToFirmen_Kundenbesuche(besuch);
                //}
                //else
                //{
                //    besuch = db.Firmen_Kundenbesuche.Where(k => k.id == id).SingleOrDefault();


                //}


                Mapper.CreateMap<KundenbesuchViewModel, Firmen_Kundenbesuch>();
                Mapper.Map<KundenbesuchViewModel, Firmen_Kundenbesuch>(this, besuch);

                // ListBoxTeilnehmerExternBesucheVM.SaveData();
                if (this.id == 0)
                {
                    db.AddToFirmen_Kundenbesuche(besuch);
                }


                db.SaveChanges();
                this.id = besuch.id;
                isDirty = false;

                //}
            }
            catch (Exception ex)
            {
                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);

            }




        }



        private void LoadFirmen(string filter)
        {

            using (var db = new SteinbachEntities())
            {
                try
                {
                    if (filter == string.Empty)
                    {

                        var query = from f in db.firmen
                                    where f.IstKunde == 1
                                    orderby f.name
                                    select f;

                        Firmen = new FirmenCollection(query, db);
                    }
                    else
                    {
                        var query = db.firmen.Where(f => f.name.Contains(filter) && f.IstKunde == 1).OrderBy(f => f.name);
                        Firmen = new FirmenCollection(query, db);


                    }
                }
                catch (Exception ex)
                {
                    CommonTools.Tools.ErrorMethods.HandleStandardError(ex);
                    // MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));
                }
            }






        }
        #endregion

        #region "DataMethods"
        public bool SaveChanges()
        {
            SaveData();
            return true;
        }

        public bool RejectChanges()
        {

            return true;
        }

        public event System.Action DoRejectChanges;


        #endregion

        #region "Commands"

        public void Save()
        {
            SaveData();

        }

        public void Reject()
        {
            // isDirty = false;
            onDoRejectChanges();


        }
        #endregion

        #region "Events"
        public void FirmenonfcbChanged(CommonTools.UserControls.FilteredComboBoxChangedEventArgs e)
        {
            LoadFirmen(e.filter);

        }

        #endregion

    }
}
