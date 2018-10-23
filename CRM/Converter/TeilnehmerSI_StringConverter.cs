using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Collections;
using DAL;


namespace SteinbachCRM.Converter
{
[ValueConversion(typeof(int),typeof(string ))]
    public class TeilnehmerSI_StringConverter:IValueConverter
    {



        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
                    
            using (var db = new SteinbachEntities())
            {
                // Test Git Comment
                if (value == null || (int)value == 0)
                {
                    return "-";
                }
                string buf = string.Empty;
                


                int v = (int)value;
                var query = from p in db.Termine_TeilnehmerSI where p.id_Termin == v select p;
                foreach (var item in query)
                {
                    string tsi = db.personen.Where(n => n.id == item.id_Teilnehmer).SingleOrDefault().kuerzel;
                    buf += tsi + ", ";
                }
                if (buf.Length>3)
                {
                    buf = buf.Substring(0, buf.Trim().Length - 1);
                }
                
                return buf;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

     
    }
}
