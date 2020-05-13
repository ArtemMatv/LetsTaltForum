using AutoMapper;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using BLL.Models.FormsToFill;

namespace BLL.Services
{
    public class TopicService : ITopicService, IDisposable
    {
        private readonly Mapper _mapper;
        private readonly IRepository<Topic> _repository;
        private bool _disposed;

        public TopicService(IRepository<Topic> repository)
        {
            _repository = repository;
            _mapper = new Mapper(new MapperConfiguration(mc => 
            { 
                mc.CreateMap<NewTopicModel, Topic>();
                mc.CreateMap<User, UserModel>();
                mc.CreateMap<UserModel, User>();
                mc.CreateMap<Post, PostModel>();
                mc.CreateMap<Comment, CommentModel>();
                mc.CreateMap<Topic, TopicModel>();
                mc.CreateMap<PostModel, Post>();
                mc.CreateMap<CommentModel, Comment>();
                mc.CreateMap<TopicModel, Topic>();
            } ));
            _disposed = false;
        }

        public async Task CreateTopic(NewTopicModel model)
        {
            if (model == null)
                throw new ModelException("Model is null");

            var topic = await _repository.GetAsync(topic => topic.Name == model.Name).ConfigureAwait(false);

            if (topic != null)
                throw new DataException("The topic with the same name already exists");

            topic = _mapper.Map<Topic>(model);

            await _repository.InsertAsync(topic).ConfigureAwait(false);
            await _repository.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteTopic(int id)
        {
            await _repository.Remove(id).ConfigureAwait(false);
            await _repository.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TopicModel>> GetAllAsync()
        {
            var topics = await _repository.GetAllAsync().ConfigureAwait(false);

            return _mapper.Map<IEnumerable<TopicModel>>(topics);
        }

        public async Task<TopicModel> GetAsync(int id)
        {
            var topic = await _repository.GetAsync(id).ConfigureAwait(false);

            return _mapper.Map<TopicModel>(topic);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repository.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
