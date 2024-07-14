using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class UserDTO
    {
        public String id { get; set; }
       
        public String name { get; set; }
      
        public String password { get; set; }
      
        public String email { get; set; }
      
        public String phone { get; set; }
    }
}
