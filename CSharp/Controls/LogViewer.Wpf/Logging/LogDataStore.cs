using System.Windows.Threading;
using LogViewer.Core;

namespace LogViewer.Wpf.Logging;

public sealed class LogDataStore : Core.LogDataStore
{
    #region Methods

    public override void AddEntry(LogModel logModel)
        => DispatcherHelper.Execute(() => base.AddEntry(logModel));

    #endregion
}