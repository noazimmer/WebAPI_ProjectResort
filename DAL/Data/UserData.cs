using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;
using AutoMapper;
using MongoDB.Driver;
using DAL.Interfaces;
using MODELS;

namespace DAL.Data
{
    public class UserData : IUserInterface
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMapper _mapper;

        public UserData(IMongoClient mongoClient, string databaseName, IMapper mapper)
        {
            var database = mongoClient.GetDatabase(databaseName);
            _usersCollection = database.GetCollection<User>("Users");
            _mapper = mapper;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _usersCollection.Find(u =>  u.email == email).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByPasswordAndEmail(string password, string email)
        {
            return await _usersCollection.Find(u => u.password == password&&u.email==email).FirstOrDefaultAsync();
        }
        public async Task<bool> AddUser(UserDTO user)
        {
            await _usersCollection.InsertOneAsync(_mapper.Map<User>(user));
            return true;
        }

        public async Task<bool> UpdateUser(string id, UserDTO user)
        {
            await _usersCollection.ReplaceOneAsync(u => u.id == id, _mapper.Map<User>(user));
            return true;
        }

        public async Task<bool> DeleteUser(string id)
        {
            await _usersCollection.DeleteOneAsync(u => u.id == id);
            return true;
        }


        //public async Task<List<User>> GetAllUsers()
        //{
        //    return await _usersCollection.Find(_ => true).ToListAsync();
        //}
    }
}
