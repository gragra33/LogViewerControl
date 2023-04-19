Imports System.Runtime.CompilerServices
Imports log4net.Appender.LogViewVB.Core
Imports LogViewerVB.Core
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Logging
Imports WpfLog4NetVBNoDI.DataStores

Public Module ServicesExtension

    <Extension>
    Public Function AddLog4NetNoDI(builder As ILoggingBuilder, config As IConfiguration) As ILoggingBuilder

        ' We need to use a shared instance of the DataStore to pass to the LogViewerControl
        builder.Services.AddSingleton(MainControlsDataStore.DataStore)

        ' call core Log4Net ServiceExtension initializer
        builder.AddLog4Net(config)

        Return builder

    End Function

    <Extension>
    Public Function AddLog4NetNoDI(builder As ILoggingBuilder, config As IConfiguration, configure As Action(Of DataStoreLoggerConfiguration)) As ILoggingBuilder

        builder.AddLog4NetNoDI(config)
        builder.Services.Configure(configure)
        Return builder

    End Function

End Module