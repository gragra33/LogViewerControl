Imports System.Reflection
Imports Microsoft.Extensions.Logging
Imports RandomLoggingVB.Service
Imports System.Threading
Imports LogViewerVB.Core
Imports WinFormsNLogVBNoDI.DataStores
Imports WinFormsNLogVBNoDI.Helpers

Public Class MyMainForm

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Initialize service and pass in the Logger
        Dim service As RandomLoggingService = New RandomLoggingService(New Logger(Of RandomLoggingService)(LoggingHelper.Factory))

        ' Get the Launch mode
        Dim isDevelopment As Boolean = String.Equals(Environment.GetEnvironmentVariable("DOTNET_MODIFIABLE_ASSEMBLIES"), "debug",
                                                     StringComparison.InvariantCultureIgnoreCase)

        ' initialize a logger & EventId
        Dim logger As Logger(Of MyMainForm) = New Logger(Of MyMainForm)(LoggingHelper.Factory)
        Dim eventId As EventId = New EventId(id:=0, name:=Assembly.GetEntryAssembly.GetName.Name)

        ' log a test pattern for each log level
        logger.TestPattern(eventId:=eventId)

        ' log that we have started...
        logger.Emit(eventId, LogLevel.Information, $"Running in {If(isDevelopment, "Debug", "Release")} mode")

        ' Start generating log entries
        Dim task As Task = service.StartAsync(CancellationToken.None)

        ' manually wire up the logging to the view ... the control will show backlog entries...
        LogViewerControl.RegisterLogDataStore(MainControlsDataStore.DataStore)

    End Sub

End Class