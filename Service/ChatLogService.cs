using BlazorServerSignalApp.Data;
using BlazorServerSignalApp.IService;
using MongoDB.Driver;

namespace BlazorServerSignalApp.Service
{
    public class ChatLogService : IChatLogService
    {
        private MongoClient _mongoClient = null;
        private IMongoDatabase _database = null;
        private IMongoCollection<ChatLog> _chatLogTable = null;
        public ChatLogService()
        {
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            _database = _mongoClient.GetDatabase("ChatDB");
            _chatLogTable = _database.GetCollection<ChatLog>("ChatLog");
        }
        public string Delete(string chatLogId)
        {
            _chatLogTable.DeleteOne(x => x.Id == chatLogId);
            return "Deleted";
        }

        public ChatLog GetChatLog(string chatLogId)
        {
            return _chatLogTable.Find(x => x.Id == chatLogId).FirstOrDefault();
        }

        public List<ChatLog> GetGroupChatLog(string group)
        {
            //Filter creation
            var filter = Builders<ChatLog>.Filter.Eq("Group", group);
            return _chatLogTable.Find(filter).ToList();
        }

        public List<ChatLog> GetChatLogs()
        {
            return _chatLogTable.Find(FilterDefinition<ChatLog>.Empty).ToList();
        }

        public void SaveOrUpdate(ChatLog chatLog)
        {
            var chatLogObj = _chatLogTable.Find(x => x.Id == chatLog.Id).FirstOrDefault();
            if (chatLogObj == null)
            {
                _chatLogTable.InsertOne(chatLog);
            }
            else
            {
                _chatLogTable.ReplaceOne(x => x.Id == chatLog.Id, chatLog);
            }
        }
        public void DeleteAllDocuments()
        {
            var filter = Builders<ChatLog>.Filter.Empty;
            _chatLogTable.DeleteMany(filter);
        }
    }
}
