using Itau.Processo.Core.Data;
using Itau.Processo.Core.Utils.Log;
using Itau.Processo.Test.Web.PetzStore.Commons;
using Itau.Processo.Test.Web.PetzStore.Model;
using Itau.Processo.Test.Web.PetzStore.PageObject;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Itau.Processo.Test.Web.PetzStore.Tests.Exercicios
{
    public class TestExercise2B : CommonsMethods
    {
        private List<Produtos> ProductsData;
        private string[] ListaProdutos;       

        [SetUp]
        public void SetBrowser()
        {
            ListaProdutos = ContextSettings.ReturnJson().Data.PetStore.Web.ListaProdutos;
            ProductsData = new List<Produtos>();

            Logg.Information($"Abrindo browser Chrome e acessando url {Url}");
            GetBrowser(Url);
        }

        [Test]
        public void BuscarDezProdutosInserirCarrinhoScreenShoot()
        {
            int itemNumero = 1;
            foreach (string produto in ListaProdutos)
            {
                Logg.Information($"Buscando item : {itemNumero} - {produto} ");
                BuscarProduto(produto);

                Logg.Information($"Incluindo item no carrinho: {itemNumero} - {produto} ");
                var produtoData = DataInsertCart();

                var cartDataProduto = CarrinhoData(itemNumero);

                Logg.Information($"Validando dados do item: {itemNumero} - {produto} no carrinho");
                Assert.True(produtoData.NomeProduto.Trim().Contains(cartDataProduto.NomeProduto.Trim()));
                Assert.True(cartDataProduto.PriceCurrent.Equals(produtoData.PriceCurrent));

                Logg.Information($"Validando quantidade de itens no carrinho");
                Assert.AreEqual(itemNumero, new ComprarAgoraLoja().GetQuantidadeItensCarrinho());

                ProductsData.Add(produtoData);
                itemNumero++;
                new ComprarAgoraLoja().ClickHome();
            }
        }

        [TearDown]
        public void CloseBrowser()
        {
            Logg.Information($"ScreenShoot diretorio:{ScreenShotPath}");

            Finish(true);
        }

        #region PrivateMethods
        private void BuscarProduto(string produto)
        {
            Home home = new Home();
            home.PreencherSearch(produto);
            home.ClicarLupaSearch();            
            new Busca().ClicarProdutoIndex(0);
        }

        private Produtos DataInsertCart()
        {
            Produto produto = new Produto();

            var petzStoreWeb = new Produtos
            {
                NomeProduto = produto.GetProdutoName(),
                Fornecedor = produto.GetProdutoFornecedor(),
                PriceCurrent = produto.GetCurrentPrice(),
                PriceSignature = produto.GetSubscriberPrice()
            };

            produto.ClickAceitarLgpd();
            produto.ClickAdicionarAoCarrinho();
            return petzStoreWeb;
        }

        private Produtos CarrinhoData(int itemNumero)
        {
            ComprarAgoraLoja carrinho = new ComprarAgoraLoja();

            return new Produtos
            {
                NomeProduto = new Regex(@"[^-]*").Match(carrinho.GetNomeProdutoCarrinho(itemNumero)).ToString(),
                PriceCurrent = carrinho.GetPrecoCarrinho(itemNumero),
            };
        }
        #endregion
    }
}