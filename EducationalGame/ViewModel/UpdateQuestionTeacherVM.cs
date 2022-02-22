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
    public  class UpdateQuestionTeacherVM : ViewModelBase
    {
        #region Definitions
        private string question;
        private string answer;
        private List<string> quizCategories;
        private string selectedQuizCategory;
        private List<GameQuiz> gameQuizesFromSelectedCategory;
        private GameQuiz selectedQuestion;
        private Visibility wrongAnswer;
        private Visibility wrongQuestion;
        private bool isProgressVisible;
        private Teacher teacher;

        #endregion
        #region Properties
        public string Question { get { return question; } set { question = value; RaisePropertyChanged("Question"); } }
        public string Answer { get { return answer; } set { answer = value; RaisePropertyChanged("Answer"); } }
        public List<string> QuizCategories { get { return quizCategories; } set { quizCategories = value; RaisePropertyChanged("QuizCategories"); } }
        public string SelectedQuizCategory { get { return selectedQuizCategory; } set { selectedQuizCategory = value; RaisePropertyChanged("SelectedQuizCategory"); Task vis = VisualizeQuestions(); } }
        public List<GameQuiz> GameQuizesFromSelectedCategory { get { return gameQuizesFromSelectedCategory; } set {gameQuizesFromSelectedCategory = value; RaisePropertyChanged("GameQuizesFromSelectedCategory"); } }
        public Visibility WrongAnswer { get { return wrongAnswer; } set { wrongAnswer = value; RaisePropertyChanged("WrongAnswer"); } }
        public Visibility WrongQuestion { get { return wrongQuestion; } set { wrongQuestion = value; RaisePropertyChanged("WrongQuestion"); } }
        public bool IsProgressVisible { get { return isProgressVisible; } set { isProgressVisible = value; RaisePropertyChanged("IsProgressVisible"); } }
        public GameQuiz SelectedQuestion
        {
            get
            {
                return selectedQuestion;
            }

            set
            {             
                selectedQuestion = value;
                RaisePropertyChanged("SelectedQuestion");
                Question = selectedQuestion.Question;
                Answer = selectedQuestion.Answer;               
            }
        }

        public Teacher Teacher { get { return teacher; } set { teacher = value; } }

        public ICommand Update { get; set; }
        public ICommand Back { get; set; }
        #endregion
        #region Constructors
        public UpdateQuestionTeacherVM()
        {
            GameQuizesFromSelectedCategory = new List<GameQuiz>();
            WrongAnswer = Visibility.Hidden;
            WrongQuestion = Visibility.Hidden;
            Task populate = PopulateQuizCategories();

            Update = new RelayCommand(() => UpdateQuestion());
            Back = new RelayCommand(() => BackToTeachersMenu());
        }
        #endregion
        #region Methods

        private void BackToTeachersMenu()
        {
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            TeachersMenuVM smvm = ServiceLocator.Current.GetInstance<TeachersMenuVM>();
            mvm.WindowHeight = 240;
            mvm.WindowWidth = 440;

            mvm.CurrentViewModel = smvm;
        }

        private void UpdateQuestion()
        {
            
            using (EducationalGameContext context = new EducationalGameContext())
            {
                lock (context)
                {
                    bool isAnswerInCorrectFormat = Validator.ValidateAnswer(Answer);
                    bool isQuestionCorrect = Validator.ValidateQuestion(Question);

                    if (isAnswerInCorrectFormat && isQuestionCorrect)
                    {
                        WrongAnswer = Visibility.Hidden;
                        WrongQuestion = Visibility.Hidden;

                        if (SelectedQuestion != null)
                        {
                            GameQuiz questionToUpdate = (from update in context.GameQuizes
                                                         where update.Question.Equals(SelectedQuestion.Question) &&
                                                         update.Answer.Equals(SelectedQuestion.Answer)
                                                         select update).FirstOrDefault();
                            if (questionToUpdate != null)
                            {
                                questionToUpdate.Question = Question;
                                questionToUpdate.Answer = Answer;
                                context.SaveChanges();
                                
                                MessageBox.Show("Успешно редактиране на въпрос!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                                ClearControls();
                            }

                            else
                            {
                                MessageBox.Show("Вече е редактиран този въпрос! Презаредете дадената категория, за да видите промените!", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
                                ClearControls();
                            }
                        }

                        else
                        {
                            MessageBox.Show("Не сте избрали въпрос за редактиране!", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                isProgressVisible = true;
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
            Answer = string.Empty;
            Question = string.Empty;
            WrongQuestion = Visibility.Hidden;
            WrongAnswer = Visibility.Hidden;        
        }
        #endregion
    }
}
