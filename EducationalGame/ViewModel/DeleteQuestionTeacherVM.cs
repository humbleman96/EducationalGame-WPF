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
    public class DeleteQuestionTeacherVM : ViewModelBase
    {
        #region Definitions
        private string question;
        private string answer;
        private List<string> quizCategories;
        private string selectedQuizCategory;
        private List<GameQuiz> gameQuizesFromSelectedCategory;
        private GameQuiz selectedQuestion;
        private bool isProgressVisible;
        private Teacher teacher;


        #endregion
        #region Properties
        public string Question { get { return question; } set { question = value; RaisePropertyChanged("Question"); } }
        public string Answer { get { return answer; } set { answer = value; RaisePropertyChanged("Answer"); } }
        public List<string> QuizCategories { get { return quizCategories; } set { quizCategories = value; RaisePropertyChanged("QuizCategories"); } }
        public string SelectedQuizCategory { get { return selectedQuizCategory; } set { selectedQuizCategory = value; RaisePropertyChanged("SelectedQuizCategory"); Task vis = VisualizeQuestions(); } }
        public List<GameQuiz> GameQuizesFromSelectedCategory { get { return gameQuizesFromSelectedCategory; } set { gameQuizesFromSelectedCategory = value; RaisePropertyChanged("GameQuizesFromSelectedCategory"); } }
        public bool IsProgressVisible { get { return isProgressVisible; } set { isProgressVisible = value; RaisePropertyChanged("IsProgressVisible"); } }
        public Teacher Teacher { get { return teacher; } set { teacher = value; } }

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
        public ICommand Delete { get; set; }
        public ICommand Back { get; set; }
        #endregion
        #region Constructors
        public DeleteQuestionTeacherVM()
        {        
            GameQuizesFromSelectedCategory = new List<GameQuiz>();
            Task populate = PopulateQuizCategories();
 
            Delete = new RelayCommand(() => DeleteQuestion());
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

        private void DeleteQuestion()
        {
            using (EducationalGameContext context = new EducationalGameContext())
            {
                lock (context)
                {
                    if (SelectedQuestion != null)
                    {
                        GameQuiz questionToDelete = (from delete in context.GameQuizes
                                                     where delete.Question.Equals(SelectedQuestion.Question) &&
                                                     delete.Answer.Equals(SelectedQuestion.Answer)
                                                     select delete).FirstOrDefault();
                        if (questionToDelete != null)
                        {
                            context.GameQuizes.Remove(questionToDelete);
                            context.SaveChanges();
                            MessageBox.Show("Успешно изтриване на въпрос", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        else
                        {
                            MessageBox.Show("Въпросът вече е изтрит! Презаредете дадената категория, за да видите промените!", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                    }

                    else
                    {
                        MessageBox.Show("Не сте избрали въпрос за изтриване!", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
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

       
        #endregion
    }
}
