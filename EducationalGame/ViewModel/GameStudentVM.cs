using CommonServiceLocator;
using EducationalGame.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;
using Color = System.Drawing.Color;
using Image = System.Windows.Controls.Image;

namespace EducationalGame.ViewModel
{
    public class GameStudentVM : ViewModelBase
    {
        #region Definitions
        private Student student;
        private ulong coinBlockTime;
        private ulong time;
        private string timeLeft;
        private DispatcherTimer timer;
        private DispatcherTimer coinTimer;
        private int questionCounter;
        private int currentIndex;
        private uint coins;
        private string question;
        private string answer;
        private string dragon;
        private string clue;
        private string correctWrongImage;
        private string[] clues;
        private string[] dragons;
        private string[] dragonsPerQuestion;
        private string[] correctWrongImages;
        private string[] correctWrongPerQuestion;
        private bool isPreviousEnabled;
        private bool isNextEnabled;
        private Visibility grid1;
        private Visibility grid2;
        private Visibility grid3;
        private Visibility grid4;
        private Visibility grid5;
        private Visibility grid6;
        private Visibility grid7;
        private Visibility grid8;
        private Visibility grid9;
        private Visibility grid10;

        private bool isGrid1Enabled;
        private bool isGrid2Enabled;
        private bool isGrid3Enabled;
        private bool isGrid4Enabled;
        private bool isGrid5Enabled;
        private bool isGrid6Enabled;
        private bool isGrid7Enabled;
        private bool isGrid8Enabled;
        private bool isGrid9Enabled;
        private bool isGrid10Enabled;
        private bool isClueEnabled;

        private List<GameQuiz> randomQuestions;
        private string[] answersGiven;
        private int[] strikes;
        private uint rightAnswersCounter;
        private int[] cluesPerQuestion;
        private bool[] isClueEnabledPerQuestion;
        private bool isCalculatePointsPressed;
        private bool isCalculateEnabled;
        private Visibility isBackVisible;

        private bool isProgressVisible;

        private string fireworks;
        private string[] fireworksPerQuestion;

        
       

        #endregion
        #region Properties
        public string TimeLeft { get { return timeLeft; } set { timeLeft = value; RaisePropertyChanged("TimeLeft"); } }
        public DispatcherTimer Timer { get; set; }
        public DispatcherTimer CoinTimer { get; set; }
        public int QuestionCounter { get { return questionCounter; } set { questionCounter = value; RaisePropertyChanged("QuestionCounter"); } }
        public uint Coins { get { return coins; } set { coins = value; RaisePropertyChanged("Coins"); } }
        public string Question { get { return question; } set { question = value; RaisePropertyChanged("Question"); } }
        public string Answer { get { return answer; } set { answer = value; RaisePropertyChanged("Answer"); } }
        public string Dragon { get { return dragon; } set { dragon = value; RaisePropertyChanged("Dragon"); } }
        public string CorrectWrongImage { get { return correctWrongImage; } set { correctWrongImage = value; RaisePropertyChanged("CorrectWrongImage"); } }

        public string Clue { get { return clue; } set { clue = value; RaisePropertyChanged("Clue"); } }

        public double StudentGrade { get; set; }

        public bool IsPreviousEnabled { get { return isPreviousEnabled; } set { isPreviousEnabled = value; RaisePropertyChanged("IsPreviousEnabled"); } }
        public bool IsNextEnabled { get { return isNextEnabled; } set { isNextEnabled = value; RaisePropertyChanged("IsNextEnabled"); } }
        public bool IsCalculateEnabled { get { return isCalculateEnabled; } set { isCalculateEnabled = value; RaisePropertyChanged("IsCalculateEnabled"); } }

        public Student Student { get { return student; } set { student = value; } }
        public Visibility Grid1
        {
            get
            {
                return grid1;
            }

            set
            {
                grid1 = value;
                RaisePropertyChanged("Grid1");
            }
        }

        public Visibility Grid2 { get { return grid2; } set { grid2 = value; RaisePropertyChanged("Grid2"); } }
        public Visibility Grid3 { get { return grid3; } set { grid3 = value; RaisePropertyChanged("Grid3"); } }
        public Visibility Grid4 { get { return grid4; } set { grid4 = value; RaisePropertyChanged("Grid4"); } }
        public Visibility Grid5 { get { return grid5; } set { grid5 = value; RaisePropertyChanged("Grid5"); } }
        public Visibility Grid6 { get { return grid6; } set { grid6 = value; RaisePropertyChanged("Grid6"); } }
        public Visibility Grid7 { get { return grid7; } set { grid7 = value; RaisePropertyChanged("Grid7"); } }
        public Visibility Grid8 { get { return grid8; } set { grid8 = value; RaisePropertyChanged("Grid8"); } }
        public Visibility Grid9 { get { return grid9; } set { grid9 = value; RaisePropertyChanged("Grid9"); } }
        public Visibility Grid10 { get { return grid10; } set { grid10 = value; RaisePropertyChanged("Grid10"); } }

