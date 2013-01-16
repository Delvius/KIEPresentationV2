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

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236


//voir pour remplacer le clic par le doubletapped 
namespace KIEPresentation.UserControls
{
    public sealed partial class WeekCalendar : UserControl
    {

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
        private List<StackPanel> Panels;
        private List<WeekEvent> Events;
        private EventControler Controler;

        private Popup pop;

      

        /// <summary>
        /// Constructeur du WeekCalendar
        /// </summary>
        /// <param name="date">La date récupérée depuis le WeekBorderCalendar</param>
        public WeekCalendar(DateTime date, EventControler controler)
        {
            Date = date;
            this.InitializeComponent();
            Canvas = new Canvas[24, 7];
            Events = new List<WeekEvent>();
            InitEvent(date, controler);
            initBorder();
        }

        public WeekCalendar(DateTime date)
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
        public async void InitEvent(DateTime date, EventControler controler)
        {
            Controler = controler;
            List<GoogleCalendar.Data.Event> events = await Controler.GetAllEvents();
            UpdateEvent(date, events);
            
        }

   

        /// <summary>
        /// Met à jour les évènements
        /// </summary>
        /// <param name="date"></param>
        /// <param name="eventList"></param>
        public void UpdateEvent(DateTime date, List<GoogleCalendar.Data.Event> eventList)
        {
            //si on a des UserControl dans la liste, on les supprime pour ajouter les nouveaux
            foreach (WeekEvent we in Events)
            {
                LayoutWeek.Children.Remove(we);
            }

            DateTime nextweek = Date.AddDays(7);
            foreach (GoogleCalendar.Data.Event e in eventList)
            {
                DateTime eventDate;
                if(!e.Status.Equals("cancelled"))
                {
                    if (!String.IsNullOrEmpty(e.Start.DateTime))
                        eventDate = DateTime.Parse(e.Start.DateTime);
                    else if (!String.IsNullOrEmpty(e.Start.Date))
                        eventDate = DateTime.Parse(e.Start.Date);
                    else
                        continue;

                    //on crée l'evenement
                    if (eventDate.Date >= Date.Date && eventDate.Date < nextweek.Date)
                        createEvent(e);
                }
            }
            
        }



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

            // on récupère le nombre de jour dans le mois de la date du jour
            int dayInMonth = DateTime.DaysInMonth(Date.Year, Date.Month);
            int col;

            //si la date du jour est plus grande que la date de l'évenement (31>2) alors la semaine comprend deux mois
            if (Date.Day > time.Day)
            {
                //on initialise un compteur (31+1)-31 pour déterminer le nb de colonne à ajouter a la date de l'event pour trouver sa position dans la grille
                int cpt = (dayInMonth + 1) - Date.Day;
                col = time.Day + cpt;
            }
            else
            {
                col = (time.Day - Date.Day) + 1;
            }

            int hour = time.Hour;

            int n = timeEnd.DayOfYear - time.DayOfYear;

            for (int i = 0; i <= n; i++)
            {
                //On créer un user Control et on lui donne l'evenement comme contexte
                WeekEvent we = new WeekEvent(e);

                //on définit l'action a effectuer lors d'un double tap 
                we.DoubleTapped += new Windows.UI.Xaml.Input.DoubleTappedEventHandler(EventDoubleTapped);

                // on définit la ligne où l'on doit ajouter le userControl
                Grid.SetRow(we, hour);

                //on définit la colonne ou l'on doit ajouter le userControl
                Grid.SetColumn(we, col + i);

                // si i est différent de n
                if (i != n)
                {
                    // on précise que l'évenement doit prendre plusieurs lignes
                    Grid.SetRowSpan(we, (24 - hour));
                    hour = 0;
                }
                else
                {   // si l'heure de fin est égale à l'heure de début
                    if (timeEnd.Hour == hour)
                        Grid.SetRowSpan(we, 24);
                    else
                        Grid.SetRowSpan(we, (timeEnd.Hour - hour));
                    
                }
                Events.Add(we);

                
                StackPanel pan = GetStackPanel(hour, col + i);

                IEnumerable<WeekEvent> collection = pan.Children.OfType<WeekEvent>();

               double width = pan.ActualWidth;

                if (collection.Count()==1)
                {
                    we.Width = width/2;
                    foreach (WeekEvent old in collection)
                    {
                        old.Width = width/2;
                    }
                }
                else if (collection.Count() == 2)
                {
                    we.Width = width/3;
                    foreach (WeekEvent old in collection)
                    {
                        old.Width = width/3;
                    }
                }
                else if (collection.Count() == 3)
                {
                    we.Width = width/4;
                    foreach (WeekEvent old in collection)
                    {
                        old.Width = width/4;
                    }
                }
                else
                {

                    we.Width = width;
                }
                pan.Children.Add(we);
                LayoutWeek.UpdateLayout();
            }
        }


        // récupère le stackpanel à l'index specifié
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


   
        private void EventDoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            
            WeekEvent we = (WeekEvent)sender;
            Event.ModifEvent evtDetail = new Event.ModifEvent(we.Evt);
            
            pop = new Popup()
            {
                Child = evtDetail,
                VerticalOffset = (Window.Current.Bounds.Height-394)/2,
                HorizontalOffset = (Window.Current.Bounds.Width-596)/2,
                Visibility = Windows.UI.Xaml.Visibility.Visible
            };
       
            pop.IsLightDismissEnabled = true;
            pop.IsOpen = true;
        
            
        }



        private void initBorder()
        {

            Thickness th = new Thickness();
            th.Bottom = 0.2;
            th.Top = 0.2;
            th.Left = 0.2;
            th.Right = 0.2;
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
        }

    }
}
