using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorServerSignalApp.Data
{
    public class ChatLog
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
        public string User { get; set; } = "";
        public string Message { get; set; } = "";
        public string Group { get; set; } = "";
    }
}
