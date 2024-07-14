using Microsoft.AspNetCore.Mvc;
using DAL.Interfaces;
using DAL.DTO;

namespace WebAPIproject.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
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
    }
}
