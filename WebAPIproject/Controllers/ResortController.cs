using DAL.DTO;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MODELS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPIproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResortController : ControllerBase
    {
        private readonly IResortInterface _resortData;
        private readonly IAuthInterface _IAuthInterface;
        private readonly IUserInterface _dbUser;
        public ResortController(IResortInterface resortData, IAuthInterface IAuthInterface, IUserInterface dbUser)
        {
            _resortData = resortData;
            _IAuthInterface = IAuthInterface;
            _dbUser = dbUser;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateResort([FromBody] ResortDTO resort)
        {
            if (resort == null)
            {
                return BadRequest("Resort is null");
            }

            var result = await _resortData.CreateResort(resort);

            if (result)
            {
                return Ok("Resort created successfully");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Error creating resort");
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resort>>> GetAllResorts()
        {
            var resorts = await _resortData.GetAllResorts();
            return Ok(resorts);
        }
        [Authorize]
        [HttpGet("{resortName}")]
        public async Task<ActionResult<Resort>> GetResortByName(string resortName)
        {
            var resort = await _resortData.GetResortByName(resortName);
            if (resort == null)
            {
                return NotFound();
            }
            return Ok(resort);
        }

        [Authorize]
        [HttpPut("{resortName}")]
        public async Task<ActionResult> UpdateResort(string resortName, [FromBody] ResortDTO updatedResort)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var (emailClaim, passwordClaim) = _IAuthInterface.DecodeJwtToken(token);

            if (emailClaim == null || passwordClaim == null)
            {
                return Unauthorized();
            }
            var user = await _dbUser.GetUserByPasswordAndEmail(passwordClaim, emailClaim);

            if (user == null)
            {
                return Unauthorized();
            }
            var res = await _resortData.GetResortByName(resortName);
            // Compare the Phone
            if (user.phone != res.ownerPhone)
            {
                return Forbid();
            }
            bool result = await _resortData.UpdateResort(resortName, updatedResort);
            if (result)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpDelete("{resortName}")]
        public async Task<ActionResult> DeleteResort(string resortName)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var (emailClaim, passwordClaim) = _IAuthInterface.DecodeJwtToken(token);

            if (emailClaim == null || passwordClaim == null)
            {
                return Unauthorized();
            }
            var user = await _dbUser.GetUserByPasswordAndEmail(passwordClaim, emailClaim);

            if (user == null)
            {
                return Unauthorized();
            }
            var res = await _resortData.GetResortByName(resortName);
            // Compare the Phone
            if (user.phone != res.ownerPhone)
            {
                return Forbid();
            }

            await _resortData.DeleteResort(resortName);
            return NoContent();
        }
    }
}
