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
    class CalendarGoogleViewModel : ViewModelBase
    {
        private CalendarViewModel cal;

        private CalendarsListConfigViewModel parent;
     

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="ListecalVM"> Le CalendarListConfigViewModel</param>
        public CalendarGoogleViewModel(CalendarsListConfigViewModel ListecalVM)
        {
            this.parent = ListecalVM;
            this.cal = new CalendarViewModel(new CalendarListEntry());
        }

        /// <summary>
        /// Constructeur pour la Consultation et la Modification
        /// </summary>
        /// <param name="cal">Le calendarViewModel à gérer</param>
        /// <param name="ListecalVM">Le CalendarListConfigViewModel</param>
        public CalendarGoogleViewModel(CalendarViewModel cal, CalendarsListConfigViewModel ListecalVM)
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
                    RaisePropertyChanged("Summary");
                    _addCommand.UpdateCanExecute();
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
        private RelayCommand _deleteCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _editCommand;
        private RelayCommand _addCommand;
        
        /// <summary>
        /// Commande liée au bouton Accueil
        /// </summary>
        /// 
        public ICommand CancelCommand
        {
            get
            {
                this._cancelCommand = new RelayCommand(() => this.ExecuteCancel(),
                                                         () => true);
                return this._cancelCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Accueil
        /// </summary>
        public void ExecuteCancel()
        {
            parent.SelectedWindow = new CalendarWelcome();
        }


        /// <summary>
        /// Commande liée au bouton Accueil
        /// </summary>
        public ICommand AddCommand
        {
            get
            {
                this._addCommand = new RelayCommand(() => this.ExecuteAdd(),
                                                         () => this.CanAdd());
                return this._addCommand;
            }
        }

        private bool CanAdd()
        {
            return (!Summary.Equals(""));
        }
        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Accueil
        /// </summary>
        public async void ExecuteAdd()
        {
            try{

             // Create the message dialog and set its content and title
                var messageDialog = new MessageDialog(IConstantes.MSG_CONFIRM_ADD_CAL);

                // Add commands and set their command ids
                messageDialog.Commands.Add(new UICommand(IConstantes.BUTTON_CONFIRM, null, 0));
                messageDialog.Commands.Add(new UICommand(IConstantes.BUTTON_CANCEL, null, 1));

                // Set the command that will be invoked by default
                messageDialog.DefaultCommandIndex = 0;

                // Show the message dialog and get the event that was invoked via the async operator
                var commandChosen = await messageDialog.ShowAsync();

                if (commandChosen.Label.Equals(IConstantes.BUTTON_CONFIRM))
                {
                    CalendarControler controler = new CalendarControler();
                    await controler.InsertCalendar(cal.Calendar);

                   //on met à jour la liste des calendriers
                    parent.initCalendar();

                }
            }
            catch (HttpRequestException e)
            {
                var messageDialog = new MessageDialog(e.Message + IConstantes.EXCEPTION_MSG_ADD);
                messageDialog.ShowAsync();
            }

        }


        /// <summary>
        /// Commande liée au bouton Accueil
        /// </summary>
        public ICommand UpdateCommand
        {
            get
            {
                this._editCommand = new RelayCommand(() => this.ExecuteModifier(),
                                                         () => this.CanAdd());
                return this._editCommand;
            }
        }


        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Accueil
        /// </summary>
        public async void ExecuteModifier()
        {
            try
            {

                // Create the message dialog and set its content and title
                var messageDialog = new MessageDialog(IConstantes.MSG_CONFIRM_UPDATE_CAL);

                // Add commands and set their command ids
                messageDialog.Commands.Add(new UICommand(IConstantes.BUTTON_CANCEL, null, 0));
                messageDialog.Commands.Add(new UICommand(IConstantes.BUTTON_CONFIRM, null, 1));

                // Set the command that will be invoked by default
                messageDialog.DefaultCommandIndex = 0;

                // Show the message dialog and get the event that was invoked via the async operator
                var commandChosen = await messageDialog.ShowAsync();

                if (commandChosen.Label.Equals(IConstantes.BUTTON_CONFIRM))
                {
                    CalendarControler controler = new CalendarControler();
                    await controler.UpdateCalendar(cal.Calendar);


                    //on met à jour la liste des calendriers
                    parent.MajCalendar(cal.Calendar);

                }
            }
            catch (HttpRequestException e)
            {
                var messageDialog = new MessageDialog(e.Message + IConstantes.EXCEPTION_MSG_UPDATE);
                messageDialog.ShowAsync();
            }
        }


        /// <summary>
        /// Commande liée au bouton Accueil
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                this._deleteCommand = new RelayCommand(() => this.ExecuteDelete(),
                                                         () => true);
                return this._deleteCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Accueil
        /// </summary>
        public async void ExecuteDelete()
        {
            try
            {
                // Create the message dialog and set its content and title
                var messageDialog = new MessageDialog(IConstantes.MSG_CONFIRM_DELETE_CAL);

                // Add commands and set their command ids
                messageDialog.Commands.Add(new UICommand(IConstantes.BUTTON_CONFIRM, null, 0));
                messageDialog.Commands.Add(new UICommand(IConstantes.BUTTON_CANCEL, null, 1));

                // Set the command that will be invoked by default
                messageDialog.DefaultCommandIndex = 0;

                // Show the message dialog and get the event that was invoked via the async operator
                var commandChosen = await messageDialog.ShowAsync();

                if (commandChosen.Label.Equals(IConstantes.BUTTON_CONFIRM))
                {
                    CalendarControler controler = new CalendarControler();
                    controler.DeleteCalendar(cal.Calendar);

                    //on met à jour la liste des calendriers
                    parent.DeleteCalendar(cal.Calendar);
                    parent.SelectedWindow = new CalendarWelcome();
                }
            }
            catch (Exception e)
            {
                var messageDialog = new MessageDialog(IConstantes.EXCEPTION_MSG_DELETE);
                messageDialog.ShowAsync();
            }
        }


        /// <summary>
        /// Commande liée au bouton Accueil
        /// </summary>
        public ICommand EditCommand
        {
            get
            {
                this._editCommand = new RelayCommand(() => this.ExecuteEdit(),
                                                         () => true);
                return this._editCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Accueil
        /// </summary>
        public void ExecuteEdit()
        {
            parent.SelectedWindow = new ModifCalendarGoogle(this.cal, parent);

        }

        #endregion




    }
}
