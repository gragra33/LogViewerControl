Imports System.Collections.Concurrent
Imports LogViewerVB.Core
Imports Microsoft.Extensions.Logging
Imports Microsoft.Extensions.Options

Public Class DataStoreLoggerProvider : Implements ILoggerProvider

#Region "Constructors"

    Public Sub New(config As IOptionsMonitor(Of DataStoreLoggerConfiguration), dataStore As ILogDataStore)

        _dataStore = dataStore
        _currentConfig = config.CurrentValue
        _onChangeToken = config.OnChange(Sub(updatedConfig) _currentConfig = updatedConfig)

    End Sub

#End Region

#Region "Fields"

    Private _currentConfig As DataStoreLoggerConfiguration

    Private ReadOnly _onChangeToken As IDisposable
    Protected ReadOnly _dataStore As ILogDataStore

    Protected ReadOnly _loggers As ConcurrentDictionary(Of String, DataStoreLogger) =
                         New ConcurrentDictionary(Of String, DataStoreLogger)()

#End Region

#Region "Methods"

    Public Overridable Function CreateLogger(categoryName As String) As ILogger Implements ILoggerProvider.CreateLogger

        Return _loggers.GetOrAdd(categoryName, Function(name) New DataStoreLogger(name, AddressOf GetCurrentConfig, _dataStore))

    End Function

    Protected Function GetCurrentConfig() As DataStoreLoggerConfiguration

        Return _currentConfig

    End Function

    Public Sub Dispose() Implements IDisposable.Dispose

        _loggers.Clear()
        _onChangeToken?.Dispose()

    End Sub

#End Region

End Class