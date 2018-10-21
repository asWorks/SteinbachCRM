using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using DAL;
using AutoMapper;
using SteinbachCRM.ViewModels.Interfaces;
using System.Diagnostics;
using System.Windows;

namespace SteinbachCRM.ViewModels
{
    [Export(typeof(MailViewerViewModel))]
    public class MailViewerViewModel : Screen, IDataMethods
    {

        #region "Com_Properties"
        private ObservableCollection<DAL.firma> _lfcFirma;

        public ObservableCollection<DAL.firma> lfcFirma
        {
            get { return _lfcFirma; }

            set
            {
                _lfcFirma = value;
                NotifyOfPropertyChange(() => lfcFirma);
            }
        }

        private DAL.firma _SelectedlfcFirma;

        public DAL.firma SelectedlfcFirma
        {
            get { return _SelectedlfcFirma; }
            set
            {
                if (value != _SelectedlfcFirma)
                {
                    _SelectedlfcFirma = value;
                    id_Firma = value.id;
                    FillKontakte(value.id);
                    NotifyOfPropertyChange(() => SelectedlfcFirma);
                }

            }
        }


        private ObservableCollection<Firmen_Personen> _lfcKontakt;

        public ObservableCollection<Firmen_Personen> lfcKontakt
        {
            get { return _lfcKontakt; }
            set
            {
                _lfcKontakt = value;
                NotifyOfPropertyChange(() => lfcKontakt);
            }
        }

        private Firmen_Personen _SelectedlfcKontakt;

        public Firmen_Personen SelectedlfcKontakt
        {
            get { return _SelectedlfcKontakt; }
            set
            {
                if (value != _SelectedlfcKontakt)
                {
                    _SelectedlfcKontakt = value;
                    if (value == null)
                    {
                        id_Kontakt = null;
                    }
                    else
                    {
                        id_Kontakt = value.id;
                    }

                    NotifyOfPropertyChange(() => SelectedlfcKontakt);
                }

            }
        }

        private ObservableCollection<AuswahlEintraege> _lfcAktion;

        public ObservableCollection<AuswahlEintraege> lfcAktion
        {
            get { return _lfcAktion; }
            set
            {
                _lfcAktion = value;
                NotifyOfPropertyChange(() => lfcAktion);
            }
        }

        private AuswahlEintraege _SelectedlfcAktion;

        public AuswahlEintraege SelectedlfcAktion
        {
            get { return _SelectedlfcAktion; }
            set
            {
                if (value != _SelectedlfcAktion)
                {
                    _SelectedlfcAktion = value;
                    Aktion = value.id;
                    NotifyOfPropertyChange(() => _SelectedlfcAktion);
                }

            }
        }

        private ObservableCollection<AuswahlEintraege> _lfcStatus;

        public ObservableCollection<AuswahlEintraege> lfcStatus
        {
            get { return _lfcStatus; }
            set
            {
                _lfcStatus = value;
                NotifyOfPropertyChange(() => lfcStatus);
            }
        }

        private AuswahlEintraege _SelectedlfcStatus;

        public AuswahlEintraege SelectedlfcStatus
        {
            get { return _SelectedlfcStatus; }
            set
            {
                if (value != _SelectedlfcStatus)
                {
                    _SelectedlfcStatus = value;
                    Status = value.id;
                    NotifyOfPropertyChange(() => SelectedlfcStatus);
                }

            }
        }

        private ObservableCollection<EmailAttachments> _Attachments;

        public ObservableCollection<EmailAttachments> Attachments
        {
            get { return _Attachments; }
            set
            {
                if (value != null)
                {
                    _Attachments = value;
                    NotifyOfPropertyChange(() => Attachments);
                }


            }
        }

        private EmailAttachments _SelectedAttachment;

        public EmailAttachments SelectedAttachment
        {
            get { return _SelectedAttachment; }
            set
            {
                if (value != _SelectedAttachment)
                {
                    _SelectedAttachment = value;
                    OpenAttachment();
                    NotifyOfPropertyChange(() => SelectedAttachment);
                }


            }
        }


        #endregion

        #region "Properties"

        #region "Properties"

        public int ThisAttachment { get; set; }

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

        private String _Typ;
        public String Typ
        {
            get { return _Typ; }
            set
            {
                if (value != _Typ)
                {
                    _Typ = value;
                    NotifyOfPropertyChange(() => Typ);
                    isDirty = true;
                }
            }
        }

