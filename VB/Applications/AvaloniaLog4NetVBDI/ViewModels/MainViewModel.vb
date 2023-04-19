Imports LogViewerVB.Core.ViewModels

Public Class MainViewModel : Inherits ViewModelBase

#Region "Constructor"

    Public Sub New(logViewer As LogViewerControlViewModel)
        Me.LogViewer = logViewer
    End Sub

#End Region

#Region "Properties"

    Public Property LogViewer As LogViewerControlViewModel

#End Region

End Class