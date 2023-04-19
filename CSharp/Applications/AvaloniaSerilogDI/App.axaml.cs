using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaSerilogDI.ViewModels;
using AvaloniaSerilogDI.Views;
using Common.Core.Extensions;
using LogViewer.Avalonia;
using LogViewer.Core;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
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
using Icon = MessageBox.Avalonia.Enums.Icon;

namespace AvaloniaSerilogDI;

public partial class App : Application
{
    #region Constructors
    
    public override void Initialize()
        => AvaloniaXamlLoader.Load(this); 

    #endregion

    #region Fields

    private IHost? _host;
    private CancellationTokenSource? _cancellationTokenSource;

    #endregion

    #region Methods

     public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            ExpressionObserver.DataValidators.RemoveAll(x => x is DataAnnotationsValidationPlugin);

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
        //     docker rmi datalust/seq --force

        // ref: https://stackoverflow.com/questions/66304596/how-to-dependency-inject-serilog-into-the-rest-of-my-classes-in-net-console-app
        services.AddLogging(configure: cfg =>
        {
            Log.Logger = new LoggerConfiguration()
            //Serilog.Core.Logger logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.DataStoreLoggerSink(
                    dataStoreProvider: () => _host!.Services.TryGetService<ILogDataStore>()!

                    //dataStoreProvider: () => _host!.Services.TryGetService<ILogDataStore>()!,
                    //options =>
                    //{
                    //    options.Colors[LogLevel.Trace] = new()
                    //    {
                    //        Foreground = Color.White,
                    //        Background = Color.DarkGray
                    //    };

                    //    options.Colors[LogLevel.Debug] = new()
                    //    {
                    //        Foreground = Color.White,
                    //        Background = Color.Gray
                    //    };

                    //    options.Colors[LogLevel.Information] = new()
                    //    {
                    //        Foreground = Color.White,
                    //        Background = Color.DodgerBlue
                    //    };

                    //    options.Colors[LogLevel.Warning] = new()
                    //    {
                    //        Foreground = Color.White,
                    //        Background = Color.Orchid
                    //    };
                    //}
                    )
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
            
            try
            {
                LogStartingMode();

                // set and show
                desktop.MainWindow = _host.Services.GetRequiredService<MainWindow>();
                desktop.ShutdownRequested += OnShutdownRequested;

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

                ShowMessageBox("Unhandled Error", ex.Message);

                CleanUp();
                return;
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

   private void OnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
        => CleanUp();

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Exception exception = (Exception)e.ExceptionObject;

        Log.Fatal(exception, "Application terminated unexpectedly");
        ShowMessageBox("Unhandled Error", exception.Message);

        CleanUp();
    }

    private void ShowMessageBox(string title, string message)
    {
        MessageBox.Avalonia.BaseWindows.Base.IMsBoxWindow<ButtonResult> messageBoxStandardWindow = MessageBoxManager
            .GetMessageBoxStandardWindow(title, message, ButtonEnum.Ok, Icon.Stop);

        messageBoxStandardWindow.Show();
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

    private void CleanUp()
    {
        // tell the background services that we are shutting down
        _ = _host!.StopAsync(_cancellationTokenSource!.Token);

        // flush logs
        Log.CloseAndFlush();
    }

    #endregion
}