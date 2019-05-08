using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public List<string> Followers {get; set;}
        public List<string> Following { get; set; }
        public List<string> Blocked { get; set; }
        public List<string> Circles { get; set; }
    }
}
