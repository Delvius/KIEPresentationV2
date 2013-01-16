using GoogleCalendar;
using KIEPresentation.Service;
using KIEPresentation.UserControls.Config.Calendars;
using KIEPresentation.UserControls.VIEW;
using KIEPresentation.UserControls.VIEW.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace KIEPresentation.UserControls.VIEWMODEL.Config.Calendars
{
    class ConsultCalendarGoogleViewModel : ViewModelBase
    {
        protected CalendarViewModel cal;
        private RelayCommand _supprimerCommand;
        private RelayCommand _editerCommand;

        private CalendarsListConfigViewModel parent;

        public ConsultCalendarGoogleViewModel(CalendarViewModel cal, CalendarsListConfigViewModel ListecalVM)
        {
            this.parent = ListecalVM;
            this.cal = cal;
        }


        public string Summary
        {
            get
            { if(cal.Calendar.Summary!=null)
                return cal.Calendar.Summary;
            return "";
            }
           
        }

        public string Description
        {
            get
            {
                if (cal.Calendar.Description != null)
                return cal.Calendar.Description;
                return "";
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
        public ICommand SupprimerCommand
        {
            get
            {
                this._supprimerCommand = new RelayCommand(() => this.ExecuteSupprimer(),
                                                         () => true);
                return this._supprimerCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Accueil
        /// </summary>
        public async void ExecuteSupprimer()
        {
            try
            {
                // Create the message dialog and set its content and title
                var messageDialog = new MessageDialog("Êtes-vous certain de vouloir supprimer le calendrier selectionné ?", "Demande de confirmation");

                // Add commands and set their command ids
                messageDialog.Commands.Add(new UICommand("Confirmer", null, 0));
                messageDialog.Commands.Add(new UICommand("Annuler", null, 1));

                // Set the command that will be invoked by default
                messageDialog.DefaultCommandIndex = 0;

                // Show the message dialog and get the event that was invoked via the async operator
                var commandChosen = await messageDialog.ShowAsync();

                if (commandChosen.Label.Equals("Confirmer"))
                {
                    CalendarControler controler = new CalendarControler();
                    controler.DeleteCalendar(cal.Calendar);

                    //on met à jour la liste des calendriers
                    parent.DeleteCalendar(cal.Calendar);
                    parent.SelectedWindow=new CalendarWelcome();
                }
            }
            catch (Exception e)
            {
                var messageDialog = new MessageDialog("Impossible de supprimer le calendrier. Vous n'êtes plus connecté à votre compte.", "Suppression impossible");
                messageDialog.ShowAsync();
            }
        }


        /// <summary>
        /// Commande liée au bouton Accueil
        /// </summary>
        public ICommand EditerCommand
        {
            get
            {
                this._editerCommand = new RelayCommand(() => this.ExecuteEditer(),
                                                         () => true);
                return this._editerCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Accueil
        /// </summary>
        public void ExecuteEditer()
        {
           parent.SelectedWindow = new ModifCalendarGoogle(this.cal,parent);

        }

        #endregion




    }
}
