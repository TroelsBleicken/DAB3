using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SocialNetwork.Models;

namespace SocialNetwork.DAL
{
    public class CommentRepository
    {
        private readonly IMongoCollection<Comment> _comments;

        public CommentRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("SocialNetworkDb"));
            var database = client.GetDatabase("SocialNetworkDb");

            if (!MongoDbHelpFunctions.CollectionExists(database, "Comments"))
                database.CreateCollection("Comments");

            _comments = database.GetCollection<Comment>("Comments");
        }
        
        public List<Comment> GetComments()
        {
            return _comments.Find(comment => true).ToList();
        }

        public Comment GetComment(string id)
        {
            return _comments.Find<Comment>(comment => comment.CommentId == id).FirstOrDefault();
        }

        public Comment CreateComment(Comment comment)
        {
            _comments.InsertOne(comment);
            return comment;
        }

        public void UpdateComment(string id, Comment commentIn)
        {
            var filter = Builders<Comment>.Filter.Eq(c => c.CommentId, id);
            var result = _comments.ReplaceOne(filter, commentIn, new UpdateOptions { IsUpsert = true });
        }
   
        public void RemoveComment(Comment commentIn)
        {
            _comments.DeleteOne(comment => comment.CommentId == commentIn.CommentId);
        }

        public void RemoveComment(string id)
        {
            _comments.DeleteOne(comment => comment.CommentId == id);
        }
    }
}
