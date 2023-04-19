using LogViewer.Core;
using LogDataStore = LogViewer.Avalonia.Logging.LogDataStore;

namespace AvaloniaLog4NetNoDI.DataStores;

// Application-wide shared instance of the LogDataStore logging entries
public static class MainControlsDataStore
{
    public static ILogDataStore DataStore { get; } = new LogDataStore();
}