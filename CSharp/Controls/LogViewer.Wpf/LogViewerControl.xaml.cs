using System;
using System.Linq;
using LogViewer.Core;

namespace LogViewer.Wpf;

public partial class LogViewerControl
{
    public LogViewerControl() => InitializeComponent();

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        if (!CanAutoScroll.IsChecked == true)
            return;

        // design time
        if (DataContext is null)
            return;

        // Okay, we can now get the item and scroll into view
        LogModel? item = (DataContext as ILogDataStoreImpl)?.DataStore.Entries.LastOrDefault();
        
        if (item is null)
            return;

        ListView.ScrollIntoView(item);
    }
}