using Itau.Processo.Test.Web.PetzStore.Commons;
using OpenQA.Selenium;

namespace Itau.Processo.Test.Web.PetzStore.PageObject
{
    public class Produto : CommonsMethods
    {
        public string GetProdutoName()
        {
            return GetText(By.CssSelector("h1[itemprop='name']"));
        }

        public string GetProdutoFornecedor()
        {
            return GetText(By.CssSelector("div:nth-child(3) > a > span"));
        }

        public string GetCurrentPrice()
        {
            return GetText(By.CssSelector("div.col-md-3.price > div.price-box > div.opt-box > div:nth-child(1) > div > div"));
        }

        public string GetSubscriberPrice()
        {
            return GetText(By.CssSelector("div.subscriber-info > div.values-holder > span.price-subscriber"));
        }

        public void ClickAdicionarAoCarrinho()
        {
            Click(By.CssSelector("#adicionarAoCarrinho"));
        }

        public void ClickAceitarLgpd()
        {
            if (ExistElement(By.CssSelector("#aceiteLgpd")))
                Click(By.CssSelector("#aceiteLgpd"));
        }

    }
}
