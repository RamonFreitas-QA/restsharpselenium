using RestSharp;

namespace Itau.Processo.Core.RestSharp
{
    public class ConnectAPI
    {
        private string Url { get;  }
        private string PartialUrl { get; }
        private string Token { get; }
        private string SessionId { get; }
        private RestClient client;
        private RestRequest request;


        public ConnectAPI(string url, string partialUrl, string token, string session = "")
        {
            Url = url;
            PartialUrl = partialUrl;
            Token = token;
            SessionId = session;
        }

        public IRestResponse Get()
        {
            CreateClient(true);
            CreateRequest(true);

            return client.Get(request);
        }

        public IRestResponse Post(object body)
        {
            CreateClient();
            CreateRequest();
            request.AddJsonBody(body);

            return client.Post(request);
        }

        public IRestResponse Put(object body)
        {
            CreateClient();
            CreateRequest();
            request.AddJsonBody(body);

            return client.Post(request);
        }

        public IRestResponse Patch(object body)
        {
            CreateClient();
            CreateRequest();
            request.AddJsonBody(body);

            return client.Patch(request);
        }

        public IRestResponse Delete()
        {
            CreateClient();
            CreateRequest();

            return client.Delete(request);
        }

        private void CreateRequest(bool getMethod = false)
        {
            request = new RestRequest(PartialUrl);
            if (!getMethod)
            {
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("api_key", "special-key");
                request.AddCookie("JSESSIONID", SessionId);
            }
        }

        private void CreateClient(bool getMethod = false)
        {
            client = new RestClient(Url);
            if (!getMethod)
                client.AddDefaultHeader("Authorization", $"Bearer {Token}");

        }
    }
}
