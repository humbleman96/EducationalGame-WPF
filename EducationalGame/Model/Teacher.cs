using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalGame.Model
{
   public class Teacher : User
    {
        [Key]     
        public int Id { get; set; }
        public int? PrincipalId { get; set;}

        [ForeignKey("PrincipalId"), MaxLength(255)]
        public virtual Principal Principal { get; set; }

        public virtual ICollection<Student> Students  { get; set; }
     
    }
}
