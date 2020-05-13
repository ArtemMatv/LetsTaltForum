using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Models;

namespace DAL.Models
{
    public class Post
    {
        public Post()
        {
            Comments = new List<Comment>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string DateCreated { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual User User { get; set; }
        public int TopicId { get; set; }
        [ForeignKey("TopicId")]
        [JsonIgnore]
        public virtual Topic Topic { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
