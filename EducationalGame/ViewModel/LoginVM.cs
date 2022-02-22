using CommonServiceLocator;
using EducationalGame.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EducationalGame.ViewModel
{
   public class LoginVM : ViewModelBase
   {
        #region Definitions
        private string userName;
        private string password;
        private Visibility wrongData;
        private bool isProgressVisible;
        private Student student;
        private Teacher teacher;
        private Principal principal;

        #endregion
        #region Properties
        public string UserName { get { return userName; } set { userName = value; RaisePropertyChanged("UserName"); } }
        public string Password { get { return password; } set { password = value; RaisePropertyChanged("Password"); } }
        public Visibility WrongData { get { return wrongData; } set { wrongData = value; RaisePropertyChanged("WrongData"); } }
        public bool IsProgressVisible { get { return isProgressVisible; } set { isProgressVisible = value; RaisePropertyChanged("IsProgressVisible"); } }
        public Student Student { get { return student; } set { student = value; } }
        public Teacher Teacher { get { return teacher; } set { teacher = value; } }
        public Principal Principal { get { return principal; } set { principal = value; } }

        public ICommand Login { get; set; }
        public ICommand Register { get; set; }
    
        #endregion
        #region Constructors
        public LoginVM()
        {              
           WrongData = Visibility.Hidden;
 
           Login = new RelayCommand(() => LoginUser());
           Register = new RelayCommand(() => RegisterUser());
        }


        #endregion
        #region Methods

        private void RegisterUser()
        {
            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            RegistrationVM rvm = ServiceLocator.Current.GetInstance<RegistrationVM>();
            mvm.WindowHeight = 590;
            mvm.WindowWidth = 540;
            rvm.ClearControls();
            mvm.CurrentViewModel = rvm;      
        }

        private void LoginUser()
        {
            Task.Run(() =>
            {
                IsProgressVisible = true;
                bool correctLoginPrincipal = LoginPrincipal();
                bool correctLoginTeacher = LoginTeacher();
                bool correctLoginStudent = LoginStudent();

                using (EducationalGameContext context = new EducationalGameContext())
                {

                    if (correctLoginPrincipal)
                    {
                        lock (context)
                        {
                            WrongData = Visibility.Hidden;

                            Principal = (from pr in context.Principals where pr.UserName.Equals(UserName) select pr).First();
                            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
                            PrincipalsMenuVM pmvm = ServiceLocator.Current.GetInstance<PrincipalsMenuVM>();
                            pmvm.Principal = Principal;
                            mvm.WindowHeight = 140;
                            mvm.WindowWidth = 440;
                            mvm.CurrentViewModel = pmvm;

                            ClearControls();

                        }

                    }



                    else if (correctLoginTeacher)
                    {
                        lock (context)
                        {
                            WrongData = Visibility.Hidden;

                            Teacher = (from tr in context.Teachers where tr.UserName.Equals(UserName) select tr).First();
                            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
                            TeachersMenuVM tmvm = ServiceLocator.Current.GetInstance<TeachersMenuVM>();
                            tmvm.Teacher = Teacher;
                            mvm.WindowHeight = 240;
                            mvm.WindowWidth = 440;
                            mvm.CurrentViewModel = tmvm;

                            ClearControls();
                        }
                    }


                    else if (correctLoginStudent)
                    {
                        lock (context)
                        {
                            WrongData = Visibility.Hidden;

                            Student = (from st in context.Students where st.UserName.Equals(UserName) select st).First();
                            MainViewModel mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
                            StudentsMenuVM smvm = ServiceLocator.Current.GetInstance<StudentsMenuVM>();
                            smvm.Student = Student;
                            mvm.WindowHeight = 390;
                            mvm.WindowWidth = 440;
                            mvm.CurrentViewModel = smvm;

                            ClearControls();
                        }
                    }

                    else
                    {
                        WrongData = Visibility.Visible;
                        ClearControls();
                    }
                    IsProgressVisible = false;
                }
            });
        }

        private bool LoginPrincipal()
        {        
            using (EducationalGameContext context = new EducationalGameContext())
            {
                lock (context)
                {
                    Principal principal = (from pr in context.Principals where pr.UserName.Equals(UserName) select pr).FirstOrDefault();
                    if (UserName != null && Password != null)
                    {
                        if (!UserName.Equals(string.Empty) && !Password.Equals(string.Empty))
                        {
                            if (principal is null)
                            {
                                return false;
                            }

                            else
                            {
                                bool correctPrincipalPassword = PasswordHasher.VerifyPassword(Password, principal.Password);

                                if (correctPrincipalPassword)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }

                        else
                        {
                            return false;
                        }
                    }

                    else
                    {
                        return false;
                    }
                }
            }
        }
        
        private bool LoginTeacher()
        {
            using (EducationalGameContext context = new EducationalGameContext())
            {
                lock (context)
                {
                    Teacher teacher = (from tr in context.Teachers where tr.UserName.Equals(UserName) select tr).FirstOrDefault();
                    if (UserName != null && Password != null)
                    {
                        if (!UserName.Equals(string.Empty) && !Password.Equals(string.Empty))
                        {
                            if (teacher is null)
                            {
                                return false;
                            }

                            else
                            {
                                bool correctTeacherPassword = PasswordHasher.VerifyPassword(Password, teacher.Password);

                                if (correctTeacherPassword)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }

                        else
                        {
                            return false;
                        }
                    }

                    else
                    {
                        return false;
                    }
                }
            }
        }

        private bool LoginStudent()
        {
            using (EducationalGameContext context = new EducationalGameContext())
            {
                lock (context)
                {
                    Student student = (from st in context.Students where st.UserName.Equals(UserName) select st).FirstOrDefault();
                    if (UserName != null && Password != null)
                    {
                        if (!UserName.Equals(string.Empty) && !Password.Equals(string.Empty))
                        {
                            if (student is null)
                            {
                                return false;
                            }

                            else
                            {
                                bool correctStudentPassword = PasswordHasher.VerifyPassword(Password, student.Password);

                                if (correctStudentPassword)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }

                        else
                        {
                            return false;
                        }
                    }

                    else
                    {
                        return false;
                    }
                }
            }
        }


        public void ClearControls()
        {
            UserName = null;
            Password = null;           
        }
        #endregion
    }
}
