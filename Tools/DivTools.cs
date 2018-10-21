using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media.Imaging;


namespace SteinbachCRM.Tools
{
    public class DivTools
    {
        //public static DateTime GetTodayWithCustomTime(int hour, int minute, int second)
        //{
        //    int year = DateTime.Now.Year;
        //    int month = DateTime.Now.Month;
        //    int day = DateTime.Now.Day;

        //    return new DateTime(year, month, day, hour, minute, second);

        //}

        public static BitmapImage GetImage(object o)
        {
            // Aktuelle Assembly referenzieren
            System.Reflection.Assembly assembly =
               o.GetType().Assembly;

            // Das eingebettete Bild in einen Stream lesen 
            System.IO.Stream stream =
               assembly.GetManifestResourceStream(
               assembly.GetName().Name + ".Images.msn_phone.png");

            BitmapImage image =new System.Windows.Media.Imaging.BitmapImage();
            //image.SetSource(imageStream );
            image.StreamSource = stream;
            // Den Stream in ein Bitmap umwandeln
            return image;
    
        }
    }
}
