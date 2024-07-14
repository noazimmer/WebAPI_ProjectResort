
using MODELS;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;

namespace DAL.Interfaces
{
    public interface IUserInterface
    {
        public Task<List<User>> GetAllUsers();
        public Task<User> GetUserById(string id);
        public Task<User> GetUserByEmail(string email);
        public Task<bool> AddUser(UserDTO user);
        public Task UpdateUser(string id, UserDTO user);
        public Task DeleteUser(string id);
    }
}