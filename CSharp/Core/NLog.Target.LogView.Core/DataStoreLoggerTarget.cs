using LogViewer.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Targets;
using System.Diagnostics;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace NLog.Target.LogView.Core;

[Target("DataStoreLogger")]
public class DataStoreLoggerTarget : TargetWithLayout
{
    #region Fields

    private ILogDataStore? _dataStore;
    private DataStoreLoggerConfiguration? _config;

    #endregion

    #region methods

    protected override void InitializeTarget()
    {
        // we need to inject dependencies
        IServiceProvider serviceProvider = ResolveService<IServiceProvider>();

        // reference the shared instance
        _dataStore = serviceProvider.GetRequiredService<ILogDataStore>();

        // load the config options
        IOptionsMonitor<DataStoreLoggerConfiguration>? options
            = serviceProvider.GetService<IOptionsMonitor<DataStoreLoggerConfiguration>>();

        _config = options?.CurrentValue ?? new DataStoreLoggerConfiguration();

        base.InitializeTarget();
    }

    protected override void Write(LogEventInfo logEvent)
    {
        // cast NLog Loglevel to Microsoft LogLevel type
        MsLogLevel logLevel = (MsLogLevel)Enum.ToObject(typeof(MsLogLevel), logEvent.Level.Ordinal);

        // format the message
        string message = RenderLogEvent(Layout, logEvent);

        // retrieve the EventId
        EventId eventId = (EventId)logEvent.Properties["EventId"];

        // add log entry
        _dataStore?.AddEntry(new()
        {
            Timestamp = DateTime.UtcNow,
            LogLevel = logLevel,
            // do we override the default EventId if it exists?
            EventId = eventId.Id == 0 && (_config?.EventId.Id ?? 0) != 0 ? _config!.EventId : eventId,
            State = message,
            Exception = logEvent.Exception?.Message ?? (logLevel == MsLogLevel.Error ? message : ""),
            Color = _config!.Colors[logLevel],
        });
        
        Debug.WriteLine($"--- [{logLevel.ToString()[..3]}] {message} - {logEvent.Exception?.Message ?? "no error"}");
    }

    #endregion
}