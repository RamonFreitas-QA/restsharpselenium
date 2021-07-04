using Itau.Processo.Test.Web.PetzStore.Commons;
using OpenQA.Selenium;

namespace Itau.Processo.Test.Web.PetzStore.PageObject
{
    class Home : CommonsMethods
    {
        public void PreencherSearch(string produto)
        {
            Fill(By.CssSelector("#search"), produto);
        }

        public void ClicarLupaSearch()
        {
            Click(By.CssSelector("i[class='custom-icon icon-lupa']"));
        }




    }
}
