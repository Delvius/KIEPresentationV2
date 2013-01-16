using KIEPresentation.UserControls.VIEW;
using KIEPresentation.UserControls.VIEWMODEL.Config.Calendars;
using KIEPresentation.UserControls.VIEWMODEL.Event;
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


// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace KIEPresentation.UserControls.Config.Calendars
{
    public sealed partial class ModifCalendarGoogle : UserControl
    {
        private VIEWMODEL.CalendarViewModel calendarViewModel;

        public ModifCalendarGoogle(VIEWMODEL.CalendarViewModel calendarViewModel, CalendarsListConfigViewModel ListeCalVM)
        {
            this.InitializeComponent();
            this.calendarViewModel = calendarViewModel;
            this.DataContext = new CalendarGoogleViewModel(calendarViewModel, ListeCalVM);
        }
    }
}
