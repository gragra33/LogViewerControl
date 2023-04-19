using Common.Core.Extensions;
using LogViewer.Core;
using LogViewer.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RandomLogging.Service;
using Serilog;
using Serilog.Sinks.LogView.Core;
using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows;
using WpfSerilogDI.ViewModels;

namespace WpfSerilogDI;

public partial class App
{
    #region Constructor

    public App()
    {
        // catch all unhandled errors
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        builder
            // Register the Random Logging Service
            .AddRandomBackgroundService()

            // visual debugging tools
            .AddLogViewer();

        IServiceCollection services = builder.Services;

        // Serilog Logger

        // Azure: https://devblogs.microsoft.com/dotnet/asp-net-core-logging/
        // ApplicationInsights: https://github.com/serilog-contrib/serilog-sinks-applicationinsights
        // AmazonCloudWatch: https://blog.ivankahl.com/logging-dotnet-to-aws-cloudwatch-using-serilog/
        // video: https://www.youtube.com/watch?v=nVAkSBpsuTk (How Structured Logging With Serilog Can Make Your Life Easier)
        // video: https://www.youtube.com/watch?v=_iryZxv8Rxw (C# Logging with Serilog and Seq - Structured Logging Made Easy)
        // ps: docker run -d --restart unless-stopped --name seq -e ACCEPT_EULA=Y -v c:\WIP\LogData:/data -p 8081:80 datalust/seq:latest

        // ref: https://stackoverflow.com/questions/66304596/how-to-dependency-inject-serilog-into-the-rest-of-my-classes-in-net-console-app
        services.AddLogging(configure: cfg =>
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.DataStoreLoggerSink(

                    // Use Default Colors
                    //dataStoreProvider: () => _host!.Services.TryGetService<ILogDataStore>()!)

                    // Use Custom Colors
                    dataStoreProvider: () => _host!.Services.TryGetService<ILogDataStore>()!,
                    options =>
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
                    })
                .CreateLogger();

            cfg.ClearProviders()
                .AddSerilog(Log.Logger);
        });

        services
            .AddSingleton<MainViewModel>()
            .AddSingleton(service => new MainWindow
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
    {
        Exception exception = (Exception)e.ExceptionObject;

        Log.Fatal(exception, "Application terminated unexpectedly");

        MessageBox.Show(
                exception.Message,
                "Unhandled Error",
                MessageBoxButton.OK,
                MessageBoxImage.Stop);
    }

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
            Log.Fatal(ex, "Application terminated unexpectedly");
            MessageBox.Show(ex.Message, "Unhandled Error", MessageBoxButton.OK, MessageBoxImage.Stop);

            CleanUp();
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        CleanUp();
        base.OnExit(e);
    }

    private void LogStartingMode()
    {
        // Get the Launch mode
        bool isDevelopment = string.Equals(Environment.GetEnvironmentVariable("DOTNET_MODIFIABLE_ASSEMBLIES"), "debug",
                                           StringComparison.InvariantCultureIgnoreCase);

        //var siri = _host.Services.GetService<Serilog.ILogger>();
        //siri.Information("Hello from Siri");

        // initialize a logger & EventId
        ILogger<App> logger = _host!.Services.GetRequiredService<ILogger<App>>();
        EventId eventId = new EventId(id: 0, name: Assembly.GetEntryAssembly()!.GetName().Name);
        
        // log a test pattern for each log level
        logger.TestPattern(eventId: eventId);

        // log that we have started...
        logger.Emit(eventId, LogLevel.Information, $"Running in {(isDevelopment ? "Debug" : "Release")} mode");
    }

    private void CleanUp()
    {
        // tell the background services that we are shutting down
        _ = _host!.StopAsync(_cancellationTokenSource.Token);

        // flush logs
        Log.CloseAndFlush();
    }

    #endregion
}