using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace MODELS
{
    public class User
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public String id { get; set; }
        //[BsonElement("name")]
        public String name { get; set; }
        //[BsonElement("password")]
        public String password { get; set; }
        //[BsonElement("email")]
        public String email { get; set; }
        //[BsonElement("phone")]
        public String phone { get; set; }
   
    }
}
