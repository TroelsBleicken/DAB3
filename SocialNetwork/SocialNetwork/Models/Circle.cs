
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    public class Circle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CircleId { get; set; }
        public string Name { get; set; }
        public string WallId { get; set; }
        public List<string> Users { get; set; }

        public Circle()
        {
            Users = new List<string>();
        }
    }
}
