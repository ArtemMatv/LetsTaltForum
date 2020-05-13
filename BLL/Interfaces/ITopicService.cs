using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Models.FormsToFill;

namespace BLL.Interfaces
{
    public interface ITopicService : IDisposable
    {
        Task<TopicModel> GetAsync(int id);
        Task<IEnumerable<TopicModel>> GetAllAsync();
        Task CreateTopic(NewTopicModel model);
        Task DeleteTopic(int id);
    }
}
