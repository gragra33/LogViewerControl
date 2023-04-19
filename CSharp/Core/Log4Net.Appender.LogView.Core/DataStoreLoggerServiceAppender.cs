using System.Diagnostics;
using log4net.Appender;
using log4net.Core;
using LogViewer.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Log4Net.Appender.LogView.Core;

public class DataStoreLoggerServiceAppender : ServiceAppenderSkeleton
{
    #region Fields

    private ILogDataStore? _dataStore;
    private DataStoreLoggerConfiguration? _options;
    
    private IServiceProvider? _serviceProvider;
    
    #endregion

    #region Methods

    protected override void Append(LoggingEvent loggingEvent)
    {
        if (_serviceProvider is null)
            Initialize();

        // cast matching Log4Net Loglevel to Microsoft LogLevel type
        LogLevel logLevel = loggingEvent.Level.Value switch 
        {
            int.MaxValue => LogLevel.None,
            120000 => LogLevel.Debug,
            90000 => LogLevel.Critical,
            70000 => LogLevel.Error,
            60000 => LogLevel.Warning,
            20000 => LogLevel.Trace,
            _ => LogLevel.Information
        };

        DataStoreLoggerConfiguration config = _options ?? new DataStoreLoggerConfiguration();

        EventId? eventId = (EventId?)loggingEvent.LookupProperty(nameof(EventId));
        eventId = eventId is null && config.EventId.Id != 0 ? config.EventId : eventId;

        string message = loggingEvent.RenderedMessage ?? string.Empty;
        
        string exceptionMessage = loggingEvent.GetExceptionString();

        _dataStore!.AddEntry(new()
        {
            Timestamp = DateTime.UtcNow,
            LogLevel = logLevel,
            EventId = eventId ?? new(),
            State = message,
            Exception = exceptionMessage,
            Color = config.Colors[logLevel],
        });

        Debug.WriteLine($"--- [{logLevel.ToString()[..3]}] {message} - {exceptionMessage ?? "no error"}");
    }

    private void Initialize()
    {
        _serviceProvider = ResolveService<IServiceProvider>();
        _dataStore = _serviceProvider.GetRequiredService<ILogDataStore>();
        _options = _serviceProvider.GetService<DataStoreLoggerConfiguration>();
    }

    #endregion
}