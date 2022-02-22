using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalGame.Model
{
   public class GameQuiz
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string Question { get; set; }
        [Required, MaxLength(50)]
        public string Answer { get; set; }
        
        public int? QuizCategoryId { get; set; }

        [ForeignKey("QuizCategoryId")]
        public virtual QuizCategory QuizCategory { get; set;}
        
        public ICollection<StudentGameQuiz> StudentGameQuizes { get; set; }

    }
}
