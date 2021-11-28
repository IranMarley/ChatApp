using RestSharp;

namespace ChatApp.Application.Interface
{
    public interface IRequestService
    {
        string SendRequest(string host, string json, Method method);
    }
}
