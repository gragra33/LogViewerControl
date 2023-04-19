Imports Log4Net.Core
Imports LogViewerVB.Core
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Logging

Public Class DataStoreLoggerAppender : Inherits ServiceAppenderSkeleton

#Region "Fields"

    Private _dataStore As ILogDataStore
    Private _options As DataStoreLoggerConfiguration

    Private _serviceProvider As IServiceProvider

#End Region

#Region "Methods"

    Protected Overrides Sub Append(loggingEvent As LoggingEvent)

        If _serviceProvider Is Nothing Then
            Initialize()
        End If

        ' cast matching Log4Net Loglevel to Microsoft LogLevel type
        Dim logLevel As LogLevel

        Select Case loggingEvent.Level.Value
            Case Integer.MaxValue : logLevel = LogLevel.None
            Case 120000 : logLevel = LogLevel.Debug
            Case 90000 : logLevel = LogLevel.Critical
            Case 70000 : logLevel = LogLevel.Error
            Case 60000 : logLevel = LogLevel.Warning
            Case 20000 : logLevel = LogLevel.Trace
            Case Else : logLevel = LogLevel.Information
        End Select

        Dim config As DataStoreLoggerConfiguration = If(_options Is Nothing, New DataStoreLoggerConfiguration, _options)

        Dim eventId As EventId = loggingEvent.LookupProperty(NameOf(eventId))
        eventId = If(eventId = Nothing AndAlso config.EventId.Id <> 0, config.EventId, eventId)

        Dim message As String = loggingEvent.RenderedMessage

        Dim exceptionMessage = loggingEvent.GetExceptionString()

        _dataStore.AddEntry(
            New LogModel() With
            {
                .Timestamp = Date.UtcNow,
                .LogLevel = logLevel,
                .State = message,
                .Exception = exceptionMessage,
                .EventId = eventId,
                .Color = config.Colors(logLevel)
            })

        Debug.WriteLine($"--- [{logLevel.ToString()(0.3)}] {message} - {If(String.IsNullOrWhiteSpace(exceptionMessage), "no error", exceptionMessage)}")

    End Sub

    Private Sub Initialize()

        _serviceProvider = ResolveService(Of IServiceProvider)()
        _dataStore = _serviceProvider.GetRequiredService(Of ILogDataStore)
        _options = _serviceProvider.GetService(Of DataStoreLoggerConfiguration)

    End Sub

#End Region

End Class