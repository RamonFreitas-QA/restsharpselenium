using Itau.Processo.Core.Data;
using Itau.Processo.Core.Utils.Log;
using Itau.Processo.Test.Web.PetzStore.Commons;
using Itau.Processo.Test.Web.PetzStore.Model;
using Itau.Processo.Test.Web.PetzStore.PageObject;
using NUnit.Framework;

namespace Itau.Processo.Test.Web.PetzStore.Tests.Exercicios
{
    public class TestExercise2A : CommonsMethods
    {
        [SetUp]
        public void SetBrowser()
        {
            Logg.Information($"Abrindo browser Chrome e acessando url {Url}");
            GetBrowser(Url);
        }

        [Test]
        public void BuscarProdutoValidarDadosCarrinhoTest()
        {
            Logg.Information($"Buscando item : Ração ");
            Home home = new Home();
            home.PreencherSearch("Ração");
            home.ClicarLupaSearch();

            Logg.Information($"Acessar dados do produto");
            new Busca().ClicarProdutoIndex(2);

            Logg.Information($"Gravando dados do produto");
            Produto produtoPageObject = new Produto();
            var produtos = new Produtos()
            {
                NomeProduto = produtoPageObject.GetProdutoName(),
                Fornecedor = produtoPageObject.GetProdutoFornecedor(),
                PriceCurrent = produtoPageObject.GetCurrentPrice(),
                PriceSignature = produtoPageObject.GetSubscriberPrice(),
            };

            Logg.Information($"Inserindo produto no carrinho");
            produtoPageObject.ClickAdicionarAoCarrinho();

            Logg.Information($"Validando informações no carrinho");
            ComprarAgoraLoja carrinho = new ComprarAgoraLoja();
            Assert.True(carrinho.GetNomeProdutoCarrinho().Equals(produtos.NomeProduto));
            Assert.True(carrinho.GetPrecoCarrinho().Equals(produtos.PriceCurrent));

            Logg.Information($"Validando quantidade de itens no carrinho");
            Assert.AreEqual(1, new ComprarAgoraLoja().GetQuantidadeItensCarrinho());

        }

        [TearDown]
        public void CloseBrowser()
        {
            Finish();
        }
    }
}