Imports System.ComponentModel
Imports System.Reflection
Imports System.Threading
Imports Avalonia.Controls
Imports Avalonia.Markup.Xaml
Imports AvaloniaSerilogVBNoDI.DataStores
Imports AvaloniaSerilogVBNoDI.Helpers
Imports LogViewerVB.Avalonia
Imports LogViewerVB.Core
Imports Microsoft.Extensions.Logging
Imports RandomLoggingVB.Service

Partial Public Class MainWindow : Inherits Window : Implements ILogDataStoreImpl

#Region "Constructors"

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Initialize service and pass in the Logger
        _service = New RandomLoggingService(New Logger(Of RandomLoggingService)(LoggingHelper.Factory))

        ' Get the Launch mode
        Dim isDevelopment = String.Equals(Environment.GetEnvironmentVariable("DOTNET_MODIFIABLE_ASSEMBLIES"), "debug",
                                          StringComparison.InvariantCultureIgnoreCase)

        ' initialize a logger
        Dim logger As Logger(Of MainWindow) = New Logger(Of MainWindow)(LoggingHelper.Factory)
        Dim eventId As EventId = New EventId(id:=0, name:=Assembly.GetEntryAssembly.GetName.Name)

        ' log a test pattern for each log level
        logger.TestPattern(eventId)

        ' log that we have started...
        logger.Emit(eventId, LogLevel.Information, $"Running in {If(isDevelopment, "Debug", "Release")} mode")

        ' Start generating log entries
        Dim task As Task = _service.StartAsync(CancellationToken.None)

        ' manually wire up the logging to the view ... the control will show backlog entries...
        DataStore = MainControlsDataStore.DataStore

        ' we can't bind the controls' DataContext to a static object, so assign the DataStore to the Window
        ' and pass a reference to the Window itself
        _logViewerControl.DataContext = Me

        AddHandler _window.Closing, AddressOf OnClosing

    End Sub

#End Region

#Region "Fields"

    Private _window As Window
    Private _logViewerControl As LogViewerControl

    Private ReadOnly _service As RandomLoggingService

#End Region

#Region "Properties"

    Public Property DataStore As ILogDataStore Implements ILogDataStoreImpl.DataStore

#End Region

#Region "Methods"

    ' Auto-wiring does not work for VB, so do it manually
    ' Wires up the controls and optionally loads XAML markup and attaches dev tools (if Avalonia.Diagnostics package is referenced)
    Private Sub InitializeComponent(Optional loadXaml As Boolean = True)

        If loadXaml Then
            AvaloniaXamlLoader.Load(Me)
        End If

        _window = FindNameScope().Find("Window")
        _logViewerControl = FindNameScope().Find("LogViewerControl")

    End Sub

    ' flush logs and clean up
    Private Shadows Sub OnClosing(sender As Object, e As CancelEventArgs)

        RemoveHandler _window.Closing, AddressOf OnClosing

        Dim task As Task = _service.StopAsync(CancellationToken.None)
        LoggingHelper.CloseAndFlush()

    End Sub

#End Region

End Class