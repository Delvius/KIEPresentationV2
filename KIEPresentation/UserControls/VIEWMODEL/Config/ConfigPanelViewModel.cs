using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KIEPresentation.UserControls.Config;
using System.ComponentModel;
using Windows.UI.Popups;
using KIEPresentation.Service;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using KIEPresentation.UserControls.VIEW.Config;


namespace KIEPresentation.UserControls.VIEWMODEL.Config
{
    class ConfigPanelViewModel : ViewModelBase
    {
        private UserControl _selectedCategory;
        private ICommand _couleursCommand;
        private ICommand _calendriersCommand;
        private ICommand _compteCommand;
        private ICommand _accueilCommand;

        /// <summary>
        /// Constructeur
        /// </summary>
        public ConfigPanelViewModel()
        {
            // Par défaut : la catégorie selectionnée est l'accueil
            SelectedCategory = new ConfigWelcome();
        }


        #region SELECTEDCATEGORY
        /// <summary>
        /// La catégorie selectionnée (USERCONTROL)
        /// </summary>
        public UserControl SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged("SelectedCategory");
            }
        }
        #endregion


        #region BOUTONS
        /// <summary>
        /// Commande liée au bouton Accueil
        /// </summary>
        public ICommand AccueilCommand
        {
            get
            {
                this._accueilCommand = new RelayCommand( () => this.ExecuteAccueil(), 
                                                         ()=>true);
                return this._accueilCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Accueil
        /// </summary>
        public void ExecuteAccueil()
        {
            SelectedCategory = new ConfigWelcome();
        }


        /// <summary>
        /// Commande liée au bouton Compte
        /// </summary>
        public ICommand CompteCommand
        {
            get
            {
                this._compteCommand = new RelayCommand(() => this.ExecuteCompte(),
                                                            () => true);
                return this._compteCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Compte
        /// </summary>
        public void ExecuteCompte()
        {
            SelectedCategory = new ConfigAccount();
        }


        /// <summary>
        /// Commande liée au bouton Calendriers
        /// </summary>
        public ICommand CalendriersCommand
        {
            get
            {
                this._calendriersCommand = new RelayCommand( () => this.ExecuteCalendriers(),
                                                             () => true);
                return this._calendriersCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Calendriers
        /// </summary>
        public void ExecuteCalendriers()
        {
            SelectedCategory = new ConfigCalendars();
        }

        /// <summary>
        /// Commande liée au bouton Couleurs
        /// </summary>
        public ICommand CouleursCommand
        {
            get
            {
                this._couleursCommand = new RelayCommand( () => this.ExecuteCouleurs(),
                                                          () => true);
                return this._couleursCommand;
            }
        }

        /// <summary>
        /// Methode à exécuter lors du clic sur la commande Couleurs
        /// </summary>
        public void ExecuteCouleurs()
        {
            SelectedCategory = new ConfigColors();
        }

        #endregion

    }
}
