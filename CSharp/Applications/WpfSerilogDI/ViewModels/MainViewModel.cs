using LogViewer.Core.ViewModels;
using Mvvm.Core;

namespace WpfSerilogDI.ViewModels;

public class MainViewModel : ViewModel
{
    #region Constructor

    public MainViewModel(LogViewerControlViewModel logViewer)
        => LogViewer = logViewer;

    #endregion

    #region Properties

    public LogViewerControlViewModel LogViewer { get; }

    #endregion
}