Imports Avalonia.Threading
Imports LogViewerVB.Core

Namespace Logging

    Public Class LogDataStore : Inherits Core.LogDataStore

#Region "Methods"

        Public Overrides Async Sub AddEntry(logModel As LogModel)

            ' Marshall to Main Thread
            Await Dispatcher.UIThread.InvokeAsync(Sub() MyBase.AddEntry(logModel))

        End Sub

#End Region

    End Class

End Namespace