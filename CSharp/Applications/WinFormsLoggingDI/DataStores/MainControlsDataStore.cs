using LogViewer.Core;
using LogViewer.WinForms;

namespace WinFormsLoggingDI.DataStores;

public class MainControlsDataStore
{
    #region Constructors

    public MainControlsDataStore(LogViewerControl logViewer,
                                 ILogDataStore dataStore)
    {
        LogViewer = logViewer;
        DataStore = dataStore;
    }

    #endregion

    #region Properties

    public LogViewerControl LogViewer { get; }

    public ILogDataStore DataStore { get; set; }

    #endregion
}