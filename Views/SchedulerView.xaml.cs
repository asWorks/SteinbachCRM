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
using DAL;
using C1.C1Schedule;
using Outlook = Microsoft.Office.Interop.Outlook;
using SteinbachCRM.ViewModels;

namespace SteinbachCRM.Views
{

    /// <summary>
    /// Interaction logic for SchedulerView.xaml
    /// </summary>
    public partial class SchedulerView : UserControl
    {

        SteinbachEntities db;

        public SchedulerView()
        {

            InitializeComponent();
            this.c1Scheduler1.AppointmentChanged += new EventHandler<C1.WPF.Schedule.AppointmentActionEventArgs>(c1Scheduler1_AppointmentChanged);
            this.c1Scheduler1.AppointmentAdded += new EventHandler<C1.WPF.Schedule.AppointmentActionEventArgs>(c1Scheduler1_AppointmentAdded);
            c1Scheduler1.StyleChanged += new EventHandler<RoutedEventArgs>(c1Scheduler1_StyleChanged);
            db = new SteinbachEntities();



            // Init(DateTime.Now, DateTime.Now.AddDays(30));

        }

        public SchedulerView(DateTime VonDatum, DateTime BisDatum)
        {
            //InitializeComponent();
            //this.c1Scheduler1.AppointmentChanged += new EventHandler<C1.WPF.Schedule.AppointmentActionEventArgs>(c1Scheduler1_AppointmentChanged);
            //this.c1Scheduler1.AppointmentAdded += new EventHandler<C1.WPF.Schedule.AppointmentActionEventArgs>(c1Scheduler1_AppointmentAdded);
            //c1Scheduler1.StyleChanged += new EventHandler<RoutedEventArgs>(c1Scheduler1_StyleChanged);
            //db = new SteinbachEntities();
            //LoadData(VonDatum,BisDatum);

            //   Init(VonDatum, BisDatum);

        }

        private void Init(DateTime von, DateTime bis)
        {
            InitializeComponent();
            this.c1Scheduler1.AppointmentChanged += new EventHandler<C1.WPF.Schedule.AppointmentActionEventArgs>(c1Scheduler1_AppointmentChanged);
            this.c1Scheduler1.AppointmentAdded += new EventHandler<C1.WPF.Schedule.AppointmentActionEventArgs>(c1Scheduler1_AppointmentAdded);
            c1Scheduler1.StyleChanged += new EventHandler<RoutedEventArgs>(c1Scheduler1_StyleChanged);
            db = new SteinbachEntities();
            LoadData(von, bis);

        }


        void LoadData(DateTime from, DateTime to)
        {
            var db = new SteinbachEntities();
            DateTime Bis = CommonTools.Tools.DateTools.GetDateWithCustomHour(to,23,59,59);
            DateTime Von = CommonTools.Tools.DateTools.GetDateWithCustomHour(from, 0, 0, 0);
            //var t = db.CRMTermine.Where(d => d.TerminVon >= Von && d.TerminVon <= Bis && d.AppointmentType == "Termin");
            var t = db.CRMTermine.Where(d => d.TerminBis >= Von && d.TerminVon <= Bis && d.AppointmentType == "Termin");

            c1Scheduler1.BeginInit();

            foreach (var item in t)
            {
                Appointment a = new Appointment();
                a.Start = (DateTime)item.TerminVon;
                a.End = (DateTime)item.TerminBis;
                a.Duration = item.TerminDauer == null ? TimeSpan.FromTicks(0) : TimeSpan.FromTicks((long)item.TerminDauer);

                AuswahlEintraege Aktion = db.AuswahlEintraege.Where(ak => ak.id == item.Aktion).SingleOrDefault();
                string sAction = Aktion == null ? "" : Aktion.Eintrag;
                int iAction = Aktion == null ? 8 : (int)Aktion.ai_int;
                a.Subject = String.Format("{0} => {1}", item.Betreff, sAction);

                a.Location = item.Standort;
                int ac = iAction < 12 ? iAction : 8;
                a.Label = this.c1Scheduler1.DataStorage.LabelStorage.Labels[ac];
                a.Tag = "Tag Test";
                a.CustomData = item.id.ToString();
                a.Body = item.Details;


                this.c1Scheduler1.DataStorage.AppointmentStorage.Appointments.Add(a);

            }
            c1Scheduler1.EndInit();
        }

