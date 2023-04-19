using LogViewer.WinForms;
using WinFormsSerilogDI.DataStores;

namespace WinFormsSerilogDI;

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