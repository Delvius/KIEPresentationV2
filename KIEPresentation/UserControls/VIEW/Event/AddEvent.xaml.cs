using GoogleCalendar;
using KIEPresentation.Service;
using KIEPresentation.UserControls.VIEW.Calendars;
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

namespace KIEPresentation.UserControls.Config.Event
{
    public sealed partial class AddEvent : UserControl
    {
        public AddEvent(GoogleCalendar.Data.Event monEvt, EventControler controler, ICalendar parent)
        {
            this.InitializeComponent();

            this.Title.Text = IConstantes.TITLE_ADD_EVT;
            this.Button_Cancel.Content = IConstantes.BUTTON_CANCEL;
            this.Button_Add.Content = IConstantes.BUTTON_ADD;

            this.DataContext = new VIEWMODEL.Event.EventModelView(monEvt, controler,parent);
        
        }
    }
}
