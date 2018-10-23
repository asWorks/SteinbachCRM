using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using DAL;
using System.Collections.ObjectModel;
using SteinbachCRM.ViewModels.Interfaces;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Threading;

namespace SteinbachCRM.ViewModels
{
    public class SI_MailingSelectionViewModel : PropertyChangedBase, IDataMethods
    {
        private enum SelectionType
        {
            Email,
            Serienbrief,
            Adressetikett,
            None
        }
        #region "Properties"
        SteinbachEntities db;
        private DispatcherTimer tiTimer;
        private SelectionType selectionType = SelectionType.None;

        public event Action<string, bool> ChangeVisibility;

        private Dispatcher _ThisDispatcher;
        public Dispatcher ThisDispatcher
        {
            get { return _ThisDispatcher; }
            set
            {
                if (value != _ThisDispatcher)
                {
                    _ThisDispatcher = value;
                    NotifyOfPropertyChange(() => ThisDispatcher);

                }
            }
        }






        private bool _isDirty;
        public bool isDirty
        {
            get
            {
                //return _isDirty;

                return CommonTools.Tools.ManageChanges.isDirty(db);
            }
            set
            {
                if (value != _isDirty)
                {
                    _isDirty = value;
                    NotifyOfPropertyChange(() => isDirty);

                }
            }
        }

        private string _SelectedAction;
        public string SelectedAction
        {
            get { return _SelectedAction; }
            set
            {
                if (value != _SelectedAction)
                {
                    _SelectedAction = value;
                    NotifyOfPropertyChange(() => SelectedAction);
                    isDirty = true;
                }
            }
        }

        private string _MailSubject;
        public string MailSubject
        {
            get { return _MailSubject; }
            set
            {
                if (value != _MailSubject)
                {
                    _MailSubject = value;
                    NotifyOfPropertyChange(() => MailSubject);
                    SetisExecutEnabled();
                    isDirty = true;
                }
            }
        }

        private string _MailBody;
        public string MailBody
        {
            get { return _MailBody; }
            set
            {
                if (value != _MailBody)
                {
                    _MailBody = value;
                    NotifyOfPropertyChange(() => MailBody);
                    SetisExecutEnabled();
                    isDirty = true;
                }
            }
        }

        private bool _isExecuteEnabled;
        public bool isExecuteEnabled
        {
            get { return _isExecuteEnabled; }
            set
            {
                if (value != _isExecuteEnabled)
                {
                    _isExecuteEnabled = value;
                    NotifyOfPropertyChange(() => isExecuteEnabled);

                }
            }
        }

        private bool _rbOutlook;
        public bool rbOutlook
        {
            get { return _rbOutlook; }
            set
            {
                if (value != _rbOutlook)
                {
                    _rbOutlook = value;
                    NotifyOfPropertyChange(() => rbOutlook);

                }
            }
        }

        private bool _rbExchange;
        public bool rbExchange
        {
            get { return _rbExchange; }
            set
            {
                if (value != _rbExchange)
                {
                    _rbExchange = value;
                    NotifyOfPropertyChange(() => rbExchange);
                    isDirty = true;
                }
            }
        }






        private ListboxSelectedEventsViewModel _ListboxSelectedEventsVM;
        public ListboxSelectedEventsViewModel ListboxSelectedEventsVM
        {
            get { return _ListboxSelectedEventsVM; }
            set
            {
                if (value != _ListboxSelectedEventsVM)
                {
                    _ListboxSelectedEventsVM = value;
                    NotifyOfPropertyChange(() => ListboxSelectedEventsVM);
                    isDirty = true;
                }
            }
        }


        private ListboxSelectedKategorienViewModel _ListboxSelectedKategorienVM;
        public ListboxSelectedKategorienViewModel ListboxSelectedKategorienVM
        {
            get { return _ListboxSelectedKategorienVM; }
            set
            {
                if (value != _ListboxSelectedKategorienVM)
                {
                    _ListboxSelectedKategorienVM = value;
                    NotifyOfPropertyChange(() => ListboxSelectedKategorienVM);
                    isDirty = true;
                }
            }
        }

