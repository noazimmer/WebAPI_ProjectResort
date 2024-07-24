using DAL.DTO;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MODELS;

namespace WebAPIproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResortController : ControllerBase
    {
        private readonly IResortInterface _resortData;
        public ResortController(IResortInterface resortData)
        {
            _resortData = resortData;
        }
        // יצירת ריזורט חדש
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

        // שליפת כל הריזורטים
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resort>>> GetAllResorts()
        {
            var resorts = await _resortData.GetAllResorts();
            return Ok(resorts);
        }

        // שליפת ריזורט לפי מזהה (שם הריזורט)
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

        // עדכון ריזורט קיים
        [HttpPut("{resortName}")]
        public async Task<ActionResult> UpdateResort(string resortName, [FromBody] ResortDTO updatedResort)
        {
            bool result = await _resortData.UpdateResort(resortName, updatedResort);
            if (result)
            {
                return NoContent();
            }
            return BadRequest();
        }

        // מחיקת ריזורט לפי מזהה (שם הריזורט)
        [HttpDelete("{resortName}")]
        public async Task<ActionResult> DeleteResort(string resortName)
        {
            await _resortData.DeleteResort(resortName);
            return NoContent();
        }
    }
}
