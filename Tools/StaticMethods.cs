using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SteinbachCRM.Tools
{
    public class StaticMethods
    {
        public static string GetPath(string filename)
        {
            //string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //int pos = path.IndexOf(@"\bin");
            //path = path.Substring(0, pos) + "\\Belege\\" + filename;
            //return path;

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            FileInfo fi = new FileInfo(path);
            path = fi.FullName;
            path = path.Replace(fi.Name, filename);

            return path;

        }
    }
}
