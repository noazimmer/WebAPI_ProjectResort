using MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;
namespace DAL.Interfaces
{
    public interface IReactionInterface
    {
        public  Task<bool> AddReaction(string resortName, ReactionDTO reaction);
        public Task<Stack<Reaction>> GetReactions(string resortName);
        //public  Task<bool> UpdateReaction(string resortName, ReactionDTO updatedReaction);
        public  Task DeleteReaction(string resortName, string advertiserName, DateTime dateTime);

    }
}
