Imports LogViewerVB.Core

Public Class LogViewerControl

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

    End Sub

    Private Sub OnLayoutUpdated(sender As Object, e As EventArgs)

        If Not CanAutoScroll.IsChecked Then
            Return
        End If

        ' design time
        If DataContext Is Nothing Then
            Return
        End If

        Dim store As ILogDataStoreImpl = DirectCast(DataContext, ILogDataStoreImpl)


        ' Okay, we can now get the item and scroll into view
        Dim item As LogModel = store.DataStore.Entries.LastOrDefault()

        If item Is Nothing Then
            Return
        End If

        ListView.ScrollIntoView(item)

    End Sub

End Class