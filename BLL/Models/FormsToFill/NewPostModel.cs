using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models.FormsToFill
{
    public class NewPostModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
        public string DateCreated { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TopicId { get; set; }
    }
}