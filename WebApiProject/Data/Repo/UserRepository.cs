using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebApiProject.Data.Interfaces;
using WebApiProject.DTOs;
using WebApiProject.Models;

namespace WebApiProject.Data.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext dataContext;

        public UserRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public async Task<User> Authenticate(string userName, string passwordText)
        {
            var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Username == userName);//&& u.Password == password);
            if (user == null || user.PasswordKey == null) return null;

            if (!MatchPasswordHash(passwordText, user.Password, user.PasswordKey))
                return null;

            return user;
        }

        public void Register(RegisterReqDTO loginReqDTO)
        {
            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginReqDTO.Password));
            }

            User user = new User();
            user.Username = loginReqDTO.UserName;
            user.Email = loginReqDTO.Email;
            user.Mobile = loginReqDTO.Mobile;
            user.Password = passwordHash;
            user.PasswordKey = passwordKey;

            dataContext.Users.Add(user);
        }

        public async Task<bool> UserAlreadyExists(string userName)
        {
            return await dataContext.Users.AnyAsync(u => u.Username == userName);
        }

        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));

                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != password[i])
                        return false;
                }

                return true;
            }
        }
    }
}
