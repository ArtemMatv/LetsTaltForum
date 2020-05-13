using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Models.FormsToFill
{
    public class NewCommentModel
    {
        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
        public string DateCreated { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
