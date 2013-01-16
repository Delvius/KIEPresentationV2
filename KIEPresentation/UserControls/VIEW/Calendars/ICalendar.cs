using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;

namespace KIEPresentation.UserControls.VIEW.Calendars
{
    public interface ICalendar
    {
        //retourne l'élément popup
        Popup GetPop();

        //initialise les évènements de la semaine en cours
        void InitEvent();

        
    }
}
