using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegTempus.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="You must enter a valid email adress")]
        [Display(Name = "User name")]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [Display(Name ="Password(min 6 char, one digit, one symbol, one uppercase and one lowercase letter)")]
        [DataType(DataType.Password, ErrorMessage = "Password must contain one digit, one symbol, one lowercase and one uppercase letter")]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
    }
}
