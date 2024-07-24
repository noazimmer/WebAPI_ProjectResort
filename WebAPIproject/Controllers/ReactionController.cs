using DAL.DTO;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MODELS;

namespace WebAPIproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController: ControllerBase
    {
        private readonly IReactionInterface _reactionData;
        public ReactionController(IReactionInterface reactionData)
        {
            _reactionData = reactionData;
        }

        // הוספת תגובה חדשה לריזורט
        [Authorize]
        [HttpPost("{resortName}/addReaction")]
        public async Task<ActionResult> AddReaction(string resortName, [FromBody] ReactionDTO reaction)
        {
            bool result = await _reactionData.AddReaction(resortName, reaction);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        // שליפת כל התגובות לריזורט מסוים
        [Authorize]
        [HttpGet("{resortName}/reactions")]
        public async Task<ActionResult<IEnumerable<Reaction>>> GetReactions(string resortName)
        {
            var reactions = await _reactionData.GetReactions(resortName);
            if (reactions == null)
            {
                return NotFound();
            }
            return Ok(reactions);
        }

        //// עדכון תגובה קיימת לריזורט
        //[HttpPut("{resortName}/updateReaction")]
        //public async Task<ActionResult> UpdateReaction(string resortName, [FromBody] ReactionDTO updatedReaction)
        //{
        //    bool result = await _reactionData.UpdateReaction(resortName, updatedReaction);
        //    if (result)
        //    {
        //        return NoContent();
        //    }
        //    return BadRequest();
        //}

        // מחיקת תגובה לריזורט
        [Authorize]
        [HttpDelete("{resortName}/deleteReaction")]
        public async Task<ActionResult> DeleteReaction(string resortName, string advertiserName, DateTime dateTime)
        {
            await _reactionData.DeleteReaction(resortName, advertiserName, dateTime);
            return NoContent();
        }
    }
}
    
