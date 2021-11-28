using ChatApp.Application.Interface;
using RestSharp;

namespace ChatApp.Application.Service
{
    public class RequestService : IRequestService
    {
        #region Fields


        #endregion

        #region Constructors

        public RequestService()
        {
        }

        #endregion

        #region Methods
        
        public string SendRequest(string host, string json, Method method)
        {
            var client = new RestClient(host);

            var request = new RestRequest(method);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        #endregion

    }
}
