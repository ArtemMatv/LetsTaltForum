using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Models.FormsToFill;

namespace BLL.Interfaces
{
    public interface IUsersService : IDisposable
    {
        Task<UserModel> GetAsync(string username);
        Task<UserModel> GetAsync(int id);
        Task<IEnumerable<UserModel>> GetAllAsync();

        Task<UserModel> Register(RegisterModel model);
        Task<UserModel> Authenticate(AuthenticateModel model);

        Task DeleteAccount(string username);
        Task UpdateAccount(UserModel user);
        Task UpdateAccountByAdmin(UserModel user, string username);
        
    }
}
