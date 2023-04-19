Imports Avalonia.Controls
Imports Avalonia.Markup.Xaml
Imports LogViewerVB.Avalonia
Imports LogViewerVB.Core

Partial Public Class MainWindow : Inherits Window

    'Private LogViewerControl As LogViewerControl

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

    End Sub

    ' Auto-wiring does not work for VB, so do it manually
    ' Wires up the controls and optionally loads XAML markup and attaches dev tools (if Avalonia.Diagnostics package is referenced)
    Private Sub InitializeComponent(Optional loadXaml As Boolean = True)

        If loadXaml Then
            AvaloniaXamlLoader.Load(Me)
        End If

        'LogViewerControl = FindNameScope().Find("LogViewerControl")

    End Sub

End Class