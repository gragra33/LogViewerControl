Imports CommonVB.Wpf
Imports LogViewerVB.Core

Namespace Logging

    Public Class LogDataStore : Inherits Core.LogDataStore

#Region "Methods"

        Public Overrides Sub AddEntry(logModel As LogModel)

            ' Marshall to Main Thread
            'DispatcherHelper
            Execute(Sub() MyBase.AddEntry(logModel))

        End Sub

#End Region

    End Class

End Namespace