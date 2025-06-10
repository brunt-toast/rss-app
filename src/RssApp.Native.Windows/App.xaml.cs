using System;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using UnhandledExceptionEventArgs = Microsoft.UI.Xaml.UnhandledExceptionEventArgs;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using RssApp.Application.Services.Dialogs;
using RssApp.Application.Services.Navigation;
using RssApp.Native.Windows.Utils;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RssApp.Native.Windows;
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Microsoft.UI.Xaml.Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        UnhandledException += OnUnhandledException;
        this.InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        ConfigureServices();
        _mWindow = new MainWindow();
        _mWindow.Activate();
    }

    private void ConfigureServices()
    {
        ServiceCollection services = new();
        services.AddSingleton<IFeedSubscriptionContextProvider, FeedSubscriptionContextProvider>(s =>
        {
            string filePath = System.IO.Path.Join(global::Windows.Storage.ApplicationData.Current.LocalFolder.Path, "rss-app.db");
            string connString = "Data Source=" + filePath; 
            return new FeedSubscriptionContextProvider(connString);
        });
        services.AddSingleton<IDialogService, DelegatedDialogService>();
        services.AddSingleton<INavigationService, NavigationService>();

        SerilogProvider.Init();
        services.AddLogging(c =>
        {
            c.ClearProviders();
            c.AddSerilog();
        });

        Services = services.BuildServiceProvider();
    }

    private Window? _mWindow;

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ILogger logger = Services.GetRequiredService<ILoggerFactory>().CreateLogger<App>();
        logger.LogCritical(e.Exception, "{message}", e.Message);
    }
}
