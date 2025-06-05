using System.IO;
using Serilog;

namespace RssApp.Native.Windows.Utils;

internal static class SerilogProvider
{
    public static void Init()
    {
        string loggingPath = Path.Join(global::Windows.Storage.ApplicationData.Current.LocalFolder.Path, "logs", ".log");

        var loggerBuilder = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(loggingPath, rollingInterval: RollingInterval.Day, shared: true)
#if DEBUG
            .MinimumLevel.Verbose();
#else
            .MinimumLevel.Information();
#endif

        Log.Logger = loggerBuilder.CreateLogger();
    }
}
