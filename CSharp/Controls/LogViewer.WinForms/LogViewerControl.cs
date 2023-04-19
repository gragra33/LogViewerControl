using System.Collections.Specialized;
using System.Windows.Threading;
using LogViewer.Core.ViewModels;
using LogViewer.Core;

namespace LogViewer.WinForms;

public partial class LogViewerControl : UserControl
{
    #region Constructors

    // supports DI and non-DI usage

    public LogViewerControl()
    {
        InitializeComponent();

        // Stop the flickering!
        ListView.SetDoubleBuffered();

        Load += OnFormLoad;
        Disposed += OnDispose;
    }

    public LogViewerControl(LogViewerControlViewModel viewModel) : this()
        => RegisterLogDataStore(viewModel.DataStore);

    #endregion

    #region Fields

    private ILogDataStore? _dataStore;

    private static readonly SemaphoreSlim _semaphore = new(initialCount: 1);

    #endregion

    #region Methods

    public void RegisterLogDataStore(ILogDataStore dataStore)
    {
        _dataStore = dataStore;

        // moved to form load event to prevent an "index out of range" exception as the ListView for NODI projects.

        // As we are manually handling the DataBinding, we need to add existing log entries
        //AddListViewItems(_dataStore.Entries);

        //// Simple way to DataBind the ObservableCollection to the ListView is to listen to the CollectionChanged event
        //_dataStore.Entries.CollectionChanged += OnCollectionChanged;

        // NOTE: ListView is not DataBindable out of the box.But if you want to add DataBinding, here are some options:
        // https://stackoverflow.com/questions/2799017/is-it-possible-to-bind-a-list-to-a-listview-in-winforms
        // http://www.interact-sw.co.uk/utilities/bindablelistview/source/

    }

    private void OnFormLoad(object? sender, EventArgs e)
    {
        // As we are manually handling the DataBinding, we need to add existing log entries
        AddListViewItems(_dataStore!.Entries);
 
        // Simple way to DataBind the ObservableCollection to the ListView is to listen to the CollectionChanged event
        _dataStore.Entries.CollectionChanged += OnCollectionChanged;

        Load -= OnFormLoad;
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // any new items?
        if (e.NewItems?.Count > 0)
        {
            AddListViewItems(e.NewItems.Cast<LogModel>());

            ExclusiveDispatcher(() =>
            {
                // auto-scroll if required
                if (CanAutoScroll.Checked)
                    ListView.Items[^1].EnsureVisible();
            });
        }

        // any to remove? ... not required for this purpose.
        if (e.OldItems?.Count > 0)
        {
            // remove from ListView.Items
        }
    }

    private void AddListViewItems(IEnumerable<LogModel> logEntries)
    {
        ExclusiveDispatcher(() =>
        {
            // work with a snapshot of the data 
            foreach (LogModel item in logEntries)
            {
                ListViewItem lvi = new ListViewItem
                {
                    Font = new(ListView.Font, FontStyle.Regular),
                    Text = item.Timestamp.ToString("G"),
                    ForeColor = item.Color!.Foreground,
                    BackColor = item.Color.Background
                };

                lvi.SubItems.Add(item.LogLevel.ToString());
                lvi.SubItems.Add(item.EventId.ToString());
                lvi.SubItems.Add(item.State?.ToString() ?? string.Empty);
                lvi.SubItems.Add(item.Exception ?? string.Empty);
                ListView.Items.Add(lvi);
            }
        });
    }

    private void ExclusiveDispatcher(Action action)
    {
        // ensure only one operation at time from multiple threads
        _semaphore.Wait();

        // delegate to UI thread
        DispatcherHelper.Execute(action.Invoke);

        _semaphore.Release();
    }

    // cleanup time ...
    private void OnDispose(object? sender, EventArgs e)
    {
        Disposed -= OnDispose;

        if (_dataStore is null)
            return;

        _dataStore.Entries.CollectionChanged -= OnCollectionChanged;
    }

    #endregion
}