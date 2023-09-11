using LogViewer.Core.ViewModels;
using Mvvm.Core;

namespace WpfLoggingAttrDI.ViewModels;

public class MainViewModel : ViewModel
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