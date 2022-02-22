using CommonServiceLocator;
using EducationalGame.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
    public class RankingsPrincipalVM : ViewModelBase
    {
        #region Definitions
        private int rank;
        private string studentNames;
        private string teacherNames;
        private uint totalPoints;
        private uint coins;
        private List<Ranking> rankInfo;
        private List<Ranking> rankInfoOrdered;
        private Principal principal;
        private bool isProgressVisible;


        #endregion

        #region Properties
        public int Rank { get { return rank; } set { rank = value; RaisePropertyChanged("Rank"); } }
        public string StudentNames { get { return studentNames; } set { studentNames = value; RaisePropertyChanged("StudentNames"); } }
        public string TeacherNames { get { return teacherNames; } set { teacherNames = value; RaisePropertyChanged("TeacherNames"); } }
        public uint TotalPoints { get { return totalPoints; } set { totalPoints = value; RaisePropertyChanged("TotalPoints"); } }
        public uint Coins { get { return coins; } set { coins = value; RaisePropertyChanged("Coins"); } }
        public List<Ranking> RankInfoOrdered { get { return rankInfoOrdered; } set { rankInfoOrdered = value; RaisePropertyChanged("RankInfoOrdered"); } }
        public Principal Principal { get { return principal; } set { principal = value; } }
        public bool IsProgressVisible { get { return isProgressVisible; } set { isProgressVisible = value; RaisePropertyChanged("IsProgressVisible"); } }
        
        public ICommand Back { get; set; }
        #endregion

        #region Constructors
        public RankingsPrincipalVM()
        {
            Rank = 1;
            RankInfoOrdered = new List<Ranking>();
            rankInfo = new List<Ranking>();

            Back = new RelayCommand(() => BackToPrincipalsMenu());
        }
        #endregion

        #region Methods

        private void BackToPrincipalsMenu()
        {
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            PrincipalsMenuVM smvm = ServiceLocator.Current.GetInstance<PrincipalsMenuVM>();
            mvm.WindowHeight = 140;
            mvm.WindowWidth = 440;
            mvm.CurrentViewModel = smvm;
        }

        public async Task ShowRankings()
        {
            await Task.Run(() =>
            {
                IsProgressVisible = true;
                rankInfo.Clear();
                RankInfoOrdered.Clear();
                Rank = 1;
                using (EducationalGameContext context = new EducationalGameContext())
                {
                    IsProgressVisible = true;
                    lock (context)
                    {
                        List<Student> students = (from st in context.Students
                                                  join tr in context.Teachers on st.TeacherId equals tr.Id
                                                  join pr in context.Principals on tr.PrincipalId equals pr.Id
                                                  where pr.City.Equals(Principal.City) && pr.SchoolName.Equals(Principal.SchoolName)
                                                  select st).ToList();

                        foreach (Student st in students)
                        {
                            Teacher teacher = (from tr in context.Teachers
                                               join stud in context.Students on tr.Id equals st.TeacherId
                                               select tr).FirstOrDefault();
                            Ranking ranking = new Ranking();
                            ranking.StudentNames = st.Name + " " + st.FamilyName;
                            ranking.TeacherNames = teacher.Name + " " + teacher.FamilyName;
                            ranking.TotalPoints = st.TotalPoints;
                            ranking.Coins = st.Coins;
                            rankInfo.Add(ranking);
                        }


                        RankInfoOrdered = rankInfo.OrderByDescending(p => p.TotalPoints).ThenByDescending(c => c.Coins).ToList();


                        for (int i = 1; i < RankInfoOrdered.Count; i++)
                        {
                            if (RankInfoOrdered[i - 1].TotalPoints.Equals(RankInfoOrdered[i].TotalPoints) &&
                                RankInfoOrdered[i - 1].Coins.Equals(RankInfoOrdered[i].Coins))
                            {
                                RankInfoOrdered[i - 1].Rank = Rank;
                                RankInfoOrdered[i].Rank = Rank;
                            }

                            else
                            {
                                RankInfoOrdered[i - 1].Rank = Rank;
                                Rank = i + 1;
                                RankInfoOrdered[i].Rank = Rank;
                            }

                        }
                    }
                }
                IsProgressVisible = false;
            });
        }

            #endregion
    }
}
