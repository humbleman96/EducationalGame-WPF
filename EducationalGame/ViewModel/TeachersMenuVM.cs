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
    public class TeachersMenuVM : ViewModelBase
    {
        #region Definitions
        private Teacher teacher;
        #endregion
        #region Properties
        public Teacher Teacher { get { return teacher; } set { teacher = value; } }

        public ICommand Add { get; set; }
        public ICommand Update { get; set; }
        public ICommand Delete { get; set; }
        public ICommand LogOut { get; set; }
        #endregion
        #region Constructors
        public TeachersMenuVM()
        {
            Add = new RelayCommand(() => AddQuestion());
            Update = new RelayCommand(() => UpdateQuestion());
            Delete = new RelayCommand(() => DeleteQuestion());
            LogOut = new RelayCommand(() => BackToLogin());
        }
        #endregion
        #region Methods
        private void AddQuestion()
        {
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            AddQuestionTeacherVM aqtvm = ServiceLocator.Current.GetInstance<AddQuestionTeacherVM>();
            aqtvm.Teacher = Teacher;
            aqtvm.ClearControls();
            mvm.WindowHeight = 540;
            mvm.WindowWidth = 640;
            mvm.CurrentViewModel = aqtvm;
        }

        private void UpdateQuestion()
        {
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            UpdateQuestionTeacherVM uqtvm = ServiceLocator.Current.GetInstance<UpdateQuestionTeacherVM>();
            uqtvm.Teacher = Teacher;
            uqtvm.ClearControls();
            mvm.WindowHeight = 540;
            mvm.WindowWidth = 640;
            mvm.CurrentViewModel = uqtvm;
        }

        private void DeleteQuestion()
        {
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            DeleteQuestionTeacherVM dqtvm = ServiceLocator.Current.GetInstance<DeleteQuestionTeacherVM>();
            dqtvm.Teacher = Teacher;
            mvm.WindowHeight = 540;
            mvm.WindowWidth = 640;
            mvm.CurrentViewModel = dqtvm;
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
