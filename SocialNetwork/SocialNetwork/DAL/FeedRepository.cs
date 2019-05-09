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

        public void RemoveFeedById(string id)
        {
            _feeds.DeleteOne(f => f.FeedId == id);
        }
        public Feed GetFeedById(string id)
        {
            return _feeds.Find(f => f.FeedId == id).FirstOrDefault();
        }

        public void UpdateFeed(Feed update)
        {
            _feeds.ReplaceOne(f => f.FeedId == update.FeedId, update);
        }

        public void RemoveFeed(Feed remove)
        {
            _feeds.DeleteOne(f => f.FeedId == remove.FeedId);
        }

        public void AddPost(string feedId, string insert)
        {
            _feeds.Find(f => f.FeedId == feedId).FirstOrDefault().Posts.Add(insert);
        }

        public void InsertFeedFromUser(string user)
        {
            Feed f = new Feed
            {
                User = user,
                Posts = new List<string>()
            };
            _feeds.InsertOne(f);
        }

        public Feed GetFeedByUserId(string id)
        {
            return _feeds.Find(f => f.User == id).FirstOrDefault();
        }
    }
}
