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
using KIEPresentation.UserControls.VIEWMODEL;
using KIEPresentation.UserControls.VIEWMODEL.Config;
using KIEPresentation.Service;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace KIEPresentation.UserControls.VIEW.Config
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class ConfigPanel : Page
    {
        Object SelectedWindow;

        ConfigPanelViewModel _viewModel = new ConfigPanelViewModel();

        public ConfigPanel()
        {
            this.InitializeComponent();
            SelectedWindow = new ConfigWelcome();

            this.Welcome.Text = IConstantes.TITLE_CONFIG_WELCOME;
            this.Account.Text = IConstantes.TITLE_CONFIG_ACCOUNT;
            this.Calendars.Text = IConstantes.TITLE_CONFIG_CALENDARS;
            this.colors.Text = IConstantes.TITLE_CONFIG_COLORS;
            this.close.Text = IConstantes.TITLE_CONFIG_CLOSE;
                
            this.DataContext = new ConfigPanelViewModel();
        }

        /// <summary>
        /// Invoqué lorsque cette page est sur le point d'être affichée dans un frame.
        /// </summary>
        /// <param name="e">Données d'événement décrivant la manière dont l'utilisateur a accédé à cette page. La propriété Parameter
        /// est généralement utilisée pour configurer la page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        //tests de changements de usercontrol sauvage (MVVM à mettre en place)

        private void BoutonFermerClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }


    }
}
