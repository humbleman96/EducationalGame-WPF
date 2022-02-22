using CommonServiceLocator;
using EducationalGame.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EducationalGame.ViewModel
{
    public class PrincipalsMenuVM : ViewModelBase
    {
        #region Definitions
        private Principal principal;
        #endregion
        #region Properties
        public Principal Principal { get { return principal; } set { principal = value; } }

        public ICommand Rankings { get; set; }
        public ICommand LogOut { get; set; }
        #endregion
        #region Constructors
        public PrincipalsMenuVM()
        {
            Rankings = new RelayCommand(() => ShowRankings());
            LogOut = new RelayCommand(() => BackToLogin());
        }
        #endregion
        #region Methods
        private void ShowRankings()
        {
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            RankingsPrincipalVM rpvm = ServiceLocator.Current.GetInstance<RankingsPrincipalVM>();
            rpvm.Principal = Principal;
            Task show = rpvm.ShowRankings();
            mvm.WindowHeight = 490;
            mvm.WindowWidth = 640;
            mvm.CurrentViewModel = rpvm;
        }

        private void BackToLogin()
        {
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            LoginVM lvm = ServiceLocator.Current.GetInstance<LoginVM>();
            mvm.WindowHeight = 440;
            mvm.WindowWidth = 440;
            mvm.CurrentViewModel = lvm;

        }
        #endregion
    }
}
