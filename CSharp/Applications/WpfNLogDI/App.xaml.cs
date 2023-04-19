using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows;
using LogViewer.Wpf;
using RandomLogging.Service;
using WpfNLogDI.ViewModels;
using NLog.Target.LogView.Core.Extensions;

namespace WpfNLogDI;

public partial class App
{
    #region Constructor

    public App()
    {
        // catch all unhandled errors
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        builder
            /*
             * Note: For information on launch profiles for debugging,
             *     see article: https://www.codeproject.com/Articles/5354478/NET-App-Settings-Demystified-Csharp-VB
             */

            // Register the Random Logging Service
            .AddRandomBackgroundService()

            // visual debugging tools
            .AddLogViewer()

            // Nlog Target
            //.Logging.AddNLogTargets(builder.Configuration);
         
            // uncomment to use custom logging colors (note: System.Drawing namespace)
            //
            .Logging.AddNLogTargets(builder.Configuration, options =>
            {
                options.Colors[LogLevel.Trace] = new()
                {
                    Foreground = Color.White,
                    Background = Color.DarkGray
                };

                options.Colors[LogLevel.Debug] = new()
                {
                    Foreground = Color.White,
                    Background = Color.Gray
                };

                options.Colors[LogLevel.Information] = new()
                {
                    Foreground = Color.White,
                    Background = Color.DodgerBlue
                };

                options.Colors[LogLevel.Warning] = new()
                {
                    Foreground = Color.White,
                    Background = Color.Orchid
                };
            });

        builder.Services
            .AddSingleton<MainViewModel>()
            .AddSingleton<MainWindow>(service => new MainWindow
            {
                DataContext = service.GetRequiredService<MainViewModel>()
            });

        _host = builder.Build();
        _cancellationTokenSource = new();
    }

    #endregion

    #region Fields

    private readonly IHost? _host;
    private readonly CancellationTokenSource _cancellationTokenSource;

    #endregion

    #region Methods

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        => MessageBox.Show(
            ((Exception)e.ExceptionObject).Message,
            "Unhandled Error",
            MessageBoxButton.OK,
            MessageBoxImage.Stop);

    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            LogStartingMode();

            // set and show
            MainWindow = _host!.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            // startup background services
            _ = _host.StartAsync(_cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            // skip
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Unhandled Error", MessageBoxButton.OK, MessageBoxImage.Stop);
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // tell the background services that we are shutting down
        _host!.StopAsync(_cancellationTokenSource.Token);

        // Flush all messages
        NLog.LogManager.Shutdown();

        // example from console logging output:
        //2023-03-27 19:40:40.5276|Debug|Hosting stopping |EventId_Id=3, EventId_Name=Stopping, EventId=Stopping
        //2023-03-27 19:40:40.5311|Info|Application is shutting down... |EventId_Id=0, EventId=0
        //2023-03-27 19:40:40.5311|Info|Stopping |EventId_Id=420, EventId_Name=RandomLoggingService, EventId=RandomLoggingService
        //2023-03-27 19:40:40.5766|Debug|Hosting stopped |EventId_Id=4, EventId_Name=Stopped, EventId=Stopped

        base.OnExit(e);
    }

    private void LogStartingMode()
    {
        // Get the Launch mode
        bool isDevelopment = string.Equals(Environment.GetEnvironmentVariable("DOTNET_MODIFIABLE_ASSEMBLIES"), "debug",
                                           StringComparison.InvariantCultureIgnoreCase);

        // initialize a logger & EventId
        ILogger<App> logger = _host!.Services.GetRequiredService<ILogger<App>>();
        EventId eventId = new EventId(id: 0, name: Assembly.GetEntryAssembly()!.GetName().Name);
        
        // log a test pattern for each log level
        logger.TestPattern(eventId: eventId);

        // log that we have started...
        logger.Emit(eventId, LogLevel.Information, $"Running in {(isDevelopment ? "Debug" : "Release")} mode");
    }

    #endregion
}