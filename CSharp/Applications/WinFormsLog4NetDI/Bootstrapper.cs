using System.Reflection;
using LogViewer.WinForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RandomLogging.Service;
using WinFormsLog4NetDI.DataStores;
using Log4Net.Appender.LogView.Core;

namespace WinFormsLog4NetDI;

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

            // Register the Random Logging Service
            .AddRandomBackgroundService()

            // visual debugging tools
            .AddLogViewer()

            // Nlog Target
            //.Logging.AddLog4Net(builder.Configuration);
         
            // uncomment to use custom logging colors (note: System.Drawing namespace)
            //
            .Logging.AddLog4Net(
                builder.Configuration,
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
            MessageBox.Show(ex.Message, @"Unhandled Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }

    private void OnExit(object? sender, EventArgs e)
    {
        // tell the background services that we are shutting down
        _host!.StopAsync(_cancellationTokenSource!.Token);
    }

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

    #endregion
}