using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace EditBesuchsberichte
{
    class Program
    {



        static void Main(string[] args)
        {
            var m = new Program();
            m.CorrectBesuchsbericht();

        }

        public void CorrectBesuchsbericht()
        {
            using (var db = new SteinbachEntities())
            {
                foreach (var item in db.Firmen_Kundenbesuche)
                {
                    if (item.id_siperson != null)
                    {
                        var sip = new Kundenbesuche_TeilnehmerSI();
                        sip.id_TeilnemerSI = item.id_siperson;
                        sip.id_besuch = item.id;
                        db.AddToKundenbesuche_TeilnehmerSI(sip);
                        Console.WriteLine(string.Format("Hinzugefügt {0} zu {1} ",item.person.benutzername,item.besuchsthema ));
                    }

                    if (item.id_vertretenefirma != null)
                    {
                        var vf = new Kundenbesuche_VertreteneFirmen();
                        vf.id_VertreteneFirma = item.id_vertretenefirma;
                        vf.id_besuch = item.id;
                        db.AddToKundenbesuche_VertreteneFirmen(vf);
                        Console.WriteLine(string.Format("Hinzugefügt {0} zu {1} ",item.firma1.name,item.besuchsthema ));
                    }

                   
                }

                db.SaveChanges();
                Console.ReadLine();

            } 

        }
    }
}
