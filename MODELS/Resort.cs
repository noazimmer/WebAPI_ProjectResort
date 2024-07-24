using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODELS
{
    public class Resort

    {

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string resortName { get; set; }
        public string ownerPhone { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public bool isPool { get; set; }
        public int numOfBeds { get; set; }
        public Stack<Reaction> reactions { get; set; } = new Stack<Reaction>(); // אתחול ברירת מחדל
    }
}

