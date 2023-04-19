using LogViewer.WinForms;
using WinFormsLoggingDI.DataStores;

namespace WinFormsLoggingDI;

public partial class MainForm : Form
{
    #region Constructors

    public MainForm(MainControlsDataStore controlsDataStore)
    {
        InitializeComponent();

        // wire up the control
        HostPanel.AddControl(controlsDataStore.LogViewer);
    }

    #endregion
}