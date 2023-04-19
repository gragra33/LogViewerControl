using System.Collections.ObjectModel;

namespace LogViewer.Core;

public class LogDataStore : ILogDataStore
{
    #region Fields

    private static readonly SemaphoreSlim _semaphore = new(initialCount: 1);

    #endregion

    #region Properties

    public ObservableCollection<LogModel> Entries { get; } = new();

    #endregion

    #region Methods

    public virtual void AddEntry(LogModel logModel)
    {
        // ensure only one operation at time from multiple threads
        _semaphore.Wait();

        Entries.Add(logModel);

        _semaphore.Release();
    }

    #endregion
}