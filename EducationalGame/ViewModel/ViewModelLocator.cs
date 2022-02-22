/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:EducationalGame"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using EducationalGame.Model;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System.Collections.Generic;

namespace EducationalGame.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        /// 
     

        public ViewModelLocator()
        {
            

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<RegistrationVM>();
            SimpleIoc.Default.Register<LoginVM>();
            SimpleIoc.Default.Register<GameStudentVM>();
            SimpleIoc.Default.Register<AddQuestionTeacherVM>();
            SimpleIoc.Default.Register<UpdateQuestionTeacherVM>();
            SimpleIoc.Default.Register<DeleteQuestionTeacherVM>();
            SimpleIoc.Default.Register<RankingsPrincipalVM>();
            SimpleIoc.Default.Register<GameRulesVM>();
            SimpleIoc.Default.Register<StudentsMenuVM>();
            SimpleIoc.Default.Register<TeachersMenuVM>();
            SimpleIoc.Default.Register<PrincipalsMenuVM>();
            
            

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public RegistrationVM RegistrationVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RegistrationVM>();
            }
        }

        public LoginVM LoginVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginVM>();
            }
        }

        public GameStudentVM GameStudentVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GameStudentVM>();
            }

        }

        public AddQuestionTeacherVM AddQuestionTeacherVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddQuestionTeacherVM>();
            }
        }

        public UpdateQuestionTeacherVM UpdateQuestionTeacherVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UpdateQuestionTeacherVM>();
            }
        }

        public DeleteQuestionTeacherVM DeleteQuestionTeacherVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DeleteQuestionTeacherVM>();
            }
        }

        public RankingsPrincipalVM RankingsPrincipalVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RankingsPrincipalVM>();
            }
        }

        public GameRulesVM GameRulesVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GameRulesVM>();
            }
        }

        public StudentsMenuVM StudentsMenuVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StudentsMenuVM>();
            }
        }

        public TeachersMenuVM TeachersMenuVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TeachersMenuVM>();
            }
        }

        public PrincipalsMenuVM PrincipalsMenuVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PrincipalsMenuVM>();
            }
        }



        public static void Cleanup()
        {
            // TODO Clear the ViewModels               
        }
    }
}