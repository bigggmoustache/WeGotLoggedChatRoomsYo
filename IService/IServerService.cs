using BlazorServerSignalApp.Data;

namespace BlazorServerSignalApp.IService
{
    public interface IServerService
    {
        public Server CurrentServer { get; set; }
        public void CreateServer(Server server, string channelName);
        public void JoinServer(Server server);
        public Server AddDefaultServer(User user);
    }
}
