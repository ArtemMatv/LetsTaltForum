using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Models.FormsToFill
{
    public class RegisterModel
    {
        [Required]
        [MinLength(6, ErrorMessage = "Too short")]
        [MaxLength(32, ErrorMessage = "Too long")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Too short")]
        [MaxLength(32, ErrorMessage = "Too long")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }

        [Display(Name = "Age")]
        public int Age { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }
    }
}
