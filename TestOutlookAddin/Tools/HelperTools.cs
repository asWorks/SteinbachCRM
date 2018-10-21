using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestOutlookAddin.Tools
{
    public class HelperTools
    {
        public static string GetConfigEntry(string key)
        {
            try
            {
                using (var db = new SteinbachEntities())
                {

                    string res = db.configs.Where(s => s.mkey == key).FirstOrDefault().value;
                    return res;

                }
            }
            catch (Exception ex)
            {
                return "";

            }


        }
    }
}
