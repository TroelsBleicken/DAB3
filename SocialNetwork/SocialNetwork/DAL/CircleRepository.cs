using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SocialNetwork.Models;

namespace SocialNetwork.DAL
{
    public class CircleRepository
    {
        private readonly IMongoCollection<Circle> _circles;

        

        public CircleRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("SocialNetworkDb"));
            var database = client.GetDatabase("SocialNetworkDb");

            if(!MongoDbHelpFunctions.CollectionExists(database, "Circles"))
                database.CreateCollection("Circles");

            _circles = database.GetCollection<Circle>("Circles");
        }

        public List<Circle> GetCircles()
        {
            return _circles.Find(c => true).ToList();
        }

        public Circle GetCircleById(string id)
        {
            return _circles.Find(c => c.CircleId == id).FirstOrDefault();
        }

        public void InsertCircle(Circle circle)
        {
            circle.Users = new List<string>();
            _circles.InsertOne(circle);
        }

        public void UpdateCircle(Circle circle)
        {
            _circles.ReplaceOne(c => c.CircleId == circle.CircleId, circle);
        }

        public Circle GetCircleByName(string name)
        {
            return _circles.Find(c => c.Name == name).FirstOrDefault();
        }

        public void RemoveCircle(string id)
        {
            _circles.DeleteOne(c => c.CircleId == id);
        }
    }
    

}