        public bool IsGrid1Enabled { get { return isGrid1Enabled; } set { isGrid1Enabled = value; RaisePropertyChanged("IsGrid1Enabled"); } }
        public bool IsGrid2Enabled { get { return isGrid2Enabled; } set { isGrid2Enabled = value; RaisePropertyChanged("IsGrid2Enabled"); } }
        public bool IsGrid3Enabled { get { return isGrid3Enabled; } set { isGrid3Enabled = value; RaisePropertyChanged("IsGrid3Enabled"); } }
        public bool IsGrid4Enabled { get { return isGrid4Enabled; } set { isGrid4Enabled = value; RaisePropertyChanged("IsGrid4Enabled"); } }
        public bool IsGrid5Enabled { get { return isGrid5Enabled; } set { isGrid5Enabled = value; RaisePropertyChanged("IsGrid5Enabled"); } }
        public bool IsGrid6Enabled { get { return isGrid6Enabled; } set { isGrid6Enabled = value; RaisePropertyChanged("IsGrid6Enabled"); } }
        public bool IsGrid7Enabled { get { return isGrid7Enabled; } set { isGrid7Enabled = value; RaisePropertyChanged("IsGrid7Enabled"); } }
        public bool IsGrid8Enabled { get { return isGrid8Enabled; } set { isGrid8Enabled = value; RaisePropertyChanged("IsGrid8Enabled"); } }
        public bool IsGrid9Enabled { get { return isGrid9Enabled; } set { isGrid9Enabled = value; RaisePropertyChanged("IsGrid9Enabled"); } }
        public bool IsGrid10Enabled { get { return isGrid10Enabled; } set { isGrid10Enabled = value; RaisePropertyChanged("IsGrid10Enabled"); } }
        public bool IsClueEnabled { get { return isClueEnabled; } set { isClueEnabled = value; RaisePropertyChanged("IsClueEnabled"); } }

        public List<GameQuiz> RandomQuestions { get { return randomQuestions; } set { randomQuestions = value; } }
        public string[] AnswersGiven { get { return answersGiven; } set { answersGiven = value; } }
        public bool IsProgressVisible { get { return isProgressVisible; } set { isProgressVisible = value; RaisePropertyChanged("IsProgressVisible"); } }
        public Visibility IsBackVisible { get { return isBackVisible; } set { isBackVisible = value; RaisePropertyChanged("IsBackVisible"); } }

        public string Fireworks { get { return fireworks; } set { fireworks = value; RaisePropertyChanged("Fireworks"); } }        

        public ICommand Key { get; set; }
        public ICommand Previous { get; set; }
        public ICommand Next { get; set; }
        public ICommand Grade { get; set; }
        public ICommand RandomLetter { get; set; }
        public ICommand Back { get; set; }
        #endregion
        #region Constructors
        
        public GameStudentVM()
        {         
            IsBackVisible = Visibility.Hidden;
            IsCalculateEnabled = true;
            isCalculatePointsPressed = false;
            IsClueEnabled = true;
            rightAnswersCounter = 0;
            AddPlayingField();
            EnablePlayingFields();

            AddCorrectFireworks();

            AddStrikes();
            AddCluesPerQuestion();
            AddIsClueEnabledPerQuestion();
            AddCorrectWrongPerQuestion();
            AddDragonsPerQuestion();
           

            currentIndex = 0;

            AddClue();
            AddDragonError();
            AddCorrectWrongImages();

            IsNextEnabled = true;
            IsPreviousEnabled = false;

            time = 2401;
            coinBlockTime = 999999999;
            Timer = new DispatcherTimer();
            CoinTimer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            CoinTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            Timer.Tick += Timer_Tick;
            CoinTimer.Tick += ClueBoxEffect;
            Timer.Start();
            CoinTimer.Start();

            QuestionCounter = 1;

            Key = new RelayCommand<object>(KeyboardInput);
            Grade = new RelayCommand(() => CalculatePoints());
            Previous = new RelayCommand(() => PreviousQuestion());
            Next = new RelayCommand(() => NextQuestion());
            RandomLetter = new RelayCommand(() => GetRandomLetter());
            Back = new RelayCommand(() => BackToStudentsMenu());
        }



