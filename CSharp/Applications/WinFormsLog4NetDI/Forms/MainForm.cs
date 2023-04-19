using LogViewer.WinForms;
using WinFormsLog4NetDI.DataStores;

namespace WinFormsLog4NetDI;

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