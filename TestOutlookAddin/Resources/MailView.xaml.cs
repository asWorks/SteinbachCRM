using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Office.Interop.Outlook;
using System.IO;
using System.Diagnostics;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using SIO = System.IO;
using TestOutlookAddin.Tools;
using System.Windows.Media.Animation;
using System.Windows.Threading;


namespace TestOutlookAddin.Resources
{
    /// <summary>
    /// Interaction logic for MailView.xaml
    /// </summary>
    public partial class MailView : Window
    {
        //   string server = @"y:\CRMAttachments\";
        string server = Tools.HelperTools.GetConfigEntry(@"EmailAttachmentPath");
        MailItem oMailItem;
        Guid guid;
        string html;
        Dictionary<string, Tools.AttachmentInfo> Attachments = new Dictionary<string, Tools.AttachmentInfo>();
        //   Dictionary<string, string> Attachments = new Dictionary<string, string>();
        //   Dictionary<string, int> AType = new Dictionary<string, int>();
        int AttachmentCount = 0;

        public MailView(MailItem mi)
        {
            InitializeComponent();
            oMailItem = mi;

        }
        public MailView()
        {
            InitializeComponent();

        }

        #region "Functions"
        private void LoadStati(string filter = "")
        {

            using (var db = new SteinbachEntities())
            {


                try
                {
                    if (filter == string.Empty)
                    {

                        var query = from f in db.AuswahlEintraeges
                                    where f.Gruppe == "EmailStatus"
                                    orderby f.Eintrag
                                    select f;

                        this.lfcStatus.CBoxItemssource = query;

                    }
                    else
                    {
                        var query = db.AuswahlEintraeges.Where(f => f.Eintrag.Contains(filter) && f.Gruppe == "EmailStatus").OrderBy(f => f.Eintrag);
                        //AlleFirmen = new FirmenCollection(query, db);
                        this.lfcStatus.CBoxItemssource = query;

                    }
                }
                catch (System.Exception ex)
                {

                    MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));
                }

            }



        }
        private void LoadAktionen(string filter = "")
        {

            using (var db = new SteinbachEntities())
            {


                try
                {
                    if (filter == string.Empty)
                    {

                        var query = from f in db.AuswahlEintraeges
                                    where f.Gruppe == "EmailAktion"
                                    orderby f.Eintrag
                                    select f;

                        this.lfcAktion.CBoxItemssource = query;

                    }
                    else
                    {
                        var query = db.AuswahlEintraeges.Where(f => f.Eintrag.Contains(filter) && f.Gruppe == "EmailAktion").OrderBy(f => f.Eintrag);
                        //AlleFirmen = new FirmenCollection(query, db);
                        this.lfcKontakt.CBoxItemssource = query;

                    }
                }
                catch (System.Exception ex)
                {

                    MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));
                }

            }



        }

        private void LoadKontakte(int id_Firma, string filter = "")
        {

            using (var db = new SteinbachEntities())
            {


                try
                {
                    if (filter == string.Empty)
                    {

                        var query = from f in db.Firmen_Personen
                                    where f.id_Firma == id_Firma
                                    orderby f.Nachname
                                    select f;

                        this.lfcKontakt.CBoxItemssource = query;
                        // AlleFirmen = new FirmenCollection(query, db);
                    }
                    else
                    {
                        var query = db.Firmen_Personen.Where(f => f.Nachname.Contains(filter) && f.id_Firma == id_Firma).OrderBy(f => f.Nachname);
                        //AlleFirmen = new FirmenCollection(query, db);
                        this.lfcKontakt.CBoxItemssource = query;

                    }
                }
                catch (System.Exception ex)
                {

                    MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));
                }

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

                        var query = from f in db.firmas
                                    where f.IstKunde == 1
                                    orderby f.name
                                    select f;

                        this.lfcFirma.CBoxItemssource = query;

                    }
                    else
                    {
                        var query = db.firmas.Where(f => f.name.Contains(filter) && f.IstKunde == 1).OrderBy(f => f.name);

                        this.lfcFirma.CBoxItemssource = query;

                    }
                }
                catch (System.Exception ex)
                {

                    MessageBox.Show(CommonTools.Tools.ErrorMethods.GetExceptionMessageInfo(ex));
                }

            }



        }

        #endregion


        #region "Events"

        private void lfcFirma_OnFcb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            firma f = (firma)lfcFirma.CBoxSelectedItem;
            if (f != null)
            {
                LoadKontakte(f.id);
            }

        }

        private void lfcKontakt_onfcbChanged(object sender, CommonTools.UserControls.FilteredComboBoxChangedEventArgs e)
        {

        }

        private void lfcFirma_onfcbChanged(object sender, CommonTools.UserControls.FilteredComboBoxChangedEventArgs e)
        {
            LoadFirmen(e.filter);
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                LoadFirmen("");
                LoadAktionen("");
                LoadStati("");

                switch (oMailItem.BodyFormat)
                {
                    case OlBodyFormat.olFormatHTML:
                        {
                            SwitchDisplay("html");
                            // txtText.Text = oMailItem.Body;
                            rtfText.AppendText(oMailItem.Body);

                            if (oMailItem.Attachments.Count > 0)
                            {

                                SaveAttachment(oMailItem);
                                html = GetHtml();
                                string filename = Environment.GetEnvironmentVariable("LOCALAPPDATA");
                                filename += @"\temp.html";

                                var utf = Encoding.UTF8;
                                FileStream fsOpen = File.Open(filename, FileMode.Create);

                                StreamWriter sr = new StreamWriter(fsOpen, utf);
                                sr.Write(html);
                                sr.Flush();
                                sr.Close();
                                sr.Dispose();
                                sr = null;
                                fsOpen.Close();
                                fsOpen.Dispose();


                                FileStream fs = File.Open(filename, FileMode.Open);

                                Browser.NavigateToStream(fs);


                                // Browser.NavigateToString(html);
                            }
                            else
                            {
                                html = oMailItem.HTMLBody;
                                Browser.NavigateToString(html);
                            }
                            break;
                        }
                    case OlBodyFormat.olFormatPlain:
                        {
                            SwitchDisplay("plain");
                            //txtText.Text = oMailItem.Body;
                            // System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();




                            Browser.NavigateToString(oMailItem.Body);
                            rtfText.AppendText(oMailItem.Body);
                            //rtfText.AppendText( System.Text.Encoding.Default.GetString(oMailItem.RTFBody));
                            //  rtfText.AppendText(oMailItem.RTFBody);
                            if (oMailItem.Attachments.Count > 0)
                            {
                                SaveAttachment(oMailItem);
                            }
                            break;
                        }
                    case OlBodyFormat.olFormatRichText:
                        {
                            SwitchDisplay("rtf");
                            rtfText.AppendText(System.Text.Encoding.Default.GetString(oMailItem.RTFBody));

                            if (oMailItem.Attachments.Count > 0)
                            {
                                SaveAttachment(oMailItem);
                            }
                            break;


                        }
                    case OlBodyFormat.olFormatUnspecified:
                        break;
                    default:
                        break;
                }

                txtAbsender.Text = oMailItem.Sender.Name;
                txtAbsenderEmailAdresse.Text = oMailItem.SenderEmailAddress;
                txtEmpfaenger.Text = oMailItem.ReceivedByName;
                txtEmpfaengerliste.Text = oMailItem.To;
                txtDateReceived.Text = oMailItem.ReceivedTime.ToString();

                txtBetreff.Text = oMailItem.Subject;
                // Browser.NavigateToString(oMailItem.HTMLBody); 
            }
            catch (System.Exception ex)
            {

                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += @"\n" + ex.InnerException;
                }
                MessageBox.Show(String.Format(@"Fehler beim Laden der Email. \n{0}", message));
            }





        }

        string GetHtml()
        {
            try
            {
                string buf = oMailItem.HTMLBody;
                foreach (var item in Attachments)
                {
                    int index = 1;
                    int s = 0;
                    int e = 0;
                    int x = buf.IndexOf("\"");
                    Console.WriteLine(x.ToString());
                    do
                    {
                        index = buf.IndexOf(item.Key, index);
                        if (index > 0)
                        {
                            s = index;
                            e = index + item.Key.Length;
                            while (buf.Substring(--s, 1) != "\"")
                            {

                            }
                            while (buf.Substring(++e, 1) != "\"")
                            {

                            }
                            try
                            {

                                buf = buf.Remove(s, e - s);

                                buf = buf.Insert(s, "\"" + item.Value.Path);

                                // index = index + item.Value.Length;
                                index = index + item.Value.Path.Length;
                                item.Value.isEmbedded = true;

                            }
                            catch (System.Exception ex)
                            {

                                MessageBox.Show(ex.Message);
                            }

                        }
                    } while (index != -1);
                }

                return buf;
            }
            catch (System.Exception ex)
            {
                return oMailItem.HTMLBody;

            }




        }



        string GetAttachmentDirectory()
        {
            //if (lfcFirma.CBoxSelectedItem != null)
            //{
            //    string buf = server + lfcFirma.CBoxSelectedValue.ToString();
            //    if (!SIO.Directory.Exists(buf))
            //    {

            //        SIO.Directory.CreateDirectory(buf);
            //    }

            string buf = server + guid;

            if (!SIO.Directory.Exists(buf))
            {

                SIO.Directory.CreateDirectory(buf);
            }

            return buf;

            //}


        }


        private void SaveAttachment(Outlook.MailItem currentMailItem)
        {
            Attachments.Clear();
            //   AType.Clear();
            try
            {
                string fname = string.Empty;
                string Dir = string.Empty;
                if (currentMailItem != null)
                {

                    if (currentMailItem.Attachments.Count > 0)
                    {
                        guid = Guid.NewGuid();
                        AttachmentCount = currentMailItem.Attachments.Count;

                        for (int i = 1; i <= currentMailItem.Attachments.Count; i++)
                        {
                            Dir = GetAttachmentDirectory();
                            if (currentMailItem.Attachments[i].Type == OlAttachmentType.olEmbeddeditem)
                            {
                                fname = String.Format(@"{0}\{1}\{2}", Dir, "embedded", currentMailItem.Attachments[i].FileName);
                            }
                            else
                            {
                                fname = String.Format(@"{0}\{1}", Dir, currentMailItem.Attachments[i].FileName);
                            }


                            currentMailItem.Attachments[i].SaveAsFile(fname);
                            var ai = new AttachmentInfo();
                            ai.Path = fname;
                            ai.AttachmentType = (int)currentMailItem.Attachments[i].Type;
                            Attachments.Add(currentMailItem.Attachments[i].FileName, ai);


                            //Attachments.Add(currentMailItem.Attachments[i].FileName, fname);
                            //AType.Add(currentMailItem.Attachments[i].FileName, (int)currentMailItem.Attachments[i].Type);

                        }
                    }
                }
            }


            catch (System.Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += @"\n" + ex.InnerException;
                }
                MessageBox.Show(String.Format(@"Dateianhänge konnten nicht gespeichert werden. \n{0}", message));
            }



        }


        void SwitchDisplay(string mode)
        {
            switch (mode)
            {
                case "html":
                    {
                        //this.txtText.Visibility = System.Windows.Visibility.Collapsed;
                        this.rtfText.Visibility = System.Windows.Visibility.Collapsed;
                        this.Browser.Visibility = System.Windows.Visibility.Visible;
                        break;
                    }
                case "rtf":
                    {
                        //this.txtText.Visibility = System.Windows.Visibility.Collapsed;
                        this.rtfText.Visibility = System.Windows.Visibility.Visible;
                        this.Browser.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    }
                case "plain":
                    {
                        //this.txtText.Visibility = System.Windows.Visibility.Visible;
                        this.rtfText.Visibility = System.Windows.Visibility.Visible;
                        this.Browser.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    }

                default:
                    break;
            }

        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            SwitchDisplay("plain");
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            SwitchDisplay("html");
        }

        private void Process_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.lfcFirma.CBoxSelectedValue != null)
                {
                    using (var db = new SteinbachEntities())
                    {

                        CRMEmail mail = new CRMEmail();
                        mail.created = DateTime.Now;



                        foreach (var item in Attachments)
                        {
                            EmailAttachments Attach = new EmailAttachments();
                            Attach.CRMEmails = mail;
                            Attach.Filename = item.Key;
                            Attach.Path = item.Value.Path;
                            Attach.created = DateTime.Now;
                            Attach.AttachmentType = item.Value.AttachmentType;
                            Attach.isEmbedded = item.Value.isEmbedded == true ? (short)1 : (short)0;

                            db.AddToEmailAttachments(Attach);
                        }


                        mail.EntryID = oMailItem.EntryID;
                        mail.ConversationID = oMailItem.ConversationID;
                        mail.Betreff = oMailItem.Subject;
                        mail.Absender = oMailItem.SenderName;
                        var session = oMailItem.Session;
                        mail.Typ = oMailItem.SenderEmailType;
                        switch (oMailItem.BodyFormat)
                        {
                            case Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML:
                                {
                                    mail.BodyFormat = "HTML";
                                    if (html != "")
                                    {
                                        mail.HTMLText = html;
                                    }
                                    else
                                    {
                                        mail.HTMLText = oMailItem.HTMLBody;
                                    }

                                    mail.Text = oMailItem.Body;
                                    break;

                                }
                            case Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatPlain:
                                {
                                    mail.BodyFormat = "PLAIN";
                                    mail.Text = oMailItem.Body;
                                    break;
                                }

                            case Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatRichText:
                                {
                                    mail.BodyFormat = "RichText";
                                    mail.RTF_Text = oMailItem.RTFBody;
                                    mail.Text = oMailItem.Body;
                                    break;

                                }

                            case Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatUnspecified:
                                {
                                    mail.BodyFormat = "Unspecified";
                                    mail.Text = oMailItem.Body;
                                    break;
                                }

                            default:
                                break;
                        }



                        mail.OutlookUsername = session.CurrentUser.Name;
                        //var user = GetActiveUser();
                        //if (user != null)
                        //{
                        //    if (user.isActive)
                        //    {
                        //        mail.id_PersonSI = user.id;
                        //    }

                        //}
                        mail.AbsenderMailAdresse = oMailItem.SenderEmailAddress;
                        mail.Datum = oMailItem.ReceivedTime;


                        mail.Empfaenger = oMailItem.ReceivedByName;
                        mail.Empfaengerliste = oMailItem.To;
                        mail.id_Firma = (int)lfcFirma.CBoxSelectedValue;

                        if (lfcStatus.CBoxSelectedValue != null)
                        {
                            mail.Status = (int)lfcStatus.CBoxSelectedValue;
                        }

                        if (lfcAktion.CBoxSelectedValue != null)
                        {
                            mail.Aktion = (int)lfcAktion.CBoxSelectedValue;
                        }

                        if (lfcKontakt.CBoxSelectedValue != null)
                        {
                            mail.id_Kontakt = (int)lfcKontakt.CBoxSelectedValue;
                        }




                        db.AddToCRMEmails(mail);
                        db.SaveChanges();
                        // SaveAttachment(oMailItem, mail.id);

                        // this.Close();

                        PlayStoryboard();
                    }
                }
            }
            catch (System.Exception ex)
            {

                MessageBox.Show("Email konnte nicht zugeordnet werden." + "-" + ex.Message);
            }

        }


        private void PlayStoryboard()
        {
            Storyboard sbdShowResult
       = (Storyboard)FindResource("sbdShowResult");

            sbdShowResult.Begin(this);

            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 2000);
            timer.Start();
           

        }

        void timer_Tick(object sender, EventArgs e)
        {
               this.Close();
        }



        //private SteinbachService.ActiveUser GetActiveUser()
        //{
        //    try
        //    {
        //        var client = new SteinbachService.SteinbacherviceClient();
        //        var au = client.GetActiveUser();
        //        return au;
        //    }
        //    catch (System.Exception)
        //    {

        //        return null;
        //    }
        //}

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int a = 0;
                int b = 1;
                int c = b / a;
                Console.WriteLine(c.ToString());
            }
            catch (System.Exception ex)
            {

                CommonTools.Tools.ErrorMethods.ShowErrorMessageToUser(ex, "");
            }
        }
    }
}
