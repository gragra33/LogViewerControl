using LogViewer.WinForms;
using WinFormsNLogDI.DataStores;

namespace WinFormsNLogDI;

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