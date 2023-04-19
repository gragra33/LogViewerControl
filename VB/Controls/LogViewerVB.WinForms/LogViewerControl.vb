Imports System.Collections.Specialized
Imports System.Threading
Imports CommonVB.WinForms
Imports LogViewerVB.Core
Imports LogViewerVB.Core.ViewModels

Public Class LogViewerControl

#Region "Constructors"

    ' supports DI and non-DI usage

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Stop the flickering!
        ListView.SetDoubleBuffered()

        AddHandler Load, AddressOf OnFormLoad
        AddHandler Disposed, AddressOf OnDispose

    End Sub

    Public Sub New(viewModel As LogViewerControlViewModel)

        Me.New()

        RegisterLogDataStore(viewModel.DataStore)

    End Sub

#End Region

#Region "Fields"

    Private _dataStore As ILogDataStore

    Private Shared ReadOnly _semaphore As SemaphoreSlim = New SemaphoreSlim(initialCount:=1)

#End Region

#Region "Methods"

    Public Sub RegisterLogDataStore(datastore As ILogDataStore)

        _dataStore = datastore

        ' moved to form load event to prevent an "index out of range" exception as the ListView for NODI projects.

        '' As we are manually handling the DataBinding, we need to add existing log entries
        'AddListViewItems(_dataStore.Entries)

        '' Simple way to DataBind the ObservableCollection to the ListView Is to listen to the CollectionChanged event
        'AddHandler _dataStore.Entries.CollectionChanged, AddressOf OnCollectionChanged

    End Sub

    Private Sub OnFormLoad(sender As Object, e As EventArgs)

        ' As we are manually handling the DataBinding, we need to add existing log entries
        AddListViewItems(_dataStore.Entries)

        ' Simple way to DataBind the ObservableCollection to the ListView Is to listen to the CollectionChanged event
        AddHandler _dataStore.Entries.CollectionChanged, AddressOf OnCollectionChanged

        RemoveHandler Load, AddressOf OnFormLoad

    End Sub

    Private Sub OnCollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)

        ' any new items?
        If e.NewItems IsNot Nothing AndAlso e.NewItems.Count > 0 Then
            AddListViewItems(e.NewItems.Cast(Of LogModel))

            ExclusiveDispatcher(
                Sub()

                    ' auto-scroll if required
                    If CanAutoScroll.Checked Then
                        ListView.Items(ListView.Items.Count - 1).EnsureVisible()
                    End If

                End Sub)
        End If

        ' any to remove? ... not required for this purpose.
        If e.OldItems IsNot Nothing AndAlso e.OldItems.Count > 0 Then
            ' remove from ListView.Items
        End If

    End Sub

    Private Sub AddListViewItems(logEntries As IEnumerable(Of LogModel))


        ExclusiveDispatcher(
            Sub()

                For Each item As LogModel In logEntries
                    Dim lvi As ListViewItem = New ListViewItem With
                        {
                            .Font = New Font(ListView.Font, FontStyle.Regular),
                            .Text = item.Timestamp.ToString("G"),
                            .ForeColor = item.Color.Foreground,
                            .BackColor = item.Color.Background
                        }

                    lvi.SubItems.Add(item.LogLevel.ToString())
                    lvi.SubItems.Add(item.EventId.ToString())
                    lvi.SubItems.Add(If(item.State Is Nothing, String.Empty, item.State.ToString()))
                    lvi.SubItems.Add(If(item.Exception Is Nothing, String.Empty, item.Exception.ToString()))

                    ListView.Items.Add(lvi)
                Next

            End Sub)

    End Sub

    Private Sub ExclusiveDispatcher(action As Action)

        ' ensure only one operation at time from multiple threads
        _semaphore.Wait()

        ' delegate to UI thread
        'DispatcherHelper.
        Execute(Sub() action.Invoke())

        _semaphore.Release()

    End Sub

    ' cleanup time ...
    Private Sub OnDispose(sender As Object, e As EventArgs)

        RemoveHandler Disposed, AddressOf OnDispose

        If _dataStore Is Nothing Then
            Return
        End If

        RemoveHandler _dataStore.Entries.CollectionChanged, AddressOf OnCollectionChanged

    End Sub

#End Region

End Class