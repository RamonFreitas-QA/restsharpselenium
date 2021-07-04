using Itau.Processo.Core.Data;
using Itau.Processo.Core.Utils.Log;
using Itau.Processo.Core.Utils.Log.Utils;
using Itau.Processo.Test.API.PetzStore.Commons;
using Itau.Processo.Test.API.PetzStore.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace Itau.Processo.Test.API.PetzStore.Tests.Exercise
{
    class Exercicio1A : CommonsMethods
    {
        private Users User;
        private Pets Pet;
        private Order Order;

        [Test]
        [Order(1)]
        public void CreateUser()
        {
            Logg.Information($"Lendo json com dados de usuario do exercicios 1 A");
            var userDataJson = ContextSettings.ReturnJson().Data.PetStore.API.Exercicio1A.User;

            User = new Users
            {
                id = Utility.RandomNumber(9999, 99999),
                userName = Utility.GenerateHashString(10),
                firstName = userDataJson.FirstName,
                lastName = userDataJson.LastName,
                email = userDataJson.Email,
                password = Utility.RandomNumber(9999, 99999).ToString(),
                telefone = $" {Utility.RandomNumber(1000, 9999)} - {Utility.RandomNumber(1000, 9999)}",
                userStatus = 0
            };

            Logg.Information($"Criando usuario {User.userName}");

            var response = Post("/v2/user", User);
            Assert.True(response.Content.Contains(User.id.ToString()), $"Erro ao criar user {response.Content}");
        }

        [Test]
        [Order(3)]
        public void CreatePet()
        {
            Logg.Information($"Lendo json com dados de pet do exercicios 1 A");
            var petDataJson = ContextSettings.ReturnJson().Data.PetStore.API.Exercicio1A.Pet;

            Pet = new Pets
            {
                id = 10,
                category = new Category
                {
                    Id = 10,
                    Name = petDataJson.Category
                },
                name = petDataJson.Name,
                photoUrls = new[] { "Foto 1" },
                tags = new Tags[] {
                        new Tags{
                            Id = 1,
                            Name = petDataJson.Status }
                    },
                status = petDataJson.Status,
            };

            Logg.Information($"Criando pet {Pet.name}");
            var response = Post("/v2/pet", Pet);

            Assert.True(200 == (int)response.StatusCode, $"Erro no status {(int)response.StatusCode}");
            Assert.True(response.Content.Contains(Pet.id.ToString()), $"Erro ao criar pet {response.Content}");
        }

        [Test]
        [Order(4)]
        public void CheckPetCreated()
        {
            Logg.Information($"Validando dados do pet {Pet.name} id {Pet.id}");

            var response = Get($"/v2/pet/{Pet.id}");
            Assert.True(200 == (int)response.StatusCode, $"Erro no status {(int)response.StatusCode}");
        }

        [Test]
        [Order(5)]
        public void CreateOrderPlaced()
        {
            Order = new Order
            {
                id = new Random().Next(0, 999),
                petId = Pet.id,
                userId = User.id,
                quantity = 1,
                shipDate = DateTime.Now,
                status = "placed",
                complete = true
            };

            Logg.Information($"Criando pedido id {Order.id}");

            var response = Post("/v2/store/order", Order);
            Assert.True(200 == (int)response.StatusCode, $"Erro no status {(int)response.StatusCode}");
            Assert.AreEqual("placed", Order.status, $"Erro no status {Order.status} do pedido");
            Assert.True(response.Content.Contains(Order.id.ToString()), $"Erro ao registrar pedido {response.Content}");
        }

        [Test]
        [Order(6)]
        public void CheckOrder()
        {
            Logg.Information($"Criando pedido criado {Order.id} com usuario {Order.id} e pet {Pet.id}");
            var response = Get($"/v2/store/order/{Order.id}");
            Assert.True(200 == (int)response.StatusCode, $"Erro no status {(int)response.StatusCode}");
            Assert.AreEqual("placed", Order.status, $"Erro no status {Order.status} do pedido");
        }

        [Test]
        [Order(7)]
        public void CreateOrderAproved()
        {
            var orderApproved = new Order
            {
                id = Order.id,
                petId = Pet.id,
                userId = User.id,
                quantity = 1,
                shipDate = DateTime.Now,
                status = "approved",
                complete = true
            };

            Logg.Information($"Alterando status do pedido {Order.id} para {Order.status} ");

            var response = Post("/v2/store/order", orderApproved);
            Order = JsonConvert.DeserializeObject<Order>(response.Content);

            Logg.Information($"Validando status do pedido para {Order.status} ");
            Assert.AreEqual("approved", Order.status, $"Erro no status {Order.status} do pedido"); ;


        }

        [Test]
        [Order(8)]
        public void CreateOrderDelivered()
        {
            var orderDelivered = new Order
            {
                id = Order.id,
                petId = Pet.id,
                userId = User.id,
                quantity = 1,
                shipDate = DateTime.Now,
                status = "delivered",
                complete = true
            };

            Logg.Information($"Alterando status do pedido {Order.id} para {Order.status} ");

            var response = Post("/v2/store/order", orderDelivered);
            Order = JsonConvert.DeserializeObject<Order>(response.Content);

            Logg.Information($"Validando status do pedido para {Order.status} ");
            Assert.AreEqual("delivered", Order.status, $"Erro no status {Order.status} do pedido"); ;
            Assert.True(200 == (int)response.StatusCode, $"Erro no status {(int)response.StatusCode}");
        }
    }
}
