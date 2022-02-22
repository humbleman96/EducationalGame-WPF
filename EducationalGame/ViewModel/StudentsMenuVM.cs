using CommonServiceLocator;
using EducationalGame.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EducationalGame.ViewModel
{
    public class StudentsMenuVM : ViewModelBase
    {
        #region Definitions
        private List<string> quizCategories;
        private string selectedQuizCategory;
        private bool isProgressVisible;     
        private List<GameQuiz> randomQuestions;
        private bool isPlayEnabled;
        private Student student;
        #endregion
        #region Properties
        public List<string> QuizCategories { get { return quizCategories; } set { quizCategories = value; RaisePropertyChanged("QuizCategories"); } }
        public string SelectedQuizCategory { get { return selectedQuizCategory; } set { selectedQuizCategory = value; RaisePropertyChanged("SelectedQuizCategory"); } }
        public bool IsProgressVisible { get { return isProgressVisible; } set { isProgressVisible = value; RaisePropertyChanged("IsProgressVisible"); } }
        public List<GameQuiz> RandomQuestions { get { return randomQuestions; } set { randomQuestions = value; } }
        public bool IsPlayEnabled { get { return isPlayEnabled; } set { isPlayEnabled = value; RaisePropertyChanged("IsPlayEnabled"); } }
        public Student Student { get { return student; } set { student = value; } }
        public ICommand PlayGame { get; set; }
        public ICommand GameRules { get; set; }
        public ICommand LogOut { get; set; }
        #endregion
        #region Constructors
        public StudentsMenuVM()
        {
            IsPlayEnabled = false;
            Task populate = PopulateQuizCategories();

            PlayGame = new RelayCommand(() => GenerateGame());
            GameRules = new RelayCommand(() => ShowRules());
            LogOut = new RelayCommand(() => BackToLogin());
        }
        #endregion
        #region Methods
        private void GenerateGame()
        {
            IsProgressVisible = true;          
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            GameStudentVM gsvm = ServiceLocator.Current.GetInstance<GameStudentVM>();
            gsvm.Student = Student;
            gsvm.ClearControls();
            gsvm.AddQuizes(SelectedQuizCategory);

            mvm.WindowHeight = 670;
            mvm.WindowWidth = 860;
            
            mvm.CurrentViewModel = gsvm;
            IsProgressVisible = false;     
        }

        private void ShowRules()
        {       
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            GameRulesVM grvm = ServiceLocator.Current.GetInstance<GameRulesVM>();
            grvm.Student = Student;
            mvm.WindowHeight = 840;
            mvm.WindowWidth = 1040;
            mvm.CurrentViewModel = grvm;
        }

        private void BackToLogin()
        {
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            LoginVM lvm = ServiceLocator.Current.GetInstance<LoginVM>();
            mvm.WindowHeight = 440;
            mvm.WindowWidth = 440;
            mvm.CurrentViewModel = lvm;
        }

        public async Task PopulateQuizCategories()
        {
            await Task.Run(() =>
            {
                IsProgressVisible = true;
                using (EducationalGameContext context = new EducationalGameContext())
                {
                    lock (context)
                    {
                        QuizCategories = new List<string>();
                        QuizCategories = (from category in context.QuizCategories select category.QuizCategoryName).ToList();
                    }
                }
                IsProgressVisible = false;
                IsPlayEnabled = true;
            });

        }

      

        #endregion
    }
}
