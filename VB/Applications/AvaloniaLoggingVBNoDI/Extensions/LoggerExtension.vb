Imports LogViewerVB.Core
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Logging
Imports Microsoft.Extensions.Logging.Configuration
Imports System.Runtime.CompilerServices
Imports AvaloniaLoggingVBNoDI.DataStores
Imports Microsoft.Extensions.DependencyInjection.Extensions
Imports MsLoggerVB.Core

Public Module LoggerExtension

    <Extension>
    Public Function AddDataStoreLogger(builder As ILoggingBuilder) As ILoggingBuilder

        builder.AddConfiguration()

        ' We need to use a shared instance of the DataStore to pass to the LogViewerControl
        builder.Services.AddSingleton(MainControlsDataStore.DataStore)
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton(Of ILoggerProvider, DataStoreLoggerProvider))
        Return builder

    End Function

    <Extension>
    Public Function AddDataStoreLogger(builder As ILoggingBuilder, configure As Action(Of DataStoreLoggerConfiguration)) As ILoggingBuilder

        builder.AddDataStoreLogger()
        builder.Services.Configure(configure)
        Return builder

    End Function

End Module