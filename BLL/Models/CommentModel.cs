using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BLL.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string DateCreated { get; set; }
        public string Message { get; set; }
        public string UserUserName { get => User.UserName; }
        [JsonIgnore]
        public virtual UserModel User { get; set; }
        public int PostId { get; set; }
        [JsonIgnore]
        public virtual PostModel Post { get; set; }
    }
}
