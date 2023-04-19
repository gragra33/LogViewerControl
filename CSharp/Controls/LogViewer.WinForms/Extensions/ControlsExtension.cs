namespace LogViewer.WinForms;

public static class ControlsExtension
{
    public static void AddControl(this Panel panel, Control control)
    {
        panel.Controls.Add(control);
        control.Dock = DockStyle.Fill;
        control.BringToFront();
    }

    // ref: https://stackoverflow.com/a/42389596
    public static void SetDoubleBuffered(this Control control, bool doubleBuffered = true)
    {
        control
            .GetType()
            .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(control, doubleBuffered, null);
    }
}