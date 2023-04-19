namespace System.Windows.Threading;

public static class DispatcherHelper
{
    public static void Execute(Action action)
    {
        if (Application.Current is null || Application.Current.Dispatcher is null)
            // We are already on the Main Thread
            return;

        // Marshall to Main Thread
        Application.Current.Dispatcher.BeginInvoke( DispatcherPriority.Background, action);
    }
}