using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace WpfLoggingAttrDI;

public static partial class RandomServiceLog
{
    public static Dictionary<string, Action<ILogger, LogLevel, string>> Events = new()
    {
        ["OnButtonClicked"] = LogOnButtonClicked,
        ["OnMenuItemSelected"] = LogOnMenuItemSelected,
        ["OnWindowResized"] = LogOnWindowResized,
        ["OnDataLoaded"] = LogOnDataLoaded,
        ["OnFormSubmitted"] = LogOnFormSubmitted,
        ["OnTabChanged"] = LogOnTabChanged,
        ["OnItemSelected"] = LogOnItemSelected,
        ["OnValidationFailed"] = LogOnValidationFailed,
        ["OnNotificationReceived"] = LogOnNotificationReceived,
        ["OnApplicationStarted"] = LogOnApplicationStarted,
        ["OnUserLoggedIn"] = LogOnUserLoggedIn,
        ["OnUploadStarted"] = LogOnUploadStarted,
        ["OnDownloadCompleted"] = LogOnDownloadCompleted,
        ["OnProgressUpdated"] = LogOnProgressUpdated,
        ["OnNetworkErrorOccurred"] = LogOnNetworkErrorOccurred,
        ["OnPaymentSuccessful"] = LogOnPaymentSuccessful,
        ["OnProfileUpdated"] = LogOnProfileUpdated,
        ["OnSearchCompleted"] = LogOnSearchCompleted,
        ["OnFilterChanged"] = LogOnFilterChanged,
        ["OnLanguageChanged"] = LogOnLanguageChanged
    };

    public static void Emit(ILogger logger, EventId eventId, LogLevel level, string message, Exception? exception = null)
        => Events[eventId.Name!].Invoke(logger, level, exception is null ? message : $"{message} - {exception}");

    [LoggerMessage (EventId = 101, EventName = "OnButtonClicked", Message = "{msg}")]
    private static partial void LogOnButtonClicked(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 102, EventName = "OnMenuItemSelected", Message = "{msg}")]
    private static partial void LogOnMenuItemSelected(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 103, EventName = "OnWindowResized", Message = "{msg}")]
    private static partial void LogOnWindowResized(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 104, EventName = "OnDataLoaded", Message = "{msg}")]
    private static partial void LogOnDataLoaded(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 105, EventName = "OnFormSubmitted", Message = "{msg}")]
    private static partial void LogOnFormSubmitted(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 106, EventName = "OnTabChanged", Message = "{msg}")]
    private static partial void LogOnTabChanged(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 107, EventName = "OnItemSelected", Message = "{msg}")]
    private static partial void LogOnItemSelected(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 108, EventName = "OnValidationFailed", Message = "{msg}")]
    private static partial void LogOnValidationFailed(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 109, EventName = "OnNotificationReceived", Message = "{msg}")]
    private static partial void LogOnNotificationReceived(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 110, EventName = "OnApplicationStarted", Message = "{msg}")]
    private static partial void LogOnApplicationStarted(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 111, EventName = "OnUserLoggedIn", Message = "{msg}")]
    private static partial void LogOnUserLoggedIn(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 112, EventName = "OnUploadStarted", Message = "{msg}")]
    private static partial void LogOnUploadStarted(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 113, EventName = "OnDownloadCompleted", Message = "{msg}")]
    private static partial void LogOnDownloadCompleted(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 114, EventName = "OnProgressUpdated", Message = "{msg}")]
    private static partial void LogOnProgressUpdated(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 115, EventName = "OnNetworkErrorOccurred", Message = "{msg}")]
    private static partial void LogOnNetworkErrorOccurred(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 116, EventName = "OnPaymentSuccessful", Message = "{msg}")]
    private static partial void LogOnPaymentSuccessful(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 117, EventName = "OnProfileUpdated", Message = "{msg}")]
    private static partial void LogOnProfileUpdated(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 118, EventName = "OnSearchCompleted", Message = "{msg}")]
    private static partial void LogOnSearchCompleted(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 119, EventName = "OnFilterChanged", Message = "{msg}")]
    private static partial void LogOnFilterChanged(ILogger logger,  LogLevel level, string msg);

    [LoggerMessage (EventId = 120, EventName = "OnLanguageChanged", Message = "{msg}")]
    private static partial void LogOnLanguageChanged(ILogger logger,  LogLevel level, string msg);
}