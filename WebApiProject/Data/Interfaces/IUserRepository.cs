using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.DTOs;
using WebApiProject.Models;

namespace WebApiProject.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string userName, string password);
        void Register(RegisterReqDTO loginReqDTO);
        Task<bool> UserAlreadyExists(string userName);
    }
}
