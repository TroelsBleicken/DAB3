using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    public class Wall
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string WallId { get; set; }
        public List<string> Posts { get; set; }
        public string User { get; set; }
        public string Circle { get; set; }
    }
}
