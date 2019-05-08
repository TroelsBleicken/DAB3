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

        public void InsertFeed(Feed newFeed)
        {
            _feeds.InsertOne(newFeed);
        }

        public List<Feed> GetFeeds()
        {
            return _feeds.Find<Feed>(f => true).ToList();
        }

        public Feed GetFeedById(string id)
        {
            return _feeds.Find(f => f.FeedId == id);
        }
    }
}
