using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalGame.Model
{
    public abstract class User
    {      
        [Required, MaxLength(30)]
        public string Name { get; set; }
        [Required, MaxLength(30)]
        public string FamilyName { get; set; }
        [Required, MaxLength(30)]
        public string City { get; set; }
        [Required, MaxLength(100)]
        public string SchoolName { get; set; }
        [Required, MinLength(3), MaxLength(30)]
        public string UserName { get; set; }
        [Required, MinLength(8), MaxLength(255)]
        public string Password { get; set; }

        public User()
        {

        }
       
    }
}
