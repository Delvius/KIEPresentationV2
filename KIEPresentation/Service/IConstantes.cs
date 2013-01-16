using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIEPresentation.Service
{
    public abstract class IConstantes
    {
        #region Form
        public const String SUMMARY = "Summary :";
        public const String START_DATE = "Start Date :";
        public const String END_DATE = "End Date :";
        public const String START_TIME = "Start Time :";
        public const String END_TIME="End Time :";
        public const String DESCRIPTION = "Description :";
        public const String LOCATION = "Location :";
        public const String COUNTRY = "Country :";
        public const String TIMEZONE = "TimeZone :";
        #endregion

        #region Button
        //button
        public const String BUTTON_SWITCH_CALENDAR = "Change calendar";
        public const String BUTTON_DAY = "Day";
        public const String BUTTON_MONTH = "Month";
        public const String BUTTON_WEEK = "Week";
        public const String BUTTON_CONFIRM = "Confirm";
        public const String BUTTON_CANCEL = "Cancel";
        public const String BUTTON_UPDATE = "Update";
        public  const String BUTTON_DELETE = "Delete";
        public const String BUTTON_ADD = "Add";
        #endregion

        #region Success Messages
        //SUCCESS MESSAGES
        public const String MSG_SUCCESS_ADD = "Successful add";
        public const String MSG_SUCCESS_UPDATE = "Successful update";
        public const String MSG_SUCCESS_DELETE = "Successful delete";
        #endregion

        #region Confirmation messages
        //Confirmation messages Event
        public const String MSG_CONFIRM_ADD_EVT = "Are you sure you want to add this event ?";
        public const String MSG_CONFIRM_UPDATE_EVT = "Are you sure you want to update this event ?";
        public const String MSG_CONFIRM_DELETE_EVT = "Are you sure you want to delete this event ?";
      

        //Confirmation messages Calendar
        public const String MSG_CONFIRM_ADD_CAL = "Are you sure you want to add this calendar ?";
        public const String MSG_CONFIRM_UPDATE_CAL = "Are you sure you want to update this calendar ?";
        public const String MSG_CONFIRM_DELETE_CAL = "Are you sure you want to delete this calendar ?";
        #endregion

        #region Exceptions
        //Exceptions messages
        public const String EXCEPTION_MSG_ADD = "Impossible to add. Please verify your account connexion";
        public const String EXCEPTION_MSG_UPDATE = "Impossible to update. Please verify your account connexion";
        public const String EXCEPTION_MSG_DELETE = "Impossible to delete. Please verify your account connexion";
        public const String EXCEPTION_MSG_CONNECT = "Could not retrieve your informations. Please verify your account connexion";
        #endregion

        #region Titles
        //Titles Event
        public const String TITLE_ADD_EVT = "Add an event";
        public const String TITLE_UPDATE_EVT = "Update an event";

        //Titles Calendar
        public const String TITLE_ADD_CAL_GOOGLE = "Add a Google calendar";
        public const String TITLE_UPDATE_CAL_GOOGLE = "Update a Google calendar";
        public const String TITLE_CONSULT_CAL_GOOGLE = "Consult Google Calendar";

        //Titles Config
        public const String TITLE_CONFIG_WELCOME = "Welcome";
        public const String TITLE_CONFIG_CALENDARS = "Calendars Management";
        public const String TITLE_CONFIG_COLORS = "Colors Management";
        public const String TITLE_CONFIG_ACCOUNT = "User Account";
        public const String TITLE_CONFIG_CLOSE = "Close";
        #endregion

        #region Advices
        //Advices
        public const String ADVICE_CONFIG_WELCOME = "Select a calendar from the list to display its properties. You can add a new calendar by clicking on the icon 'Add' in the list.";
        public const String ADVICE_CONFIG_COLORS = "Functionality coming in next update";
        public const String ADVICE_CONFIG_ACCOUNT = "Functionality coming in next update";
        public const String ADVICE_CONFIG_CALENDARS = "Welcome to the administration panel software KIE-Calendar. From this panel, you can view information from your Google account, manage your calendar and change the colors of the application.";
        #endregion

 
    }
}
