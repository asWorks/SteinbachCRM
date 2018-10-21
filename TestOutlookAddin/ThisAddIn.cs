using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;
using System.Drawing;
using TestOutlookAddin.Resources;

namespace TestOutlookAddin
{
    public partial class ThisAddIn
    {
        private Office.CommandBarButton ctxMenuTest;
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
           // MessageBox.Show("AddIn started");

              // Add ContextMenuDisplayEventHandler
            this.Application.ItemContextMenuDisplay += new Microsoft.Office.Interop.Outlook.ApplicationEvents_11_ItemContextMenuDisplayEventHandler(Application_ItemContextMenuDisplay);

        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
       

            #region Adding Context Menu 
        private void Application_ItemContextMenuDisplay(Office.CommandBar cb, Outlook.Selection selection)
        {
            try
            {
                // add the context menu 'Test Forward'
                // this will appear as the first item in context menu list when you rignt click on any message 
                ctxMenuTest = (Office.CommandBarButton)cb.Controls.Add(Office.MsoControlType.msoControlButton,Type.Missing, "Test Forward", 1, Type.Missing);
                ctxMenuTest.Caption = "An Steinbach CRM übergeben";
                ctxMenuTest.Style = Microsoft.Office.Core.MsoButtonStyle.msoButtonIconAndCaption;
                ctxMenuTest.Visible = true;

                // assign image for the context menu
               // ctxMenuTest.Picture = GetImage("Options");
                
                // add click event handler
                ctxMenuTest.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(ctxMenuTest_Click);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ItemContextMenuDisplay - " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Context Menu Click Event 

        private void ctxMenuTest_Click(Office.CommandBarButton cbb, ref bool bol)
        {
            ctxMenuTest.Click -= new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(ctxMenuTest_Click);
            ForwardSelectedMessage();
        }

        private void ForwardSelectedMessage()
        {
            try
            {
                // create a new mail message 
                Outlook.MailItem ForwardingMessage = (Outlook.MailItem)this.Application.CreateItem(Outlook.OlItemType.olMailItem);
                // In case you want to implement the custom outlook form you have use following commented 2 lines of code
                //Outlook.MAPIFolder templateFolder = this.Application.Session.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderInbox);
                //ForwardingMessage = (Outlook.MailItem)templateFolder.Items.Add("IPM.Note.OutlookCustomForm1");

                // get the currently selected message (the message on which the user right-clicked)
                Outlook.MailItem SelectedMessage;
                SelectedMessage = (Outlook.MailItem)this.Application.ActiveExplorer().Selection[1];
                var cf = this.Application.ActiveExplorer().CurrentFolder;
                Console.WriteLine(cf.Name);
                var n=  this.Application.ActiveExplorer().CurrentFolder.Name;
                Console.WriteLine(n);
                //using (var f = new SelectContact(SelectedMessage))
                //{
                //    f.ShowDialog();

                //}

                var x = new MailView(SelectedMessage);
                x.ShowDialog();
                

                return;


                if (SelectedMessage != null)
                {
                    if (ForwardingMessage != null)
                    {
                        //Subject 
                        ForwardingMessage.Subject = "FW: " + SelectedMessage.Subject;

                        #region Attahment 
                        // Get the count of Attachments
                        int attachCount = SelectedMessage.Attachments.Count;
                        if (attachCount > 0)
                        {
                            // loop through each attachment 
                            for (int idx = 1; idx <= attachCount; idx++)
                            {
                                string sysPath = System.IO.Path.GetTempPath();

                                if (!System.IO.Directory.Exists(sysPath + "~test"))
                                {
                                    System.IO.Directory.CreateDirectory(sysPath + "~test");
                                }

                                // get attached file and save in temp folder
                                string strSourceFileName = sysPath + "~test\\" + SelectedMessage.Attachments[idx].FileName;
                                SelectedMessage.Attachments[idx].SaveAsFile(strSourceFileName);
                                string strDisplayName = "Attachment";
                                int intPosition = 1;
                                int intAttachType = (int)Outlook.OlAttachmentType.olEmbeddeditem;

                                // Add the current attachment
                                ForwardingMessage.Attachments.Add(strSourceFileName, intAttachType, intPosition, strDisplayName);
                            }
                        }
                        #endregion

                        #region Body 

                        string strHeader = "<p><br><br>" + "-----Original Message-----" + "<br>";
                        strHeader += "From: " + SelectedMessage.SenderName + "<br>";
                        strHeader += "Sent: " + SelectedMessage.SentOn.ToString() + "<br>";
                        strHeader += "To: " + SelectedMessage.To + "<br>";
                        strHeader += "Subject: " + SelectedMessage.Subject + "<br><br>";

                        ForwardingMessage.HTMLBody = strHeader + GetTrimmedBodyText(SelectedMessage.HTMLBody);

                        #endregion

                        ForwardingMessage.Display(false);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().StartsWith("unable to cast com object"))
                {
                    MessageBox.Show("This is not a valid message and it can not be sent.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ForwardSelectedMessage - " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        #region Handling inline images in the message body 
        /* by defaule the inline images in the selected message will be added as attachments to the new message
           the reason is in htmlbody the image src will be as follows:
           src = "cid:image001.jpg@01C82869373A65B0"
           to view inline images (and avoid image attachments) it should changed as follows:
           src = "image001.jpg"
           replace "cid" with "" (empty string) and 
           replace "image001.jpg@01C82869373A65B0" with "image001.jpg" (only file name)
        */
        private string GetTrimmedBodyText(string strHTMLBody)
        {

            string strTrimmedHTMLBody = strHTMLBody;

            int start = 0;
            start = strHTMLBody.IndexOf("<img", start);

            /* the following loop will check for each src attribute value for each image and 
               it will replace them only with image file name (by removing cid: and @ part)
            */
            while (start > 0)
            {
                int count = strHTMLBody.IndexOf('>', start);

                string str = strHTMLBody.Substring(start);
                string strActualImgTag = str.Substring(0, str.IndexOf(">") + 1);
                string strTrimImgTag = strActualImgTag.Replace("cid:", "");

                int intAtPosition = 0;
                intAtPosition = strTrimImgTag.IndexOf("@");
                while (intAtPosition > 0)
                {
                    string strAt = strTrimImgTag.Substring(strTrimImgTag.IndexOf("@"), 18);
                    strTrimImgTag = strTrimImgTag.Replace(strAt, "");

                    intAtPosition = strTrimImgTag.IndexOf("@");
                }

                strTrimmedHTMLBody = strTrimmedHTMLBody.Replace(strActualImgTag, strTrimImgTag);
                start = strHTMLBody.IndexOf("<img", start + 1);
            }

            return strTrimmedHTMLBody;
        }
        #endregion

        #endregion

        #region Load Icons
        public stdole.IPictureDisp GetImage(string imageName)
        {
            object retObj = null;

            try
            {
                if (imageName == "Options")
                {
                    //retObj = PictureConverter.IconToPictureDisp(Properties.Resources.Options);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetImage - " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return (stdole.IPictureDisp)retObj;
        }
        #endregion

    }

    #region Picture Converter Class 
    /// <summary>
    /// To manage pictures
    /// </summary>
    public class PictureConverter : AxHost
    {
        private PictureConverter() : base(String.Empty) { }

        static public stdole.IPictureDisp ImageToPictureDisp(Image image)
        {

           // return null;
             return (stdole.IPictureDisp)GetIPictureDispFromPicture(image);
        }

    

        static public stdole.IPictureDisp IconToPictureDisp(Icon icon)
        {
            return ImageToPictureDisp(icon.ToBitmap());
        }

        static public Image PictureDispToImage(stdole.IPictureDisp picture)
        {
            return GetPictureFromIPicture(picture);
        }

    }
    #endregion
#endregion
   
}
