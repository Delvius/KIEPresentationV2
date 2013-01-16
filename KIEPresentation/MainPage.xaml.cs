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
using Windows.UI.ApplicationSettings;
using KIEPresentation.UserControls;
using Windows.ApplicationModel.Resources;
using KIEPresentation.UserControls.Config;
using GoogleCalendar.Data;
using GoogleCalendar;
using KIEPresentation.UserControls.VIEWMODEL;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.Net.Http;
using KIEPresentation.UserControls.VIEW.Config;
using KIEPresentation.UserControls.VIEW.Calendars;
using KIEPresentation.UserControls.VIEW.CalendarsGrid;
using KIEPresentation.Service;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace KIEPresentation
{
    

    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static bool connected=false;
        public IBorderCalendar icalendar;
        public ContentPresenter contentPage;
        Popup _settingsPopup;
        private static bool first = true;
        private ObservableVector<object> _calendars;

        private DateTime _dateMain;


        public MainPage()
        {

           
            VerifToken();
           
            SettingsPane.GetForCurrentView().CommandsRequested -= _CommandsRequested;
            this.InitializeComponent();
            this.ChangeCalendar.Content = IConstantes.BUTTON_SWITCH_CALENDAR;
            this.Day.Content = IConstantes.BUTTON_DAY;
            this.Week.Content = IConstantes.BUTTON_WEEK;
            this.Month.Content = IConstantes.BUTTON_MONTH;

            _dateMain = DateTime.Now;
            icalendar = new WeekBorderCalendar(ref _dateMain);
            contenuPage.Content = icalendar;
            contentPage = contenuPage;
            ObservableCollection<object> list = new ObservableCollection<object>();
            this._calendars = list.ToObservableVector<object>();

           

            if (first)
            {
                SettingsPane.GetForCurrentView().CommandsRequested += _CommandsRequested;
                first = false;
            }
        }


        public async void VerifToken()
        {
            // si on a pu rafraichir le token alors on récupère les calendriers et on charge la vue de base
            if (connected = await GoogleCalendar.CalendarService.Instance.IsInitToken())
            {
                await GetCalendars();
            }
          
         
        }


        /// <summary>
        /// Methode qui permet d'ajouter des nouvelles commandes au SettingPane
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void _CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {


            SettingsCommand cmd = new SettingsCommand("Connexion", "Se connecter", (x) =>
            {
                this.Authentification();
            });

            /* Ne fonctionne pas correctement - A revoir
            SettingsCommand cmd1 = new SettingsCommand("Déconnexion", "Se Déconnecter", (x) =>
                {
                    this.Deconnexion();
                });
            */
            SettingsCommand cmd2 = new SettingsCommand("Config", "Configurer l'application", (x) =>
            {
                this.Frame.Navigate(typeof(ConfigPanel));

            });


            SettingsCommand cmd3 = new SettingsCommand("APropos", "A Propos...", (x) =>
            {
                Rect windowBounds = Window.Current.Bounds;

                // Create popup
                _settingsPopup = new Popup()
                {
                    // Permet de définir si le popup doit se fermer au clic sur le background
                    IsLightDismissEnabled = true,
                    Width = 346,
                    Height = windowBounds.Height
                };
                _settingsPopup.Closed += OnPopupClosed;
                _settingsPopup.SetValue(Canvas.LeftProperty, windowBounds.Width - 346);
                _settingsPopup.SetValue(Canvas.TopProperty, 0);


                // Create settings pane
                APROPOS appSettingsPane = new APROPOS()
                {
                    Width = 346,
                    Height = windowBounds.Height,

                };


                // Hook settings pane in popup
                _settingsPopup.Child = appSettingsPane;

                // Display popup
                _settingsPopup.IsOpen = true;
            });

            args.Request.ApplicationCommands.Add(cmd);
            //args.Request.ApplicationCommands.Add(cmd1);
            args.Request.ApplicationCommands.Add(cmd2);
            args.Request.ApplicationCommands.Add(cmd3);
        }

        public void Deconnexion()
        {
            GoogleCalendar.CalendarService.Instance.CleanToken();
            this.Frame.Navigate(this.GetType());
        }


        /// <summary>
        /// Settings pane is closed. Broadcast new settings.
        /// </summary>
        private void OnPopupClosed(object sender, object e)
        {
            _settingsPopup.Closed -= OnPopupClosed;

            SettingsPane.Show();
        }

     
        public async void Authentification()
        {
            try
            {
                await GoogleCalendar.CalendarService.Instance.LoadToken();
                await GetCalendars();
            }
            catch(Exception e)
            {
            }
           
        }


        public async Task GetCalendars()
        {
            GoogleCalendar.CalendarControler controler = new GoogleCalendar.CalendarControler();

            List<CalendarListEntry> calendars = await controler.GetAllCalendars();

            CalendarViewModel defaultCal = new CalendarViewModel(calendars.Last());
                this._calendars.Add(defaultCal);

                icalendar = new WeekBorderCalendar(_calendars, ref _dateMain);

              contenuPage.Content = icalendar;
        }

        /// <summary>
        /// Invoqué lorsque cette page est sur le point d'être affichée dans un frame.
        /// </summary>
        /// <param name="e">Données d'événement décrivant la manière dont l'utilisateur a accédé à cette page. La propriété Parameter
        /// est généralement utilisée pour configurer la page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void bout_ChangerCalendrier_click(object sender, RoutedEventArgs e)
        {
            try
            {

                Popup pop = new Popup()
                {
                   
                    Child = new CalendarsList(this),
                    VerticalOffset = (Window.Current.Bounds.Height - 200) / 2,
                    HorizontalOffset = (Window.Current.Bounds.Width - 1000) / 2,
                    Visibility = Windows.UI.Xaml.Visibility.Visible
                };

                pop.IsLightDismissEnabled = true;
                pop.IsOpen = true;
            }
            catch (Exception e4)
            {
                //httprequest exception
                var result = new MessageDialog("Cette fonctionnalité nécessite une connexion à un compte google !");
                result.ShowAsync();
            }
          
           

        }

        private void bout_back_click(object sender, RoutedEventArgs e)
        {
            try
            {
                icalendar.PrevWeek();
            }
            catch (Exception e2)
            {
            }

          }

        private void bout_next_click(object sender, RoutedEventArgs e)
        {
           try
            {
            icalendar.NextWeek();
            }
           catch (Exception e3)
           {
           }
        }

        private void day_click(object sender, RoutedEventArgs e)
        {
            if (_dateMain.DayOfWeek != DayOfWeek.Monday)
            {
              
                while (_dateMain.DayOfWeek != DayOfWeek.Monday)
                    _dateMain = _dateMain.AddDays(-1);
                
            }

            icalendar = new DayBorderCalendar(_calendars, _dateMain);

            contenuPage.Content = icalendar;

        }

        private void week_click(object sender, RoutedEventArgs e)
        {

            if (_dateMain.DayOfWeek != DayOfWeek.Monday)
            {

                while (_dateMain.DayOfWeek != DayOfWeek.Monday)
                    _dateMain = _dateMain.AddDays(-1);

            }

            icalendar = new WeekBorderCalendar(_calendars, ref _dateMain);

            contenuPage.Content = icalendar;

        }






    }
}
