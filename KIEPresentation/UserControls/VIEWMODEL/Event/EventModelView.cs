using GoogleCalendar;
using KIEPresentation.Service;
using KIEPresentation.UserControls.VIEW.Calendars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Primitives;

namespace KIEPresentation.UserControls.VIEWMODEL.Event
{
    class EventModelView : ViewModelBase
    {
        //Evenement à gérer
        protected GoogleCalendar.Data.Event _mEvent;
        
        //Controlleur de l'evenement
        private EventControler _controller;

        //Fenetre parent qui a créé le POPUP contenant le userControl
        private ICalendar _parent;


        #region Listes des dates

        //Liste des dates
        private List<int> _listdaysStart = new List<int>();
        private List<int> _listdaysEnd = new List<int>();
        private List<int> _listMonth = new List<int>();
        private List<int> _listYears = new List<int>();

        public List<int> ListdaysStart
        {
            get { return _listdaysStart; }
            set
            {
                _listdaysStart = value;
                RaisePropertyChanged("ListdaysStart");
            }
        }

        public List<int> ListdaysEnd
        {
            get { return _listdaysEnd; }
            set
            {
                _listdaysEnd = value;
                RaisePropertyChanged("ListdaysEnd");
            }
        }

        public List<int> ListMonth
        {
            get { return _listMonth; }
            set
            {
                _listMonth = value;
                RaisePropertyChanged("ListMonth");
            }
        }

        public List<int> ListYears
        {
            get { return _listYears; }
            set
            {
                _listYears = value;
                RaisePropertyChanged("ListYears");
            }
        }

        #endregion

        #region Liste des heures
        //liste des heures 
        private List<int> _listHours = new List<int>();
        private List<int> _listMinutes = new List<int>();
        
        public List<int> ListHours
        {
            get { return _listHours; }
            set { _listHours = value;
                RaisePropertyChanged("ListHours");
            }
        }

        public List<int> ListMinutes
        {
            get { return _listMinutes; }
            set { _listMinutes = value; 
                RaisePropertyChanged("ListMinutes"); }
        }
        #endregion

        #region Selected items
        //Date début selectionnée
        private int _selectedDayStart;
        private int _selectedMonthStart;
        private int _selectedYearStart;


        public int SelectedDayStart
        {
            get { return _selectedDayStart; }
            set { _selectedDayStart = value; }
        }

        public int SelectedMonthStart
        {
            get { return _selectedMonthStart; }
            set { _selectedMonthStart = value; }
        }
        public int SelectedYearStart
        {
            get { return _selectedYearStart; }
            set { _selectedYearStart = value; }
        }


        //Date fin selectionnée
        private int _selectedDayEnd;
        private int _selectedMonthEnd;
        private int _selectedYearEnd;

       
        public int SelectedDayEnd
        {
            get { return _selectedDayEnd; }
            set { _selectedDayEnd = value; }
        }
       

        public int SelectedMonthEnd
        {
            get { return _selectedMonthEnd; }
            set { _selectedMonthEnd = value; }
        }


        public int SelectedYearEnd
        {
            get { return _selectedYearEnd; }
            set { _selectedYearEnd = value; }
        }

        private int _selectedHoursStart;
        private int _selectedMinutesStart;
        private int _selectedHoursEnd;
        private int _selectedMinutesEnd;


        public int SelectedHoursStart
        {
            get { return _selectedHoursStart; }
            set { _selectedHoursStart = value; }
        }
       

        public int SelectedMinutesStart
        {
            get { return _selectedMinutesStart; }
            set { _selectedMinutesStart = value; }
        }

        public int SelectedHoursEnd
        {
            get { return _selectedHoursEnd; }
            set { _selectedHoursEnd = value; }
        }

        public int SelectedMinutesEnd
        {
            get { return _selectedMinutesEnd; }
            set { _selectedMinutesEnd = value; }
        }

        #endregion

        #region Initialisation des listes de dates
        public void initMonth()
        {
              for (int i = 1; i < 13; i++)
            {
                    ListMonth.Add(i);  
            }
        }

        public void initYear()
        {   
             DateTime t= DateTime.Parse(_mEvent.Start.DateTime);

             for (int i = t.Year - 10; i < t.Year+10; i++)
            {
                    ListYears.Add(i);
            }
        }