        #endregion
        #region Methods
        private void BackToStudentsMenu()
        {
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            StudentsMenuVM smvm = ServiceLocator.Current.GetInstance<StudentsMenuVM>();
            smvm.Student = Student;
            mvm.WindowHeight = 390;
            mvm.WindowWidth = 440;
            mvm.CurrentViewModel = smvm;
        }

        private void KeyboardInput(object sender)
        {       
            Button pressedButton = (Button) sender;
            char letterToCheck = Convert.ToChar(pressedButton.Content);
            bool guessedRight = false;

            int sizeOfCurrentWord = RandomQuestions[currentIndex].Answer.Length;
            char[] guessingWord = AnswersGiven[currentIndex].ToCharArray();

            for (int i = 0; i < sizeOfCurrentWord; i++)
            {
                char rightLetter = RandomQuestions[currentIndex].Answer[i];

                if (rightLetter.Equals(letterToCheck))
                {
                    pressedButton.IsEnabled = false;
                    pressedButton.Foreground = new SolidColorBrush(Colors.Green);
                    guessingWord[i * 2] = letterToCheck;
                    guessedRight = true;
                }
            }

            int strikesPerQuestion = strikes[currentIndex];

            if (guessedRight == false)
            {
                pressedButton.IsEnabled = false;
                pressedButton.Foreground = new SolidColorBrush(Colors.Red);
                strikesPerQuestion++;
                strikes[currentIndex] = strikesPerQuestion;
                GetDragon(strikesPerQuestion);
            }

            AnswersGiven[currentIndex] = string.Empty;

            foreach (char ch in guessingWord)
            {
                AnswersGiven[currentIndex] += ch;
            }

            Answer = AnswersGiven[currentIndex];

            using (EducationalGameContext context = new EducationalGameContext())
            {
                if (strikesPerQuestion == 6)
                {
                    DisableKeyboard(QuestionCounter);
                    
                    StudentGameQuiz sgq = new StudentGameQuiz();
                    sgq.StudentId = Student.Id;
                    sgq.GameQuizId = RandomQuestions[currentIndex].Id;

                    StudentGameQuiz exist = (from mix in context.StudentGameQuizes
                           where mix.GameQuizId.Equals(sgq.GameQuizId) && mix.StudentId.Equals(sgq.StudentId)
                           select mix).FirstOrDefault();

                    if(exist is null)
                    {
                        context.StudentGameQuizes.Add(sgq);
                        context.SaveChanges();
                    }
                }

                if (!Answer.Contains("_"))
                {
                    rightAnswersCounter++;

                    Coins += 10;

                    DisableKeyboard(QuestionCounter);

                    fireworksPerQuestion[currentIndex] = @"E:\8th semester things\Дипломна работа\EducationalGame\EducationalGame\Resources\correct answer gif.gif";
                    Fireworks = fireworksPerQuestion[currentIndex];
                    
                    StudentGameQuiz sgq = new StudentGameQuiz();
                    sgq.StudentId = Student.Id;
                    sgq.GameQuizId = RandomQuestions[currentIndex].Id;

                    StudentGameQuiz exist = (from mix in context.StudentGameQuizes
                                             where mix.GameQuizId.Equals(sgq.GameQuizId) && mix.StudentId.Equals(sgq.StudentId)
                                             select mix).FirstOrDefault();

                    if (exist is null)
                    {
                        context.StudentGameQuizes.Add(sgq);
                        context.SaveChanges();
                    }
                }
            }
        }

        public void DisableKeyboard(int questionCounter)
        {
            if (questionCounter == 1)
            {
                IsGrid1Enabled = false;
                isClueEnabledPerQuestion[0] = false;
                IsClueEnabled = isClueEnabledPerQuestion[0];
            }

            if (questionCounter == 2)
            {
                IsGrid2Enabled = false;
                isClueEnabledPerQuestion[1] = false;
                IsClueEnabled = isClueEnabledPerQuestion[1];
            }

            if (questionCounter == 3)
            {
                IsGrid3Enabled = false;
                isClueEnabledPerQuestion[2] = false;
                IsClueEnabled = isClueEnabledPerQuestion[2];
            }

            if (questionCounter == 4)
            {
                IsGrid4Enabled = false;
                isClueEnabledPerQuestion[3] = false;
                IsClueEnabled = isClueEnabledPerQuestion[3];
            }

            if (questionCounter == 5)
            {
                IsGrid5Enabled = false;
                isClueEnabledPerQuestion[4] = false;
                IsClueEnabled = isClueEnabledPerQuestion[4];
            }

            if (questionCounter == 6)
            {
                IsGrid6Enabled = false;
                isClueEnabledPerQuestion[5] = false;
                IsClueEnabled = isClueEnabledPerQuestion[5];
            }

            if (questionCounter == 7)
            {
                IsGrid7Enabled = false;
                isClueEnabledPerQuestion[6] = false;
                IsClueEnabled = isClueEnabledPerQuestion[6];
            }

            if (questionCounter == 8)
            {
                IsGrid8Enabled = false;
                isClueEnabledPerQuestion[7] = false;
                IsClueEnabled = isClueEnabledPerQuestion[7];
            }

            if (questionCounter == 9)
            {
                IsGrid9Enabled = false;
                isClueEnabledPerQuestion[8] = false;
                IsClueEnabled = isClueEnabledPerQuestion[8];
            }

            if (questionCounter == 10)
            {
                IsGrid10Enabled = false;
                isClueEnabledPerQuestion[9] = false;
                IsClueEnabled = isClueEnabledPerQuestion[9];
            }
        }

