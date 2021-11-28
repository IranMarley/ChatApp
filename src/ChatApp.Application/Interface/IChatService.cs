namespace ChatApp.Application.Interface
{
    public interface IChatService
    {
        bool SendMsg(string userName, string message, string friend);

        string ReceiveMsg(string userName);

    }
}
