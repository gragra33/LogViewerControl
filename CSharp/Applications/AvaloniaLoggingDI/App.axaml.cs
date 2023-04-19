using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaLoggingDI.ViewModels;
using AvaloniaLoggingDI.Views;
using LogViewer.Avalonia;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MsLogger.Core;
using RandomLogging.Service;
using Icon = MessageBox.Avalonia.Enums.Icon;

namespace AvaloniaLoggingDI;

public partial class App : Application
{
    public override void Initialize()
        => AvaloniaXamlLoader.Load(this);

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
                /*
                 * Note: For information on launch profiles for debugging,
                 *     see article: https://www.codeproject.com/Articles/5354478/NET-App-Settings-Demystified-Csharp-VB
                 */

                // Register the Random Logging Service
                .AddRandomBackgroundService()

                // visual debugging tools
                .AddLogViewer()

                // Microsoft Logger
                //.Logging.AddDefaultDataStoreLogger();

                // uncomment to use custom logging colors (note: System.Drawing namespace)
                //
                .Logging.AddDefaultDataStoreLogger(options =>
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

            IServiceCollection services = builder.Services;

            services
                .AddSingleton<MainViewModel>()
                .AddSingleton<MainWindow>(service => new MainWindow
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
                ShowMessageBox("Unhandled Error", ex.Message);
                return;
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void OnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
        => _ = _host!.StopAsync(_cancellationTokenSource!.Token);

    #region Fields

    private IHost? _host;
    private CancellationTokenSource? _cancellationTokenSource;

    #endregion

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        => ShowMessageBox("Unhandled Error", ((Exception)e.ExceptionObject).Message);

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
}