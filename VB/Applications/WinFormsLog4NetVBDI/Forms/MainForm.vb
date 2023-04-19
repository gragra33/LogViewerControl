Imports LogViewerVB.WinForms

Public Class MainForm

#Region "Constructors"

    Sub New(controlsDataStore As MainControlsDataStore)

        ' This call is required by the designer.
        InitializeComponent()

        ' wire up the control
        HostPanel.AddControl(controlsDataStore.LogViewer)

    End Sub

#End Region

End Class