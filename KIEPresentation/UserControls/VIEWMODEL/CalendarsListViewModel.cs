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
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Popups;
using System.Net.Http;
using KIEPresentation.UserControls.VIEW.Config;
using KIEPresentation.UserControls.VIEW.Calendars;

namespace KIEPresentation.UserControls
{
   public class CalendarsListViewModel : ViewModelBase
    {
        //liste de tous les calendriers
        List<CalendarListEntry> calendars;

        private List<CalendarBase> _calendriersIMG = new List<CalendarBase>();

        //liste des calendriers selectionnés
        private ObservableVector<object> _calendriersSelected;




        // Elements à stocker
        private MainPage parent;
        private StackPanel monpan;
        private CalendarsList listecal;

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
        /// <param name="listecal"> Le usercontrol ListeCalendriers</param>
        /// <param name="parent">La main page permettant de naviguer vers une autre page</param>
        public CalendarsListViewModel(StackPanel pan, CalendarsList listecal, MainPage parent)
        {
            this.listecal = listecal;
            this.parent = parent;
            monpan = pan;

            initCalendar();
        }

        /// <summary>
        /// Initialise les calendriers à afficher
        /// </summary>
        public async void initCalendar()
        {
            try
            {

                ObservableCollection<object> list = new ObservableCollection<object>();
                _calendriersSelected = list.ToObservableVector<object>();

                GoogleCalendar.CalendarControler controler = new GoogleCalendar.CalendarControler();

                // on récupère tous les calendriers
                calendars = await controler.GetAllCalendars();

                //on crée les userControls Calendriers avec leurs datacontextes
                foreach (CalendarListEntry calendar in calendars)
                {
                    this.CalendriersIMG.Add(new CalendarGoogle(new CalendarViewModel(calendar)));
                }

                //pour chaque calendrier, on définit ses paramètres et une commande au click
                foreach (CalendarBase cal in _calendriersIMG)
                {
                    cal.PointerPressed += cal_PointerPressed;
                    cal.IsHitTestVisible = true;
                    cal.Opacity = 0.5;
                    cal.Margin = new Thickness(0, 0, 15, 0);
                    monpan.Children.Add(cal);
                }

                //on met à jour le Layout
                monpan.UpdateLayout();
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
        private void cal_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            CalendarBase selection = (CalendarBase)sender;

            if (selection.Opacity != 1)
            {
                selection.Opacity = 1;

               
                        _calendriersSelected.Add(selection.cal);
             

            }
            else
            {
                selection.Opacity = 0.5;
                _calendriersSelected.Remove(selection.cal);
            }
        }


        #region boutons

        public RelayCommand _gererCommand { get; set; }
        public RelayCommand _annulerCommand { get; set; }


        /// <summary>
        /// Commande liée au bouton Annuler
        /// </summary>
        public ICommand AnnulerCommand
        {
            get
            {
                this._annulerCommand = new RelayCommand(() => this.ExecuteAnnuler(),
                                                         () => true);
                return this._annulerCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Annuler
        /// </summary>
        public void ExecuteAnnuler()
        {
            Popup pop = (Popup)listecal.Parent;
            pop.IsOpen = false;
        }


        /// <summary>
        /// Commande liée au bouton Consulter
        /// </summary>
        public ICommand ConsulterCommand
        {
            get
            {
                this._consulterCommand = new RelayCommand(() => this.ExecuteConsulter(),
                                                         () => true);
                return this._consulterCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Consulter
        /// </summary>
        public void ExecuteConsulter()
        {

            if (_calendriersSelected.Count > 0)
            {
                Popup pop = (Popup)listecal.Parent;
                pop.IsOpen = false;
                MainPage page = (MainPage)parent;
                page.icalendar.InitCalendars(_calendriersSelected);
               

            }
        }



        /// <summary>
        /// Commande liée au bouton Gerer
        /// </summary>
        public ICommand GererCommand
        {
            get
            {
                this._gererCommand = new RelayCommand(() => this.ExecuteGerer(),
                                                         () => true);
                return this._gererCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Gerer
        /// </summary>
        public void ExecuteGerer()
        {
            ConfigPanel panel = new ConfigPanel();
            Popup pop = (Popup)listecal.Parent;
            pop.IsOpen = false;
            parent.Frame.Navigate(typeof(ConfigPanel));

        }


        #endregion






        public RelayCommand _consulterCommand { get; set; }
    }
}
