using KIEPresentation.Service;
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
    public sealed partial class CalendarWelcome : UserControl
    {
        public CalendarWelcome()
        {
            this.InitializeComponent();
            this.Title.Text = IConstantes.TITLE_CONFIG_CALENDARS;
            this.Advice.Text = IConstantes.ADVICE_CONFIG_WELCOME;
        }
    }
}
