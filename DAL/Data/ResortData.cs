using DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using MODELS;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using AutoMapper;
using DAL.DTO;


namespace DAL.Data
{
    public class ResortData: IResortInterface
    {
        private readonly IMongoCollection<Resort> _resortsCollection;
        private readonly IMapper _mapper;
        public ResortData(IMongoClient mongoClient, string databaseName, IMapper mapper)
        {
            var database = mongoClient.GetDatabase(databaseName);
            _resortsCollection = database.GetCollection<Resort>("Resorts");
            _mapper = mapper;
        }

        // יצירת ריזורט חדש
        public async Task<bool> CreateResort(ResortDTO resort)
        {
            var resortModel = _mapper.Map<Resort>(resort);
            resortModel.reactions = new Stack<Reaction>(); // אתחול ברירת מחדל

            await _resortsCollection.InsertOneAsync(resortModel);
            return true;
        }

        // שליפת כל הריזורטים
        public async Task<List<Resort>> GetAllResorts()
        {
            return await _resortsCollection.Find(_ => true).ToListAsync();
        }

        // שליפת ריזורט לפי מזהה (שם הריזורט, למשל)
        public async Task<Resort> GetResortByName(string resortName)
        {
            return await _resortsCollection.Find(r => r.resortName == resortName).FirstOrDefaultAsync();
        }

        // עדכון ריזורט קיים
        public async Task<bool> UpdateResort(string resortName, ResortDTO updatedResort)
        {
            await _resortsCollection.ReplaceOneAsync(r => r.resortName == resortName, _mapper.Map <Resort>( updatedResort));
        return true;
        }

        // מחיקת ריזורט לפי מזהה (שם הריזורט, למשל)
        public async Task DeleteResort(string resortName)
        {
            await _resortsCollection.DeleteOneAsync(r => r.resortName == resortName);
        }
    }
}
    