        public void initDaysStart()
        {
            DateTime t = DateTime.Parse(_mEvent.Start.DateTime);     
            int daysinMonth= DateTime.DaysInMonth(t.Year, t.Month);

            for (int i = 1; i <= daysinMonth; i++)
            {
                    ListdaysStart.Add(i);
            }
        }

        public void initDaysEnd()
        {
            DateTime t = DateTime.Parse(_mEvent.End.DateTime);
            int daysinMonth = DateTime.DaysInMonth(t.Year, t.Month);

            for (int i = 1; i <= daysinMonth; i++)
            {
                    ListdaysEnd.Add(i);
            }
        }

        public void initHours()
        {
            for (int i = 0; i < 24; i++)
            {
                    ListHours.Add(i);
            }
        }

        public void initMinutes()
        {
            for (int i = 0; i < 60; i++)
            {
                    ListMinutes.Add(i);      
            }
        }

        /// <summary>
        /// Initialise les dates
        /// </summary>
        public void initDates()
        {
            initYear();
            initMonth();
            initDaysStart();
            initDaysEnd();
            initHours();
            initMinutes();

            DateTime dateDeb = DateTime.Parse( _mEvent.Start.DateTime);
            DateTime dateFin = DateTime.Parse(_mEvent.End.DateTime);

            SelectedDayStart = dateDeb.Day;
            SelectedMonthStart = dateDeb.Month;
            SelectedYearStart = dateDeb.Year;
            SelectedDayEnd = dateFin.Day;
            SelectedMonthEnd = dateFin.Month;
            SelectedYearEnd = dateFin.Year;

            SelectedHoursStart = dateDeb.Hour;
            SelectedMinutesStart = dateDeb.Minute;
            SelectedHoursEnd = dateFin.Hour;
            SelectedMinutesEnd = dateFin.Minute;
        }
        #endregion

        #region Initialisation du nom des champs

        private String _field_Summary = IConstantes.SUMMARY;
        private String _field_Location = IConstantes.LOCATION;
        private String _field_StartDate = IConstantes.START_DATE;
        private String _field_EndDate = IConstantes.END_DATE;
        private String _field_StartTime = IConstantes.START_TIME;
        private String _field_EndTime = IConstantes.END_TIME;
        private String _field_Description = IConstantes.DESCRIPTION;

        public String Field_Summary
        {
            get { return _field_Summary; }
        }
       
        public String Field_Location
        {
            get { return _field_Location; }
        }

        public String Field_StartDate
        {
            get { return _field_StartDate; }
        }
       
        public String Field_EndDate
        {
            get { return _field_EndDate; }
            set { _field_EndDate = value; }
        }
        

        public String Field_StartTime
        {
            get { return _field_StartTime; }
        }
     
        public String Field_EndTime
        {
            get { return _field_EndTime; }
        }
        
        public String Field_Description
        {
            get { return _field_Description; }
        }
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="evt">L'évènement à gérer</param>
        /// <param name="control">Le controleur de l'évènement</param>
        /// <param name="parent">Fenêtre parent qui a créé le POPUP contenant le userControls</param>
        public EventModelView(GoogleCalendar.Data.Event evt, EventControler control, ICalendar parent)
        {
            this._parent = parent;
            _controller = control;
            _mEvent = evt;
            initDates();
        }




        public string Summary
        {
            get
            {
                return _mEvent.Summary;
            }
            set
            {
                 _mEvent.Summary = value;
                RaisePropertyChanged("Summary");
                if(_addCommand!=null)
                _addCommand.UpdateCanExecute();
            }
        }

        public string Description
        {
            get
            {
                return _mEvent.Description;
            }
            set
            {
                 _mEvent.Description = value;
                RaisePropertyChanged("Description");
            }
        }

        public string Location
        {
            get
            {
                return _mEvent.Location;
            }
            set
            {
                _mEvent.Location = value;
                RaisePropertyChanged("Location");
            }
        }

       

        #region BOUTONS

        private RelayCommand _cancelCommand;
        private RelayCommand _addCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _modifyCommand;

        /// <summary>
        /// Commande liée au bouton Accueil
        /// </summary>
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
           _parent.GetPop().IsOpen = false;
        }


