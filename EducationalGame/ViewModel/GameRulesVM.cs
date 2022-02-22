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
   public class GameRulesVM : ViewModelBase
   {
        #region Definitions
        private Student student;
        #endregion 
        #region Properties
        public Student Student { get { return student; } set { student = value; } }

        public ICommand Back { get; set; }
        #endregion

        #region Constructors
        public GameRulesVM()
        {
            Back = new RelayCommand(() => BackToStudentsMenu());
        }
        #endregion

        #region Methods
        private void BackToStudentsMenu()
        {
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            StudentsMenuVM smvm = ServiceLocator.Current.GetInstance<StudentsMenuVM>();
            mvm.WindowHeight = 390;
            mvm.WindowWidth = 440;
            mvm.CurrentViewModel = smvm;
        }
        #endregion

    }
}
