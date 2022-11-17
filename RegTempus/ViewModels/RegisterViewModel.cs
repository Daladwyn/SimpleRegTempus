using System.ComponentModel.DataAnnotations;

namespace RegTempus.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "First name")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
