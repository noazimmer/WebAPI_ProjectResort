using Microsoft.AspNetCore.Mvc;
using DAL.Interfaces;
using DAL.DTO;
using MODELS;
namespace WebAPIproject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _dbUser;
        public UserController(IUserInterface dbUser)
        {
            _dbUser = dbUser;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDTO value)
        {
            bool create = await _dbUser.AddUser(value);
            if (create)
                return Ok();
            return BadRequest();

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UserDTO value)
        {
            bool create = await _dbUser.UpdateUser(id, value);
            if (create)
                return Ok();
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            bool create = await _dbUser.DeleteUser(id);
            if (create)
                return Ok();
            return BadRequest();
        }
        [HttpGet("{password}/{email}")]
        public async Task<User> Get(string password, string email)
        {
            return await _dbUser.GetUserByPasswordAndEmail(password, email);

        }
        //רק בדקנו שעובד ,נצטרך את זה לבדיקות תקינות
        //[HttpGet("{email}")]
        //public async Task<User> Get( string email)
        //{
        //    return await _dbUser.GetUserByEmail( email);

        //}
        //    [HttpGet]
        //    public async Task<List<User>> Get()
        //    {
        //        return await _dbUser.GetAllUsers();

        //    }
    }
}
