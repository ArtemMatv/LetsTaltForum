using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using DAL.UnitOfWork;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using BLL.Models.FormsToFill;

namespace BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork<Comment, Post> _unitOfWork;
        private readonly IRepository<User> _repository;
        private bool _disposed;
        private readonly Mapper _mapper;

        public CommentService(IUnitOfWork<Comment, Post> unitOfWork,
            IRepository<User> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = new Mapper(
                new MapperConfiguration(mc =>
                {
                    mc.CreateMap<NewCommentModel, Comment>();
                    mc.CreateMap<User, UserModel>();
                    mc.CreateMap<UserModel, User>();
                    mc.CreateMap<Post, PostModel>();
                    mc.CreateMap<Comment, CommentModel>();
                    mc.CreateMap<Topic, TopicModel>();
                    mc.CreateMap<PostModel, Post>();
                    mc.CreateMap<CommentModel, Comment>();
                    mc.CreateMap<TopicModel, Topic>();
                }));
        }

        public async Task<IEnumerable<CommentModel>> GetAllAsync()
        {
            var comms = await _unitOfWork.TRepository.GetAllAsync().ConfigureAwait(false);
            return _mapper.Map<IEnumerable<CommentModel>>(comms);
        }

        public async Task<CommentModel> GetAsync(int id)
        {
            var comm = await _unitOfWork.TRepository.GetAsync(id).ConfigureAwait(false);
            return _mapper.Map<CommentModel>(comm);
        }

        public async Task<int> NewComment(NewCommentModel model)
        {
            if (model == null)
                throw new ModelException("Model is null");

            var comment = _mapper.Map<Comment>(model);
            comment.User = await _repository.GetAsync(model.UserId).ConfigureAwait(false);

            if (comment.User.SilencedTo > DateTime.Now)
                throw new AccessException($"This user is in ReaOnly mode up to {comment.User.SilencedTo}");

            comment.DateCreated = DateTime.Now.ToString("MM/dd/yyyy HH:mm");

            comment.Post = await _unitOfWork.URepository
                .GetAsync(model.PostId).ConfigureAwait(false);

            await _unitOfWork.TRepository.InsertAsync(comment).ConfigureAwait(false);
            await _unitOfWork.TRepository.SaveChangesAsync().ConfigureAwait(false);

            return comment.Id;
        }

        public async Task DeleteComment(int id)
        {
            var entity = await _unitOfWork.TRepository
                .GetAsync(comment => comment.Id == id).ConfigureAwait(false);

            if (entity == null)
                throw new DataException("There is no such comment");

            _unitOfWork.TRepository.Remove(entity);
            await _unitOfWork.TRepository.SaveChangesAsync().ConfigureAwait(false);
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
                    _unitOfWork.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
