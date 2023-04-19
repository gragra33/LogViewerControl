using AvaloniaSerilogNoDI.DataStores;
using Common.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.LogView.Core;
using System.Drawing;

namespace AvaloniaSerilogNoDI.Helpers;

// application-wide DataStoreLogger Factory ... returns a wired up Logger instance
public static class LoggingHelper
{
    #region Constructors

    static LoggingHelper()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .Initialize()
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.DataStoreLoggerSink(
                //dataStoreProvider: () => MainControlsDataStore.DataStore

                dataStoreProvider: () => MainControlsDataStore.DataStore,
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
                }
            )
            .CreateLogger();

        // wire up the loggers
        Factory = LoggerFactory.Create(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger));
    }

    #endregion

    #region Properties

    public static ILoggerFactory Factory { get; }

    #endregion

    #region Methods

    public static void CloseAndFlush()
        => Log.CloseAndFlush();

    #endregion
}