        void c1Scheduler1_StyleChanged(object sender, RoutedEventArgs e)
        {
            // update toolbar buttons state according to the current C1Scheduler view
            if (c1Scheduler1.Style == c1Scheduler1.WorkingWeekStyle)
            {
                btnWorkWeek.IsChecked = true;
            }
            else if (c1Scheduler1.Style == c1Scheduler1.WeekStyle)
            {
                btnWeek.IsChecked = true;
            }
            else if (c1Scheduler1.Style == c1Scheduler1.MonthStyle)
            {
                btnMonth.IsChecked = true;
            }
            else if (c1Scheduler1.Style == c1Scheduler1.OneDayStyle)
            {
                btnDay.IsChecked = true;
            }
            else
            {
                btnTimeLine.IsChecked = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


           

           
        }





        void c1Scheduler1_AppointmentAdded(object sender, C1.WPF.Schedule.AppointmentActionEventArgs e)
        {

            var cd = e.Appointment.CustomData;

            if (cd == null)
            {
                CRMTermine t = new CRMTermine();
                t.TerminVon = e.Appointment.Start;
                t.TerminBis = e.Appointment.End;
                t.TerminDauer = e.Appointment.Duration.Ticks;
                t.Betreff = e.Appointment.Subject;
                t.Standort = e.Appointment.Location;
                t.AppointmentType = "Termin";
                t.Details = e.Appointment.Body;

                var r = e.Appointment.Reminder;
                db.AddToCRMTermine(t);
                db.SaveChanges();
            }

        }



        void c1Scheduler1_AppointmentChanged(object sender, C1.WPF.Schedule.AppointmentActionEventArgs e)
        {
            var x = e.Appointment;
            var cd = e.Appointment.CustomData;

            if (cd != string.Empty)
            {
                int _id = int.Parse(cd);
                CRMTermine t = db.CRMTermine.Where(a => a.id == _id).SingleOrDefault();
                t.TerminVon = e.Appointment.Start;
                t.TerminBis = e.Appointment.End;
                t.TerminDauer = e.Appointment.Duration.Ticks;
                string subj = e.Appointment.Subject;
                if (subj.Contains("=>"))
                {
                    subj = subj.Substring(0, subj.IndexOf("=>") - 1);
                }
                t.Betreff = subj;
                t.Standort = e.Appointment.Location;

                db.SaveChanges();
            }

        }



        private void btnToday_Click(object sender, RoutedEventArgs e)
        {
            c1Scheduler1.VisibleDates.BeginUpdate();
            c1Scheduler1.VisibleDates.Clear();
            c1Scheduler1.VisibleDates.Add(DateTime.Today);
            c1Scheduler1.VisibleDates.EndUpdate();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDay_Click(object sender, RoutedEventArgs e)
        {
            c1Scheduler1.Style = c1Scheduler1.OneDayStyle;
        }

        private void btnWorkWeek_Click(object sender, RoutedEventArgs e)
        {
            c1Scheduler1.Style = c1Scheduler1.WorkingWeekStyle;
        }

        private void btnWeek_Click(object sender, RoutedEventArgs e)
        {
            c1Scheduler1.Style = c1Scheduler1.WeekStyle;
        }

        private void btnMonth_Click(object sender, RoutedEventArgs e)
        {
            c1Scheduler1.Style = c1Scheduler1.MonthStyle;
        }

        private void btnTimeLine_Click(object sender, RoutedEventArgs e)
        {
            c1Scheduler1.Style = c1Scheduler1.TimeLineStyle;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // update toolbar buttons state
            c1Scheduler1_StyleChanged(null, null);
        }


        private void ReminderExample()
        {
            Outlook.Application outlookApp = new Outlook.Application();

            Outlook.AppointmentItem appt = outlookApp.CreateItem(Outlook.OlItemType.olAppointmentItem)
                as Outlook.AppointmentItem;
            appt.Subject = "Wine Tasting";
            appt.Location = "Napa CA";
            appt.Sensitivity = Outlook.OlSensitivity.olPrivate;
            appt.Start = new DateTime(2013, 01, 24, 20, 55, 00); //DateTime.Parse("10/21/2006 10:00 AM");
            appt.End = new DateTime(2013, 01, 24, 22, 55, 00); //DateTime.Parse("10/21/2006 3:00 PM");
            appt.ReminderSet = true;
            appt.ReminderMinutesBeforeStart = 8;
            appt.Save();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vModel = (SchedulerViewModel)this.DataContext;
            //C1.WPF.Schedule.DateList dl = new C1.WPF.Schedule.DateList();
            //DateTime dt = vModel.VonDatum.Date;
         
            //do
            //{
            //    dl.Add(dt);
            //    dt = dt.AddDays(1);

            //} while (dt <= vModel.BisDatum.AddDays(1));

            
            //this.cal1.SelectedDates = dl;
           // cal1.Month = vModel.VonDatum.Month;
           // this.c1Scheduler1.SelectedDateTime = vModel.VonDatum.Date;
            
         
            LoadData(vModel.VonDatum, vModel.BisDatum);


        }



    }
}
