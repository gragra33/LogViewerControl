Imports MvvmVB.Core

Namespace ViewModels

    Public Class LogViewerControlViewModel : Inherits ViewModel : Implements ILogDataStoreImpl

#Region "Constructors"

        Public Sub New(store As ILogDataStore)
            DataStore = store
        End Sub

#End Region

#Region "Properties"

        Public ReadOnly Property DataStore As ILogDataStore Implements ILogDataStoreImpl.DataStore

#End Region

    End Class

End Namespace