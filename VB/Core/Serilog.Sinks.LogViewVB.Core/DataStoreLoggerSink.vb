Imports LogViewerVB.Core
Imports Microsoft.Extensions.Logging
Imports Serilog.Core
Imports Serilog.Events

Public Class DataStoreLoggerSink : Implements ILogEventSink

    Protected ReadOnly _dataStoreProvider As Func(Of ILogDataStore)

    Private ReadOnly _formatProvider As IFormatProvider
    Private ReadOnly _getCurrentConfig As Func(Of DataStoreLoggerConfiguration)

    Public Sub New(dataStoreProvider As Func(Of ILogDataStore),
                   Optional getCurrentConfig As Func(Of DataStoreLoggerConfiguration) = Nothing,
                   Optional formatProvider As IFormatProvider = Nothing)
        _dataStoreProvider = dataStoreProvider
        _formatProvider = formatProvider
        _getCurrentConfig = getCurrentConfig
    End Sub


    Public Sub Emit(logEvent As LogEvent) Implements ILogEventSink.Emit

        Dim logLevel As LogLevel

        Select Case logEvent.Level
            Case LogEventLevel.Verbose : logLevel = LogLevel.Trace
            Case LogEventLevel.Debug : logLevel = LogLevel.Debug
            Case LogEventLevel.Warning : logLevel = LogLevel.Warning
            Case LogEventLevel.Error : logLevel = LogLevel.Error
            Case LogEventLevel.Fatal : logLevel = LogLevel.Critical
            Case Else : logLevel = LogLevel.Information
        End Select

        Dim config As DataStoreLoggerConfiguration = If(_getCurrentConfig Is Nothing,
                                                        New DataStoreLoggerConfiguration(),
                                                        _getCurrentConfig.Invoke())

        Dim eventId As EventId = EventIdFactory(logEvent)
        If eventId.Id = 0 AndAlso config.EventId <> 0 Then
            eventId = config.EventId
        End If

        Dim message As String = logEvent.RenderMessage(_formatProvider)

        Dim exception As String = If(logEvent.Exception Is Nothing,
                                     If(logEvent.Level >= LogEventLevel.Error, message, String.Empty),
                                     logEvent.Exception.Message)

        Dim color As LogEntryColor = config.Colors(logLevel)

        AddLogEntry(logLevel, eventId, message, exception, color)

    End Sub

    Protected Overridable Sub AddLogEntry(logLevel As LogLevel, eventId As EventId, message As String, exception As String, color As LogEntryColor)

        Dim dataStore As ILogDataStore = _dataStoreProvider.Invoke()

        If dataStore Is Nothing Then
            Return
        End If

        dataStore.AddEntry(
            New LogModel() With
            {
                .Timestamp = DateTime.UtcNow,
                .LogLevel = logLevel,
                .EventId = eventId,
                .State = message,
                .Exception = exception,
                .Color = color
            })

    End Sub

    Private Shared Function EventIdFactory(logEvent As LogEvent) As EventId

        Dim eventId As EventId
        Dim src As LogEventPropertyValue

        If Not logEvent.Properties.TryGetValue("EventId", src) Then
            Return New EventId()
        End If

        Dim id As Integer = Nothing
        Dim eventName As String = Nothing

        ' ref: https://stackoverflow.com/a/56722516
        Dim value As StructureValue = DirectCast(src, StructureValue)

        Dim idProperty As LogEventProperty = value.Properties.FirstOrDefault(Function(x) x.Name.Equals("Id"))
        If idProperty IsNot Nothing Then
            id = Integer.Parse(idProperty.Value.ToString())
        End If

        Dim nameProperty As LogEventProperty = value.Properties.FirstOrDefault(Function(x) x.Name.Equals("Name"))
        If nameProperty IsNot Nothing Then
            eventName = nameProperty.Value.ToString().Trim(""""c)
        End If

        eventId = New EventId(If(id = Nothing, 0, id), If(String.IsNullOrWhiteSpace(eventName), String.Empty, eventName))

        Return eventId

    End Function

End Class