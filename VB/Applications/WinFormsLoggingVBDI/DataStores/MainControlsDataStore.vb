Imports LogViewerVB.Core
Imports LogViewerVB.WinForms

Public Class MainControlsDataStore

#Region "Constructors"

    Public Sub New(logViewer As LogViewerControl,
                   dataStore As ILogDataStore)

        Me.LogViewer = logViewer
        Me.DataStore = dataStore

    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property LogViewer As LogViewerControl

    Public Property DataStore As ILogDataStore

#End Region

End Class