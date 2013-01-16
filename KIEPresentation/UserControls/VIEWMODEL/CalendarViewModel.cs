using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleCalendar.Data;

namespace KIEPresentation.UserControls.VIEWMODEL
{
   public class CalendarViewModel : ViewModelBase
    {
        private CalendarListEntry _calendar;

        public CalendarViewModel(CalendarListEntry calendar)
        {
            _calendar = calendar;
        }

        public CalendarListEntry Calendar
        {
            get
            {
                return this._calendar;
            }
        }

        public string Summary
        {
            get
            {
                return this._calendar.Summary;
            }
            set
            {
                if (value == this._calendar.Summary)
                    return;
                this._calendar.Summary = value;
                RaisePropertyChanged("Summary");
            }
        }
    }
}
