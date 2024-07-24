using MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;
namespace DAL.Interfaces
{
    public interface IResortInterface
    {
        public  Task<bool> CreateResort(ResortDTO resort);
        public  Task<List<Resort>> GetAllResorts();
        public  Task<Resort> GetResortByName(string resortName);
        public  Task<bool> UpdateResort(string resortName, ResortDTO updatedResort);
        public Task DeleteResort(string resortName);
    }
}
