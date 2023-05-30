using BlazorServerSignalApp.Data;

namespace BlazorServerSignalApp.IService
{
    public interface IChatLogService
    {
        void SaveOrUpdate(ChatLog chatLog);
        ChatLog GetChatLog(string chatLogId);
        List<ChatLog> GetGroupChatLog(string group);
        List<ChatLog> GetChatLogs();
        string Delete(string chatLogId);
        void DeleteAllDocuments();
        
    }
}

