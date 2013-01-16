using KIEPresentation.UserControls.VIEWMODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIEPresentation.UserControls.VIEW.CalendarsGrid
{
    public interface IBorderCalendar
    {
        /// <summary>
        /// Initialise les calendriers à afficher
        /// </summary>
        /// <param name="calendars"></param>
        void InitCalendars(ObservableVector<object> calendars);

        /// <summary>
        /// Retourne la date de la semaine courante
        /// </summary>
        /// <returns></returns>
        DateTime getDate();

        /// <summary>
        /// Charge la semaine précédente
        /// </summary>
        void PrevWeek();

        /// <summary>
        /// Charge la semaine suivante
        /// </summary>
        void NextWeek();

      

       
    }
}
