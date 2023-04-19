using System.Collections.Concurrent;
using LogViewer.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MsLogger.Core;

public class DataStoreLoggerProvider: ILoggerProvider
{

    #region Constructor
    
    public DataStoreLoggerProvider(IOptionsMonitor<DataStoreLoggerConfiguration> config, ILogDataStore dataStore)
    {
        _dataStore = dataStore;
        _currentConfig = config.CurrentValue;
        _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
    }

    #endregion

    #region fields
    
    private DataStoreLoggerConfiguration _currentConfig;

    private readonly IDisposable? _onChangeToken;
    protected readonly ILogDataStore _dataStore;

    protected readonly ConcurrentDictionary<string, DataStoreLogger> _loggers = new();
    
    #endregion

    #region Methods
    
    public ILogger CreateLogger(string categoryName)
        => _loggers.GetOrAdd(categoryName, name => new DataStoreLogger(name, GetCurrentConfig, _dataStore));

    protected DataStoreLoggerConfiguration GetCurrentConfig()
        => _currentConfig;

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    } 

    #endregion
}