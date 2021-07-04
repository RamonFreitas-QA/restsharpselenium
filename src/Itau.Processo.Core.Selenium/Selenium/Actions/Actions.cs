using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Itau.Processo.Core.Selenium.Factory;
using Itau.Processo.Core.Utils.Log.Utils;
using Itau.Processo.Core.Utils.Log;

namespace Itau.Processo.Core.Selenium.Selenium.Actions
{
    public class Actions : DriverFactory
    {
        #region Attributes
        private static string urlAtual = string.Empty;
        private static string urlAnterior = string.Empty;
        public static string ResourcePath = AppDomain.CurrentDomain.BaseDirectory.Replace("Release", "");
        protected static string ScreenShotPath = ResourcePath + "\\Target\\";
        protected static bool AlreadyCreated = false;
        #endregion

        #region ActionsElements
        /// <summary>
        /// Method that click on elements. || Método clicar nos elementos
        /// /// </summary>
        /// <param name="locator"></param> : You have to pass position element, sample (Css Selector, Xpath or Id...).
        protected void Click(By locator)
        {
            WaitElement(locator);
            driver.FindElement(locator).Click();
        }

        /// <summary>
        ///  Method that fill input elements 
        /// </summary>
        /// <param name="locator"></param> : You have to pass position element, sample (Css Selector, Xpath or Id...).
        /// <param name="texto"></param> : You have to pass the test you want fill the input.
        protected void Fill(By locator, String text)
        {
            WaitElement(locator);
            driver.FindElement(locator).Clear();
            driver.FindElement(locator).SendKeys(text);
        }

        /// <summary>
        /// Get text in elements
        /// </summary>
        /// <param name="locator"></param> You have to pass position element, sample (Css Selector, Xpath or Id...).
        /// <returns></returns> retorna uma string 
        protected string GetText(By locator)
        {
            WaitElement(locator);
            return driver.FindElement(locator).Text;
        }

        /// <summary>
        /// That method return list of elements.
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        protected List<IWebElement> GetElements(By locator)
        {
            WaitElement(locator);
            return driver.FindElements(locator).ToList();
        }

        /// <summary>
        /// that method return true if element exist on the page and false if don't exist.
        /// </summary>
        /// <param name="locator"></param> You have to pass position element, sample (Css Selector, Xpath or Id...).
        /// <returns></returns>
        public bool ExistElement(By locator)
        {
            try
            {
                return driver.FindElements(locator).Count != 0;
            }
            catch (Exception e)
            {
                Logg.Information($"ELEMENT NOT FOUND : {e} ");
                return false;
            }
        }
        #endregion

        #region UrlAndPageActionsMethods
        private void CheckAndPrintUrl()
        {
            urlAtual = GetURL();
            if (!urlAtual.Equals(urlAnterior))
            {
                //Logg.Information("Url ... : " + urlAtual);
                urlAnterior = urlAtual;
            }
        }

        /// <summary>
        /// Acess url
        /// </summary>
        /// <param name="url"></param>
        public void AcessUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public string GetURL()
        {
            return driver.Url;
        }
        #endregion

        #region WaitMethods

        /// <summary>
        /// Wait element show up in the page
        /// </summary>
        /// <param name="locator"></param> You have to pass position element, sample (Css Selector, Xpath or Id...)
        /// <param name="timeOut"></param> If you want wait more than 30 second, you can pass after locator.
        protected void WaitElement(By locator, int timeOut = 30)
        {
            VerifyReadState();

            try
            {
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
                wait.Until(condition: ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
            }
            catch (Exception e)
            {
                Logg.Information($"ELEMENT NOT FOUND : {locator} ");
                Logg.Information($"ERRO : {e} ");
            }

            HighLight(locator);

            CheckAndPrintUrl();

            Screenshot();
        }

        #endregion

        #region ScreenShotMethods
        /// <summary>
        /// Take screenshot, will create a folder using name of test and get picture after each action  
        /// </summary>
        public void Screenshot()
        {
            CreateOrVerifyFolder();

            for (int i = 0; i <= 1000; i++)
            {
                Directory.Exists(ScreenShotPath + folderName);

                if (!VerifyArchiveExist(ScreenShotPath + folderName, DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + i + ".png"))
                {
                    ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(
                        ScreenShotPath + folderName + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + i + ".png", ScreenshotImageFormat.Png);
                    break;
                }
            }
        }

        #endregion

        #region JsCommandMethods
        /// <summary>
        /// that method wait the load of page till complete
        /// </summary>
        private void VerifyReadState()
        {
            var countTry = 0;

            try
            {
                while (!((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").ToString()
                           .Equals("complete") && countTry++ != 10000)
                    countTry++;
            }
            catch (Exception ex)
            {
                Logg.Information("the page doesn't load completely " + ex);
            }
        }

        /// <summary>
        /// that method show up what element are be clicking 
        /// </summary>
        /// <param name="locator"></param> You have to pass position element, sample (Css Selector, Xpath or Id...).
        protected void HighLight(By locator)
        {
            JsExecutor("arguments[0].style.border='4px solid red'", locator);
        }

        /// <summary>
        /// That method execute javascript comand 
        /// </summary>
        /// <param name="comando"></param> You have to pass the comand javascript you want execute
        /// <param name="locator"></param> You have to pass position element, sample (Css Selector, Xpath or Id...)        
        public void JsExecutor(string comand, By locator)
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript(comand, driver.FindElement(locator));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something wrong happen " + ex);
            }
        }
        #endregion

        #region FolderMethods
        /// <summary>
        /// Create folder for screenshot for each Test.
        /// </summary>
        /// 
        /// <returns></returns>
        protected string CreateOrVerifyFolder(string folderNameCreate, string alternativePath = "Null")
        {
            string path = ResourcePath;

            if (alternativePath != "Null")
                path = alternativePath;

            if (Directory.Exists(path + folderNameCreate))
            {
                Directory.Delete(path + folderNameCreate, true);
                Directory.CreateDirectory(path + folderNameCreate);
            }
            else
                Directory.CreateDirectory(path + folderNameCreate);


            var countWait = 0;
            while (!Directory.Exists(path + folderNameCreate) && countWait++ <= 20)
                Utility.Delay(250);

            return path + folderNameCreate;
        }

        /// <summary>
        /// Create folder for screenshot for each Test.
        /// </summary>
        /// <returns></returns>
        protected void CreateOrVerifyFolder()
        {
            if (!AlreadyCreated)
            {
                if (Directory.Exists(ScreenShotPath + folderName))
                {
                    Directory.Delete(ScreenShotPath + folderName, true);
                    Directory.CreateDirectory(ScreenShotPath + folderName);
                }
                else
                    Directory.CreateDirectory(ScreenShotPath + folderName);

                var countWait = 0;
                while (!Directory.Exists(ScreenShotPath + folderName) && countWait++ <= 20)
                    Utility.Delay(250);

                AlreadyCreated = true;
            }
        }

        /// <summary>
        /// Boolean Verify if archive exist on the folder.
        /// </summary>
        /// <param name="path"></param> Path where the archive is.
        /// <param name="archiveName"></param> Name of archive.
        /// <returns></returns> 
        protected bool VerifyArchiveExist(string path, string archiveName)
        {
            if (AlreadyCreated)
            {
                if (Directory.GetFiles(path).Length != 0)
                {
                    FileInfo[] pictures = new DirectoryInfo(path).GetFiles("*.png");
                    foreach (var picture in pictures)
                    {
                        if (archiveName.Equals(picture.Name))
                            return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}
