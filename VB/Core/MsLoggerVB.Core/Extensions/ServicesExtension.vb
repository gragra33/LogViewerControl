Imports System.Runtime.CompilerServices
Imports LogViewerVB.Core
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.DependencyInjection.Extensions
Imports Microsoft.Extensions.Logging

Public Module ServicesExtension

    <Extension>
    Public Function AddDefaultDataStoreLogger(builder As ILoggingBuilder) As ILoggingBuilder

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton(Of ILoggerProvider, DataStoreLoggerProvider))
        Return builder

    End Function

    <Extension>
    Public Function AddDefaultDataStoreLogger(builder As ILoggingBuilder, configure As Action(Of DataStoreLoggerConfiguration)) As ILoggingBuilder

        builder.AddDefaultDataStoreLogger()
        builder.Services.Configure(configure)
        Return builder

    End Function

End Module