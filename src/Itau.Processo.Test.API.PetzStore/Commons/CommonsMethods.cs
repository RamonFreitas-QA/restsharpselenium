using Itau.Processo.Core.Data;
using Itau.Processo.Core.RestSharp;
using Itau.Processo.Core.Utils.Log;
using Itau.Processo.Core.Utils.Log.Utils;
using NUnit.Framework;
using RestSharp;
using System.Diagnostics;

namespace Itau.Processo.Test.API.PetzStore.Commons
{

    public class CommonsMethods : ContextSettings
    {
        private IRestResponse response;
        private static string Session;
        private int Retry = 0;
        private string loginUrl = "/oauth/login";

        public CommonsMethods()
        {
            if (string.IsNullOrEmpty(Session))
            {
                Session = LoginOAuths();
                Logg.Information($"Obter session {Session}");
            }
        }

        private string LoginOAuths()
        {
            var body = new
            {
                username = GetSection("Data:PetStore:API:User"),
                password = GetSection("Data:PetStore:API:Password")
            };

            return Post(loginUrl, body).Cookies[0].Value;
        }

        public IRestResponse Post(string middleUrl, object body)
        {
            Logg.Information($"Method: {new StackTrace().GetFrame(0).GetMethod().Name}, Url: {middleUrl}, Body: {body}, Session: {Session}");

            response = new ConnectAPI(GetSection("Data:PetStore:API:Url"), middleUrl, "", Session).Post(body);

            while (!response.IsSuccessful && Retry++ < 5)
            {
                if ((int)response.StatusCode == 302)
                    break;

                response = new ConnectAPI(GetSection("Data:PetStore:API:Url"), middleUrl, "", Session).Post(body);
                Utility.Delay(2000);
            }

            CheckStatusResponse(response);

            return response;
        }

        public IRestResponse Get(string middleUrl)
        {
            Logg.Information($"Method: {new StackTrace().GetFrame(0).GetMethod().Name}, Url: {middleUrl}, Session: {Session}");

            response = new ConnectAPI(GetSection("Data:PetStore:API:Url"), middleUrl, "", Session).Get();

            while ((int)response.StatusCode != 200 && Retry++ < 5)
            {
                response = new ConnectAPI(GetSection("Data:PetStore:API:Url"), middleUrl, "", Session).Get();
                Utility.Delay(2000);
            }

            CheckStatusResponse(response);

            return response;
        }

        public IRestResponse Delete(string middleUrl)
        {
            Logg.Information($"Method: {new StackTrace().GetFrame(0).GetMethod().Name}, Url: {middleUrl}, Session: {Session}");

            response = new ConnectAPI(GetSection("Data:PetStore:API:Url"), middleUrl, "", Session).Delete();

            while ((int)response.StatusCode != 200 && Retry++ < 5)
            {
                response = new ConnectAPI(GetSection("Data:PetStore:API:Url"), middleUrl, "", Session).Delete();
                Utility.Delay(2000);
            }

            CheckStatusResponse(response);

            return response;
        }

        private void CheckStatusResponse(IRestResponse response)
        {
            Logg.Information(response.Content.ToString());

            if (!response.IsSuccessful && (int)response.StatusCode != 302)
                Assert.True(200 == (int)response.StatusCode, $"Error status {(int)response.StatusCode}, " +
                                                            $"Response Error Message: {response.ErrorMessage}, " +
                                                            $"Response Error Exception {response.ErrorException}");
        }
    }
}
