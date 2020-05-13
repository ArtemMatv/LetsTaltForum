using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace DAL.Models
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            Comments = new List<Comment>();
            Posts = new List<Post>();
        }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string AvatarPath { get; set; }
        public DateTime? BannedTo { get; set; }
        public DateTime? SilencedTo { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual IdentityRole<int> UserRole { get; set; }
        public bool CapableToBan { get; set; }
        public bool CapableToSilence { get; set; }
        public string Token { get; set; }
    }
}
