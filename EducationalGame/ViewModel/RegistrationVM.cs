using CommonServiceLocator;
using EducationalGame.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EducationalGame.ViewModel
{
   public class RegistrationVM : ViewModelBase
    {
        #region Definitions    
        private List<string> cities;
        private string selectedCity;
        private List<string> schoolNames;
        private string selectedSchoolName;
        private List<string> positions;
        private string selectedPosition;
        private string repeatedPassword;
        private Visibility wrongName;
        private Visibility wrongFamilyName;
        private Visibility wrongUserName;
        private Visibility wrongPassword;
        private Visibility wrongRepeatedPassword;
        private Dictionary<string, List<string>> sortedSchoolsAndCities = new Dictionary<string, List<string>>();
        private List<string> teachers;
        private string selectedTeacher;
        private Visibility teacher;
        private User user;
        private bool isProgressVisible;
        #endregion
        #region Properties
        public List<string> Cities { get { return cities; } set {cities = value; RaisePropertyChanged("Cities"); } }
        public string SelectedCity {
            get
            {
                return selectedCity;         
            }

            set
            {              
                selectedCity = value;            
                RaisePropertyChanged("SelectedCity");
                SchoolNames = sortedSchoolsAndCities[SelectedCity];            
            }
        }

        public List<string> SchoolNames
        {
            get
            {       
                return schoolNames;
            }

            set
            {         
                schoolNames = value;
                RaisePropertyChanged("SchoolNames");
            }

        }
        public string SelectedSchoolName { get { return selectedSchoolName; } set { selectedSchoolName = value; RaisePropertyChanged("SelectedSchoolName"); Task ct = ChooseTeacher();} }
        public List<string> Positions { get { return positions; } set { positions = value; RaisePropertyChanged("Positions"); } }
        public string SelectedPosition
        {
            get
            {
                return selectedPosition;
            }

            set
            {
                selectedPosition = value;
                RaisePropertyChanged("SelectedPosition");

                if (SelectedPosition.Equals("Ученик"))
                {
                    Teacher = Visibility.Visible;
                }
                else
                {
                    Teacher = Visibility.Hidden;
                }         
            }
        }

        
        public string RepeatedPassword { get { return repeatedPassword; } set { repeatedPassword = value; RaisePropertyChanged("RepeatedPassword"); } }
        public Visibility WrongName { get { return wrongName; } set { wrongName = value; RaisePropertyChanged("WrongName"); }  }
        public Visibility WrongFamilyName { get { return wrongFamilyName; } set { wrongFamilyName = value; RaisePropertyChanged("WrongFamilyName"); } }
        public Visibility WrongUserName { get { return wrongUserName; } set { wrongUserName = value; RaisePropertyChanged("WrongUserName"); } }
        public Visibility WrongPassword { get { return wrongPassword; } set { wrongPassword = value; RaisePropertyChanged("WrongPassword"); } }
        public Visibility WrongRepeatedPassword { get { return wrongRepeatedPassword; } set { wrongRepeatedPassword = value; RaisePropertyChanged("WrongRepeatedPassword"); } }
        public Visibility Teacher
        {
            get
            {
                return teacher;
            }

            set
            {
                teacher = value;
                RaisePropertyChanged("Teacher");
            }
        }

        public User User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
                RaisePropertyChanged("User");         
            }
        }
        public List<string> Teachers { get { return teachers; } set { teachers = value; RaisePropertyChanged("Teachers"); } }
        public string SelectedTeacher { get { return selectedTeacher; } set {selectedTeacher = value; RaisePropertyChanged("SelectedTeacher"); } }
        public bool IsProgressVisible { get { return isProgressVisible; } set { isProgressVisible = value; RaisePropertyChanged("IsProgressVisible"); } }
        public ICommand Register { get; set; }
        public ICommand Back { get; set; }
        #endregion
        #region Constructors
        public RegistrationVM()
        {      
           AddCitiesAndSchools();
           AddPostions();
           WrongName = Visibility.Hidden;
           WrongFamilyName = Visibility.Hidden;
           WrongUserName = Visibility.Hidden;
           WrongPassword = Visibility.Hidden;
           WrongRepeatedPassword = Visibility.Hidden;

           Teachers = new List<string>();
           User = new Student();

           Register = new RelayCommand(() => RegisterCommand());
           Back = new RelayCommand(() => BackToLogin());       
        }
        #endregion
        #region Methods

        private void BackToLogin()
        {
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            LoginVM lvm = ServiceLocator.Current.GetInstance<LoginVM>();
            lvm.ClearControls();
            lvm.WrongData = Visibility.Hidden;
            mvm.CurrentViewModel = lvm;
            mvm.WindowHeight = 440;
            mvm.WindowWidth = 440;
        }

        private void RegisterCommand()
        {
            bool isNameCorrect = Validator.ValidateName(User.Name);
            bool isFamilyNameCorrect = Validator.ValidateFamilyName(User.FamilyName);
            bool isUserNameCorrect = Validator.ValidateUserName(User.UserName);
            bool isPasswordCorrect = Validator.ValidatePassword(User.Password);
            bool isRepeatedPasswordCorrect = Validator.ValidateRepeatedPassword(User.Password, RepeatedPassword);

            if(!isNameCorrect)
            {
                WrongName = Visibility.Visible;
            }

            else
            {
                WrongName = Visibility.Hidden;
            }

            if (!isFamilyNameCorrect)
            {
                WrongFamilyName = Visibility.Visible;
            }

            else
            {
                WrongFamilyName = Visibility.Hidden;
            }

            if (!isUserNameCorrect)
            {
                WrongUserName = Visibility.Visible;
            }

            else
            {
                WrongUserName = Visibility.Hidden;
            }

            if (!isPasswordCorrect)
            {
                WrongPassword = Visibility.Visible;
            }

            else
            {
                WrongPassword = Visibility.Hidden;
            }

            if (!isRepeatedPasswordCorrect)
            {
                WrongRepeatedPassword = Visibility.Visible;
            }

            else
            {
                WrongRepeatedPassword = Visibility.Hidden;
            }

            if (isNameCorrect && isFamilyNameCorrect && isUserNameCorrect && isPasswordCorrect &&
                isRepeatedPasswordCorrect)
            {
                if (SelectedPosition.Equals("Директор"))
                {
                    RegisterPrincipal(User);
                }

                if (SelectedPosition.Equals("Учител"))
                {
                    RegisterTeacher(User);
                }

                if (SelectedPosition.Equals("Ученик"))
                {
                    RegisterStudent(User);
                }
            }
      
        }

        private void RegisterPrincipal(User user)
        {          
            using (EducationalGameContext context = new Model.EducationalGameContext())
            {
                lock (context)
                {
                    List<Principal> principals = context.Principals.ToList();

                    if (SelectedPosition.Equals("Директор"))
                    {
                        Principal principal = new Principal();
                        principal.Name = user.Name;
                        principal.FamilyName = user.FamilyName;
                        principal.City = SelectedCity;
                        principal.SchoolName = SelectedSchoolName;
                        principal.UserName = user.UserName;
                        principal.Password = PasswordHasher.CreateSaltedHash(user.Password);

                        Principal existingPrincipal = (from pr in principals
                                                       where (pr.City.Equals(SelectedCity) && pr.SchoolName.Equals(SelectedSchoolName)) ||
                                                       pr.UserName.Equals(user.UserName) select pr).FirstOrDefault();

                        Teacher existingTeacher = (from tr in context.Teachers
                                                   where tr.UserName.Equals(user.UserName)
                                                   select tr).FirstOrDefault();

                        Student existingStudent = (from st in context.Students
                                                   where st.UserName.Equals(user.UserName)
                                                   select st).FirstOrDefault();



                        if (existingPrincipal is null && existingTeacher is null && existingStudent is null)
                        {
                            context.Principals.Add(principal);
                            context.SaveChanges();

                            MessageBox.Show("Успешна регистрация!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearControls();
                        }

                        else if(existingPrincipal != null)
                        {
                            MessageBox.Show("Има регистриран вече директор от избраните град и училище!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        else
                        {
                            MessageBox.Show("Има регистриран вече такъв потребител!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }         
        }

        private void RegisterTeacher(User user)
        {
            using (EducationalGameContext context = new Model.EducationalGameContext())
            {
                lock (context)
                {
                    List<Principal> principals = context.Principals.ToList();
                    List<Teacher> teachers = context.Teachers.ToList();

                    if (SelectedPosition.Equals("Учител"))
                    {
                        Teacher teacher = new Teacher();
                        teacher.Name = user.Name;
                        teacher.FamilyName = user.FamilyName;
                        teacher.City = SelectedCity;
                        teacher.SchoolName = SelectedSchoolName;
                        teacher.UserName = user.UserName;
                        teacher.Password = PasswordHasher.CreateSaltedHash(user.Password);

                        Principal principalOfSelectedSchool = (from pr in principals
                                                               where (pr.City.Equals(SelectedCity) &&
                                                               pr.SchoolName.Equals(SelectedSchoolName))
                                                               select pr).FirstOrDefault();

                        Principal existingPrincipal = (from pr in principals where pr.UserName.Equals(user.UserName) select pr).FirstOrDefault();

                        Teacher existingTeacher = (from tr in teachers where tr.UserName.Equals(user.UserName) select tr).FirstOrDefault();

                        Student existingStudent = (from st in context.Students where st.UserName.Equals(user.UserName) select st).FirstOrDefault();



                        if (existingTeacher is null && existingPrincipal is null && existingStudent is null)
                        {
                            if (principalOfSelectedSchool != null)
                            {
                                teacher.Principal = principalOfSelectedSchool;
                                context.Teachers.Add(teacher);
                                context.SaveChanges();
                                MessageBox.Show("Успешна регистрация!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
                                ClearControls();
                            }

                            else
                            {
                                MessageBox.Show("Не се е регистрирал директор от избраното училище все още!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }

                        else
                        {
                            MessageBox.Show("Има регистриран вече такъв потребител!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
        }

        private void RegisterStudent(User user)
        {
            using (EducationalGameContext context = new Model.EducationalGameContext())
            {
                lock (context)
                {
                    List<Principal> principals = context.Principals.ToList();
                    List<Teacher> teachers = context.Teachers.ToList();
                    List<Student> students = context.Students.ToList();

                    if (SelectedPosition.Equals("Ученик"))
                    {
                        Student student = new Student();
                        student.Name = user.Name;
                        student.FamilyName = user.FamilyName;
                        student.City = SelectedCity;
                        student.SchoolName = SelectedSchoolName;
                        student.UserName = user.UserName;
                        student.Password = PasswordHasher.CreateSaltedHash(user.Password);
                        student.Coins = 0;
                        student.TotalPoints = 0;

                        Teacher chosenTeacher = (from tr in teachers where tr.UserName.Equals(SelectedTeacher) select tr).FirstOrDefault();

                        Principal existingPrincipal = (from pr in principals where pr.UserName.Equals(user.UserName) select pr).FirstOrDefault();

                        Teacher existingTeacher = (from tr in teachers where tr.UserName.Equals(user.UserName) select tr).FirstOrDefault();

                        Student existingStudent = (from st in students where st.UserName.Equals(user.UserName) select st).FirstOrDefault();

                        if (existingTeacher is null && existingPrincipal is null && existingStudent is null)
                        {
                            if (chosenTeacher != null)
                            {
                                student.Teacher = chosenTeacher;
                                context.Students.Add(student);
                                context.SaveChanges();
                                MessageBox.Show("Успешна регистрация!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
                                ClearControls();
                            }

                            else
                            {
                                MessageBox.Show("Не е избран учител!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }

                        else
                        {
                            MessageBox.Show("Има регистриран вече такъв потребител!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
        }

        private void AddCitiesAndSchools()
        {        
            Task.Run(() =>
            {
                IsProgressVisible = true;
                Cities = new List<string>();
                List<string> cities = new List<string>();

                Dictionary<string, List<string>> schoolsInCities = new Dictionary<string, List<string>>();


                string[] file = File.ReadAllLines(@"Resources\cities and schools.txt");
               
                int cityIndex = -1;


                for (int i = 0; i < file.Length; i++)
                {
                    string[] line = file[i].Split(' ');

                    if (line.Length == 1 && !line[0].Equals(string.Empty) || line.Length == 2)
                    {
                        cities.Add(file[i]);
                        schoolsInCities.Add(file[i], new List<string>());
                        cityIndex++;
                    }

                    else
                    {
                        if (!line[0].Equals(string.Empty))
                        {
                            schoolsInCities[cities[cityIndex]].Add(file[i]);
                        }
                    }
                    
                }

                foreach (var item in schoolsInCities.OrderBy(x => x.Key))
                {
                    Cities.Add(item.Key);
                    sortedSchoolsAndCities.Add(item.Key, item.Value);
                }

                IsProgressVisible = false;
            });
         
        }

        private void AddPostions()
        {
           
           Positions = new List<string>();
           Positions.Add("Ученик");
           Positions.Add("Учител");
           Positions.Add("Директор");
           
        }

        private async Task ChooseTeacher()
        {
            
            await Task.Run(() =>
            {
                IsProgressVisible = true;
                using (EducationalGameContext context = new EducationalGameContext())
                {
                    lock (context)
                    {
                        Teachers = (from tr in context.Teachers
                                    where tr.City.Equals(SelectedCity) && tr.SchoolName.Equals(SelectedSchoolName)
                                    select tr.UserName).ToList();
                    }
                }
                IsProgressVisible = false;
            });
            
        }       

        public void ClearControls()
        {
            User = new Student();         
            RepeatedPassword = string.Empty;
            WrongName = Visibility.Hidden;
            WrongFamilyName = Visibility.Hidden;
            WrongUserName = Visibility.Hidden;
            WrongPassword = Visibility.Hidden;
            WrongRepeatedPassword = Visibility.Hidden;

        }
        #endregion
    }
}
