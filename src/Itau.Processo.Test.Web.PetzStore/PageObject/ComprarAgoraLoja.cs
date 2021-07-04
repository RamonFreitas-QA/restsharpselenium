using Itau.Processo.Test.Web.PetzStore.Commons;
using OpenQA.Selenium;
using System;

namespace Itau.Processo.Test.Web.PetzStore.PageObject
{
    class ComprarAgoraLoja : CommonsMethods
    {
        public string GetNomeProdutoCarrinho(int item = 1)
        {
            return GetText(By.CssSelector($"tr:nth-child({item}) > td.td-resumo > a"));
        }

        public string GetPrecoCarrinho(int item = 1)
        {
            return GetText(By.CssSelector($"tr:nth-child({item}) > td.preco"));
        }

        public void ClickHome()
        {
            Click(By.CssSelector("a[class='logo-holder'"));
        }

        public int GetQuantidadeItensCarrinho()
        {
            return Convert.ToInt16(GetText(By.CssSelector("i[class='icon-carrinho'] > span")));
        }
    }
}
