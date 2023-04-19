using LogViewer.Core.ViewModels;

namespace AvaloniaLoggingDI.ViewModels;

public class MainViewModel : ViewModelBase
{
    #region Constructor

    public MainViewModel(LogViewerControlViewModel logViewer)
    {
        LogViewer = logViewer;
    }

    #endregion

    #region Properties

    public LogViewerControlViewModel LogViewer { get; }

    #endregion
}