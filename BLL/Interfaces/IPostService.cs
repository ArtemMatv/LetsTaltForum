using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Models.FormsToFill;

namespace BLL.Interfaces
{
    public interface IPostService : IDisposable
    {
        Task<int> NewPost(NewPostModel model);
        Task DeletePost(int id);

        Task<IEnumerable<PostModel>> GetAllAsync();

        Task<PostModel> GetAsync(int id);
    }
}
