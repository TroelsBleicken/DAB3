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

            _posts = database.GetCollection<Post>("Posts");
        }

        public List<Post> GetPosts()
        {
            return _posts.Find(post => true).ToList();
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

        public Post CreatePost(Post post)
        {
            _posts.InsertOne(post);
            return post;
        }

        public void UpdatePost(string id, Post postIn)
        {
            var filter = Builders<Post>.Filter.Eq(p => p.PostId, id);
            var result = _posts.ReplaceOne(filter, postIn, new UpdateOptions{IsUpsert = true});
        }
        
        public void RemovePost(string id)
        {
            _posts.DeleteOne(p => p.PostId == id);
        }

        public void AddComment(string postId, string userId ,string cmnt)
        {
            var post = GetPost(postId);
            var comment = new Comment
            {
                CommenterId = userId,
                PostId = postId,
                Text = cmnt,
                TimeStamp = DateTime.Now
            };
            post.Comments.Add(comment.CommentId);
            UpdatePost(post.PostId, post);
        }
    }
}
