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
    public class PostService : IPostService
    {
        private readonly Mapper _mapper;
        private readonly IUnitOfWork<User, Post> _unitOfWork;
        private bool _disposed;

        public PostService(IUnitOfWork<User, Post> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new Mapper(
                new MapperConfiguration(mc =>
                {
                    mc.CreateMap<NewPostModel, Post>();
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
        public async Task DeletePost(int id)
        {
            var entity = await _unitOfWork.URepository
                .GetAsync(post => post.Id == id).ConfigureAwait(false);

            if (entity == null)
                throw new DataException("There is no such post");

            _unitOfWork.URepository.Remove(entity);

            await _unitOfWork.URepository
                .SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<int> NewPost(NewPostModel model)
        {
            if (model == null)
                throw new ModelException("Model is null");

            var author = await _unitOfWork.TRepository
                .GetAsync(model.UserId).ConfigureAwait(false);

            if (author == null)
                throw new DataException("Author does not exist!");

            if (author.SilencedTo > DateTime.Now)
                throw new AccessException($"This user is in ReadOnly mode up to {author.SilencedTo}");

            var post = _mapper.Map<Post>(model);

            post.DateCreated = DateTime.Now.ToString("MM/dd/yyyy HH:mm");

            await _unitOfWork.URepository
                .InsertAsync(post).ConfigureAwait(false);

            await _unitOfWork.URepository
                .SaveChangesAsync().ConfigureAwait(false);

            return post.Id;
        }

        public async Task<IEnumerable<PostModel>> GetAllAsync()
        {
            var posts = await _unitOfWork.URepository
                .GetAllAsync().ConfigureAwait(false);

            var result = _mapper.Map<IEnumerable<PostModel>>(posts);
            return result;
        }

        public async Task<PostModel> GetAsync(int id)
        {
            var post = await _unitOfWork.URepository
                .GetAsync(id).ConfigureAwait(false);

            return _mapper.Map<PostModel>(post);
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
