using Amazon.Runtime.Internal.Transform;
using BlazorServerSignalApp.Data;
using BlazorServerSignalApp.IService;
using BlazorServerSignalApp.Pages;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Security.Claims;
using BlazorServerSignalApp.IService;

namespace BlazorServerSignalApp.Service
{
    public class UserService : IUserService
    {
        private MongoClient _mongoClient = null;
        private IMongoDatabase _database = null;
        private IMongoCollection<User> _userTable = null;

        private IServerService _serverService;

        public bool isLoggedIn;
        private User? _currentUser;
        public User CurrentUser { get => _currentUser; set => _currentUser = value; }
        public bool IsLoggedIn { get => isLoggedIn; set => isLoggedIn = value; }

        public UserService(IServerService serverService)
        {
            _mongoClient = new MongoClient();
            _database = _mongoClient.GetDatabase("ChatDB");
            _userTable = _database.GetCollection<User>("User");
            _serverService = serverService;
        }
        public User GetUser(string email)
        {
            return _userTable.Find(x => x.Email == email).FirstOrDefault();
        }
        public void SetCurrentUser(string email)
        {
            _currentUser = _userTable.Find(x => x.Email == email).FirstOrDefault();
        }
        public void SaveOrUpdate(User user)
        {
            var userObj = _userTable.Find(x => x.Email == user.Email).FirstOrDefault();
            if (userObj == null)
            {
                _userTable.InsertOne(user);
            }
            else
            {
                _userTable.ReplaceOne(x => x.Email == user.Email, user);
            }
        }
        public void Logout()
        {
            _currentUser = null;
            isLoggedIn = false;
        }
        public void ExternalLoginProcess(string userEmail)
        {

            var userObj = _userTable.Find(x => x.Email == userEmail).FirstOrDefault();
            if (userObj == null)
            {
                //it seems like userObj can't be used after returning null
                User userObj2 = new User();
                userObj2.Email = userEmail;
                userObj2.Servers = new List<Server>();

                //adds default server to userObj2
                userObj2.Servers.Add(_serverService.AddDefaultServer(userObj2));
                _userTable.InsertOne(userObj2);
                _currentUser = userObj2;
            }
            else
            {
                _currentUser = userObj;
            }

            isLoggedIn = true;
        }
        public void AddServerToUser(User user, Server server)
        {
            var userObj = GetUser(user.Email);
            userObj.Servers.Add(server);
            _currentUser = userObj;
        }

    }
}
