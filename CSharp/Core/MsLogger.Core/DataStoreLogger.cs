using System.Diagnostics;
using LogViewer.Core;
using Microsoft.Extensions.Logging;

namespace MsLogger.Core;

public class DataStoreLogger: ILogger
{
    // ref: https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider

    #region Constructor

    public DataStoreLogger(string name, Func<DataStoreLoggerConfiguration> getCurrentConfig, ILogDataStore dataStore)
    {
        (_name, _getCurrentConfig) = (name, getCurrentConfig);
        _dataStore = dataStore;
    }

    #endregion

    #region Fields

    private readonly ILogDataStore _dataStore;
    private readonly string _name;
    private readonly Func<DataStoreLoggerConfiguration> _getCurrentConfig;

    #endregion

    #region methods

    public IDisposable BeginScope<TState>(TState state)  where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
    {
        // check if we are logging for passed log level
        if (!IsEnabled(logLevel))
            return;

        DataStoreLoggerConfiguration config = _getCurrentConfig();

        _dataStore.AddEntry(new()
        {
            Timestamp = DateTime.UtcNow,
            LogLevel = logLevel,
            // do we override the default EventId if it exists?
            EventId = eventId.Id == 0 && config.EventId != 0 ? config.EventId : eventId,
            State = state,
            Exception = exception?.Message ?? (logLevel == LogLevel.Error ? state?.ToString() ?? "" : ""),
            Color = config.Colors[logLevel],
        });
        
        Debug.WriteLine($"--- [{logLevel.ToString()[..3]}] {_name} - {formatter(state, exception!)}");
    }

    #endregion
}