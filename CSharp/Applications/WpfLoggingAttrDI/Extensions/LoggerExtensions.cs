using System;
using WpfLoggingAttrDI;

namespace Microsoft.Extensions.Logging;

public static class LoggerExtensions
{
    public static void Emitter(this ILogger logger, LogLevel logLevel, string message, Exception? exception = null, params object?[] args)
    {
        if (logger is null)
            return;

        switch (logLevel)
        {
            case LogLevel.Trace:
            case LogLevel.Debug:
            case LogLevel.Information:
                ApplicationLog.Emit(logger, logLevel, message);
                break;

            case LogLevel.Warning:
            case LogLevel.Error:
            case LogLevel.Critical:
                if (exception is null)
                    ApplicationLog.Emit(logger, logLevel, message);
                else
                    ApplicationLog.Emit(logger, logLevel, message, exception);
                break;
        }
    }

    public static void EmitterTestPattern(this ILogger logger)
    {
        Exception exception = new Exception("Test Error Message");

        ApplicationLog.Emit(logger, LogLevel.Trace, "Trace Test Pattern");
        ApplicationLog.Emit(logger, LogLevel.Debug, "Debug Test Pattern");
        ApplicationLog.Emit(logger, LogLevel.Information, "Information Test Pattern");
        ApplicationLog.Emit(logger, LogLevel.Warning, "Warning Test Pattern");
        ApplicationLog.Emit(logger, LogLevel.Error, "Error Test Pattern", exception);
        ApplicationLog.Emit(logger, LogLevel.Critical, "Critical Test Pattern", exception);
    }
}
