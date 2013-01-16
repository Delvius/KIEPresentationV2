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
using KIEPresentation.UserControls.Config.Calendars;
using KIEPresentation.UserControls.VIEW.Config;
using KIEPresentation.UserControls.VIEWMODEL.Config.Calendars;
// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace KIEPresentation.UserControls.VIEW.Config
{
       
    public sealed partial class ConfigCalendars : UserControl
    {
        public ContentPresenter contentPage;

        public ConfigCalendars()
        {
            this.InitializeComponent();
            this.DataContext = new CalendarsListConfigViewModel(listeCal, this);
            contentPage = this.ContenuPage;

        }
    }
}
