Imports System.Runtime.CompilerServices
Imports LogViewerVB.Core
Imports Serilog.Configuration

Public Module DataStoreLoggerSinkExtensions

    <Extension>
    Public Function DataStoreLoggerSink(loggerConfiguration As LoggerSinkConfiguration,
                                        dataStoreProvider As Func(Of ILogDataStore),
                                        Optional configuration As Action(Of DataStoreLoggerConfiguration) = Nothing,
                                        Optional formatProvider As IFormatProvider = Nothing) As LoggerConfiguration

        Return loggerConfiguration.Sink(New DataStoreLoggerSink(dataStoreProvider, GetConfig(configuration), formatProvider))

    End Function

    Private Function GetConfig(configuration As Action(Of DataStoreLoggerConfiguration)) As Func(Of DataStoreLoggerConfiguration)

        Dim data As DataStoreLoggerConfiguration = New DataStoreLoggerConfiguration()

        If configuration IsNot Nothing Then
            configuration.Invoke(data)
        End If

        Return Function() data

    End Function

End Module