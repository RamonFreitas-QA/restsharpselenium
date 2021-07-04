using OpenQA.Selenium.Chrome;
using System.IO;

namespace Itau.Processo.Core.Selenium.Browser.Chrome
{
    public class ChromeBrowser
    {
        public ChromeDriver GetChromeDriver()
        {
            return GetChromeDriverInstance();
        }

        #region Private Methods
        private ChromeDriver GetChromeDriverInstance()
        {
            return new ChromeDriver(Directory.GetCurrentDirectory(), SetCapabilities());
        }

        private ChromeOptions SetCapabilities()
        {
            ChromeOptions chromeOptions = new ChromeOptions();            
            chromeOptions.AddArguments("test-type");
            chromeOptions.AddArgument("--sandbox");            
            chromeOptions.AddArgument("--lang=pt-br");
            chromeOptions.AddArgument("--disable-infobars");
            chromeOptions.AddArgument("--enable-popup-blocking");
            chromeOptions.AddArgument("--disable-infobars");
            chromeOptions.AddArgument("--disable-notifications");
            //chromeOptions.AddArguments("--headless");

            return chromeOptions;
        }

        #endregion
    }
}