        /// <summary>
        /// Commande liée au bouton Accueil
        /// </summary>
        public ICommand AddCommand
        {
            get
            {
                this._addCommand = new RelayCommand(
                                                         () => this.ExecuteAdd(),
                                                         () => this.CanAdd()
                                                         );
                return this._addCommand;
            }
        }
       
        private bool CanAdd()
        {
            return (!_mEvent.Summary.Equals(""));
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Accueil
        /// </summary>
        public async void ExecuteAdd()
        {
            DateTimeOffset startTime = new DateTimeOffset(new DateTime(_selectedYearStart, _selectedMonthStart, _selectedDayStart, _selectedHoursStart, _selectedMinutesStart, 0));
            _mEvent.Start.DateTime = startTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");

            DateTimeOffset endTime = new DateTimeOffset(new DateTime(_selectedYearEnd, _selectedMonthEnd, _selectedDayEnd, _selectedHoursEnd, _selectedMinutesEnd, 0));
            _mEvent.End.DateTime = endTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");

            try
            {
                await _controller.InsertEvent(_mEvent);
                _parent.InitEvent();

                _parent.GetPop().IsOpen = false;
            
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
                var messageDialog = new MessageDialog(IConstantes.MSG_CONFIRM_DELETE_EVT);

                // Add commands and set their command ids
                messageDialog.Commands.Add(new UICommand(IConstantes.BUTTON_CONFIRM, null, 0));
                messageDialog.Commands.Add(new UICommand(IConstantes.BUTTON_CANCEL, null, 1));

                // Set the command that will be invoked by default
                messageDialog.DefaultCommandIndex = 0;

                // Show the message dialog and get the event that was invoked via the async operator
                var commandChosen = await messageDialog.ShowAsync();

                if (commandChosen.Label.Equals(IConstantes.BUTTON_CONFIRM))
                {
                    _controller.DeleteEvent(_mEvent);

                    messageDialog = new MessageDialog(IConstantes.MSG_SUCCESS_DELETE);
                    await messageDialog.ShowAsync();

                    //on met à jour la liste des evenements
                     _parent.InitEvent();
                    
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
        public ICommand ModifyCommand
        {
            get
            {
                this._modifyCommand = new RelayCommand(() => this.ExecuteModify(),
                                                         () => true);
                return this._modifyCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Accueil
        /// </summary>
        public async void ExecuteModify()
        {
            try
            {
                DateTimeOffset startTime = new DateTimeOffset(new DateTime(_selectedYearStart, _selectedMonthStart, _selectedDayStart, _selectedHoursStart, _selectedMinutesStart, 0));
                _mEvent.Start.DateTime = startTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");

                DateTimeOffset endTime = new DateTimeOffset(new DateTime(_selectedYearEnd, _selectedMonthEnd, _selectedDayEnd, _selectedHoursEnd, _selectedMinutesEnd, 0));
                _mEvent.End.DateTime = endTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");

                // Create the message dialog and set its content and title
                var messageDialog = new MessageDialog(IConstantes.MSG_CONFIRM_UPDATE_EVT);

                // Add commands and set their command ids
                messageDialog.Commands.Add(new UICommand(IConstantes.BUTTON_CONFIRM, null, 0));
                messageDialog.Commands.Add(new UICommand(IConstantes.BUTTON_CANCEL, null, 1));

                // Set the command that will be invoked by default
                messageDialog.DefaultCommandIndex = 0;

                // Show the message dialog and get the event that was invoked via the async operator
                var commandChosen = await messageDialog.ShowAsync();

                if (commandChosen.Label.Equals(IConstantes.BUTTON_CONFIRM))
                {
                   await _controller.UpdateEvent(_mEvent);

                    //on met à jour la liste des evenements
                    _parent.InitEvent();

                    messageDialog = new MessageDialog(IConstantes.MSG_SUCCESS_UPDATE);
                    await messageDialog.ShowAsync();

                }
            }
            catch (Exception e)
            {
                var messageDialog = new MessageDialog(IConstantes.EXCEPTION_MSG_UPDATE);
                messageDialog.ShowAsync();
            }
        }



        #endregion


    }
}
