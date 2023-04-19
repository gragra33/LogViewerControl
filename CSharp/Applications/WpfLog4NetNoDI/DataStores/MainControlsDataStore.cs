using LogViewer.Core;
using LogDataStore = LogViewer.Wpf.Logging.LogDataStore;

namespace WpfLog4NetNoDI.DataStores;

// Application-wide shared instance of the LogDataStore logging entries
public static class MainControlsDataStore
{
    public static ILogDataStore DataStore { get; } = new LogDataStore();
}