namespace System.Windows.Threading;

public static class DispatcherHelper
{
    public static void Execute(Action action)
    {
        // no cross-thread concerns
        if (Application.OpenForms.Count == 0)
        {
            action.Invoke();
			return;
        }

		try
		{
			if (Application.OpenForms[0]!.InvokeRequired)
                // Marshall to Main Thread
				Application.OpenForms[0]?.Invoke(action);
            else
                // We are already on the Main Thread
                action.Invoke();
        }
		catch (Exception)
		{
			// ignore as might be thrown on shutting down
		}
    }
}