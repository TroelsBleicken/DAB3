using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public class Comment
    {
        public string Commenter { get; set; }
        public string Text { get; set; }

    }
}
