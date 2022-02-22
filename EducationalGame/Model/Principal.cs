using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalGame.Model
{
   public class Principal : User
    {
        [Key]
        public int Id { get; set; }
  
        public ICollection<Teacher> Teachers { get; set; }

    }
}
