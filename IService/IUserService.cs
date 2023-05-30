using BlazorServerSignalApp.Data;

namespace BlazorServerSignalApp.IService
{
    public interface IUserService
    {
        public User CurrentUser { get; set; }
        public bool WasSuccessful { get; set; }
        public User RegisterUser(User user);
        public User GetUser(string user);
        public User Login(string username, string password);
        public void Logout();
    }
}
