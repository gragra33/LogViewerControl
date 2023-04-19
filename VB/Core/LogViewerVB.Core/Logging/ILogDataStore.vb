Imports System.Collections.ObjectModel

Public Interface ILogDataStore

    ReadOnly Property Entries As ObservableCollection(Of LogModel)

    Sub AddEntry(logModel As LogModel)

End Interface
