using AutoMapper;
using DAL.DTO;
using DAL.Interfaces;
using MODELS;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;
namespace DAL.Data
{
    public class ReactionData: IReactionInterface
    {
        private readonly IMongoCollection<Resort> _resortsCollection;
        private readonly IMapper _mapper;
        public ReactionData(IMongoClient mongoClient, string databaseName, IMapper mapper)
        {
            var database = mongoClient.GetDatabase(databaseName);
            _resortsCollection = database.GetCollection<Resort>("Resorts");
            _mapper = mapper;
        }

        // הוספת תגובה חדשה לריזורט
        public async Task<bool> AddReaction(string resortName, ReactionDTO reaction)
        {
            var filter = Builders<Resort>.Filter.Eq(r => r.resortName, resortName);
            var update = Builders<Resort>.Update.Push(r => r.reactions, _mapper.Map<Reaction>(reaction));
            await _resortsCollection.UpdateOneAsync(filter, update);
            return true;
        }

        // שליפת כל התגובות לריזורט מסוים
        public async Task<Stack<Reaction>> GetReactions(string resortName)
        {
            var resort = await _resortsCollection.Find(r => r.resortName == resortName).FirstOrDefaultAsync();
            return resort?.reactions;
        }

       

        // מחיקת תגובה לריזורט
        public async Task DeleteReaction(string resortName, string advertiserName, DateTime dateTime)
        {
            var resort = await _resortsCollection.Find(r => r.resortName == resortName).FirstOrDefaultAsync();
            if (resort != null)
            {
                var reactions = resort.reactions.ToList();
                reactions.RemoveAll(r => r.advertiserName == advertiserName && r.dateTime == dateTime);
                var update = Builders<Resort>.Update.Set(r => r.reactions, new Stack<Reaction>(reactions));
                await _resortsCollection.UpdateOneAsync(r => r.resortName == resortName, update);
            }
        }
    }
}


//  צריך לתקן את הקוד הזה הוא לא עובד )עדכון תגובה קיימת לריזורט)
//public async Task<bool> UpdateReaction(string resortName, ReactionDTO updatedReaction)
//{
//    var resort = await _resortsCollection.Find(r => r.resortName == resortName).FirstOrDefaultAsync();
//    if (resort != null)
//    {
//        var reactions = resort.reactions.ToList();
//        var index = reactions.FindIndex(r => r.advertiserName == _mapper.Map<Reaction>(updatedReaction).advertiserName && r.dateTime == _mapper.Map<Reaction>(updatedReaction).dateTime);
//        if (index >= 0)
//        {
//            reactions[index] = _mapper.Map < Reaction > (updatedReaction);
//            var update = Builders<Resort>.Update.Set(r => r.reactions, new Stack<Reaction>(reactions));
//            await _resortsCollection.UpdateOneAsync(r => r.resortName == resortName, update);
//        }
//    }
//    return true;
//}