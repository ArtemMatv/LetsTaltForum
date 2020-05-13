using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class TopicModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PostModel> Posts { get; set; }
    }
}
