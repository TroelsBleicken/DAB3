
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{

    public class Comment
    {
        public string Commenter { get; set; }
        public string Text { get; set; }

    }
}
