using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.DTO;
using MODELS;

namespace WebAPIproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase 
    {
        private readonly IConfiguration _configuration;
        private readonly IUserInterface _userInterface;

        public AuthController(IConfiguration configuration, IUserInterface userInterface)
        {
            _configuration = configuration;
            _userInterface = userInterface;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _userInterface.GetUserByPasswordAndEmail(loginModel.Password, loginModel.Email);

            if (user != null)
            {
                var tokenString = GenerateJwtToken(user);
                return Ok(new { Token = tokenString });
            }

            return Unauthorized("Invalid credentials.");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            // בדיקת קיום משתמש עם האימייל הנתון
            var existingUser = await _userInterface.GetUserByEmail(registerModel.Email);

            // אם המשתמש קיים, החזרת הודעת שגיאה
            if (existingUser != null)
            {
                return Conflict("User already exists.");
            }

            // יצירת אובייקט UserDTO מהנתונים שבקלט
            var userDTO = new UserDTO
            {
                id = registerModel.Id,
                email = registerModel.Email,
                password = registerModel.Password,
                name = registerModel.Name,
                phone = registerModel.Phone
            };

            // הוספת המשתמש למערכת
            await _userInterface.AddUser(userDTO);

            // שליפת המשתמש שהוסף כדי ליצור עבורו טוקן
            var newUser = await _userInterface.GetUserByEmail(registerModel.Email);
            var tokenString = GenerateJwtToken(newUser);

            // החזרת הטוקן שנוצר
            return Ok(new { Token = tokenString });
        }


        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // Ensure UTF8 encoding
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.password), // Use NameIdentifier for password
            new Claim(ClaimTypes.Email, user.email) // Use Email claim type
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
