using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteinbachCRM.Models
{
    public class KommunikationModel
    {


        public int ItemID { get; set; }

     

        private DateTime _DatumUhrzeit;

        public DateTime DatumUhrzeit
        {
            get { return _DatumUhrzeit; }
            set
            {
                _DatumUhrzeit = value;
               // NotifyOfPropertyChange(() => DatumUhrzeit);
            }
        }

        private string _Aktion;

        public string Aktion
        {
            get { return _Aktion; }
            set
            {
                _Aktion = value;
               // NotifyOfPropertyChange(() => Aktion);
            }
        }

        private string _Person;

        public string Person
        {
            get { return _Person; }
            set
            {
                _Person = value;
               // NotifyOfPropertyChange(() => Person);
            }
        }


        private string _Betreff;

        public string Betreff
        {
            get { return _Betreff; }
            set
            {
                _Betreff = value;
               // NotifyOfPropertyChange(() => Betreff);
            }
        }

        private string _Benutzer;

        public string Benutzer
        {
            get { return _Benutzer; }
            set
            {
                _Benutzer = value;
                //NotifyOfPropertyChange(() => Benutzer);
            }
        }

        private string _Gebiet;

        public string Gebiet
        {
            get { return _Gebiet; }
            set
            {
                _Gebiet = value;
               // NotifyOfPropertyChange(() => Gebiet);
            }
        }

        private string[] _ExterneTeilnehmer;

        public string[] ExterneTeilnehmer
        {
            get { return _ExterneTeilnehmer; }
            set
            {
                _ExterneTeilnehmer = value;
              //  NotifyOfPropertyChange(() => ExterneTeilnehmer);
            }
        }


        private string _ExterneTeilnehmerString;

        public string ExterneTeilnehmerString
        {
            get { return _ExterneTeilnehmerString; }
            set
            {
                _ExterneTeilnehmerString = value;
               // NotifyOfPropertyChange(() => ExterneTeilnehmerString);
            }
        }

        private string _Info;

        public string Info
        {
            get { return _Info; }
            set
            {
                _Info = value;
              //  NotifyOfPropertyChange(() => Info);
            }
        }

        private string _Status;

        public string Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
              //  NotifyOfPropertyChange(() => Status);
            }
        }

        private string _Type;

        public string Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
              //  NotifyOfPropertyChange(() => Type);
            }
        }
    }
}
