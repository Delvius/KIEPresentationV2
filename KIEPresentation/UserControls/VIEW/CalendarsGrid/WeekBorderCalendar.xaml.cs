using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GoogleCalendar.Data;
using KIEPresentation.UserControls.VIEWMODEL;
using System.Diagnostics;
// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace KIEPresentation.UserControls.VIEW.CalendarsGrid
{
    public sealed partial class WeekBorderCalendar : UserControl, IBorderCalendar
    {
        private KIEPresentation.UserControls.VIEWMODEL.ObservableVector<object> _calendars;

        private WeekCalendar week;
        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;

            }
        }

        /// <summary>
        /// Retourne la date de la semaine courante
        /// </summary>
        /// <returns></returns>
        public DateTime getDate()
        {
            return Date;
        }

        /// <summary>
        /// Constructeur de la vue sans aucun calendrier chargé
        /// </summary>
        public WeekBorderCalendar(ref DateTime dateMain)
        {

            this.InitializeComponent();
            initDay(dateMain);
            this.ScrollView.Content = new WeekCalendar(Date,this.ScrollView);
            
        }


        /// <summary>
        /// Constructeur de la vue avec un calendrier chargé
        /// </summary>
        public WeekBorderCalendar(ObservableVector<object> calendars, ref DateTime dateMain)
        {
            _calendars = calendars;
            this.InitializeComponent();
            Date = dateMain;
            initDay(dateMain);
            InitCalendars(_calendars);
           
        }



        /// <summary>
        /// Initialise les jours de la semaine
        /// </summary>
        /// <param name="date"></param>
        private void initDay(DateTime date)
        {
            DateTime dat1 = date;
            if (date.DayOfWeek == DayOfWeek.Monday)
            {
                Date = date;
                this.Year.Text = date.Year.ToString();
                TextBlock[] textblock = new TextBlock[] { day1, day2, day3, day4, day5, day6, day7 };
                for (int i = 0; i < textblock.Length; i++)
                {
                    textblock[i].Text = date.ToString("dddd dd/MM");
                    date = date.AddDays(1);
                }

            }
            else
            {
                while (date.DayOfWeek != DayOfWeek.Monday)
                    date = date.AddDays(-1);
                initDay(date);
            }
      
        }

        private void changeDate(ref DateTime date)
        {
            Date=date;
        }

        /// <summary>
        /// Charge la semaine précédente
        /// </summary>
        public void PrevWeek()
        {
            DateTime dat = Date;
            DateTime dat2=dat.AddDays(-7);
            initDay(dat2);
            InitCalendars(_calendars);
        }

        /// <summary>
        /// Charge la semaine suivante
        /// </summary>
        public void NextWeek()
        {
            DateTime dat = Date;
            DateTime dat2 = dat.AddDays(7);
            initDay(dat2);
            InitCalendars(_calendars);

            Debug.WriteLine(Date);
        }

        /// <summary>
        /// Initialise les calendriers à afficher
        /// </summary>
        /// <param name="calendars"></param>
        public void InitCalendars(ObservableVector<object> calendars)
        {
            _calendars = calendars;
            CalendarViewModel cal1 = (CalendarViewModel)calendars.Last();
            CalendarListEntry cal = cal1.Calendar;
            week = new WeekCalendar(Date, new GoogleCalendar.EventControler(cal),this.ScrollView);
            this.ScrollView.Content = week;
            this.UpdateLayout();

          
        }
    }
}
