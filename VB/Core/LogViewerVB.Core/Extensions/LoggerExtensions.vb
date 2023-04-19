Imports Microsoft.Extensions.Logging
Imports System.Runtime.CompilerServices

Public Module LoggerExtensions

    ' NOTE Optional And ParamArray are not allowed in same method call, so used overload instead

    <Extension>
    Sub Emit(logger As ILogger, eventId As EventId,
             logLevel As LogLevel, message As String, ParamArray args As Object())
        logger.Emit(eventId, logLevel, message, Nothing, args)
    End Sub

    <Extension>
    Sub Emit(logger As ILogger, eventId As EventId,
        logLevel As LogLevel, message As String, [exception] As Exception, ParamArray args As Object())

        If logger Is Nothing Then
            Return
        End If

        If Not logger.IsEnabled(logLevel) Then
            Return
        End If

        Select Case logLevel
            Case LogLevel.Trace
                logger.LogTrace(eventId, message, args)

            Case LogLevel.Debug
                logger.LogDebug(eventId, message, args)

            Case LogLevel.Information
                logger.LogInformation(eventId, message, args)

            Case LogLevel.Warning
                logger.LogWarning(eventId, message, args)

            Case LogLevel.[Error]
                logger.LogError(eventId, [exception], message, args)

            Case LogLevel.Critical
                logger.LogCritical(eventId, message, args)

        End Select

    End Sub

    <Extension>
    Sub TestPattern(logger As ILogger, Optional eventId As EventId = Nothing)

        Dim exception As Exception = New Exception("Test Error Message")

        logger.Emit(eventId, LogLevel.Trace, "Trace Test Pattern")
        logger.Emit(eventId, LogLevel.Debug, "Debug Test Pattern")
        logger.Emit(eventId, LogLevel.Information, "Information Test Pattern")
        logger.Emit(eventId, LogLevel.Warning, "Warning Test Pattern")
        logger.Emit(eventId, LogLevel.Error, "Error Test Pattern", exception)
        logger.Emit(eventId, LogLevel.Critical, "Critical Test Pattern", exception)

    End Sub

End Module