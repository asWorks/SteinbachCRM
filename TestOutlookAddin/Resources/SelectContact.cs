using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using SIO = System.IO;
using System.Diagnostics;


namespace TestOutlookAddin.Resources
{
    public partial class SelectContact : Form
    {

        string server = @"y:\CRMAttachments\";

        Outlook.MailItem oMailItem;
        public SelectContact()
        {
            InitializeComponent();
        }

        public SelectContact(Outlook.MailItem mi)
        {
            InitializeComponent();
            oMailItem = mi;

        }



        private void SelectContact_Load(object sender, EventArgs e)
        {
            txtSubject.Text = oMailItem.Subject;
            txtDate.Text = oMailItem.ReceivedTime.ToString();
            txtRecipient.Text = oMailItem.ReceivedByName;
            txtSender.Text = oMailItem.SenderName;
            txtSenderEmail.Text = oMailItem.SenderEmailAddress;
            txtRecipientEmail.Text = oMailItem.To;
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            richTextBox1.Rtf = System.Text.Encoding.Default.GetString(oMailItem.RTFBody);
        }


        private void cboFirma_KeyDown(object sender, KeyEventArgs e)
        {
            using (var db = new SteinbachEntities())
            {
                if (e.KeyCode == Keys.Return)
                {
                    cboFirma.DataSource = from f in db.firmas
                                          where f.name.Contains(cboFirma.Text) && f.IstKunde == 1
                                          select f;
                    cboFirma.DisplayMember = "name";
                    cboFirma.ValueMember = "id";
                }
            }
        }

        //private SteinbachService.ActiveUser GetActiveUser()
        //{
        //    try
        //    {
        //        var client = new SteinbachService.SteinbacherviceClient();
        //        var au = client.GetActiveUser();
        //        return au;
        //    }
        //    catch (Exception)
        //    {

        //        return null;
        //    }



        //}

        private void cmdProcess_Click(object sender, EventArgs e)
        {



            try
            {
                if (cboFirma.SelectedValue != null)
                {
                    using (var db = new SteinbachEntities())
                    {
                        CRMEmail mail = new CRMEmail();
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
                                    mail.HTMLText = oMailItem.HTMLBody;
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
                        mail.id_Firma = (int)cboFirma.SelectedValue;


                        if (cboKontakt.SelectedValue != null)
                        {
                            mail.id_Kontakt = (int)cboKontakt.SelectedValue;
                        }

                        db.AddToCRMEmails(mail);
                        db.SaveChanges();
                        SaveAttachment(oMailItem, mail.id);

                        this.Close();


                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Email konnte nicht zugeordnet werden." + "-" + ex.Message);
            }

        }

        private void cboFirma_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                using (var db = new SteinbachEntities())
                {

                    int v = (int)cboFirma.SelectedValue;
                    cboKontakt.DataSource = from k in db.Firmen_Personen
                                            where k.id_Firma == v
                                            select k;
                    cboKontakt.DisplayMember = "Nachname";
                    cboKontakt.ValueMember = "id";
                    cboKontakt.SelectedItem = null;
                    cboKontakt.Text = "";

                }
            }
            catch (Exception)
            {


            }
        }

        private void SaveAttachment(Outlook.MailItem currentMailItem, int id)
        {
           
            try
            {
                if (currentMailItem != null)
                {

                    if (currentMailItem.Attachments.Count > 0)
                    {
                        for (int i = 1; i <= currentMailItem.Attachments.Count; i++)
                        {

                            SIO.DirectoryInfo f_Server = new SIO.DirectoryInfo(server);
                            Console.WriteLine(f_Server.ToString());
                            SIO.DirectoryInfo f_Firma = new SIO.DirectoryInfo(server + cboFirma.SelectedValue);
                            if (!SIO.Directory.Exists(server + cboFirma.SelectedValue))
                            {

                                SIO.Directory.CreateDirectory(server + cboFirma.SelectedValue);
                            }
                            if (!SIO.Directory.Exists(server + cboFirma.SelectedValue + @"\" + id.ToString()))
                            {
                                SIO.Directory.CreateDirectory(server + cboFirma.SelectedValue + @"\" + id.ToString());
                            }

                            string fName = String.Format(@"{0}{1}\{2}\{3}", server, cboFirma.SelectedValue, id, currentMailItem.Attachments[i].FileName);
                            currentMailItem.Attachments[i].SaveAsFile(fName);


                            if (currentMailItem.BodyFormat == Outlook.OlBodyFormat.olFormatHTML)
                            {
                                try
                                {
                                    fName = String.Format(@"{0}{1}\{2}\{3}", server, cboFirma.SelectedValue, id, "mail.html");
                                    System.IO.StreamWriter fs = new System.IO.StreamWriter(fName, false);
                                    fs.WriteLine(currentMailItem.HTMLBody);
                                    fs.Close();
                                    fs = null;
 
                                    
                                }

                                catch (Exception ex)
                                {

                                    MessageBox.Show(ex.Message);
                                    //throw;
                                }
                            }

                           ProcessStartInfo psi = new ProcessStartInfo(fName);
                            psi.UseShellExecute = true;
                            Process.Start(psi);



                            //DllImport("shell32.dll", EntryPoint = "ShellExecute")]
//public static extern long ShellExecute(int hwnd, string cmd, string file, string param1, string param2, int swmode);

///// swmode 0=sw_hide, 5=SW Show
///// Weitere siehe Windows API
//public static string TestRun()
//{
//    // email öffnen
//    ShellExecute (0, "mailto", "tester@testdomain.de", "", "", 5)

//    // Notepad starten
//    ShellExecute (0, "open", "notepad", "", "", 5)

//    // Bild oder Datei offnen mit Pfad 
//    ShellExecute (0, "open", "C:\WINDOWS\Zapotec.bmp", "", "", 5)
//}






                        }
                    }
                }
            }


            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += @"\n" + ex.InnerException;
                }
                MessageBox.Show(String.Format(@"Dateianhänge konnten nicht gespeichert werden. \n{0}", message));
            }



        }
    }
}
