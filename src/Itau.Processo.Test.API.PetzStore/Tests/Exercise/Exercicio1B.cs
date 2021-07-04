using Itau.Processo.Core.Utils.Log;
using Itau.Processo.Test.API.PetzStore.Commons;
using Itau.Processo.Test.API.PetzStore.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Itau.Processo.Test.API.PetzStore.Tests.Exercise
{
    public class Exercicio1B : CommonsMethods
    {
        private List<Users> listUser = new List<Users>();
        private List<Pets> listUserPet = new List<Pets>();
        private List<Order> listOrder = new List<Order>();

        [Test]
        [Order(1)]
        public void CreateFiveUsers()
        {
            Logg.Information($"Criando 5 usuarios com dados aleatórios");

            for (int i = 1; i <= 5; i++)
            {
                var body = new Users
                {
                    id = new Random().Next(0, 999),
                    userName = $"User{i}",
                    firstName = $"User {i}",
                    lastName = $"{i}",
                    email = $"user{i}@automationtest-ramonfreitasqa.com",
                    password = $"123456{i}",
                    telefone = $" {new Random().Next(1000, 9999)} - {new Random().Next(100, 999)} {i}",
                    userStatus = 0
                };

                Logg.Information($"Adiciando usuario {body.userName} a lista");
                listUser.Add(body);

                Logg.Information($"Criando usuario {body.userName}");
                var response = Post("/v2/user", body);

                Logg.Information($"Validando usuario {body.userName} pelo id");
                Assert.True(response.Content.Contains(body.id.ToString()), $"Erro ao criar user {response.Content}");
                Assert.True(200 == (int)response.StatusCode, $"Erro no stastus {(int)response.StatusCode}");
            }
        }

        [Test]
        [Order(2)]
        public void CreateTenPets()
        {
            Logg.Information($"Criando 10 pets 5 cachorros e 5 gatos");

            for (int i = 1; i <= 10; i++)
            {
                var body = new Pets
                {
                    id = new Random().Next(0, 999),
                    category = new Category
                    {
                        Id = i <= 5 ? 1 : 2,
                        Name = i <= 5 ? "Cachorro" : "Gato"
                    },
                    name = i <= 5 ? $"Bingo {i}" : $"Felix {i}",
                    photoUrls = new[] { $"Foto {i}" },
                    status = "available",
                    tags = new Tags[] {
                        new Tags{
                            Id = 1,
                            Name = i <= 5 ? "Late" : "Mia" }
                    }
                };

                Logg.Information($"Adiciando pet {body.category.Name} nome {body.name} a lista");
                listUserPet.Add(body);

                Logg.Information($"Criando pet {body.category.Name} nome {body.name}");
                var response = Post("/v2/pet", body);

                Logg.Information($"Validando dados pet {body.name}");
                Assert.True(200 == (int)response.StatusCode, $"Erro no stastus {(int)response.StatusCode}");
                Assert.True(response.Content.Contains(body.id.ToString()), $"Ao ao registrar valor incorreto {response.Content}");
                Assert.True(response.Content.Contains(body.name), $"Ao ao registrar valor incorreto {response.Content}");
            }
        }

        [Test]
        [Order(3)]
        public void CreateTenOrders()
        {
            Logg.Information($"Criando um pedido do pet Gato para cada usuario");
            GenerateOrder("Gato");

            Logg.Information($"Criando um pedido do pet cachorro para cada usuario");
            GenerateOrder("Cachorro");
        }

        [Test]
        [Order(4)]
        public void ChangeStatusOrder()
        {
            Logg.Information($"Alterando status dos pedidos de cachorros para delivered");
            var listOrderCachorro = ChangeStatus(listUserPet.Where(x => x.category.Name.Equals("Cachorro")).ToList(), "delivered");

            Logg.Information($"Validando status dos pedidos de pet cachorros");
            Assert.True(listOrderCachorro.All(x => x.status.Equals("delivered")));

            Logg.Information($"Alterando status dos pedidos de gatos para approved");
            var listOrderGatos = ChangeStatus(listUserPet.Where(x => x.category.Name.Equals("Gatos")).ToList(), "approved");

            Logg.Information($"Validando status dos pedidos de cachorros");
            Assert.True(listOrderGatos.All(x => x.status.Equals("approved")));

            Logg.Information($"Validando se a lista de pedidos possui pedidos com status placed");
            Assert.True(listOrder.Any(x => x.status.Equals("placed")));
        }

        #region PrivateMethods
        private List<Order> ChangeStatus(List<Pets> listPet, string status)
        {
            Logg.Information($"Localizando pedido para alterar para o status {status}");

            var orderNew = new List<Order>();
            var orderRemove = new List<Order>();

            foreach (var order in listOrder)
            {
                foreach (var pet in listPet)
                {
                    if (order.petId == pet.id)
                    {
                        var orderNewStatus = new Order
                        {
                            id = order.id,
                            petId = pet.id,
                            userId = order.userId,
                            quantity = 1,
                            shipDate = order.shipDate,
                            status = status,
                            complete = true
                        };

                        orderRemove.Add(order);
                        orderNew.Add(orderNewStatus);

                        Logg.Information($"Alterando pedido {orderNewStatus.id} para status {orderNewStatus.status}");

                        var response = Post("/v2/store/order", orderNewStatus);
                        Assert.True(200 == (int)response.StatusCode, $"Erro no status {(int)response.StatusCode}");
                        Assert.True(response.Content.Contains(orderNewStatus.id.ToString()), $"Erro ao atualizar valor incorreto {response.Content}");
                        Assert.True(response.Content.Contains(orderNewStatus.status), $"Erro ao atualizar valor incorreto {response.Content}");
                        Assert.True(response.Content.Contains(orderNewStatus.petId.ToString()), $"Erro ao atualizar valor incorreto {response.Content}");
                    }
                }
            }

            Logg.Information($"Removendo pedidos para com status anterior");
            listOrder = listOrder.Except(orderRemove).ToList();

            Logg.Information($"Incluindo os pedidos para com status atual");
            listOrder = listOrder.Concat(orderNew).ToList();

            return orderNew;
        }



        private void GenerateOrder(string petType)
        {
            Logg.Information($"Obtendo apenas {petType} da lista de pets");
            var petList = listUserPet.Where(x => x.category.Name.Equals(petType));
            int i = 0;

            foreach (var user in listUser)
            {
                var body = new Order
                {
                    id = new Random().Next(0, 999),
                    petId = petList.ToList()[i].id,
                    userId = user.id,
                    quantity = 1,
                    shipDate = DateTime.Now,
                    status = "placed",
                    complete = true
                };

                Logg.Information($"Adicionando a lista o pedido {body.id} para o usuario {body.userId} o pet {petType} id {body.petId}");

                listOrder.Add(body);

                Logg.Information($"Criando o pedido {body.id} para o usuario {body.userId} o pet {petType} id {body.petId}");
                var response = Post("/v2/store/order", body);

                Logg.Information($"Adicionando a lista o pedido {body.id}");
                Assert.True(200 == (int)response.StatusCode, $"Erro no stastus {(int)response.StatusCode}");
                Assert.True(response.Content.Contains(body.id.ToString()), $"Erro ao registrar valor incorreto {response.Content}");
                Assert.True(response.Content.Contains(body.status), $"Erro ao registrar valor incorreto {response.Content}");
                Assert.True(response.Content.Contains(body.petId.ToString()), $"Erro ao registrar valor incorreto {response.Content}");
                i++;
            }
        }
        #endregion
    }
}
