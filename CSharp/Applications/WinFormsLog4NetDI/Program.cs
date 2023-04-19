namespace WinFormsLog4NetDI;

internal static class Program
{
    #region Bootstrap

    [STAThread]
    static void Main() => _ = new Bootstrapper();

    #endregion
}