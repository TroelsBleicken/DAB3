using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SocialNetwork.Models;

namespace SocialNetwork.DAL
{
    public class FeedRepository
    {
        private readonly IMongoCollection<Feed> _feeds;

        public FeedRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("SocialNetworkDb");
            _feeds = database.GetCollection<Feed>("Feeds");
        }

    }
}
