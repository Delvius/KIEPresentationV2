using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace KIEPresentation.UserControls
{
    public sealed partial class WeekEvent : UserControl
    {
        /// <summary>
        /// The calEvent corresponding at the controlUser
        /// </summary>
        public GoogleCalendar.Data.Event Evt
        {
            get
            {
                return mEvent;
            }
            private set
            {
                mEvent = value;

            }

        }
        private GoogleCalendar.Data.Event mEvent;

        public WeekEvent(GoogleCalendar.Data.Event evenement)
        {
            
            InitializeComponent();
            Evt = evenement;
            if (mEvent.Summary == null)
                mEvent.Summary = "";
            this.title.Text = mEvent.Summary;
            if (!String.IsNullOrEmpty(mEvent.Start.DateTime))
                this.time.Text = DateTime.Parse(mEvent.Start.DateTime).ToString("HH:mm") + "~~" + DateTime.Parse(mEvent.End.DateTime).ToString("HH:mm");
           
        
        }

    }
}
