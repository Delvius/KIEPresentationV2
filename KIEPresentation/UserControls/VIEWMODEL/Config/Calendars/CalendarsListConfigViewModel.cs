using GoogleCalendar.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleCalendar;
using KIEPresentation.UserControls.VIEWMODEL;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;
using System.ComponentModel;
using System.Windows.Input;
using KIEPresentation.Service;
using KIEPresentation.UserControls.Config;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Popups;
using System.Net.Http;
using KIEPresentation.UserControls.VIEW.Calendars;
using KIEPresentation.UserControls.VIEW.Config;
using KIEPresentation.UserControls.Config.Calendars;

namespace KIEPresentation.UserControls.VIEWMODEL.Config.Calendars
{
   public class CalendarsListConfigViewModel : ViewModelBase
    {
        //liste de tous les calendriers (objets)
        List<CalendarListEntry> _calendars;

        //Liste des calendriers (visuels)
        private List<CalendarBase> _calendriersIMG = new List<CalendarBase>();

        //liste des calendriers selectionnés
        private CalendarBase _calendriersSelected;

        public CalendarBase CalendriersSelected
        {
            get { return _calendriersSelected; }
            set { _calendriersSelected = value;
            }
        }

        //Fenêtre à afficher en fonction des actions
        private UserControl _selectedWindow;

        public UserControl SelectedWindow
        {
            get { return _selectedWindow; }
            set
            {
                _selectedWindow = value;
                RaisePropertyChanged("SelectedWindow");
            }
        }


        // Elements à stocker
        public StackPanel monpan;
        private ConfigCalendars parent;

        public List<CalendarBase> CalendriersIMG
        {
            get { return _calendriersIMG; }
            set
            {
                _calendriersIMG = value;
                RaisePropertyChanged("CalendriersIMG");
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="pan"> Le stackpanel auquel les éléments doivent être ajoutés</param>
        /// <param name="parent">La main page permettant de naviguer vers une autre page</param>
        public CalendarsListConfigViewModel(StackPanel pan, ConfigCalendars parent)
        {
            monpan = pan;
            initCalendar();
            this.parent = parent;
        }

       /// <summary>
       /// Supprime un calendrier de la liste
       /// </summary>
       /// <param name="cal"></param>
        public void DeleteCalendar(CalendarListEntry cal)
        {

            CalendarBase calfind = null;
            int index = -1;

            foreach (CalendarBase calbase in monpan.Children)
            {
                if (calbase.cal != null)
                {
                    if (calbase.cal.Calendar.Id == cal.Id)
                    {
                        calfind = calbase;
                        index = monpan.Children.IndexOf(calbase);
                    }
                }
            }

            if (index != -1)
            {
                monpan.Children.RemoveAt(index);
                _calendriersIMG.Remove(calfind);
            }

        }


        /// <summary>
        /// Ajoute un calendrier de la liste
        /// </summary>
        /// <param name="cal"></param>
        public void AddCalendar(CalendarListEntry cal)
        {
            CalendarBase newcal = new CalendarGoogle(new CalendarViewModel(cal));
                newcal.PointerPressed += Cal_PointerPressed;
                newcal.IsHitTestVisible = true;
                newcal.Opacity = 0.5;
                newcal.Margin = new Thickness(0, 0, 15, 0);
                monpan.Children.Add(newcal);
                _calendriersIMG.Add(newcal);
                _calendriersSelected = null;
           


        }


       /// <summary>
       /// Met à jour un calendrier de la liste
       /// </summary>
       /// <param name="cal"></param>
        public void MajCalendar(CalendarListEntry cal)
        {

            CalendarBase calfind = null;
            int index=-1;

            foreach (CalendarBase calbase in monpan.Children)
            {
                if(calbase.cal!=null)
                {   if (calbase.cal.Calendar.Id == cal.Id)
                {
                    calfind = calbase;
                   
                    index = monpan.Children.IndexOf(calbase);
                }
            }
            }

            if (index != -1)
            {
                
                monpan.Children.RemoveAt(index);
                _calendriersIMG.Remove(calfind);
                CalendarBase newcal = new CalendarGoogle(new CalendarViewModel(cal));
                newcal.PointerPressed += Cal_PointerPressed;
                newcal.IsHitTestVisible = true;
                newcal.Opacity = 0.5;
                newcal.Margin = new Thickness(0, 0, 15, 0);
                monpan.Children.Insert(index,newcal);
                _calendriersIMG.Add(newcal);
                _calendriersSelected = null;
            
                 }

           
        }


        /// <summary>
        /// Initialise les calendriers à afficher
        /// </summary>
        public async void initCalendar()
        {
            try
            {
                int nb =monpan.Children.Count();
                if (nb > 0)
                {
                    for (int i = nb-1; i >=0; i--)
                    monpan.Children.RemoveAt(i);
                }

                nb = _calendriersIMG.Count();
                if (nb > 0)
                {
                    for (int i = nb - 1; i >= 0; i--)
                        _calendriersIMG.RemoveAt(i);
                }
                

                   ObservableCollection<object> list = new ObservableCollection<object>();
               

                GoogleCalendar.CalendarControler controler = new GoogleCalendar.CalendarControler();

                // on récupère tous les calendriers
                _calendars = await controler.GetAllCalendars();

                //on crée les userControls Calendriers avec leurs datacontextes
                foreach (CalendarListEntry calendar in _calendars)
                {
                    this.CalendriersIMG.Add(new CalendarGoogle(new CalendarViewModel(calendar)));
                }

                CalendarAdd addCal = new CalendarAdd();
               
               
                this._calendriersIMG.Add(addCal);
                //pour chaque calendrier, on définit ses paramètres et une commande au click

                foreach (CalendarBase cal in _calendriersIMG)
                {
                    
                    cal.PointerPressed += Cal_PointerPressed;
                    cal.IsHitTestVisible = true;
                    cal.Opacity = 0.5;
                    cal.Margin = new Thickness(0, 0, 15, 0);
                    monpan.Children.Add(cal);
                }

                _calendriersSelected = _calendriersIMG.Last();
                SelectedWindow = new CalendarWelcome();

                
                //on met à jour le Layout
                //monpan.UpdateLayout();
            }
            catch (HttpRequestException)
            {
                //throw new HttpRequestException("Cette fonctionnalité nécessite une connexion à un compte google !");
            }

        }


        /// <summary>
        /// Methode utilisée lors d'un click sur un calendrier
        /// </summary>
        /// <param name="sender">Le calendrier qui a émis l'événement</param>
        /// <param name="e">L'événement reçu</param>
        private void Cal_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_calendriersSelected != null)
            {
               _calendriersSelected.Opacity=0.5;
            }

            CalendarBase selection = (CalendarBase)sender;

            if (selection.Opacity != 1)
            {
                selection.Opacity = 1;   
                        _calendriersSelected=selection;
            }
            else
            {
                selection.Opacity = 0.5;
                 _calendriersSelected=null;
            }
            if (_calendriersSelected != null)
            {
                String type;
                switch (type = _calendriersSelected.GetType().Name.ToString())
                {
                    case "CalendarAdd":
                        SelectedWindow = new AddCalendarGoogle(this);
                        break;

                    case "CalendarGoogle":
                        CalendarGoogle goog = (CalendarGoogle)_calendriersSelected;
                        SelectedWindow = new ConsultCalendarGoogle(goog.cal, this);
                        break;

                    case "CalendarURL":

                        break;
                }
            }

        }


     
    }
}
