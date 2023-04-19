using LogViewer.Core;
using LogDataStore = LogViewer.Wpf.Logging.LogDataStore;

namespace WpfNLogNoDI.DataStores;

// Application-wide shared instance of the LogDataStore logging entries
public static class MainControlsDataStore
{
    public static ILogDataStore DataStore { get; } = new LogDataStore();
}