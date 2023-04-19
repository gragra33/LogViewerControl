using Serilog.Configuration;
using LogViewer.Core;

namespace Serilog.Sinks.LogView.Core;

public static class DataStoreLoggerSinkExtensions
{
    public static LoggerConfiguration DataStoreLoggerSink
    (
        this LoggerSinkConfiguration loggerConfiguration,
        Func<ILogDataStore> dataStoreProvider, 
        Action<DataStoreLoggerConfiguration>? configuration = null,
        IFormatProvider formatProvider = null!
    )
        => loggerConfiguration.Sink(new DataStoreLoggerSink(dataStoreProvider, GetConfig(configuration), formatProvider));

    private static Func<DataStoreLoggerConfiguration> GetConfig(Action<DataStoreLoggerConfiguration>? configuration)
    {
        // convert from Action to Func delegate to pass data
        DataStoreLoggerConfiguration data = new();
        configuration?.Invoke(data);
        return () => data;
    }
}