using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Models.FormsToFill;

namespace BLL.Interfaces
{
    public interface ICommentService : IDisposable
    {
        Task<int> NewComment(NewCommentModel model);
        Task DeleteComment(int id);
        Task<IEnumerable<CommentModel>> GetAllAsync();
        Task<CommentModel> GetAsync(int id);
    }
}
