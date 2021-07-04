using Serilog;

namespace Itau.Processo.Core.Utils.Log
{
    public static class Logg
    {
        public static void Information(string text)
        {
            new LoggerConfiguration()
                     .WriteTo.Console()
                        .CreateLogger()
                            .Information(text);
        }
    }
}
