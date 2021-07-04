using Itau.Processo.Test.Web.PetzStore.Commons;
using OpenQA.Selenium;

namespace Itau.Processo.Test.Web.PetzStore.PageObject
{
    class Busca : CommonsMethods
    {
        public void ClicarProdutoIndex(int indexItem)
        {
            if (ExistElement(By.CssSelector("#gridProdutos > li")))
                GetElements(By.CssSelector("#gridProdutos > li"))[indexItem].Click();
        }
    }
}
