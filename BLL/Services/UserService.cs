using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BLL.Exceptions;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models;
using BLL.Models.FormsToFill;
using System.Configuration;

namespace BLL.Services
{
    public class UserService : IUsersService
    {
        private readonly IUnitOfWork<User, IdentityRole<int>> _unitOfWork;
        private readonly AppSettings _appSettings;
        private readonly Mapper _mapper;
        private bool _disposed;

        public UserService(IOptions<AppSettings> appSettings,
            IUnitOfWork<User, IdentityRole<int>> unitOfWork)
        {
            if (appSettings != null)
                _appSettings = appSettings.Value;

            _unitOfWork = unitOfWork;
            _mapper = new Mapper(
                new MapperConfiguration(mc =>
                {
                    mc.CreateMap<RegisterModel, User>();
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

        public async Task<UserModel> Authenticate(AuthenticateModel model)
        {
            if (model == null)
                throw new ModelException("Model is null");

            var passhash = PasswordHelper.HashPassword(model.Password);

            var user = await _unitOfWork.TRepository.GetAsync(x =>
                x.UserName == model.UserName && x.PasswordHash == passhash
            ).ConfigureAwait(false);

            if (user == null)
                return null;

            if (user.BannedTo > DateTime.Now)
                throw new AccessException($"This user is banned up to {user.BannedTo}");

            user.Token = CreateToken(user);

            _unitOfWork.TRepository.Update(user);

            await _unitOfWork.Commit().ConfigureAwait(false);

            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> Register(RegisterModel model)
        {
            if (model == null)
                throw new ModelException("Model is null");

            var entity = await _unitOfWork.TRepository
                .GetAsync(u => u.UserName == model.UserName).ConfigureAwait(false);

            if (entity == null)
            {
                model.PasswordHash = PasswordHelper.HashPassword(model.PasswordHash);

                User user = _mapper.Map<User>(model);

                user.UserRole = await _unitOfWork.URepository.GetAsync(r => r.Name == "User").ConfigureAwait(false);

                user.AvatarPath = ConfigurationManager.AppSettings["defaultAvatar"];

                user.Token = CreateToken(user);

                await _unitOfWork.TRepository.InsertAsync(user).ConfigureAwait(false);

                await _unitOfWork.Commit().ConfigureAwait(false);

                return _mapper.Map<UserModel>(user);
            }
            else
                throw new DataException("This username already exists!");
        }

        private string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                            SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task DeleteAccount(string username)
        {
            var user = await _unitOfWork.TRepository.GetAsync(u => u.UserName == username).ConfigureAwait(false);

            _unitOfWork.TRepository.Remove(user);
            await _unitOfWork.Commit().ConfigureAwait(false);
        }

        public async Task UpdateAccount(UserModel user)
        {
            if (user == null)
                throw new ModelException("User is null");

            var entity = await _unitOfWork.TRepository
                .GetAsync(u => u.UserName == user.UserName).ConfigureAwait(false);

            if (entity == null)
                throw new DataException("There is no such user");

            entity.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash);
            entity.Email = user.Email;
            entity.Gender = user.Gender;
            entity.AvatarPath = user.AvatarPath;
            entity.Age = user.Age;

            _unitOfWork.TRepository.Update(entity);
            await _unitOfWork.Commit().ConfigureAwait(false);
        }

        public async Task<UserModel> GetAsync(string username)
        {
            var user = await _unitOfWork.TRepository.GetAsync(u => u.UserName == username).ConfigureAwait(false);
            return _mapper.Map<UserModel>(user).WithoutPassword();
        }

        public async Task<UserModel> GetAsync(int id)
        {
            var user = await _unitOfWork.TRepository.GetAsync(id).ConfigureAwait(false);
            return _mapper.Map<UserModel>(user).WithoutPassword();
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            var users = await _unitOfWork.TRepository.GetAllAsync().ConfigureAwait(false);
            return _mapper.Map<IEnumerable<UserModel>>(users).WithoutPasswords();
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

        public async Task UpdateAccountByAdmin(UserModel user, string username)
        {
            if (user == null)
                throw new ModelException("User is null");

            var admin = await _unitOfWork.TRepository
                .GetAsync(u => u.UserName == username).ConfigureAwait(false);

            if (admin == null || admin.UserRole.Name != "Admin")
                throw new AccessException("You are not administrator!");

            var entity = await _unitOfWork.TRepository
                .GetAsync(u => u.UserName == user.UserName).ConfigureAwait(false);

            if (entity == null)
                throw new DataException("There is no such user");

            entity.SilencedTo = user.SilencedTo;
            entity.BannedTo = user.BannedTo;
            entity.UserRole = user.UserRole;

            _unitOfWork.TRepository.Update(entity);
            await _unitOfWork.Commit().ConfigureAwait(false);
        }
    }
}