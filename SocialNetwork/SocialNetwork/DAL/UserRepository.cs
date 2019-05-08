using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.DAL
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("SocialNetworkDb"));
            var database = client.GetDatabase("SocialNetworkDb");

            if (!MongoDbHelpFunctions.CollectionExists(database, "Users"))
                database.CreateCollection("Users");

            _users = database.GetCollection<User>("Users");
        }

        #region Interface implementation
        public List<User> GetUsers()
        {
            return _users.Find(book => true).ToList();
        }

        public User GetUser(string id)
        {
            return _users.Find<User>(user => user.UserId == id).FirstOrDefault();
        }

        public User CreateUser(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public void UpdateUser(string id, User userIn)
        {
            _users.ReplaceOne(user => userIn.UserId == id, userIn);
        }

        public void RemoveUser(User userIn)
        {
            _users.DeleteOne(user => user.UserId == userIn.UserId);
        }

        public void RemoveUser(string id)
        {
            _users.DeleteOne(user => user.UserId == id);
        }

        public void AddFollower(string id, string followerId)
        {
            var user = GetUser(id);

            user.Followers.Add(followerId);

            UpdateUser(user.UserId, user);
        }

        public void AddFollowing(string id, string followingId)
        {
            var user = GetUser(id);

            user.Following.Add(followingId);

            UpdateUser(user.UserId, user);
        }

        public void AddBlocked(string id, string BlockedId)
        {
            var user = GetUser(id);

            user.Blocked.Add(BlockedId);

            UpdateUser(user.UserId, user);
        }

        public void AddCircle(string id, string circleId)
        {
            var user = GetUser(id);

            user.Circles.Add(circleId);

            UpdateUser(user.UserId, user);
        }

        #endregion
    }
}
