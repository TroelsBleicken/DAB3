﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string OwnerId { get; set; }
        public string CircleId { get; set; }
        public DateTime CreationTime { get; set; }
        public Detail Details { get; set; }
        public List<string> Comments { get; set; }

        public Post()
        {
            if(Comments == null)
                Comments = new List<string>();
        }
    }
}
