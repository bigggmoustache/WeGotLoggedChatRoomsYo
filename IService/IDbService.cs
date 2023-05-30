using BlazorServerSignalApp.Data;

namespace BlazorServerSignalApp.IService
{
    public interface IDbService
    {
        List<Server> GetServerList();
    }
}