        private ListboxSelectedTypFirmaEigenschaftenViewModel _ListboxSelectedTypFirmaEigenschaftenVM;
        public ListboxSelectedTypFirmaEigenschaftenViewModel ListboxSelectedTypFirmaEigenschaftenVM
        {
            get { return _ListboxSelectedTypFirmaEigenschaftenVM; }
            set
            {
                if (value != _ListboxSelectedTypFirmaEigenschaftenVM)
                {
                    _ListboxSelectedTypFirmaEigenschaftenVM = value;
                    NotifyOfPropertyChange(() => ListboxSelectedTypFirmaEigenschaftenVM);
                    isDirty = true;
                }
            }
        }




        #endregion


        #region "ObservableCollection"


        private ObservableCollection<Models.SelectedAdressesModel> _SelectedAdressen;
        public ObservableCollection<Models.SelectedAdressesModel> SelectedAdressen
        {
            get { return _SelectedAdressen; }
            set
            {
                if (value != _SelectedAdressen)
                {
                    _SelectedAdressen = value;
                    NotifyOfPropertyChange(() => SelectedAdressen);
                    isDirty = true;
                }
            }
        }


        private ObservableCollection<string> _AttachedFiles;
        public ObservableCollection<string> AttachedFiles
        {
            get { return _AttachedFiles; }
            set
            {
                if (value != _AttachedFiles)
                {
                    _AttachedFiles = value;
                    NotifyOfPropertyChange(() => AttachedFiles);
                    isDirty = true;
                }
            }
        }






        #endregion

        #region "Constructors"
        public SI_MailingSelectionViewModel()
        {
            db = new SteinbachEntities();

            ListboxSelectedEventsVM = new ListboxSelectedEventsViewModel(null, db);
            ListboxSelectedKategorienVM = new ListboxSelectedKategorienViewModel(db);
            ListboxSelectedTypFirmaEigenschaftenVM = new ListboxSelectedTypFirmaEigenschaftenViewModel(db);


            tiTimer = new DispatcherTimer();
            tiTimer.Tick += tiTimer_Tick;
            tiTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            tiTimer.Start();
            selectionType = SelectionType.None;
            SelectedAction = "Keine Aktion gewählt";
            AttachedFiles = new ObservableCollection<string>();
            SetisExecutEnabled();

            int mailMethod = CommonTools.Tools.ConfigEntry<int>.GetConfigEntry("MailMethod", "1", "1 = Exchangeserver, 0=Outlook Client");

            rbExchange = mailMethod == 1 ? true : false;
            rbOutlook = !rbExchange;


            db.ExecuteStoreCommand("delete from SI_SelectedEvents");
            db.ExecuteStoreCommand("delete from SI_SelectedKategorien");
            db.ExecuteStoreCommand("delete from SI_SelectedTypFirmaEigenschaften");





        }

        void tiTimer_Tick(object sender, EventArgs e)
        {
            ListboxSelectedEventsVM.AddSelectedNames();
            ListboxSelectedKategorienVM.AddSelectedNames();
            ListboxSelectedTypFirmaEigenschaftenVM.AddSelectedNames();
            tiTimer.Stop();
        }
        #endregion

