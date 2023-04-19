Imports System.Collections.ObjectModel
Imports System.Threading

Public Class LogDataStore : Implements ILogDataStore

#Region "Fields"

    Private Shared ReadOnly _semaphore = New SemaphoreSlim(initialCount:=1)

#End Region

#Region "Properties"

    Public ReadOnly Property Entries As ObservableCollection(Of LogModel) _
        = New ObservableCollection(Of LogModel) _
        Implements ILogDataStore.Entries

#End Region

#Region "Methods"

    Public Overridable Sub AddEntry(logModel As LogModel) Implements ILogDataStore.AddEntry

        ' ensure only one operation at time from multiple threads
        _semaphore.Wait()

        Entries.Add(logModel)

        _semaphore.Release()

    End Sub

#End Region

End Class