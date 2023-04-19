Imports LogViewerVB.Core
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Logging
Imports Microsoft.Extensions.Options
Imports NLog.Targets
Imports MsLogLevel = Microsoft.Extensions.Logging.LogLevel

<Target("DataStoreLogger")>
Public Class DataStoreLoggerTarget : Inherits TargetWithLayout

#Region "Fields"

    Private _dataStore As ILogDataStore
    Private _config As DataStoreLoggerConfiguration

#End Region

#Region "methods"

    Protected Overrides Sub InitializeTarget()

        ' we need to inject dependencies
        Dim serviceProvider As IServiceProvider = ResolveService(Of IServiceProvider)()

        ' reference the shared instance
        _dataStore = serviceProvider.GetRequiredService(Of ILogDataStore)

        ' load the config options
        Dim options As IOptionsMonitor(Of DataStoreLoggerConfiguration) _
            = serviceProvider.GetService(Of IOptionsMonitor(Of DataStoreLoggerConfiguration))

        _config = If(options Is Nothing, New DataStoreLoggerConfiguration(), options.CurrentValue)

        MyBase.InitializeTarget()

    End Sub

    Protected Overrides Sub Write(logEvent As LogEventInfo)

        ' cast NLog Loglevel to Microsoft LogLevel type
        Dim logLevel As MsLogLevel = [Enum].ToObject(GetType(MsLogLevel), logEvent.Level.Ordinal)

        ' format the message
        Dim message As String = RenderLogEvent(Layout, logEvent)

        ' retrieve the EventId
        Dim eventId As EventId = logEvent.Properties("EventId")

        If eventId.Id = 0 AndAlso _config.EventId.Id <> 0 Then
            eventId = _config.EventId
        End If

        Dim exMessage As String = String.Empty

        If logEvent.Exception IsNot Nothing Then
            If String.IsNullOrEmpty(logEvent.Exception.Message) Then
                If logLevel = MsLogLevel.Error AndAlso message IsNot Nothing Then
                    exMessage = message.ToString()
                End If
            Else
                exMessage = logEvent.Exception.Message
            End If
        End If


        ' add log entry
        _dataStore.AddEntry(New LogModel() With
        {
            .Timestamp = Date.UtcNow,
           .LogLevel = logLevel,
           .EventId = eventId,
           .State = message,
           .Exception = exMessage,
           .Color = _config.Colors(logLevel)
        })

        Debug.WriteLine($"--- [{logLevel.ToString()(0.3)}] {message} - {If(String.IsNullOrWhiteSpace(exMessage), "no error", exMessage)}")

        MyBase.Write(logEvent)

    End Sub

#End Region

End Class