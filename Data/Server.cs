using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BlazorServerSignalApp.Data
{
    public class Server
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
        public string Name { get; set; }
        public string Description {  get; set; }
        public List<TextChannel> TextChannels { get; set; }
        public List<User> Users { get; set; }


    }
}