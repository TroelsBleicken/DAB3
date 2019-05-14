using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SocialNetwork.Models;

namespace SocialNetwork.DAL
{
    public class WallRepository
    {
        private readonly IMongoCollection<Wall> _walls;

        public WallRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("SocialNetworkDb"));
            var database = client.GetDatabase("SocialNetworkDb");

            if (!MongoDbHelpFunctions.CollectionExists(database, "Walls"))
                database.CreateCollection("Walls");

            _walls = database.GetCollection<Wall>("Walls");
        }

        public void AddWall(Wall wall)
        {
            _walls.InsertOne(wall);
        }

        public void Update(Wall wall)
        {
            _walls.ReplaceOne(w => w.WallId == wall.WallId, wall);

        }

        public void AddPostToWall(string id, string postId)
        {
            var wall = _walls.Find(w => w.WallId == id).FirstOrDefault();
            wall.Posts.Add(postId);
            _walls.ReplaceOne(w => w.WallId == id, wall);
        }

        public Wall GetWallById(string id)
        {
           return _walls.Find(w => w.WallId == id).FirstOrDefault();
        }

        public List<Wall> GetAll()
        {
            return _walls.Find(w => true).ToList();
        }

        public void Delete(string id)
        {
            _walls.DeleteOne(w => w.WallId == id);
        }

        public Wall GetWallByUserId(string id)
        {
           return _walls.Find(w => w.User == id).FirstOrDefault();
        }

        public List<string> GetAllPosts(string id)
        {
            var _wall = _walls.Find(w => w.User == id).FirstOrDefault();
            return _wall.Posts;
        }
    }
}