        private String _Absender;
        public String Absender
        {
            get { return _Absender; }
            set
            {
                if (value != _Absender)
                {
                    _Absender = value;
                    NotifyOfPropertyChange(() => Absender);
                    isDirty = true;
                }
            }
        }

        private String _AbsenderMailAdresse;
        public String AbsenderMailAdresse
        {
            get { return _AbsenderMailAdresse; }
            set
            {
                if (value != _AbsenderMailAdresse)
                {
                    _AbsenderMailAdresse = value;
                    NotifyOfPropertyChange(() => AbsenderMailAdresse);
                    isDirty = true;
                }
            }
        }

        private String _Empfaenger;
        public String Empfaenger
        {
            get { return _Empfaenger; }
            set
            {
                if (value != _Empfaenger)
                {
                    _Empfaenger = value;
                    NotifyOfPropertyChange(() => Empfaenger);
                    isDirty = true;
                }
            }
        }

        private String _Empfaengerliste;
        public String Empfaengerliste
        {
            get { return _Empfaengerliste; }
            set
            {
                if (value != _Empfaengerliste)
                {
                    _Empfaengerliste = value;
                    NotifyOfPropertyChange(() => Empfaengerliste);
                    isDirty = true;
                }
            }
        }

        private String _Betreff;
        public String Betreff
        {
            get { return _Betreff; }
            set
            {
                if (value != _Betreff)
                {
                    _Betreff = value;
                    NotifyOfPropertyChange(() => Betreff);
                    isDirty = true;
                }
            }
        }

        private String _Text;
        public String Text
        {
            get { return _Text; }
            set
            {
                if (value != _Text)
                {
                    _Text = value;
                    NotifyOfPropertyChange(() => Text);
                    isDirty = true;
                }
            }
        }

        private DateTime? _Datum;
        public DateTime? Datum
        {
            get { return _Datum; }
            set
            {
                if (value != _Datum)
                {
                    _Datum = value;
                    NotifyOfPropertyChange(() => Datum);
                    isDirty = true;
                }
            }
        }

        private String _HTMLText;
        public String HTMLText
        {
            get { return _HTMLText; }
            set
            {
                if (value != _HTMLText)
                {
                    _HTMLText = value;
                    NotifyOfPropertyChange(() => HTMLText);
                    isDirty = true;
                }
            }
        }

        private Int64? _RTFText;
        public Int64? RTFText
        {
            get { return _RTFText; }
            set
            {
                if (value != _RTFText)
                {
                    _RTFText = value;
                    NotifyOfPropertyChange(() => RTFText);
                    isDirty = true;
                }
            }
        }

        private String _EntryID;
        public String EntryID
        {
            get { return _EntryID; }
            set
            {
                if (value != _EntryID)
                {
                    _EntryID = value;
                    NotifyOfPropertyChange(() => EntryID);
                    isDirty = true;
                }
            }
        }

        private String _ConversationID;
        public String ConversationID
        {
            get { return _ConversationID; }
            set
            {
                if (value != _ConversationID)
                {
                    _ConversationID = value;
                    NotifyOfPropertyChange(() => ConversationID);
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

        private Int32? _id_Kontakt;
        public Int32? id_Kontakt
        {
            get { return _id_Kontakt; }
            set
            {
                if (value != _id_Kontakt)
                {
                    _id_Kontakt = value;
                    NotifyOfPropertyChange(() => id_Kontakt);
                    isDirty = true;
                }
            }
        }

        private Int32? _id_PersonSI;
        public Int32? id_PersonSI
        {
            get { return _id_PersonSI; }
            set
            {
                if (value != _id_PersonSI)
                {
                    _id_PersonSI = value;
                    NotifyOfPropertyChange(() => id_PersonSI);
                    isDirty = true;
                }
            }
        }

        private Int64? _RTF_Text;
        public Int64? RTF_Text
        {
            get { return _RTF_Text; }
            set
            {
                if (value != _RTF_Text)
                {
                    _RTF_Text = value;
                    NotifyOfPropertyChange(() => RTF_Text);
                    isDirty = true;
                }
            }
        }

        private Int32? _id_Projekt;
        public Int32? id_Projekt
        {
            get { return _id_Projekt; }
            set
            {
                if (value != _id_Projekt)
                {
                    _id_Projekt = value;
                    NotifyOfPropertyChange(() => id_Projekt);
                    isDirty = true;
                }
            }
        }

        private String _OutlookUsername;
        public String OutlookUsername
        {
            get { return _OutlookUsername; }
            set
            {
                if (value != _OutlookUsername)
                {
                    _OutlookUsername = value;
                    NotifyOfPropertyChange(() => OutlookUsername);
                    isDirty = true;
                }
            }
        }

        private String _BodyFormat;
        public String BodyFormat
        {
            get { return _BodyFormat; }
            set
            {
                if (value != _BodyFormat)
                {
                    _BodyFormat = value;
                    NotifyOfPropertyChange(() => BodyFormat);
                    isDirty = true;
                }
            }
        }

        private Int32? _Aktion;
        public Int32? Aktion
        {
            get { return _Aktion; }
            set
            {
                if (value != _Aktion)
                {
                    _Aktion = value;
                    NotifyOfPropertyChange(() => Aktion);
                    isDirty = true;
                }
            }
        }

        private Int32? _Status;
        public Int32? Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    NotifyOfPropertyChange(() => Status);
                    isDirty = true;
                }
            }
        }

