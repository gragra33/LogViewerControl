using Common.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using Microsoft.Extensions.Configuration;
using WpfNLogNoDI.Extensions;
using Common.Core.Extensions;

namespace WpfNLogNoDI.Helpers;

// application-wide DataStoreLogger Factory ... returns a wired up Logger instance
public static class LoggingHelper
{
    #region Constructors

    static LoggingHelper()
    {
        // retrieve the log level from 'appsettings'
        string value = AppSettings<string>.Current("Logging:LogLevel", "Default") ?? "Information";
        Enum.TryParse(value, out LogLevel logLevel);

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .Initialize()
            .Build();

        // wire up the loggers
        Factory = LoggerFactory.Create(builder => builder

            // visual debugging tools
            .AddNLogTargetsNoDI(configuration)

            // uncomment to use custom logging colors (note: System.Drawing namespace)
            //
            //.AddNLogTargets(configuration, options =>
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
            //})

            // set minimum log level from 'appsettings*.json'
            .SetMinimumLevel(logLevel));
    }

    #endregion

    #region Properties

    public static ILoggerFactory Factory { get; }

    #endregion
}