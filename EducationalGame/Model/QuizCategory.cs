using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalGame.Model
{
    public class QuizCategory
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string QuizCategoryName { get; set; }

        public ICollection<GameQuiz> GameQuizes { get; set; }

    }
}
