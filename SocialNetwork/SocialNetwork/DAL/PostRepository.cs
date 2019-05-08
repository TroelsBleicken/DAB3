using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.DAL
{
    public class PostRepository
    {
        private readonly IMongoCollection<Post> _posts;

        public PostRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("SocialNetworkDb"));
            var database = client.GetDatabase("SocialNetworkDb");

            if (!MongoDbHelpFunctions.CollectionExists(database, "Posts"))
                database.CreateCollection("Posts");
            _posts = database.GetCollection<Post>("Users");
        }

        public Post GetPost(string id)
        {
            return _posts.Find<Post>(post => post.PostId == id).FirstOrDefault();
        }

        public List<Post> GetPostsByUser(string id)
        {
            return _posts.Find(post => post.OwnerId == id).ToList();
        }

        public List<Post> GetPostsByUserCircle(string id)
        {
            return _posts.Find(post => post.CircleId == id).ToList();
        }
    }
}
