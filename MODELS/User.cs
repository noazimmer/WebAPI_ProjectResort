using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace MODELS
{
    public class User
    {
       
        public String id { get; set; }
    
        public String name { get; set; }
       
        public String password { get; set; }
      
        public String email { get; set; }
     
        public String phone { get; set; }
   
    }
}
