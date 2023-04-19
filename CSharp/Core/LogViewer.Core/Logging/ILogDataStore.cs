using System.Collections.ObjectModel;

namespace LogViewer.Core;

public interface ILogDataStore
{
    ObservableCollection<LogModel> Entries { get; }
    void AddEntry(LogModel logModel);
}
