using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalGame.Model
{
   public class Student : User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public uint Coins { get; set; }
        [Required]
        public uint TotalPoints { get; set; }

        public ICollection<StudentGameQuiz> StudentGameQuizes { get; set; }
      
        public int? TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }

    }
}
