using Common.Core;
using Microsoft.Extensions.Logging;
using WinFormsLoggingNoDI.Extensions;

namespace WinFormsLoggingNoDI.Helpers;

// application-wide DataStoreLogger Factory ... returns a wired up Logger instance
public static class LoggingHelper
{
    #region Constructors

    static LoggingHelper()
    {
        // retrieve the log level from 'appsettings'
        string value = AppSettings<string>.Current("Logging:LogLevel", "Default") ?? "Information";
        Enum.TryParse(value, out LogLevel logLevel);

        // wire up the loggers
        Factory = LoggerFactory.Create(builder => builder

            // visual debugging tools
            //.AddDataStoreLogger()

            // uncomment to use custom logging colors (note: System.Drawing namespace)
            //
            .AddDataStoreLogger(options =>
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

            // examples of adding other loggers...
            .AddSimpleConsole(options =>
            {
                options.SingleLine = true;
                options.TimestampFormat = "hh:mm:ss ";
            })

            // note:
            //  * The IDE will automatically add the Debugger Logger, even though not visible
            //  * Adding the DebugLogger is useful for remote debugging
            //.AddDebug()

            // set minimum log level from 'appsettings'
            .SetMinimumLevel(logLevel));
    }

    #endregion

    #region Properties

    public static ILoggerFactory Factory { get; }

    #endregion
}