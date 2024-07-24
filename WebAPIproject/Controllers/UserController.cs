using Microsoft.AspNetCore.Mvc;
using DAL.Interfaces;
using DAL.DTO;
using MODELS;
using Microsoft.AspNetCore.Authorization;

namespace WebAPIproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _dbUser;
        private readonly IAuthInterface _IAuthInterface;

        public UserController(IUserInterface dbUser, IAuthInterface iAuthInterface)
        {
            _dbUser = dbUser;
            _IAuthInterface = iAuthInterface;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDTO value)
        {
            bool create = await _dbUser.AddUser(value);
            if (create)
                return Ok();
            return BadRequest();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UserDTO value)
        {
            // Retrieve the token from the Authorization header
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Decode the token to get the email and password
            var (emailClaim, passwordClaim) = _IAuthInterface.DecodeJwtToken(token);

            if (emailClaim == null || passwordClaim == null)
            {
                return Unauthorized();
            }

            // Retrieve the user object from the database using the email and password
            var user = await _dbUser.GetUserByPasswordAndEmail(passwordClaim, emailClaim);

            if (user == null)
            {
                return Unauthorized();
            }

            // Compare the ID
            if (user.id != id)
            {
                return Forbid();
            }

            // Update the user
            bool updated = await _dbUser.UpdateUser(id, value);
            if (updated)
            {
                return Ok();
            }

            return BadRequest();
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            // Retrieve the token from the Authorization header
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Decode the token to get the email and password
            var (emailClaim, passwordClaim) = _IAuthInterface.DecodeJwtToken(token);

            if (emailClaim == null || passwordClaim == null)
            {
                return Unauthorized();
            }

            // Retrieve the user object from the database using the email and password
            var user = await _dbUser.GetUserByPasswordAndEmail(passwordClaim, emailClaim);

            if (user == null)
            {
                return Unauthorized();
            }

            // Compare the ID
            if (user.id != id)
            {
                return Forbid();
            }

            // Delete the user
            bool delete = await _dbUser.DeleteUser(id);
            if (delete)
                return Ok();
            return BadRequest();
        }



        [HttpGet("{password}/{email}")]
        public async Task<User> Get(string password, string email)
        {
            return await _dbUser.GetUserByPasswordAndEmail(password, email);
        }
    }
}
