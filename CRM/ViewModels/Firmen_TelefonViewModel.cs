using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using SteinbachCRM.ViewModels.Interfaces;
using DAL;
using AutoMapper;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace SteinbachCRM.ViewModels
{
    [Export(typeof(Firmen_TelefonViewModel))]
    public class Firmen_TelefonViewModel : Screen, IDataMethods
    {

        BitmapImage[] Images;
        #region "Properties"

        private bool _isDirty;
        public bool isDirty
        {
            get { return _isDirty; }
            set
            {
                if (value != _isDirty)
                {
                    _isDirty = value;
                    Background = value == true ? System.Windows.Media.Brushes.Cyan : System.Windows.Media.Brushes.Lavender;
                    NotifyOfPropertyChange(() => isDirty);
                   
                }
            }
        }


        private System.Data.EntityState _EntityState;
        public System.Data.EntityState EntityState
        {
            get { return _EntityState; }
            set
            {
                if (value != _EntityState)
                {
                    _EntityState = value;
                    NotifyOfPropertyChange(() => EntityState);
                   
                }
            }
        }




        private Firmen_Personen _Firmen_Personen;
        public Firmen_Personen Firmen_Personen
        {
            get { return _Firmen_Personen; }
            set
            {
                if (value != _Firmen_Personen)
                {
                    _Firmen_Personen = value;
                    NotifyOfPropertyChange(() => Firmen_Personen);
                    isDirty = true;
                }
            }
        }



        private bool _ShowDialer;
        public bool ShowDialer
        {
            get { return _ShowDialer; }
            set
            {
                if (value != _ShowDialer)
                {
                    _ShowDialer = value;
                    NotifyOfPropertyChange(() => ShowDialer);
                    isDirty = true;
                }
            }
        }


        private BitmapImage _DialerImage;
        public BitmapImage DialerImage
        {
            get { return _DialerImage; }
            set
            {
                if (value != _DialerImage)
                {
                    _DialerImage = value;
                    NotifyOfPropertyChange(() => DialerImage);
                    isDirty = true;
                }

            }
        }


        private System.Windows.Media.Brush _Background;
        public System.Windows.Media.Brush Background
        {
            get { return _Background; }
            set
            {
                if (value != _Background)
                {
                    _Background = value;
                    NotifyOfPropertyChange(() => Background);
                   
                }
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

        private Int32? _id_Firma;
        public Int32? id_Firma
        {
            get { return _id_Firma; }
            set
            {
                if (value != _id_Firma)
                {
                    _id_Firma = value;
                    NotifyOfPropertyChange(() => id_Firma);
                    isDirty = true;
                }
            }
        }

        private Int32? _id_Person;
        public Int32? id_Person
        {
            get { return _id_Person; }
            set
            {
                if (value != _id_Person)
                {
                    _id_Person = value;
                    NotifyOfPropertyChange(() => id_Person);
                    isDirty = true;
                }
            }
        }

        private int _id_Standort;
        public int id_Standort
        {
            get { return _id_Standort; }
            set
            {
                if (value != _id_Standort)
                {
                    _id_Standort = value;
                    NotifyOfPropertyChange(() => id_Standort);
                    isDirty = true;
                }
            }
        }





        private Int32? _Typ;
        public Int32? Typ
        {
            get { return _Typ; }
            set
            {
                if (value != _Typ)
                {
                    _Typ = value;

                    ShowDialer = value == 16 ? false : true;
                    //DialerImage = value == 16 ? Images[1] : Images[0];
                    switch (value)
                    {
                        case 16:
                            {
                                DialerImage = Images[1];
                                break;
                            }
                        case 17:
                            {
                                DialerImage = Images[2];
                                break;
                            }

                        default:
                            {
                                DialerImage = Images[0];
                                break;
                            }
                    }

                    NotifyOfPropertyChange(() => Typ);
                    isDirty = true;
                }
            }
        }

        private String _Vorwahl;
        public String Vorwahl
        {
            get { return _Vorwahl; }
            set
            {
                if (value != _Vorwahl)
                {
                    _Vorwahl = value;
                    NotifyOfPropertyChange(() => Vorwahl);
                    isDirty = true;
                }
            }
        }

        private String _Rufnummer;
        public String Rufnummer
        {
            get { return _Rufnummer; }
            set
            {
                if (value != _Rufnummer)
                {
                    _Rufnummer = value;
                    NotifyOfPropertyChange(() => Rufnummer);
                    isDirty = true;
                }
            }
        }

        private String _Memo;
        public String Memo
        {
            get { return _Memo; }
            set
            {
                if (value != _Memo)
                {
                    _Memo = value;
                    NotifyOfPropertyChange(() => Memo);
                    isDirty = true;
                }
            }
        }

        #endregion

        #region "Automapper"
        public static Personen_Telefon GetPersonTelefon(SteinbachEntities db, Firmen_TelefonViewModel vm)
        {
            Personen_Telefon tel;
            if (vm.id == 0)
            {
                tel = new Personen_Telefon();
                db.AddToPersonen_Telefon(tel);

            }
            else
            {
                tel = db.Personen_Telefon.Where(i => i.id == vm.id).SingleOrDefault();
            }

            if (tel != null)
            {
                Mapper.Map<Firmen_TelefonViewModel, Personen_Telefon>(vm, tel);
                return tel;
            }
            return null;


        }
        #endregion

        #region "Constructors"
        public Firmen_TelefonViewModel()
        {
            BitmapImage[] _images = {new BitmapImage(new Uri("pack://application:,,,/SteinbachCRM;component/Images/Phone_thumb.png")), 
               new BitmapImage(new Uri("pack://application:,,,/SteinbachCRM;component/Images/Fax.png")),
                                 new BitmapImage(new Uri("pack://application:,,,/SteinbachCRM;component/Images/phone_mobile.png"))};


            Images = _images;
            isDirty = false;


        }
        #endregion


        public static Firmen_TelefonViewModel GetFirmenTelefonVM(Personen_Telefon pt)
        {
            var vm = new Firmen_TelefonViewModel();
            Mapper.Map<Personen_Telefon, Firmen_TelefonViewModel>(pt, vm);
            return vm;
        }


        public void SaveChanges(SteinbachEntities db)
        {
            if (isDirty)
            {



                Personen_Telefon tel;
                if (id == 0)
                {
                    tel = new Personen_Telefon();
                    db.AddToPersonen_Telefon(tel);

                }
                else
                {
                    tel = db.Personen_Telefon.Where(i => i.id == id).SingleOrDefault();
                }

                if (tel != null)
                {
                    
                    Mapper.Map<Firmen_TelefonViewModel, Personen_Telefon>(this, tel);
                  //  tel.id_Person = this.id_Person;
                    db.SaveChanges();
                    this.id = tel.id;
                    isDirty = false;
                   
                }
            }

        }


        public bool SaveChanges()
        {
            if (isDirty)
            {
               
                using (var db = new DAL.SteinbachEntities())
                {
                    Personen_Telefon tel;
                    if (id == 0)
                    {
                        tel = new Personen_Telefon();
                        db.AddToPersonen_Telefon(tel);

                    }
                    else
                    {
                        tel = db.Personen_Telefon.Where(i => i.id == id).SingleOrDefault();
                    }

                    if (tel != null)
                    {
                        this.Firmen_Personen = null;
                        Mapper.Map<Firmen_TelefonViewModel, Personen_Telefon>(this, tel);
                        db.SaveChanges();
                        isDirty = false;
                        return true;
                    }

                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool RejectChanges()
        {
            //throw new NotImplementedException();
            return false;
        }

        public event System.Action DoRejectChanges;


    }
}
