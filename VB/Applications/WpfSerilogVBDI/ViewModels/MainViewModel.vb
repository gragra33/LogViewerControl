Imports LogViewerVB.Core.ViewModels
Imports MvvmVB.Core

Public Class MainViewModel :  Inherits Viewmodel

#Region "Constructor"

    Public Sub New(logViewer As LogViewerControlViewModel)
        Me.LogViewer = logViewer
    End Sub

#End Region

#Region "Properties"

    Public Property LogViewer As LogViewerControlViewModel

#End Region

End Class