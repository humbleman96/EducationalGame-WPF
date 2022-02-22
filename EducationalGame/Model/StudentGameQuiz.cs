using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalGame.Model
{
   public class StudentGameQuiz
    {           
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int GameQuizId { get; set; }
        public GameQuiz GameQuiz { get; set; }   
    }
}
