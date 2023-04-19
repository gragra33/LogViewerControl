using LogViewer.Core;
using LogDataStore = LogViewer.WinForms.Logging.LogDataStore;

namespace WinFormsSerilogNoDI.DataStores;

// Application-wide shared instance of the LogDataStore logging entries
public static class MainControlsDataStore
{
    public static ILogDataStore DataStore { get; } = new LogDataStore();
}