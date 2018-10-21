using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SteinbachCRM.Models
{
    public class SelectedAdressesModel:INotifyPropertyChanged
    {

        public bool isDirty { get; set; }
        private bool _AdresseChanged;
        public bool AdresseChanged
        {
            get { return _AdresseChanged; }
            set
            {
                if (value != _AdresseChanged)
                {
                    _AdresseChanged = value;
                    OnPropertyChanged("AdresseChanged");
                    isDirty = true;
                 
                }
            }
        }
        public int id { get; set; }
        public string Nachname { get; set; }
        public string Vorname { get; set; }
        public string Mailadresse { get; set; }
        public string Vorname2 { get; set; }
        public string Namenszusatz { get; set; }
        public string Anrede { get; set; }
        public string Anrede1 { get; set; }
        public string Funktion { get; set; }
        public string Titel { get; set; }
        private string _PLZ;
        public string PLZ
        {
            get { return _PLZ; }
            set
            {
                if (value != _PLZ)
                {
                    _PLZ = value;
                    OnPropertyChanged("PLZ");
                    AdresseChanged = true;
                    isDirty = true;
                }
            }
        }


        public string Ort { get; set; }
        public string Straße { get; set; }
        public string Land { get; set; }
        public string Vorwahl { get; set; }
        public string Rufnummer { get; set; }
        public string Bezeichnung { get; set; }
        public bool UnSelect { get; set; }
        public string Firmenname { get; set; }
        public string Firmenkurzname { get; set; }
        private string _Result;
        public string Result
        {
            get { return _Result; }
            set
            {
                if (value != _Result)
                {
                    _Result = value;
                    OnPropertyChanged("Result");
                    
                }
            }
        }


    
      
        private bool _isSent;
        public bool isSent
        {
            get { return _isSent; }
            set
            {
                if (value != _isSent)
                {
                    _isSent = value;
                  OnPropertyChanged("isSent");
                   
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


    }
}
