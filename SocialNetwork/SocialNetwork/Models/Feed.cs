using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    public class Feed
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FeedId { get; set; }

        public List<string> Posts { get; set; } = new List<string>();
        public string User { get; set; }
    }
}
