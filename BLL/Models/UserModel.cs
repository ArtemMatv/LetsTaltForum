using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BLL.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string AvatarPath { get; set; }
        public DateTime? BannedTo { get; set; }
        public DateTime? SilencedTo { get; set; }
        public virtual ICollection<CommentModel> Comments { get; set; }
        public virtual ICollection<PostModel> Posts { get; set; }
        public virtual IdentityRole<int> UserRole { get; set; }
        public bool CapableToBan { get; set; }
        public bool CapableToSilence { get; set; }
        public string Token { get; set; }
    }
}