        #region "UI Methods and Events"
        private void SetisExecutEnabled()
        {
            switch (selectionType)
            {
                case SelectionType.Email:
                    {
                        if (!string.IsNullOrEmpty(MailBody) && !string.IsNullOrEmpty(MailSubject) && SelectedAdressen.Count() > 0)
                        {
                            isExecuteEnabled = true;
                        }
                        else
                        {
                            isExecuteEnabled = false;
                        }

                        break;
                    }

                case SelectionType.Serienbrief:
                    {
                        if (SelectedAdressen.Count() > 0)
                        {
                            isExecuteEnabled = true;
                        }
                        else
                        {
                            isExecuteEnabled = false;
                        }
                        break;
                    }

                case SelectionType.Adressetikett:
                    {
                        if (SelectedAdressen.Count() > 0)
                        {
                            isExecuteEnabled = true;
                        }
                        else
                        {
                            isExecuteEnabled = false;
                        }
                        break;
                    }
                case SelectionType.None:
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region "Functions"
        //public void AddEvent()
        //{
        //    var ev = new SI_Events();
        //    db.AddToSI_Events(ev);
        //    events.Add(ev);
        //}

        public void Save()
        {
            db.SaveChanges();
        }

        public void TestIsDirty()
        {
                       ObservableCollection<Models.SelectedAdressesModel> test = new ObservableCollection<Models.SelectedAdressesModel>(SelectedAdressen.Where(n => n.isDirty == true));
                       foreach (var item in test)
                       {
                           CommonTools.Tools.UserMessage.NotifyUser(string.Format("{0} {1} {2} {3}",item.id,item.Vorname,item.Nachname,item.PLZ));
;
                       }
            
            
 
        }

        public void SelectEmails()
        {

            var x = (from t in db.vw_UnionSelectedAdressen
                     group t by new
                     {
                         t.id_Firmen_Personen,
                         t.Firmenname,
                         t.Nachname,
                         t.Vorname,
                         t.Anrede,
                         t.Titel,
                         t.Funktion,
                         t.Vorname2,
                         t.Namenszusatz,
                         t.Mailadresse
                     } into g

                     select new Models.SelectedAdressesModel
                     {
                         id=g.Key.id_Firmen_Personen,
                         Nachname = g.Key.Nachname,
                         Vorname = g.Key.Vorname,
                         Vorname2 = g.Key.Vorname2,
                         Anrede = g.Key.Anrede,
                         Titel = g.Key.Titel,
                         Funktion = g.Key.Funktion,
                         Firmenname = g.Key.Firmenname,
                         Mailadresse = g.Key.Mailadresse,
                         UnSelect = string.IsNullOrEmpty(g.Key.Mailadresse) ? true : false,
                         isSent = false,
                         isDirty = false,
                         AdresseChanged = false,
                         Result = " ? "

                     }

                         ).OrderBy(n => n.Nachname).OrderBy(n => n.Firmenname);

            SelectedAdressen = new ObservableCollection<Models.SelectedAdressesModel>(x);
            ResetSelectedAdresses();

            if (ChangeVisibility != null)
            {
                ChangeVisibility("8", false);
                ChangeVisibility("9,10,11,12,13,14", true);
            }

            selectionType = SelectionType.Email;
            SelectedAction = "Email versenden";
            SetisExecutEnabled();

        }

        public void SelectSerialLetters()
        {
            var x = (from t in db.vw_UnionSelectedAdressen
                     group t by new
                     {
                         t.id_Firmen_Personen,
                         t.Nachname,
                         t.Vorname,
                         t.Anrede,
                         t.Anrede1,
                         t.Titel,
                         t.Funktion,
                         t.Vorname2,
                         t.Namenszusatz,
                         t.Ort,
                         t.Straße,
                         t.PLZ,
                         t.Land,
                         t.Firmenname
                     } into g

                     select new Models.SelectedAdressesModel
                     {
                         id=g.Key.id_Firmen_Personen,
                         Nachname = g.Key.Nachname,
                         Vorname = g.Key.Vorname,
                         Vorname2 = g.Key.Vorname2,
                         Anrede = g.Key.Anrede,
                         Anrede1 = g.Key.Anrede1,
                         Titel = g.Key.Titel,
                         Funktion = g.Key.Funktion,
                         Ort = g.Key.Ort,
                         Straße = g.Key.Straße,
                         PLZ = g.Key.PLZ,
                         Land = g.Key.Land,
                         Firmenname = g.Key.Firmenname,
                         UnSelect = string.IsNullOrEmpty(g.Key.Ort)
                                        || string.IsNullOrEmpty(g.Key.Straße)
                                            || string.IsNullOrEmpty(g.Key.PLZ)
                                                || string.IsNullOrEmpty(g.Key.Land)
                                                    || string.IsNullOrEmpty(g.Key.Nachname)
                                                          || string.IsNullOrEmpty(g.Key.Anrede) ? true : false,
                         isSent = false,
                         isDirty=false,
                         AdresseChanged=false,
                         Result = " ? "
                     }

                           ).OrderBy(n => n.Nachname).OrderBy(n => n.Firmenname);

            SelectedAdressen = new ObservableCollection<Models.SelectedAdressesModel>(x);
            ResetSelectedAdresses();
            if (ChangeVisibility != null)
            {
                ChangeVisibility("8,9,10", true);
                ChangeVisibility("11,12,13,14", false);
            }

            selectionType = SelectionType.Serienbrief;

            SelectedAction = "Serienbrief erstellen";
            SetisExecutEnabled();

        }

        public void SelectLabels()
        {
            var x = (from t in db.vw_UnionSelectedAdressen
                     group t by new
                     {
                         t.id_Firmen_Personen,
                         t.Nachname,
                         t.Vorname,
                         t.Anrede,
                         t.Anrede1,
                         t.Titel,
                         t.Funktion,
                         t.Vorname2,
                         t.Namenszusatz,
                         t.Ort,
                         t.Straße,
                         t.PLZ,
                         t.Land,
                         t.Firmenname
                     } into g

                     select new Models.SelectedAdressesModel
                     {
                         id=g.Key.id_Firmen_Personen,
                         Nachname = g.Key.Nachname,
                         Vorname = g.Key.Vorname,
                         Vorname2 = g.Key.Vorname2,
                         Anrede = g.Key.Anrede,
                         Anrede1 = g.Key.Anrede1,
                         Titel = g.Key.Titel,
                         Funktion = g.Key.Funktion,
                         Ort = g.Key.Ort,
                         Straße = g.Key.Straße,
                         PLZ = g.Key.PLZ,
                         Land = g.Key.Land,
                         Firmenname = g.Key.Firmenname,
                         UnSelect = string.IsNullOrEmpty(g.Key.Ort)
                                        || string.IsNullOrEmpty(g.Key.Straße)
                                            || string.IsNullOrEmpty(g.Key.PLZ)
                                                || string.IsNullOrEmpty(g.Key.Land)
                                                    || string.IsNullOrEmpty(g.Key.Nachname)
                                                      || string.IsNullOrEmpty(g.Key.Anrede) ? true : false,
                         isSent = false,
                         isDirty = false,
                         AdresseChanged = false,
                         Result = " ? "
                     }

                         ).OrderBy(n => n.Nachname).OrderBy(n => n.Firmenname);

            SelectedAdressen = new ObservableCollection<Models.SelectedAdressesModel>(x);
            ResetSelectedAdresses();

            if (ChangeVisibility != null)
            {
                ChangeVisibility("8,9,10", true);
                ChangeVisibility("11,12,13,14", false);
            }

            selectionType = SelectionType.Adressetikett;
            SelectedAction = "Adressetiketten drucken";
            SetisExecutEnabled();
        }

        public void ExecuteAction()
        {
            switch (selectionType)
            {
                case SelectionType.Email:
                    {
                        var task1 = System.Threading.Tasks.Task.Factory.StartNew(() =>
                        {

                            ThisDispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (new System.Action(() =>
                            {

                                SendSelectedMails();

                            })));

                        });





                        break;
                    }

                case SelectionType.Serienbrief:
                    {
                        InsertIntoTable();
                        break;
                    }

                case SelectionType.Adressetikett:
                    {
                        ShowSelectAdresses();
                        break;
                    }
                case SelectionType.None:
                    break;
                default:
                    break;
            }

        }


        public void SelectContacts()
        {

            var x = (from t in db.vw_UnionSelectedAdressen
                     where t.TelefonTyp == 16
                     group t by new
                     {
                         t.Nachname,
                         t.Vorname,
                         t.Anrede,
                         t.Titel,
                         t.Funktion,
                         t.Vorname2,
                         t.Namenszusatz,
                         t.Ort,
                         t.Straße,
                         t.PLZ,
                         t.Land,
                         t.Firmenname,
                         t.Firmenkurzname,
                         t.Vorwahl,
                         t.Rufnummer,
                         t.Mailadresse
                     } into g

                     select new Models.SelectedAdressesModel
                         {
                             Nachname = g.Key.Nachname,
                             Vorname = g.Key.Vorname,
                             Vorname2 = g.Key.Vorname2,
                             Anrede = g.Key.Anrede,
                             Titel = g.Key.Titel,
                             Funktion = g.Key.Funktion,
                             Ort = g.Key.Ort,
                             Straße = g.Key.Straße,
                             PLZ = g.Key.PLZ,
                             Land = g.Key.Land,
                             Firmenkurzname = g.Key.Firmenkurzname,
                             Firmenname = g.Key.Firmenname,
                             Vorwahl = g.Key.Vorwahl,
                             Rufnummer = g.Key.Rufnummer,
                             Mailadresse = g.Key.Mailadresse,
                             UnSelect = false
                         }

                        ).OrderBy(n => n.Nachname).OrderBy(n => n.Firmenname);

            SelectedAdressen = new ObservableCollection<Models.SelectedAdressesModel>(x);


        }

        public void doChangeVisibilityFalse()
        {
            if (ChangeVisibility != null)
            {
                ChangeVisibility("1,4,5", true);
            }
        }

        public void doChangeVisibilityTrue()
        {
            if (ChangeVisibility != null)
            {
                ChangeVisibility("1,4,5", false);
            }
        }


        private void InsertIntoTable()
        {
            using (var context = new DAL.SteinbachEntities())
            {
                int rowsAdded = 0;


                context.ExecuteStoreCommand("delete from SI_SerienbriefeEmpfaenger");


              //  string AnredeWennLeer = CommonTools.Tools.ConfigEntry<string>.GetConfigEntry("AnredeWennLeer", "Herrn/Frau", "Anrede in Sertienbrief oder Adressetikett");

                foreach (var item in SelectedAdressen.Where(n => n.UnSelect != true))
                {
                    try
                    {
                       // string Anrede = GetAnredeWennLeer(AnredeWennLeer, item.Anrede);
                        string items = GetStringWithHochKommata(item.Nachname, item.Vorname, item.Vorname2, item.Anrede,item.Anrede1, item.Namenszusatz, item.Funktion, item.Bezeichnung, item.Mailadresse, item.PLZ, item.Straße, item.Land, item.Ort,
                                               item.Vorwahl, item.Rufnummer, item.Firmenname, item.Firmenkurzname, item.Titel);

                      

                       // Console.WriteLine(items);
                       // items = "insert into" + " " + "SI_SerienbriefeEmpfaenger (Nachname,Vorname,Vorname2,Anrede,Namenszusatz,Funktion,Bezeichnung,Mailadresse,PLZ,Straße,Land,Ort,Vorwahl,Rufnummer,Firmenname,Firmenkurzname,Titel)" +" Values(" + items + ")";
                                               // Einfügen des Leerzeichen war notwendig da auf Kundenserver 

                        items = String.Format("insert into SI_SerienbriefeEmpfaenger (Nachname,Vorname,Vorname2,Anrede,Anrede1,Namenszusatz,Funktion,Bezeichnung,Mailadresse,PLZ,Straße,Land,Ort,Vorwahl,Rufnummer,Firmenname,Firmenkurzname,Titel) Values({0})", items);
                        Console.WriteLine(items);

                        //context.ExecuteStoreCommand("insert into SI_SerienbriefeEmpfaenger (Nachname,Vorname,Vorname2,Anrede,Namenszusatz,Funktion,Bezeichnung,Mailadresse,PLZ,Straße,Land,Ort,Vorwahl,Rufnummer,Firmenname,Firmenkurzname,Titel) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16})",
                        //                       item.Nachname, item.Vorname, item.Vorname2, Anrede, item.Namenszusatz, item.Funktion, item.Bezeichnung, item.Mailadresse, item.PLZ, item.Straße, item.Land, item.Ort,
                        //                       item.Vorwahl, item.Rufnummer, item.Firmenname, item.Firmenkurzname, item.Titel);

                        context.ExecuteStoreCommand(items);

                        ++rowsAdded;


                    }
                    catch (Exception ex)
                    {

                        CommonTools.Tools.ErrorMethods.HandleStandardError(ex, item.Firmenname + ", " + item.Nachname + ", " + item.Vorname);
                    }




                }


                CommonTools.Tools.UserMessage.NotifyUser(string.Format("Es wurden {0} von {1} Datensätzen in die Serienbrieftabelle geschrieben.", rowsAdded, SelectedAdressen.Count()));



            }

        }

        private string GetStringWithHochKommata(params string[] args)
        {
            string res = "";
            foreach (var item in args)
            {
                res += String.Format("'{0}', ", item);

            }
           
            int pos = res.LastIndexOf(", ");
            res =  res.Substring(0,pos);
           
            return res;
        }

        //private static string GetAnredeWennLeer(string AnredeWennLeer, Models.SelectedAdressesModel item)
        //{


        //    string Anrede = string.Empty;
        //    if (string.IsNullOrEmpty(item.Anrede) || item.Anrede.Trim() == "")
        //    {
        //        Anrede = AnredeWennLeer;
        //    }
        //    else
        //    {
        //        if (item.Anrede.Contains("Herr"))
        //        {
        //            Anrede = item.Anrede.Replace("Herr", "Herrn");
        //        }
        //        else
        //        {
        //            Anrede = item.Anrede;
        //        }
        //    }
        //    return Anrede;
        //}

        private static string GetAnredeWennLeer(string AnredeWennLeer, string item)
        {


            string Anrede = string.Empty;
            if (string.IsNullOrEmpty(item) || item.Trim() == "")
            {
                Anrede = AnredeWennLeer;
            }
            else
            {
                if (item.Contains("Herr"))
                {
                    Anrede = item.Replace("Herr", "Herrn");
                }
                else
                {
                    Anrede = item;
                }
            }
            return Anrede;
        }

        public void SendSelectedMails()
        {


            string from = CommonTools.Tools.ConfigEntry<string>.GetConfigEntry("SMTPSenderAdress", "me@asWorks.de");
            string server = Session.SMTPServer;
            string user = Session.UsernameSMTP;
            string pw = Session.PasswordSMTP;


            foreach (var item in SelectedAdressen.Where(n => n.UnSelect != true))
            {
                try
                {


                    if (rbExchange == true)      
                    {



                        if (AttachedFiles.Count() > 0)
                        {
                            SteinbachMail.TestMail.TestSendMail(server, user, pw, from, item.Mailadresse, MailSubject, MailBody, AttachedFiles.ToArray<string>(), SteinbachMail.TestMail.MessageType.Text);
                        }
                        else
                        {

                            SteinbachMail.TestMail.TestSendMail(server, user, pw, from, item.Mailadresse, MailSubject, MailBody, SteinbachMail.TestMail.MessageType.Text);
                            //SteinbachMail.TestMail.TestSendMail("mail.hostedoffice.ag", "dev@asWorks.de", "MnAbQHJ2010", "me@asWorks.de", "Arpad.Stoever@asWorks.de", MailSubject, MailBody);
                        }

                    }
                    else if (rbOutlook == true)
                    {
                        if (AttachedFiles.Count() > 0)
                        {
                            asOutlookMail.OutlookMail.sendmail(item.Mailadresse, MailSubject, MailBody, "", AttachedFiles.ToArray<string>(), asOutlookMail.MessageType.text);
                        }
                        else
                        {
                            asOutlookMail.OutlookMail.sendmail(item.Mailadresse, MailSubject, MailBody);
                        }
                    }


                    ThisDispatcher.BeginInvoke(new Action<Models.SelectedAdressesModel, bool>(SetisSent), item, true);
                    ThisDispatcher.BeginInvoke(new Action<Models.SelectedAdressesModel, string>(SetResult), item, "OK");



                }
                catch (Exception ex)
                {

                    // CommonTools.Tools.ErrorMethods.HandleStandardError(ex, item.Firmenname + ", " + item.Nachname + ", " + item.Vorname);
                    ThisDispatcher.BeginInvoke(new Action<Models.SelectedAdressesModel, string>(SetResult), item, ex.Message);
                    //item.Result = ex.Message;
                }


            }
        }

        private void SetisSent(Models.SelectedAdressesModel SelAdress, bool state)
        {
            SelAdress.isSent = state;
        }

        private void SetResult(Models.SelectedAdressesModel SelAdress, string message)
        {
            SelAdress.Result = message;
        }


        public void ShowSelectAdresses()
        {
            IEnumerable<Models.SelectedAdressesModel> Labels = SelectedAdressen.Where(n => n.UnSelect != true);

            int c = 10;
            foreach (var item in Labels)
            {
                item.id = c++;
            }


            var w = new ReportViewer.AdressEtiketten((IEnumerable<Models.SelectedAdressesModel>)Labels);
            w.ShowDialog();

        }

        public void AddAttachment()
        {
            var fo = new System.Windows.Forms.OpenFileDialog();
            fo.InitialDirectory = CommonTools.Tools.ConfigEntry<string>.GetConfigEntry("InitialDirectoryAttachments", @"C:\Users\Noone\Documents");
            fo.ShowDialog();
            if (fo.FileName != string.Empty)
            {
                AttachedFiles.Add(fo.FileName);
                // CommonTools.Tools.UserMessage.NotifyUser(fo.FileName);
            }
        }

        public void AccountInfo()
        {
            CommonTools.Tools.UserMessage.NotifyUser(asOutlookMail.OutlookMail.DisplayAccountInformation());
        }

        private void ResetSelectedAdresses()
        {
            foreach (var item in SelectedAdressen)
            {
                item.isDirty = false;
                item.AdresseChanged = false;
            }
        }

        #endregion

        #region "Events"

        public void ListBox_MouseDoubleClick(MouseButtonEventArgs e)
        {
            var lb = (System.Windows.Controls.ListBox)e.Source;
            string x = (string)lb.SelectedItem;
            AttachedFiles.Remove(x);
        }

        #endregion




        public bool SaveChanges()
        {
            Save();
            // isDirty = false;
            return true;
        }

        public bool RejectChanges()
        {
            // isDirty = false;
            return true;
        }

        public event System.Action DoRejectChanges;
    }
}
