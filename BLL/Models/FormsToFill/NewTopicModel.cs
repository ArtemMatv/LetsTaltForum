using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models.FormsToFill
{
    public class NewTopicModel
    {
        [Required]
        [Display(Name = "Title of a topic")]
        public string Name { get; set; }
        [Required]
        public IdentityRole<int> AuthorRole { get; set; }
    }
}
