using BlazorServerSignalApp.Data;
using BlazorServerSignalApp.IService;
using MongoDB.Driver;
using System.Runtime.CompilerServices;

namespace BlazorServerSignalApp.Service
{
    public class ServerService : IServerService
    {
        private MongoClient _mongoClient = null;
        private IMongoDatabase _database = null;
        IMongoCollection<Server> _serverTable = null;

        public Server _currentServer;
        public Server CurrentServer { get => _currentServer; set => _currentServer = value; }

        public ServerService()
        {
            _mongoClient = new MongoClient();
            _database = _mongoClient.GetDatabase("ChatDB");
        }
        //requires channelName because channel instance might not exist yet
        public void CreateServer(Server server, string channelName)
        {
            _serverTable = _database.GetCollection<Server>
                    ($"{server.Name}_{channelName}");
            _serverTable.InsertOne(server);
        }
        public void JoinServer(Server server)
        {
            _currentServer = server;
        }

        public Server AddDefaultServer(User user)
        {
            //make default so we have something to check for
            Server server = new Server();
            server.Name = "Haha You Clowns";
            server.TextChannels = new List<TextChannel>()
            {
                new TextChannel()
                {
                    Name = "Default",
                    ChatLogs = new List<ChatLog>()
                    {
                        new ChatLog()
                        {
                            User = "Dad",
                            Message = "Mom would be so proud."
                        }
                    }
                }
            };
            server.Users = new List<User>() { user };

            //name of collection
            string collectionName = $"{server.Name}_{server.TextChannels.FirstOrDefault().Name}";
            //query for collection
            _serverTable = _database.GetCollection<Server>
                (collectionName);

            if (_serverTable == null)
            {
                //if null create collection
                _database.CreateCollection("collectionName");
            }
            //create collection returns null, so retrieving created collection
            _serverTable = _database.GetCollection<Server>
                (collectionName);
            //searching collection for server
            var serverObj = _serverTable.Find(x => x.Name == server.Name).FirstOrDefault();
            if (serverObj == null)
            {
                _serverTable.InsertOne(server);
                serverObj = server;
            }
            return serverObj;
        }
    }
}

