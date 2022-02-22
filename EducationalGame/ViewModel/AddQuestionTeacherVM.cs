using CommonServiceLocator;
using EducationalGame.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EducationalGame.ViewModel
{
    public class AddQuestionTeacherVM : ViewModelBase
    {
        #region Definitions
        private string question;
        private string answer;
        private string newAnswer;
        private string newQuestion;
        private List<string> quizCategories;
        private string selectedQuizCategory;     
        private List<GameQuiz> gameQuizesFromSelectedCategory;
        private Visibility wrongAnswer;
        private Visibility wrongQuestion;
        private bool isProgressVisible;
        private Teacher teacher;

        #endregion
        #region Properties
        public string Question { get { return question; } set { question = value; RaisePropertyChanged("Question"); }  }
        public string Answer { get { return answer; } set { answer = value; RaisePropertyChanged("Answer"); } }
        public string NewQuestion { get { return newQuestion; } set { newQuestion = value; RaisePropertyChanged("NewQuestion"); } }
        public string NewAnswer { get { return newAnswer; } set { newAnswer = value; RaisePropertyChanged("NewAnswer"); } }
        public List<string> QuizCategories { get { return quizCategories; } set { quizCategories = value; RaisePropertyChanged("QuizCategories") ; } }
        public string SelectedQuizCategory { get { return selectedQuizCategory; } set { selectedQuizCategory = value; RaisePropertyChanged("SelectedQuizCategory"); Task vis = VisualizeQuestions(); } }   
        public List<GameQuiz> GameQuizesFromSelectedCategory { get { return gameQuizesFromSelectedCategory; } set { gameQuizesFromSelectedCategory = value; RaisePropertyChanged("GameQuizesFromSelectedCategory"); } }
        public Visibility WrongAnswer { get { return wrongAnswer; } set { wrongAnswer = value; RaisePropertyChanged("WrongAnswer"); } }
        public Visibility WrongQuestion { get { return wrongQuestion; } set { wrongQuestion = value; RaisePropertyChanged("WrongQuestion");  } }
        public bool IsProgressVisible { get { return isProgressVisible; } set { isProgressVisible = value; RaisePropertyChanged("IsProgressVisible"); } }
        public Teacher Teacher { get { return teacher; } set { teacher = value; } }

        public ICommand Add { get; set; }
        public ICommand Back { get; set; }
        #endregion
        #region Constructors
        public AddQuestionTeacherVM()
        {  
            GameQuizesFromSelectedCategory = new List<GameQuiz>();

            WrongAnswer = Visibility.Hidden;
            WrongQuestion = Visibility.Hidden;
            Task populateQuizCategories = PopulateQuizCategories();

            Add = new RelayCommand(() => AddQuestion());
            Back = new RelayCommand(() => BackToTeachersMenu());
        }
        #endregion
        #region Methods

        private void BackToTeachersMenu()
        {
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            TeachersMenuVM tmvm = ServiceLocator.Current.GetInstance<TeachersMenuVM>();
            mvm.WindowHeight = 240;
            mvm.WindowWidth = 440;
            mvm.CurrentViewModel = tmvm;
        }

        private void AddQuestion()
        {        
            using (EducationalGameContext context = new EducationalGameContext())
            {
                lock (context)
                {
                    bool isAnswerInCorrectFormat = Validator.ValidateAnswer(NewAnswer);
                    bool isQuestionCorrect = Validator.ValidateQuestion(NewQuestion);

                    if (isAnswerInCorrectFormat && isQuestionCorrect)
                    {
                        WrongAnswer = Visibility.Hidden;
                        WrongQuestion = Visibility.Hidden;
                        GameQuiz questionToAdd = new GameQuiz();
                        questionToAdd.Question = NewQuestion;
                        questionToAdd.Answer = NewAnswer;

                        QuizCategory currentCategory = (from current in context.QuizCategories
                                                        where current.QuizCategoryName.Equals(SelectedQuizCategory)
                                                        select current).First();

                        GameQuiz existingQuestion = (from exist in context.GameQuizes
                                                     where exist.Question.Equals(NewQuestion) && exist.Answer.Equals(NewAnswer)
                                                     select exist).FirstOrDefault();
                        if (existingQuestion == null)
                        {
                            questionToAdd.QuizCategory = currentCategory;
                            context.GameQuizes.Add(questionToAdd);
                            context.SaveChanges();
                                                   
                            MessageBox.Show("Успешно добавяне на въпрос!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearControls();
                        }

                        else
                        {                        
                            MessageBox.Show("Въпросът вече съществува!", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
                            ClearControls();
                        }
                    }

                    else
                    {
                        if (isAnswerInCorrectFormat)
                        {
                            WrongAnswer = Visibility.Hidden;
                        }

                        else
                        {
                            WrongAnswer = Visibility.Visible;
                        }

                        if (isQuestionCorrect)
                        {
                            WrongQuestion = Visibility.Hidden;
                        }

                        else
                        {
                            WrongQuestion = Visibility.Visible;
                        }
                    }
                }
            }
           
        }

        public async Task VisualizeQuestions()
        {
            
            await Task.Run(() =>
            {
                IsProgressVisible = true;
                using (EducationalGameContext context = new EducationalGameContext())
                {
                    lock (context)
                    {

                        GameQuizesFromSelectedCategory = (from q in context.GameQuizes
                                                          join questionsCategory in context.QuizCategories on q.QuizCategoryId equals
                                                          questionsCategory.Id
                                                          where questionsCategory.QuizCategoryName.Equals(SelectedQuizCategory)
                                                          select q).ToList();
                        for (int i = 0; i < GameQuizesFromSelectedCategory.Count; i++)
                        {
                            Question = GameQuizesFromSelectedCategory[i].Question;
                            Answer = GameQuizesFromSelectedCategory[i].Answer;
                        }
                    }
                }
                IsProgressVisible = false;
            });
                   
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
            });
       
        }
    
        public void ClearControls()
        {
            NewAnswer = string.Empty;
            NewQuestion = string.Empty;
            Question = string.Empty;
            Answer = string.Empty;
            WrongAnswer = Visibility.Hidden;
            WrongQuestion = Visibility.Hidden;          
        }
        #endregion

    }
}
