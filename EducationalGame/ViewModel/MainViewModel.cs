using CommonServiceLocator;
using EducationalGame.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace EducationalGame.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Definitions

        private ViewModelBase _currentViewModel;
        private int windowWidth;
        private int windowHeight;
      
      
     //   private Visibility loginVM;
  
        #endregion
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        ///  
        #region Properties
            
        public int WindowWidth { get { return windowWidth; } set { windowWidth = value; RaisePropertyChanged("WindowWidth"); } }
        public int WindowHeight { get { return windowHeight; } set { windowHeight = value; RaisePropertyChanged("WindowHeight"); } }
      

        #endregion

        #region Constructors
        public MainViewModel()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["EducationalGameDB"].ConnectionString;
            
            LoginVM lvm = ServiceLocator.Current.GetInstance<LoginVM>();
            CurrentViewModel = lvm;

            WindowHeight = 440;
            WindowWidth = 440;
           
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"

            ////}
        }
        #endregion

        #region Methods   
        #endregion

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }

    }

}


