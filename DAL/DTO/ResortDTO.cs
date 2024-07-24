using MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class ResortDTO
    {
        public string ownerPhone { get; set; }
        public string resortName { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public bool isPool { get; set; }
        public int numOfBeds { get; set; }
       
    }
}
