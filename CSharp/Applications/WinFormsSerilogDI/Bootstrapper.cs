using Common.Core.Extensions;
using LogViewer.Core;
using LogViewer.WinForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RandomLogging.Service;
using Serilog;
using Serilog.Sinks.LogView.Core;
using System.Reflection;
using WinFormsSerilogDI.DataStores;

namespace WinFormsSerilogDI;

public class Bootstrapper
{
    #region Constructors

    public Bootstrapper()
    {
        ApplicationConfiguration.Initialize();
        Application.ApplicationExit += OnExit;
     
        InitializeDI();

        _cancellationTokenSource = new();

        OnStartup();
    }

    #endregion
    
    #region Fields

    private static IHost? _host;
    private static CancellationTokenSource? _cancellationTokenSource;

    #endregion

    #region Methods

    private static void InitializeDI()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        builder
            /*
             * Note: For information on launch profiles for debugging,
             *     see article: https://www.codeproject.com/Articles/5354478/NET-App-Settings-Demystified-Csharp-VB
             */

            // Random Logging Service
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

        builder.Services.AddSingleton<MainControlsDataStore>();
        builder.Services.AddSingleton<MainForm>();

        _host = builder.Build();
    }

   private void OnStartup()
    {
        try
        {
            LogStartingMode();

            // startup background services
            _ = _host!.StartAsync(_cancellationTokenSource!.Token);

            // set and show
            Application.Run(_host.Services.GetRequiredService<MainForm>());
        }
        catch (OperationCanceledException)
        {
            // skip
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");

            MessageBox.Show(ex.Message, @"Unhandled Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
       
            CleanUp();
        }
    }

    private void OnExit(object? sender, EventArgs e)
        => CleanUp();

    private void LogStartingMode()
    {
        // Get the Launch mode
        bool isDevelopment = string.Equals(Environment.GetEnvironmentVariable("DOTNET_MODIFIABLE_ASSEMBLIES"), "debug",
                                           StringComparison.InvariantCultureIgnoreCase);

        // initialize a logger & EventId
        ILogger<Bootstrapper> logger = _host!.Services.GetRequiredService<ILogger<Bootstrapper>>();
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