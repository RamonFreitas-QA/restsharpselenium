using Itau.Processo.Core.Data;
using Itau.Processo.Core.Utils.Log;
using Itau.Processo.Core.Utils.Log.Utils;
using Itau.Processo.Test.API.PetzStore.Commons;
using Itau.Processo.Test.API.PetzStore.Model;
using NUnit.Framework;

namespace Itau.Processo.Test.API.PetzStore.Tests.User
{
    public class UserRegressionTests : CommonsMethods
    {
        private Users User;

        [Test]
        [Order(1)]
        public void CreateUser()
        {
            Logg.Information($"Lendo json com dados do fluxo usuarios");
            var userDataJson = ContextSettings.ReturnJson().Data.PetStore.API.UserRegressionTest;

            User = new Users
            {
                id = Utility.RandomNumber(9999, 99999),
                userName = userDataJson.UserName,
                firstName = userDataJson.FirstName,
                lastName = userDataJson.LastName,
                email = userDataJson.Email,
                password = Utility.RandomNumber(9999, 99999).ToString(),
                telefone = $" {Utility.RandomNumber(1000, 9999)} - {Utility.RandomNumber(1000, 9999)}",
                userStatus = 0
            };

            Logg.Information($"Criando usuario {User.userName}");
            var response = Post("/v2/user", User);

            Logg.Information($"Validando usuario {User.userName}");
            Assert.True(response.Content.Contains(User.id.ToString()), $"Erro ao criar user {response.Content}");
            Assert.True(200 == (int)response.StatusCode, $"Erro no status {(int)response.StatusCode}");
        }

        [Test]
        [Order(2)]
        public void LoginUser()
        {
            Logg.Information($"Fazendo login com usuario {User.userName} criado");
            var response = Get($"/v2/user/login?username={User.userName}&password={User.password}");

            Logg.Information($"Validando usuario {User.userName}");
            Assert.True(200 == (int)response.StatusCode, $"Erro no status {(int)response.StatusCode}");
            Assert.True(response.Content.Contains("logged in user session:"), $"Erro ao criar user {response.Content}");
        }

        [Test]
        [Order(3)]
        public void EditUser()
        {
            Logg.Information($"Lendo json com dados de pet do fluxo usuarios update");
            var userDataJson = ContextSettings.ReturnJson().Data.PetStore.API.UserRegressionTest;

            var body = new Users
            {
                id = User.id,
                userName = User.userName,
                firstName = User.firstName,
                lastName = userDataJson.LastNameUpdate,
                email = userDataJson.EmailUpdate,
                password = User.password,
                telefone = User.telefone,
                userStatus = 1
            };

            Logg.Information($"Atualizando dados do usuario {User.userName} criado");
            var response = Post("/v2/user", body);
            Assert.True(200 == (int)response.StatusCode, $"Erro no status {(int)response.StatusCode}");

        } 
    }
}