        private void PreviousQuestion()
        {
            IsNextEnabled = true;
            ChoosePlayingFieldToHide();
            QuestionCounter--;
            ChoosePlayingFieldToDisplay();

            if (QuestionCounter == 1)
            {
                IsPreviousEnabled = false;
            }
        }

        private void NextQuestion()
        {
            IsPreviousEnabled = true;
            ChoosePlayingFieldToHide();
            QuestionCounter++;
            ChoosePlayingFieldToDisplay();


            if (QuestionCounter == 10)
            {
                IsNextEnabled = false;
            }

        }

        private void CalculatePoints()
        {
           Task.Run(() =>
           {
                IsProgressVisible = true;
                Timer.Stop();
                CoinTimer.Stop();
                Clue = clues[1];
                for (int i = 1; i <= RandomQuestions.Count; i++)
                {
                    DisableKeyboard(i);
                }

                for (int i = 0; i < RandomQuestions.Count; i++)
                {
                    if (AnswersGiven[i].Contains("_"))
                    {
                        correctWrongPerQuestion[i] = correctWrongImages[0];
                        CorrectWrongImage = correctWrongPerQuestion[currentIndex];
                    }

                    else
                    {
                        correctWrongPerQuestion[i] = correctWrongImages[1];
                        CorrectWrongImage = correctWrongPerQuestion[currentIndex];
                    }
                }

                isCalculatePointsPressed = true;

                Answer = Solve();
                MessageBox.Show("Поздравления! Имате " + rightAnswersCounter.ToString() + "/10 верни отговора!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                using (EducationalGameContext context = new EducationalGameContext())
                {
                    lock(context)
                    {                      
                       Student = (from st in context.Students where st.UserName.Equals(Student.UserName) select st).First();
                       Student.TotalPoints += rightAnswersCounter;
                       Student.Coins = Coins;
                       context.SaveChanges();
                    }
                }

                IsCalculateEnabled = false;
                IsBackVisible = Visibility.Visible;
                IsProgressVisible = false;
            });
        }

       
        private void Timer_Tick(object sender, EventArgs e)
        {
            time--;       

            if  (time == 2400 || (time < 2350 && time >= 2340) || (time < 2290 && time >= 2280)  || (time < 2230 && time >= 2220) || 
                (time < 2170 && time >= 2160) || (time < 2110 && time >= 2100) || (time < 2050 && time >= 2040) || 
                (time < 1990 && time >= 1980) || (time < 1930 && time >= 1920) || (time < 1870 && time >= 1860) ||
                (time < 1810 && time >= 1800) || (time < 1750 && time >= 1740) || (time < 1690 && time >= 1680) ||
                (time < 1630 && time >= 1620) || (time < 1570 && time >= 1560) || (time < 1510 && time >= 1500) ||
                (time < 1450 && time >= 1440) || (time < 1390 && time >= 1380) || (time < 1330 && time >= 1320) ||
                (time < 1270 && time >= 1260) || (time < 1210 && time >= 1200) || (time < 1150 && time >= 1140) ||
                (time < 1090 && time >= 1080) || (time < 1030 && time >= 1020) || (time < 970 && time >= 960) ||
                (time < 910 && time >= 900) || (time < 850 && time >= 840) || (time < 790 && time >= 780) ||
                (time < 730 && time >= 720) || (time < 670 && time >= 660) || (time < 610 && time >= 600) ||
                (time < 550 && time >= 540) || (time < 490 && time >= 480) || (time < 430 && time >= 420) || 
                (time < 370 && time >= 360) || (time < 310 && time >= 300) || (time < 250 && time >= 240) || 
                (time < 190 && time >= 180) || (time < 130 && time >= 120) || (time < 70 && time > 60))
            {
                TimeLeft = string.Format("{0}:0{1}", time / 60, time % 60);
            }

            else
            {
                TimeLeft = string.Format("{0}:{1}", time / 60, time % 60);
            }

            if (time < 60)
            {
                TimeLeft = string.Format("{0}", time % 60);
            }
            
            if (time == 0)
            {             
                MessageBox.Show("Времето свърши !");
                CalculatePoints();
            }

        }

        private void ClueBoxEffect(object sender, EventArgs e)
        {
            coinBlockTime--;
            
            if (coinBlockTime % 2 == 0)
            {
                Clue = clues[0];
            }

            else
            {
                Clue = clues[1];
            }
        }

        private void GetRandomLetter()
        {
            if (cluesPerQuestion[currentIndex] == 0 && Coins>=20)
            {
                Coins -= 20;
                Random random = new Random();

                char[] correctAnswer = RandomQuestions[currentIndex].Answer.ToCharArray();
                int currentLength = RandomQuestions[currentIndex].Answer.Length;
                int randomIndex;
                char correctLetter;

                while (true)
                {
                    randomIndex = random.Next(currentLength);
                    correctLetter = correctAnswer[randomIndex];

                    if (!Answer.Contains(correctLetter))
                    {
                        if (!Char.IsWhiteSpace(correctLetter))
                        {
                            break;
                        }
                    }
               
                }

                Button pressedLetter = new Button();
                pressedLetter.Content = correctLetter;
                pressedLetter.IsEnabled = false;
                pressedLetter.Foreground = new SolidColorBrush(Colors.Green);
             
                KeyboardInput(pressedLetter);
 
                cluesPerQuestion[currentIndex]++;
            
            }
        }


        public void AddClue()
        {
            Task.Run(() =>
            {
                IsProgressVisible = true;

                clues = new string[2];

                clues[0] = @"E:\8th semester things\Дипломна работа\EducationalGame\EducationalGame\Resources\clue1.png";
                clues[1] = @"E:\8th semester things\Дипломна работа\EducationalGame\EducationalGame\Resources\clue2.png";
                IsProgressVisible = false;
            });

        }

        public void AddDragonError()
        {
            Task.Run(() =>
            {
                IsProgressVisible = true;

                dragons = new string[6];

                dragons[0] = @"E:\8th semester things\Дипломна работа\EducationalGame\EducationalGame\Resources\dragon1.png";
                dragons[1] = @"E:\8th semester things\Дипломна работа\EducationalGame\EducationalGame\Resources\dragon2.png";
                dragons[2] = @"E:\8th semester things\Дипломна работа\EducationalGame\EducationalGame\Resources\dragon3.png";
                dragons[3] = @"E:\8th semester things\Дипломна работа\EducationalGame\EducationalGame\Resources\dragon4.png";
                dragons[4] = @"E:\8th semester things\Дипломна работа\EducationalGame\EducationalGame\Resources\dragon5.png";
                dragons[5] = @"E:\8th semester things\Дипломна работа\EducationalGame\EducationalGame\Resources\dragon6.png";
                IsProgressVisible = false;
            });
        }

        public void AddCorrectWrongImages()
        {
            Task.Run(() =>
            {
                IsProgressVisible = true;

                correctWrongImages = new string[2];

                correctWrongImages[0] = @"E:\8th semester things\Дипломна работа\EducationalGame\EducationalGame\Resources\Wrong.png";
                correctWrongImages[1] = @"E:\8th semester things\Дипломна работа\EducationalGame\EducationalGame\Resources\Correct.png";
                IsProgressVisible = false;
            });

        }

        public void GetDragon(int strikes)
        {
            if(strikes == 1)
            {
                dragonsPerQuestion[currentIndex] = dragons[0];
                Dragon = dragonsPerQuestion[currentIndex];
            }

            if (strikes == 2)
            {
                dragonsPerQuestion[currentIndex] = dragons[1];
                Dragon = dragonsPerQuestion[currentIndex];
            }

            if (strikes == 3)
            {
                dragonsPerQuestion[currentIndex] = dragons[2];
                Dragon = dragonsPerQuestion[currentIndex];
            }

            if (strikes == 4)
            {
                dragonsPerQuestion[currentIndex] = dragons[3];
                Dragon = dragonsPerQuestion[currentIndex];
            }

            if (strikes == 5)
            {
                dragonsPerQuestion[currentIndex] = dragons[4];
                Dragon = dragonsPerQuestion[currentIndex];
            }

            if (strikes == 6)
            {
                dragonsPerQuestion[currentIndex] = dragons[5];
                Dragon = dragonsPerQuestion[currentIndex];
            }
        }

        public void AddPlayingField()
        {
            Task.Run(() =>
            {
                IsProgressVisible = true;
                Grid1 = Visibility.Visible;
                Grid2 = Visibility.Hidden;
                Grid3 = Visibility.Hidden;
                Grid4 = Visibility.Hidden;
                Grid5 = Visibility.Hidden;
                Grid6 = Visibility.Hidden;
                Grid7 = Visibility.Hidden;
                Grid8 = Visibility.Hidden;
                Grid9 = Visibility.Hidden;
                Grid10 = Visibility.Hidden;
                IsProgressVisible = false;
            });
        }

        public void EnablePlayingFields()
        {
            Task.Run(() =>
            {
                IsProgressVisible = true;
                IsGrid1Enabled = true;
                IsGrid2Enabled = true;
                IsGrid3Enabled = true;
                IsGrid4Enabled = true;
                IsGrid5Enabled = true;
                IsGrid6Enabled = true;
                IsGrid7Enabled = true;
                IsGrid8Enabled = true;
                IsGrid9Enabled = true;
                IsGrid10Enabled = true;
                IsProgressVisible = false;
            });
        }

        public void ChoosePlayingFieldToDisplay()
        {
            if (QuestionCounter == 1)
            {
                currentIndex = 0;
                Grid1 = Visibility.Visible;
                currentIndex = 0;
                Question = RandomQuestions[0].Question;
                if (isCalculatePointsPressed == false)
                {
                    Answer = AnswersGiven[currentIndex];
                }
                else
                {
                    Answer = Solve();
                }
               
                Dragon = dragonsPerQuestion[0];
                IsClueEnabled = isClueEnabledPerQuestion[currentIndex];
                CorrectWrongImage = correctWrongPerQuestion[currentIndex];
                Fireworks = fireworksPerQuestion[currentIndex];
            }

            if (QuestionCounter == 2)
            {
                currentIndex = 1;
                Grid2 = Visibility.Visible;
                
                Question = RandomQuestions[1].Question;
                if (isCalculatePointsPressed == false)
                {
                    Answer = AnswersGiven[currentIndex];
                }
                else
                {
                    Answer = Solve();
                }

                Dragon = dragonsPerQuestion[1];
                IsClueEnabled = isClueEnabledPerQuestion[currentIndex];
                CorrectWrongImage = correctWrongPerQuestion[currentIndex];
                Fireworks = fireworksPerQuestion[currentIndex];
            }

            if (QuestionCounter == 3)
            {
                currentIndex = 2;
                Grid3 = Visibility.Visible;           
                Question = RandomQuestions[2].Question;
                if (isCalculatePointsPressed == false)
                {
                    Answer = AnswersGiven[currentIndex];
                }
                else
                {
                    Answer = Solve();
                }

                Dragon = dragonsPerQuestion[2];
                IsClueEnabled = isClueEnabledPerQuestion[currentIndex];
                CorrectWrongImage = correctWrongPerQuestion[currentIndex];
                Fireworks = fireworksPerQuestion[currentIndex];
            }

            if (QuestionCounter == 4)
            {
                currentIndex = 3;
                Grid4 = Visibility.Visible;            
                Question = RandomQuestions[3].Question;
                if (isCalculatePointsPressed == false)
                {
                    Answer = AnswersGiven[currentIndex];
                }
                else
                {
                    Answer = Solve();
                }

                Dragon = dragonsPerQuestion[3];
                IsClueEnabled = isClueEnabledPerQuestion[currentIndex];
                CorrectWrongImage = correctWrongPerQuestion[currentIndex];
                Fireworks = fireworksPerQuestion[currentIndex];
            }

            if (QuestionCounter == 5)
            {
                currentIndex = 4;
                Grid5 = Visibility.Visible;          
                Question = RandomQuestions[4].Question;
                if (isCalculatePointsPressed == false)
                {
                    Answer = AnswersGiven[currentIndex];
                }
                else
                {
                    Answer = Solve();
                }

                Dragon = dragonsPerQuestion[4];
                IsClueEnabled = isClueEnabledPerQuestion[currentIndex];
                CorrectWrongImage = correctWrongPerQuestion[currentIndex];
                Fireworks = fireworksPerQuestion[currentIndex];
            }

            if (QuestionCounter == 6)
            {
                currentIndex = 5;
                Grid6 = Visibility.Visible;           
                Question = RandomQuestions[5].Question;
                if (isCalculatePointsPressed == false)
                {
                    Answer = AnswersGiven[currentIndex];
                }
                else
                {
                    Answer = Solve();
                }

                Dragon = dragonsPerQuestion[5];
                IsClueEnabled = isClueEnabledPerQuestion[currentIndex];
                CorrectWrongImage = correctWrongPerQuestion[currentIndex];
                Fireworks = fireworksPerQuestion[currentIndex];
            }

            if (QuestionCounter == 7)
            {
                currentIndex = 6;
                Grid7 = Visibility.Visible;             
                Question = RandomQuestions[6].Question;
                if (isCalculatePointsPressed == false)
                {
                    Answer = AnswersGiven[currentIndex];
                }
                else
                {
                    Answer = Solve();
                }

                Dragon = dragonsPerQuestion[6];
                IsClueEnabled = isClueEnabledPerQuestion[currentIndex];
                CorrectWrongImage = correctWrongPerQuestion[currentIndex];
                Fireworks = fireworksPerQuestion[currentIndex];
            }

            if (QuestionCounter == 8)
            {
                currentIndex = 7;
                Grid8 = Visibility.Visible;           
                Question = RandomQuestions[7].Question;
                if (isCalculatePointsPressed == false)
                {
                    Answer = AnswersGiven[currentIndex];
                }
                else
                {
                    Answer = Solve();
                }

                Dragon = dragonsPerQuestion[7];
                IsClueEnabled = isClueEnabledPerQuestion[currentIndex];
                CorrectWrongImage = correctWrongPerQuestion[currentIndex];
                Fireworks = fireworksPerQuestion[currentIndex];
            }

            if (QuestionCounter == 9)
            {
                currentIndex = 8;
                Grid9 = Visibility.Visible;            
                Question = RandomQuestions[8].Question;
                if (isCalculatePointsPressed == false)
                {
                    Answer = AnswersGiven[currentIndex];
                }
                else
                {
                    Answer = Solve();
                }

                Dragon = dragonsPerQuestion[8];
                IsClueEnabled = isClueEnabledPerQuestion[currentIndex];
                CorrectWrongImage = correctWrongPerQuestion[currentIndex];
                Fireworks = fireworksPerQuestion[currentIndex];
            }

            if (QuestionCounter == 10)
            {
                currentIndex = 9;
                Grid10 = Visibility.Visible;             
                Question = RandomQuestions[9].Question;
                if (isCalculatePointsPressed == false)
                {
                    Answer = AnswersGiven[currentIndex];
                }

                else
                {
                    Answer = Solve();
                }

                Dragon = dragonsPerQuestion[9];
                IsClueEnabled = isClueEnabledPerQuestion[currentIndex];
                CorrectWrongImage = correctWrongPerQuestion[currentIndex];
                Fireworks = fireworksPerQuestion[currentIndex];
            }
        }

        public string Solve()
        {
            char[] letters = RandomQuestions[currentIndex].Answer.ToCharArray();
            string solved = string.Empty;

            for (int i = 0; i < letters.Length; i++)
            {
                if (Char.IsWhiteSpace(letters[i]))
                {
                    solved += "  ";
                }
                else
                {
                    solved += letters[i] + " ";
                }
            }

            return solved;
        }

        public void ChoosePlayingFieldToHide()
        {
            if(QuestionCounter == 1)
            {
                Grid1 = Visibility.Hidden;          
            }

            if (QuestionCounter == 2)
            {
                Grid2 = Visibility.Hidden;           
            }

            if (QuestionCounter == 3)
            {
                Grid3 = Visibility.Hidden;             
            }

            if (QuestionCounter == 4)
            {
                Grid4 = Visibility.Hidden;           
            }

            if (QuestionCounter == 5)
            {
                Grid5 = Visibility.Hidden;             
            }

            if (QuestionCounter == 6)
            {
                Grid6 = Visibility.Hidden;          
            }

            if (QuestionCounter == 7)
            {
                Grid7 = Visibility.Hidden;           
            }

            if (QuestionCounter == 8)
            {
                Grid8 = Visibility.Hidden;            
            }

            if (QuestionCounter == 9)
            {
                Grid9 = Visibility.Hidden;          
            }

            if (QuestionCounter == 10)
            {
                Grid10 = Visibility.Hidden;           
            }
        }


        public string GenerateGame(string answer)
        {
            char[] letters = answer.ToCharArray();
            answer = string.Empty;
            for (int i=0; i<letters.Length; i++)
            {
                if(Char.IsWhiteSpace(letters[i]))
                {
                    answer += "  ";
                }
                else
                {
                    answer += "_ ";
                }
            }

            return answer;
        }
        // ИВАН АСЕН ВТОРИ -> _ _ _ _  _ _ _ _  _ _ _ _ _

        private void AddAnswersGiven()
        {

            IsProgressVisible = true;
            AnswersGiven = new string[10];

           for (int i = 0; i < AnswersGiven.Length; i++)
           {
               AnswersGiven[i] = RandomQuestions[i].Answer;
               AnswersGiven[i] = GenerateGame(AnswersGiven[i]);
           }

            IsProgressVisible = false;
  
        }

        private void AddStrikes()
        {
            Task.Run(() =>
            {
                IsProgressVisible = true;
                strikes = new int[10];

                for (int i = 0; i < 10; i++)
                {
                    strikes[i] = 0;
                }
                IsProgressVisible = false;
            });
        }

        private void AddCluesPerQuestion()
        {
            Task.Run(() =>
            {
                IsProgressVisible = true;
                cluesPerQuestion = new int[10];

                for (int i = 0; i < 10; i++)
                {
                    cluesPerQuestion[i] = 0;
                }
                IsProgressVisible = false;
            });
        }

        private void AddIsClueEnabledPerQuestion()
        {
            Task.Run(() =>
            {
                IsProgressVisible = true;
                isClueEnabledPerQuestion = new bool[10];

                for (int i = 0; i < 10; i++)
                {
                    isClueEnabledPerQuestion[i] = true;
                }
                IsProgressVisible = false;
            });
        }

        private void AddCorrectWrongPerQuestion()
        {
            correctWrongPerQuestion = new string[10];
        }

        private void AddDragonsPerQuestion()
        {
            Task.Run(() =>
            {
                IsProgressVisible = true;
                dragonsPerQuestion = new string[10];

                for (int i = 0; i < 10; i++)
                {
                    dragonsPerQuestion[i] = string.Empty;
                }
                IsProgressVisible = false;
            });
        }

        private void AddCorrectFireworks()
        {
            Task.Run(() =>
            {
                IsProgressVisible = true;
                fireworksPerQuestion = new string[10];

                for (int i = 0; i < fireworksPerQuestion.Length; i++)
                {
                    fireworksPerQuestion[i] = null;
                }

                Fireworks = fireworksPerQuestion[0];
                IsProgressVisible = false;
            });
        }

        public void AddQuizes(string selectedCategory)
        {
           Task.Run(() =>
           {
              IsProgressVisible = true;
              using (EducationalGameContext context = new EducationalGameContext())
              {
                lock (context)
                {
                    Random random = new Random();

                    List<GameQuiz> questionsCategory = (from q in context.GameQuizes
                                                        join c in context.QuizCategories on q.QuizCategoryId equals c.Id
                                                        where c.QuizCategoryName.Equals(selectedCategory)
                                                        select q).ToList();

                    RandomQuestions = new List<GameQuiz>();
                    for (int i = 0; i < 10; i++)
                    {
                        GameQuiz questionToEnter = questionsCategory[random.Next(questionsCategory.Count)];
                        if (!RandomQuestions.Contains(questionToEnter))
                        {
                            RandomQuestions.Add(questionToEnter);
                        }

                        else
                        {
                            i--;
                        }
                    }
                }
              }

               AddAnswersGiven();

               Question = RandomQuestions[0].Question;
               Answer = AnswersGiven[0];
               Coins = Student.Coins;

               IsProgressVisible = false;
          });
        
        }

        public void ClearControls()
        {
            Task.Run(() =>
            {
                IsProgressVisible = true;
                Timer.Stop();
                CoinTimer.Stop();

                IsBackVisible = Visibility.Hidden;
                IsCalculateEnabled = true;
                isCalculatePointsPressed = false;
                IsClueEnabled = true;
                rightAnswersCounter = 0;
                AddPlayingField();
                EnablePlayingFields();

                AddCorrectFireworks();

                AddStrikes();
                AddCluesPerQuestion();
                AddIsClueEnabledPerQuestion();
                AddCorrectWrongPerQuestion();
                AddDragonsPerQuestion();

                currentIndex = 0;

                AddClue();
                AddDragonError();
                AddCorrectWrongImages();

                IsNextEnabled = true;
                IsPreviousEnabled = false;

                time = 2401;
                coinBlockTime = 999999999;

                Timer.Start();
                CoinTimer.Start();

                QuestionCounter = 1;
                Dragon = string.Empty;
                CorrectWrongImage = string.Empty;

                IsProgressVisible = false;
            });
        }

        #endregion
    }
}
               
            