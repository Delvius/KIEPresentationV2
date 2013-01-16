using KIEPresentation.UserControls.VIEWMODEL;
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

namespace KIEPresentation.UserControls.VIEW.Calendars
{
    public sealed partial class CalendarGoogle : CalendarBase
    {

        public CalendarGoogle(CalendarViewModel cal)
        {

            this.cal = cal;
            this.DataContext = cal;
            this.InitializeComponent();
        }
          
    }
}
