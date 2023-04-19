using LogViewer.Core.ViewModels;

namespace AvaloniaNlogDI.ViewModels;

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