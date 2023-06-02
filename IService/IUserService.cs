using BlazorServerSignalApp.Data;
using System.Security.Claims;

namespace BlazorServerSignalApp.IService
{
    public interface IUserService
    {
        public User? CurrentUser { get; set; }
        public bool IsLoggedIn { get; set; }
        public User GetUser(string email);
        public void SetCurrentUser(string email);
        public void Logout();
        public void ExternalLoginProcess(string userEmail);
        public void AddServerToUser(User user, Server server);

    }
}
