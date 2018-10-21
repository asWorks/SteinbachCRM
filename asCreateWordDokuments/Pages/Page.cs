using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace asCreateWordDokuments.Pages
{
    public class Page
    {

        public int Blatt { get; set; }
        public int Rechnungsnummer { get; set; }
        public string Kundennummer { get; set; }
        public string Datum { get; set; }
        public bool isError { get; set; }
        public string ErrorMessage { get; set; }
        public int MyProperty { get; set; }

        internal string text;



        public Page()
        {
            isError = false;
            ErrorMessage = string.Empty;
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                try
                {
                    text = value;
                }
                catch (Exception ex)
                {
                    // Console.WriteLine(ex.Message);
                    ErrorMessage += ex.Message;
                    isError = true;

                    throw;
                }


            }
        }


        public bool SetText(string Text)
        {

            try
            {
                this.text = Text;
                return ProcessData();
            }
            catch (Exception ex)
            {
                ErrorMessage += ex.Message;
                isError = true;
                return false;

            }

        }

       
        public virtual bool ProcessData()
        {
            return false;
        }


       


        public void WriteSheet(string filename)
        {

            using (var sw = new System.IO.StreamWriter(filename, false, System.Text.Encoding.GetEncoding(850)))
            {
                sw.Write(Text);
                sw.Flush();

            }

        }

        //protected virtual string ClearText(string Text)
        //{
        //    try
        //    {
        //        string lz = "\r\n"; // + new string(' ', Session.EntfernenLeerzeichenLinks);
        //        string buf = string.Empty;
        //        //buf = Text.Replace("A  \r\n\0  \r\n   \r\nç", "");
        //       // buf = Text.Replace(Session.Trennzeichenfolge, "");

        //        buf = buf.Replace('\0', ' ');
        //        // buf = buf.Replace(lz, "\r\n");
        //        // buf = buf.Remove(1, Session.EntfernenLeerzeichenLinks - 1);
        //        char[] c = { '\n' };
        //        int[] zub = new int[20];
        //        int index = 0;
        //        for (int i = 0; i < 20; i++)
        //        {
        //            index = buf.IndexOf("\r\n", index + 2);
        //            zub[i] = index;

        //        }

        //        buf = buf.Remove(zub[11] + 2, 2);
        //        buf = buf.Insert(zub[11] + 2, "- ");

        //        buf = buf.Remove(zub[12] + 2, 21);
        //        buf = buf.Insert(zub[12] + 2, "        Ihr Auftrag :");

        //        buf = buf.Remove(zub[13] + 7, 16);
        //        buf = buf.Insert(zub[13] + 7, "   Liefertermin:");

        //        //buf = buf.Insert(zub[14] + 7, "   ");

        //        buf = buf.Insert(zub[12], "\r\n");


        //        for (int i = 0; i < 14; i++)
        //        {
        //            if (index != -1)
        //            {
        //                index = buf.IndexOf("\r\n", index + 2);
        //                buf = buf.Remove(index + 2, 2);
        //                buf = buf.Insert(index + 2, "  ");
        //            }
        //        }



        //        return buf;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage += ex.Message;
        //        isError = true;
        //        throw;
        //    }


        //}


        protected int GetIndex(string[] data, int startindex)
        {
            int index = -1;
            foreach (var s in data)
            {
                index = (this.text.IndexOf(s, startindex));
                {
                    if (index != -1)
                    {
                        return index;
                    }
                }

            }

            return index;

        }

        private string GetFileName()
        {
            return "";

        }
    }
}