        #endregion


        #endregion

        #region "Constructors"

        public MailViewerViewModel(int MailId)
        {

            try
            {
                using (var db = new SteinbachEntities())
                {
                    var m = db.CRMEmails.Where(id => id.id == MailId).SingleOrDefault();
                    // mv = new MailViewerViewModel();
                    Mapper.CreateMap<CRMEmails, MailViewerViewModel>();
                    Mapper.Map<CRMEmails, MailViewerViewModel>(m, this);
                    //InitCombBoxes();
                    lfcFirma = new ObservableCollection<firma>(db.firmen.Where(f => f.IstKunde == 1));
                    SelectedlfcFirma = db.firmen.Where(f => f.id == id_Firma).SingleOrDefault();

                    lfcAktion = new ObservableCollection<AuswahlEintraege>(db.AuswahlEintraege.Where(e => e.Gruppe == "EmailAktion"));
                    SelectedlfcAktion = db.AuswahlEintraege.Where(e => e.id == Aktion).SingleOrDefault();

                    lfcStatus = new ObservableCollection<AuswahlEintraege>(db.AuswahlEintraege.Where(e => e.Gruppe == "EmailStatus"));
                    SelectedlfcStatus = db.AuswahlEintraege.Where(e => e.id == Status).SingleOrDefault();

                    FillKontakte((int)id_Firma);

                   // Attachments = new ObservableCollection<EmailAttachments>(db.EmailAttachments.Where(a => a.id_Email == id && a.isEmbedded == 0));
                    Attachments = new ObservableCollection<EmailAttachments>(db.EmailAttachments.Where(a => a.id_Email == id));
                    SelectedAttachment = db.EmailAttachments.Where(a => a.id == ThisAttachment).SingleOrDefault();

                    isDirty = false;

                }
            }
            catch (Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);
            }

        }


        #endregion

        #region "Methods"

        public bool RejectChanges()
        {
            return true;
        }

        public event System.Action DoRejectChanges;


        public bool SaveChanges()
        {
            SaveData();
            return true;
        }
        public void FillKontakte(int id_Firma)
        {
            try
            {
                using (var db = new SteinbachEntities())
                {
                    lfcKontakt = new ObservableCollection<Firmen_Personen>(db.Firmen_Personen.Where(p => p.id_Firma == id_Firma));
                    _SelectedlfcKontakt = db.Firmen_Personen.Where(p => p.id == id_Kontakt).SingleOrDefault();

                }
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
                using (var db = new SteinbachEntities())
                {
                    CRMEmails mail = db.CRMEmails.Where(id => id.id == this.id).SingleOrDefault();
                    Mapper.CreateMap<MailViewerViewModel, CRMEmails>();
                    Mapper.Map<MailViewerViewModel, CRMEmails>(this, mail);

                    db.SaveChanges();
                    isDirty = false;

                }
            }
            catch (Exception ex)
            {
                CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex);

            }




        }

        public void OpenAttachment()
        {
            if (SelectedAttachment != null)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(SelectedAttachment.Path));
                }
                catch (Exception ex)
                {
                    CommonTools.Tools.ErrorMethods.ShowErrorMessage(ex, true);

                }

            }

        }

        public int DeleteMail()
        {
            if (MessageBox.Show(String.Format("Email {0} wirklich löschen ?",this.Betreff),"Sicherheitsabfrage",MessageBoxButton.YesNo)== MessageBoxResult.Yes)
            {
                if (CommonTools.ObjectFactories.EmailFactory.DeleteEmail(this.id) == 0 )
                {
                    MessageBox.Show("Beim Löschen der Email trat ein Fehler auf.");

                    return 0;
                }

                return 1;
            }

            return 0;
        }
        #endregion
    }
}
