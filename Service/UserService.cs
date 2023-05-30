using BlazorServerSignalApp.Data;
using BlazorServerSignalApp.IService;
using BlazorServerSignalApp.Pages;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace BlazorServerSignalApp.Service
{
    public class UserService : IUserService
    {
        private MongoClient _mongoClient = null;
        private IMongoDatabase _database = null;
        private IMongoCollection<User> _userTable = null;

        public bool wasSuccessful;
        public bool UniqueName;
        private Random random = new Random();
        private User _currentUser;
        public User CurrentUser { get => _currentUser; set => _currentUser = value; }
        public bool WasSuccessful { get => wasSuccessful; set => wasSuccessful = value; }

        public UserService()
        {
            _mongoClient = new MongoClient();
            _database = _mongoClient.GetDatabase("ChatDB");
            _userTable = _database.GetCollection<User>("User");
        }
        public User RegisterUser(User user)
        {
            user.Username = GenerateUniqueUsername(user.Username);

            SaveOrUpdate(user);
            wasSuccessful = true;
            _currentUser = user;
            return user;
        }
        private string GenerateUniqueUsername(string usernameInput)
        {
            int min = 0001;
            int max = 9999;
            int number = random.Next(min, max);
            var username = String.Concat(usernameInput + "#" + number);
            while (!IsUsernameUnique(username))
            {
                GenerateUniqueUsername(usernameInput);
            }
            return username;
        }
        private bool IsUsernameUnique(string username)
        {
            var userCheck = GetUser(username);
            if (userCheck == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //CREATE BUTTON JUST TO TEST IF USERNAME IS AVAILABLE SO YOU CAN SUS OUT DB PROBLEM

        public User GetUser(string username)
        {
            return _userTable.Find(x => x.Username == username).FirstOrDefault();
        }
        public void SaveOrUpdate(User user)
        {
            var userObj = _userTable.Find(x => x.Username == user.Username).FirstOrDefault();
            if (userObj == null)
            {
                _userTable.InsertOne(user);
            }
            else
            {
                _userTable.ReplaceOne(x => x.Username == user.Username, user);
            }
        }
        public User Login(string username, string userPassword)
        {
            //Let user login with name excluding unique identifier number
            var user = GetUser(username);
            if (user != null && user.Password == userPassword)
            {
                _currentUser = user;
                wasSuccessful = true;
                return user;
            }
            else
                wasSuccessful = false;
                return null;
        }
        public void Logout()
        {
            _currentUser = null;
            wasSuccessful = false;
        }
    }
}
