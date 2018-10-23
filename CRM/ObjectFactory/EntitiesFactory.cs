using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Threading;


namespace SteinbachCRM.ObjectFactory
{
    public  class EntitiesFactory
    {
        //public static CRMTermine  GetNewTermin(string type)
        //{
        //    CRMTermine t = new CRMTermine();
        //    int hour = DateTime.Now.Hour + 1;
        //    DateTime dt;
        //    if (hour >= 15)
        //    {
        //        dt = DateTime.Now.AddDays(1);
        //        t.TerminVon = Tools.DateTools.GetDateWithCustomHour(dt, 07, 45, 0);
        //        t.TerminBis = Tools.DateTools.GetDateWithCustomHour(dt, 08, 45, 0);
        //        t.ErinerungDatum = Tools.DateTools.GetDateWithCustomHour(dt, 07, 30, 0);

        //    }
        //    else
        //    {
        //        dt = DateTime.Now;
        //        t.TerminVon = Tools.DateTools.GetDateWithCustomHour(dt, hour, 0, 0);
        //        t.TerminBis = Tools.DateTools.GetDateWithCustomHour(dt, hour, 30, 0);
        //        t.ErinerungDatum = Tools.DateTools.GetDateWithCustomHour(dt, hour - 1, 0, 0);
              
        //    }

            

        //    TimeSpan ts = new TimeSpan(0, 15, 0);
        //    t.ErinnerungTimespan = ts.Ticks;
        //    t.Angelegt = DAL.Session.User.id;
        //    t.EmailSent = 0;
        //    t.Erinnerung = 0;
        //    t.AppointmentType = type;

           

        //    if (type.ToLower() == "termin")
        //    {
        //        t.Betreff = "neuer Termin . . .";
        //    }
        //    else if (type.ToLower() == "aufgabe")
        //    {
        //        t.Betreff = "neue Aufgabe . . .";
        //    }

        //    return t;
        //}

    }
}
