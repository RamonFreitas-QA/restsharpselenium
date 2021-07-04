using Itau.Processo.Core.Utils.Log;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Itau.Processo.Core.Utils.Log.Utils
{
    public static class Utility
    {
        public static void Delay(int milliseconds)
        {
            System.Threading.Thread.Sleep(milliseconds);
        }              

        /// <summary>
        /// Generate random number 
        /// </summary>
        /// <param name="numberInitial"></param> Initial number
        /// <param name="numberFinal"></param> Final number
        /// <returns></returns>
        public static int RandomNumber(int numberInitial, int numberFinal)
        {
            var random = new Random();
            return random.Next(numberInitial, numberFinal);
        }

        /// <summary>
        /// Create  new hash string 
        /// </summary>
        /// <returns></returns>
        public static string GenerateHashString(int max)
        {
            Random random = new Random();
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", max).Select(s => s[random.Next(s.Length)]).ToArray());
        }               

        public static void KillChromeDriverInstance()
        {
            ProcessKill("chromedriver");
            ProcessKill("chrome");
        }
        
        public static void OpenScreenShootDirectory()
        {
            Process.Start("explorer.exe", Directory.GetCurrentDirectory() + "\\Target");
        }
        
        private static void ProcessKill(string nameProcess)
        {
            var process = Process.GetProcessesByName(nameProcess);
            for (int i = 0; i < process.Count(); i++)
            {
                try
                {
                    if (!Process.GetProcessById(process[i].Id).HasExited)
                        process[i].Kill();
                }
                catch (ArgumentException)
                {
                    Logg.Information("Erro ao tentar encerrar processo : " + process[i].Id);
                }
            }
        }

    }
}
