using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    public class Comment
    {
        public Comment(string commenterId, string comment)
        {

        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CommenterId { get; set; }
        public string Text { get; set; }

    }
}
