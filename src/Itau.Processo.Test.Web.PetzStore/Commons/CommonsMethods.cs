using Itau.Processo.Core.Data;
using Itau.Processo.Core.Data.Model;
using Itau.Processo.Core.Selenium.Selenium.Actions;
using Itau.Processo.Core.Utils.Log;
using Itau.Processo.Core.Utils.Log.Utils;

namespace Itau.Processo.Test.Web.PetzStore.Commons
{
    public class CommonsMethods : Actions
    {
        public string Url { get; set; }

        public CommonsMethods()
        {            
            Url = ContextSettings.ReturnJson().Data.PetStore.Web.Url;
        }
        protected void GetBrowser(string url)
        {
            Logg.Information("Limpando drivers e browser do teste caso existir.");
            Utility.KillChromeDriverInstance();

            Logg.Information("Create new instance browser right archive appconfig");
            GetBrowser();

            Logg.Information("Acess environment");
            AcessUrl(url);
        }

        protected void Finish(bool openDirectory = false)
        {
            Logg.Information("Close Instance WebDriver");
            BrowserQuit();

            Logg.Information($"Open ScreenShoot directory");
            if(openDirectory)
            Utility.OpenScreenShootDirectory();
        }
    }
}
