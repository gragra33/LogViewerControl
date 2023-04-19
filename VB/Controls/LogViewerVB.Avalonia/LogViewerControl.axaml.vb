Imports System.Collections.Specialized
Imports Avalonia.Controls
Imports Avalonia.LogicalTree
Imports Avalonia.Markup.Xaml
Imports LogViewerVB.Core

Partial Public Class LogViewerControl : Inherits UserControl

    Private _vm As ILogDataStoreImpl
    Private _model As LogModel

    Private MyDataGrid As DataGrid
    Private CanAutoScroll As CheckBox

    Sub New()
        InitializeComponent()
    End Sub

    ' Auto-wiring does not work for VB, so do it manually
    ' Wires up the controls and optionally loads XAML markup and attaches dev tools (if Avalonia.Diagnostics package is referenced)
    Private Sub InitializeComponent(Optional loadXaml As Boolean = True)

        If loadXaml Then
            AvaloniaXamlLoader.Load(Me)
        End If

        MyDataGrid = FindNameScope().Find("MyDataGrid")
        CanAutoScroll = FindNameScope().Find("CanAutoScroll")

    End Sub

    Private Shadows Sub OnDataContextChanged(sender As Object, e As EventArgs)

        If DataContext Is Nothing Then
            Return
        End If

        _vm = DirectCast(DataContext, ILogDataStoreImpl)
        AddHandler _vm.DataStore.Entries.CollectionChanged, AddressOf OnCollectionChanged


    End Sub

    Private Sub OnCollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)

        _model = MyDataGrid.Items.Cast(Of LogModel).LastOrDefault()

    End Sub

    Private Sub OnLayoutUpdated(sender As Object, e As EventArgs)

        If CanAutoScroll.IsChecked <> True OrElse _model Is Nothing Then
            Return
        End If

        MyDataGrid.ScrollIntoView(_model, Nothing)
        _model = Nothing

    End Sub

    Private Shadows Sub OnDetachedFromLogicalTree(sender As Object, e As LogicalTreeAttachmentEventArgs)

        If _vm Is Nothing Then
            Return
        End If

        RemoveHandler _vm.DataStore.Entries.CollectionChanged, AddressOf OnCollectionChanged

    End Sub

End Class