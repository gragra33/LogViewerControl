Imports System.Diagnostics
Imports LogViewerVB.Core
Imports Microsoft.Extensions.Logging

Public Class DataStoreLogger : Implements ILogger

    ' ref https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider

#Region "Constructors"

    Public Sub New(name As String, getCurrentConfig As Func(Of DataStoreLoggerConfiguration), dataStore As ILogDataStore)
        _name = name
        _getCurrentConfig = getCurrentConfig
        _dataStore = dataStore
    End Sub

#End Region

#Region "Fields"

    Private ReadOnly _dataStore As ILogDataStore
    Private ReadOnly _name As String
    Private ReadOnly _getCurrentConfig As Func(Of DataStoreLoggerConfiguration)

#End Region

#Region "Methods"

    Public Function BeginScope(Of TState)(state As TState) As IDisposable Implements ILogger.BeginScope
        Return Nothing
    End Function

    Public Function IsEnabled(logLevel As LogLevel) As Boolean Implements ILogger.IsEnabled
        Return True
    End Function

    Public Overridable Sub Log(Of TState)(logLevel As LogLevel, eventId As EventId, state As TState, exception As Exception, formatter As Func(Of TState, Exception, String)) Implements ILogger.Log

        If Not IsEnabled(logLevel) Then
            Return
        End If

        Dim exMessage As String = String.Empty

        If exception IsNot Nothing Then
            If String.IsNullOrEmpty(exception.Message) Then
                If logLevel = LogLevel.Error AndAlso state IsNot Nothing Then
                    exMessage = state.ToString()
                End If
            Else
                exMessage = exception.Message
            End If
        End If

        Dim internalEventId As EventId = eventId
        Dim config As DataStoreLoggerConfiguration = _getCurrentConfig()

        If eventId.Id = 0 AndAlso config.EventId.Id <> 0 Then
            internalEventId = config.EventId
        End If

        _dataStore.AddEntry(New LogModel() With
        {
            .Timestamp = Now,
            .LogLevel = logLevel,
            .EventId = internalEventId,
            .State = state,
            .Exception = exMessage,
            .Color = config.Colors(logLevel)
        })

        Debug.WriteLine($"--- [{logLevel.ToString()(0.3)}] {_name} - {formatter(state, exception)}")

    End Sub

#End Region

End Class