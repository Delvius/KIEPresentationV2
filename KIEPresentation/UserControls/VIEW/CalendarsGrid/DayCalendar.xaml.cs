using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using GoogleCalendar;
using GoogleCalendar.Data;
using System.Collections.ObjectModel;
using System.Net.Http;
using KIEPresentation.UserControls.Config.Event;
using KIEPresentation.UserControls.VIEW.Calendars;
using System.Diagnostics;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace KIEPresentation.UserControls.VIEW.CalendarsGrid
{
    public sealed partial class DayCalendar : UserControl, ICalendar
    {
        public Grid layout;
        private ScrollViewer scroll;

        /// <summary>
        /// Date de la semaine courante
        /// </summary>
        public DateTime Date
        {
            get;
            set;
        }

        /// <summary>
        /// La cellule selectionnée par l'utilisateur
        /// </summary>
        private Canvas beginCell = null;

        /// <summary>
        /// Le canvas 
        /// </summary>
        private Canvas[,] Canvas;
        private Canvas lastColored;
        private List<DayEvent> Events;
        private EventControler Controler;

        private Popup pop;

        public Popup GetPop()
        {
            return pop;
        }

        /// <summary>
        /// Constructeur du WeekCalendar
        /// </summary>
        /// <param name="date">La date récupérée depuis le WeekBorderCalendar</param>
        public DayCalendar(DateTime date, EventControler controler, ScrollViewer myScroll)
        {
            Date = date;
            Controler = controler;
            this.InitializeComponent();
            Canvas = new Canvas[24, 7];
            Events = new List<DayEvent>();
            InitEvent();
            initBorder();
            scroll = myScroll;
        }

        public DayCalendar(DateTime date, ScrollViewer myScroll)
        {
            Date = date;
            this.InitializeComponent();
            Canvas = new Canvas[24, 7];
            initBorder();
        }


        /// <summary>
        /// Récupère la liste des évenements
        /// </summary>
        /// <param name="date"></param>
        /// <param name="controler"></param>
        public async void InitEvent()
        {

            try
            {

                List<GoogleCalendar.Data.Event> events = await Controler.GetAllEvents();
                UpdateEvent(Date, events);
            }
            catch (HttpRequestException e)
            {

            }
        }



        /// <summary>
        /// Met à jour les évènements
        /// </summary>
        /// <param name="date"></param>
        /// <param name="eventList"></param>
        public void UpdateEvent(DateTime date, List<GoogleCalendar.Data.Event> eventList)
        {

            //si on a des UserControls dans la liste, on les supprime pour ajouter les nouveaux
            foreach (DayEvent we in Events)
            {
                LayoutWeek.Children.Remove(we);
            }

            //on vide le stackPanel de ses enfants
            IEnumerable<StackPanel> collection = LayoutWeek.Children.OfType<StackPanel>();
            foreach (StackPanel pan in collection)
            {
                int nb = pan.Children.Count();

                for (int i = nb - 1; i >= 0; i--)
                {
                    pan.Children.RemoveAt(i);
                }
            }

            DateTime nextweek = Date.AddDays(1);
            foreach (GoogleCalendar.Data.Event e in eventList)
            {
                DateTime eventDate;
                if (!e.Status.Equals("cancelled"))
                {
                    if (!String.IsNullOrEmpty(e.Start.DateTime))
                        eventDate = DateTime.Parse(e.Start.DateTime);
                    else if (!String.IsNullOrEmpty(e.Start.Date))
                        eventDate = DateTime.Parse(e.Start.Date);
                    else
                        continue;

                    //on crée l'evenement
                    if (eventDate.Date == Date.Date)
                        createEvent(e);
                }
            }

        }

        /// <summary>
        /// Retourne la colonne d'une date
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private int getCol(DateTime time)
        {
            // on récupère le nombre de jour dans le mois de la date du jour
            int dayInMonth = DateTime.DaysInMonth(Date.Year, Date.Month);

            //si la date du jour est plus grande que la date de l'évenement (31>2) alors la semaine comprend deux mois
            if (Date.Day > time.Day)
            {
                //on initialise un compteur (31+1)-31 pour déterminer le nb de colonne à ajouter a la date de l'event pour trouver sa position dans la grille
                int cpt = (dayInMonth + 1) - Date.Day;
                return time.Day + cpt;
            }
            else
            {
                return (time.Day - Date.Day) + 1;
            }
        }

        /// <summary>
        /// Methode qui crée les évènements et les place dans les stackpanel correspondants
        /// </summary>
        /// <param name="e"></param>
        private void createEvent(GoogleCalendar.Data.Event e)
        {
            DateTime time, timeEnd;

            if (!String.IsNullOrEmpty(e.Start.DateTime))
                time = DateTime.Parse(e.Start.DateTime);
            else
                time = DateTime.Parse(e.Start.Date);

            if (!String.IsNullOrEmpty(e.Start.DateTime))
                timeEnd = DateTime.Parse(e.End.DateTime);
            else
                timeEnd = DateTime.Parse(e.End.Date);// format : 2011-11-02T07:00:00+01:00

            //on récupère la colonne de l'evenement
            int col = getCol(time);

            int hour = time.Hour;

            int n = timeEnd.DayOfYear - time.DayOfYear;

            for (int i = 0; i <= n; i++)
            {
                //On créer un user Control et on lui donne l'evenement comme contexte
                DayEvent we = new DayEvent(e);

                //on définit l'action a effectuer lors d'un double tap 
                we.DoubleTapped += new Windows.UI.Xaml.Input.DoubleTappedEventHandler(EventDoubleTapped);

                //retourne le stackpanel à l'index spécifié
                StackPanel pan = GetStackPanel(hour, col + i);


                // on définit la ligne où l'on doit ajouter le userControl
                Grid.SetRow(pan, hour);

                //on définit la colonne ou l'on doit ajouter le userControl
                Grid.SetColumn(pan, col + i);

                // si i est différent de n
                if (i != n)
                {
                    // on précise que l'évenement doit prendre plusieurs lignes
                    Grid.SetRowSpan(pan, (24 - hour));
                    hour = 0;
                }
                else
                {   // si l'heure de fin est égale à l'heure de début
                    if (timeEnd.Hour == hour && timeEnd.Minute == time.Minute)
                    {
                        Grid.SetRowSpan(pan, 24);
                    }
                    else if (timeEnd.Hour == hour && timeEnd.Minute != time.Minute)
                    {
                        Grid.SetRowSpan(pan, (timeEnd.Hour - hour + 1));
                    }
                    else
                    {
                        Grid.SetRowSpan(pan, (timeEnd.Hour - hour));
                    }
                }
                Events.Add(we);


                IEnumerable<DayEvent> collection = pan.Children.OfType<DayEvent>();

                double width = pan.ActualWidth;

                if (collection.Count() == 1)
                {
                    we.Width = width / 2;
                    foreach (DayEvent old in collection)
                    {
                        pan.Orientation = Orientation.Horizontal;
                        old.Width = width / 2;
                    }
                }
                else if (collection.Count() == 2)
                {
                    we.Width = width / 3;
                    pan.Orientation = Orientation.Horizontal;
                    foreach (DayEvent old in collection)
                    {
                        old.Width = width / 3;
                    }
                }
                else if (collection.Count() == 3)
                {
                    we.Width = width / 4;
                    pan.Orientation = Orientation.Horizontal;
                    foreach (DayEvent old in collection)
                    {
                        old.Width = width / 4;
                    }
                }
                else
                {

                    we.Width = width;
                }
                pan.Children.Add(we);
                LayoutWeek.UpdateLayout();
                layout = this.LayoutWeek;
            }
        }


        /// <summary>
        /// Retourne le stackpanel à l'index spécifié
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public StackPanel GetStackPanel(int row, int column)
        {
            IEnumerable<StackPanel> collection = LayoutWeek.Children.OfType<StackPanel>();

            IEnumerable<StackPanel> panels = from StackPanel pan in collection
                                             where pan is StackPanel &&
                                             Grid.GetRow(pan) == row &&
                                             Grid.GetColumn(pan) == column
                                             select pan as StackPanel;

            StackPanel obj = panels.ElementAt(0);
            return obj;
        }


        /// <summary>
        /// Evenement à effectuer lors d'un doubleTapp sur un event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventDoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {

            DayEvent we = (DayEvent)sender;
            ModifEvent evtDetail = new ModifEvent(we.Evt, Controler, this);
            pop = new Popup()
            {
                Child = evtDetail,
                VerticalOffset = (Window.Current.Bounds.Height - 394) / 2,
                HorizontalOffset = (Window.Current.Bounds.Width - 596) / 2,
                Visibility = Windows.UI.Xaml.Visibility.Visible,
                IsLightDismissEnabled = true,
                IsOpen = true
            };
        }


        /// <summary>
        /// Methode qui initialise les bordures de la grille
        /// </summary>
        private void initBorder()
        {

            Thickness th = new Thickness();
            th.Bottom = 0.1;
            th.Top = 0.1;
            th.Left = 0.1;
            th.Right = 0.1;
            SolidColorBrush color = new SolidColorBrush(Color.FromArgb(205, 205, 205, 205));
            Canvas canvas;

            //pour i =0 jusqu'a 7
            for (int i = 1; i < 8; i++)
            {
                //pour i de 0 jusqu'a 24
                for (int j = 0; j < 25; j++)
                {
                    Border b = new Border();
                    if (i > 0 && j > 0)
                    {
                        canvas = createCanvas();
                        Grid.SetColumn(canvas, i);
                        Grid.SetRow(canvas, j);
                        Canvas[j - 1, i - 1] = canvas;

                    }
                    else
                    {
                        canvas = createCanvas();
                        Grid.SetColumn(canvas, i);
                        Grid.SetRow(canvas, j);
                    }
                    LayoutWeek.Children.Add(canvas);
                    b.BorderBrush = color;
                    Grid.SetRow(b, j);
                    Grid.SetColumn(b, i);
                    b.BorderThickness = th;
                    LayoutWeek.Children.Add(b);
                }
            }
        }



        /// <summary>
        /// Allow to create a canvas for implement the click on a cell.
        /// </summary>
        /// <returns></returns>
        private Canvas createCanvas()
        {
            Canvas canvas = new Canvas();
            canvas.Background = new SolidColorBrush(Colors.Transparent);
            canvas.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            canvas.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            canvas.DoubleTapped += new Windows.UI.Xaml.Input.DoubleTappedEventHandler(canvas_PointerPressed);
            canvas.PointerReleased += new Windows.UI.Xaml.Input.PointerEventHandler(canvas_PointerReleased);

            return canvas;
        }

        /// <summary>
        /// Allow to fill the form when the end cell was released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (beginCell != null)
            {
                SolidColorBrush unselected = new SolidColorBrush(Colors.Transparent);
                beginCell.Background = unselected;
                int beginPos = (Grid.GetColumn(beginCell) - 1) * 24 + (Grid.GetRow(beginCell) - 1);
            }
        }



        /// <summary>
        /// Update the begin cell to make a selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_PointerPressed(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            beginCell = (Canvas)sender;
            beginCell.Background = new SolidColorBrush(new Color() { A = 100, R = 70, B = 180, G = 130 });
            lastColored = beginCell;
            int beginPos = (Grid.GetColumn(beginCell) - 1) * 24 + (Grid.GetRow(beginCell) - 1);
            DateTime begin;
            DateTime end;

            int row = Grid.GetRow(beginCell);
            if (row == 24) row = 23;
            DateTime date = Date;
            //  begin = Date.AddDays(Grid.GetColumn(beginCell) - 1).AddHours(Grid.GetRow(beginCell));
            // begin= Date.AddDays(Grid.GetColumn(beginCell) - 1);

            Debug.WriteLine(Grid.GetRow(beginCell));

            begin = Date.AddMinutes(-(Date.Minute)).AddSeconds(-(Date.Second)).AddHours(-(Date.Hour) + row);
            begin.AddHours(Grid.GetRow(beginCell));

            //end = Date.AddDays(Grid.GetColumn(beginCell) - 1).AddHours(Grid.GetRow(beginCell)+1);
            end = Date.AddMinutes(-(Date.Minute)).AddSeconds(-(Date.Second)).AddHours(-(Date.Hour) + row + 1);
            end.AddHours(Grid.GetRow(beginCell));

            GoogleCalendar.Data.Event events = new GoogleCalendar.Data.Event();
            events.Location = "";
            events.Description = "";
            events.Summary = "";
            events.Start = new EventDateTime() { DateTime = begin.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK") };
            events.End = new EventDateTime() { DateTime = end.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK") };


            DayEvent we = new DayEvent(events);

            AddEvent evtDetail = new AddEvent(we.Evt, Controler, this);
            pop = new Popup()
            {
                Child = evtDetail,
                VerticalOffset = (Window.Current.Bounds.Height - 394) / 2,
                HorizontalOffset = (Window.Current.Bounds.Width - 596) / 2,
                Visibility = Windows.UI.Xaml.Visibility.Visible,
                IsLightDismissEnabled = true,
                IsOpen = true
            };


        }

      

        /// <summary>
        /// Methode qui place la scrollbar à 7h
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_loaded(object sender, RoutedEventArgs e)
        {
            if (scroll != null)
            {
                scroll.UpdateLayout();
                IEnumerable<StackPanel> collection = LayoutWeek.Children.OfType<StackPanel>();
                StackPanel pan = (StackPanel)collection.First();
                double offset = (pan.ActualHeight + pan.Margin.Top + pan.Margin.Bottom) * 7;
                scroll.ScrollToVerticalOffset(offset);
            }
        }

    }
}
