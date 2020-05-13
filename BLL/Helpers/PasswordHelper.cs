using AutoMapper;
using BLL.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;

namespace BLL.Helpers
{
    public static class PasswordHelper
    {
        private static Mapper _mapper = new Mapper(
            new MapperConfiguration(mc =>
            {
                mc.CreateMap<UserModel, UserModel>();
            }));
        static public UserModel WithoutPassword(this UserModel user)
        {
            if (user == null)
                return null;

            var result = _mapper.Map<UserModel>(user);
            result.PasswordHash = null;
            result.Token = null;
            return result;
        }

        static public IEnumerable<UserModel> WithoutPasswords(this IEnumerable<UserModel> users)
        {
            var result = _mapper.Map<IEnumerable<UserModel>, IEnumerable<UserModel>>(users);

            foreach (UserModel user in result)
            {
                user.PasswordHash = null;
                user.Token = null;
            }
            return result;
        }
        static public string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            Console.WriteLine($"Hashed: {hashed}");

            return hashed;
        }
    }
}
