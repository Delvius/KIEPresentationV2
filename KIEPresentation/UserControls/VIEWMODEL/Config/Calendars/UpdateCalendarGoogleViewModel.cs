using GoogleCalendar;
using GoogleCalendar.Data;
using KIEPresentation.Service;
using KIEPresentation.UserControls.Config.Calendars;
using KIEPresentation.UserControls.VIEW.Calendars;
using KIEPresentation.UserControls.VIEW.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace KIEPresentation.UserControls.VIEWMODEL.Config.Calendars
{
    class UpdateCalendarGoogleViewModel : ViewModelBase
    {
        private CalendarViewModel cal;
        private RelayCommand _supprimerCommand;
        private RelayCommand _editerCommand;

        private CalendarsListConfigViewModel parent;

        public UpdateCalendarGoogleViewModel(CalendarViewModel cal, CalendarsListConfigViewModel ListecalVM)
        {
            this.parent = ListecalVM;
            this.cal = cal;
        }


        public string Summary
        {
            get
            {
                if (cal.Calendar.Summary != null)
                    return cal.Calendar.Summary;
                return "";
            }
            set
            {
                    cal.Calendar.Summary = value;
                    _editerCommand.UpdateCanExecute();
                RaisePropertyChanged("Summary");
                    
            }

        }

        public string Description
        {
            get
            {
                    return cal.Calendar.Description;
            }
             set
            {
                    cal.Calendar.Description = value;
                    RaisePropertyChanged("Description");
            }

        }

        public string Location
        {
            get
            {
                if (cal.Calendar.Location != null)
                    return cal.Calendar.Location;
                return "";
            }
            set
            {
                   cal.Calendar.Location = value;
                   RaisePropertyChanged("Location");
           }
        }

        #region Initialisation du nom des champs

        private String _field_Summary = IConstantes.SUMMARY;
        private String _field_Location = IConstantes.LOCATION;
        private String _field_Country = IConstantes.COUNTRY;
        private String _field_TimeZone = IConstantes.TIMEZONE;
        private String _field_Description = IConstantes.DESCRIPTION;

        public String Field_Summary
        {
            get { return _field_Summary; }
        }

        public String Field_Location
        {
            get { return _field_Location; }
        }

        public String Field_TimeZone
        {
            get { return _field_TimeZone; }
        }
        public String Field_Country
        {
            get { return _field_Country; }
        }

        public String Field_Description
        {
            get { return _field_Description; }
        }
        #endregion

        #region BOUTONS
        /// <summary>
        /// Commande liée au bouton Accueil
        /// </summary>
        public ICommand AnnulerCommand
        {
            get
            {
                this._supprimerCommand = new RelayCommand(() => this.ExecuteAnnuler(),
                                                         () => true);
                return this._supprimerCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Accueil
        /// </summary>
        public void ExecuteAnnuler()
        {
            parent.SelectedWindow = new ConsultCalendarGoogle(cal, parent);
        }


        /// <summary>
        /// Commande liée au bouton Accueil
        /// </summary>
        public ICommand ModifierCommand
        {
            get
            {
                this._editerCommand = new RelayCommand(() => this.ExecuteModifier(),
                                                         () => this.CanAdd());
                return this._editerCommand;
            }
        }

        private bool CanAdd()
        {
            return (!Summary.Equals(""));
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Accueil
        /// </summary>
        public async void ExecuteModifier()
        {
            try{

             // Create the message dialog and set its content and title
                var messageDialog = new MessageDialog("Êtes-vous certain de vouloir modifier le calendrier selectionné ?", "Demande de confirmation");

                // Add commands and set their command ids
                messageDialog.Commands.Add(new UICommand("Annuler", null, 0));
                messageDialog.Commands.Add(new UICommand("Confirmer", null, 1));

                // Set the command that will be invoked by default
                messageDialog.DefaultCommandIndex = 0;

                // Show the message dialog and get the event that was invoked via the async operator
                var commandChosen = await messageDialog.ShowAsync();

                if (commandChosen.Label.Equals("Confirmer"))
                {
                    CalendarControler controler = new CalendarControler();
                    await controler.UpdateCalendar(cal.Calendar);
                   
                   
                   //on met à jour la liste des calendriers
                    parent.MajCalendar(cal.Calendar);

                }
            }
            catch (HttpRequestException e)
            {
                var messageDialog = new MessageDialog(e.Message+"Impossible de modifier le calendrier. Vous n'êtes plus connecté à votre compte.", "Suppression impossible");
                messageDialog.ShowAsync();
            }
        }

        #endregion




    }
}
