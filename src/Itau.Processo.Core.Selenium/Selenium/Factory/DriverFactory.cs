using Itau.Processo.Core.Selenium.Browser.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Itau.Processo.Core.Selenium.Factory
{
    public class DriverFactory
    {
        public static WebDriverWait wait;
        public static IWebDriver driver;
        public static string folderName;

        protected void GetBrowser()
        {
            GetBrowserInstance();
        }

        private void GetBrowserInstance()
        {
            driver = new ChromeBrowser().GetChromeDriver();
            driver.Manage().Window.Maximize();
        }

        public void BrowserQuit()
        {            
            driver.Quit();
        }
    }
}
