using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace EducationalGame.ViewModel
{
   public static class Validator
   {
        public static bool ValidateName(string name)
        {
            Regex namesRegex = new Regex(@"^[А-Я][а-я]{1,12}\-[А-Я][а-я]{1,12}\-[А-Я][а-я]{1,12}$|^[А-Я][а-я]{1,12}\-[А-Я][а-я]{1,12}$|^[А-Я][а-я]{1,12}\-[а-я][а-я]{1,12}$|^[А-Я][А-Я]{1,12}\-[А-Я][А-Я]{1,12}$|^[А-Я][а-я]{1,19}$");
        
            try
            {
                if (!namesRegex.IsMatch(name))
                {                 
                    return false;
                }
             
                return true;
            }

            catch (ArgumentNullException anEx)
            {
                return false;
            }
        }

        public static bool ValidateAnswer(string answer)
        {
            Regex answerRegex = new Regex(@"^[А-Я][А-Я]+$|^[А-Я][А-Я]+\ [А-Я][А-Я]+$|^[А-Я][А-Я]+\ [А-Я][А-Я]+\ [А-Я][А-Я]+$");

            try
            {
                if (!answerRegex.IsMatch(answer))
                {
                    return false;
                }

                return true;
            }

            catch (ArgumentNullException anEx)
            {
                return false;
            }
        }

        public static bool ValidateQuestion(string question)
        {
                
           if (string.IsNullOrWhiteSpace(question))
           {
             return false;
           }

           return true;

        }

        public static bool ValidateFamilyName(string familyName)
        {
            Regex namesRegex = new Regex(@"^[А-Я][а-я]{1,12}\-[А-Я][а-я]{1,12}\-[А-Я][а-я]{1,12}$|^[А-Я][а-я]{1,12}\-[А-Я][а-я]{1,12}$|^[А-Я][а-я]{1,12}\-[а-я][а-я]{1,12}$|^[А-Я][А-Я]{1,12}\-[А-Я][А-Я]{1,12}$|^[А-Я][а-я]{1,19}$");

            try
            {
               

                if (!namesRegex.IsMatch(familyName))
                {                  
                    return false;
                }

                return true;
            }

            catch (ArgumentNullException anEx)
            {
                return false;
            }
        }

        public static bool ValidateUserName(string userName)
        {        
            Regex userNameRegex = new Regex("^(?=.{6,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$");
            
            try
            {
               
                if (!userNameRegex.IsMatch(userName))
                {                  
                    return false;
                }
           
                return true;
            }

            catch (ArgumentNullException anEx)
            {
                return false;
            }
        }

        public static bool ValidatePassword(string password)
        {         
            Regex passwordRegex = new Regex(@"^(?=.*?[A-Z]*)(?=.*?[a-z])(?=.*?[0-9])(?=.*?[\W_]*).{8,30}$");

            try
            {
                if (!passwordRegex.IsMatch(password))
                {
                    return false;
                }

                return true;
            }

            catch (ArgumentNullException anEx)
            {
                return false;
            }
        }

        public static bool ValidateRepeatedPassword(string password,string repeatedPassword)
        {
            try
            {          
                if (!password.Equals(repeatedPassword))
                {
                    return false;
                }

                if(repeatedPassword.Equals(string.Empty))
                {
                    return false;
                }

                return true;
            }

            catch (ArgumentNullException anEx)
            {
                return false;
            }

            catch(NullReferenceException nullEx)
            {
                return false;
            }
        }
    }
}
