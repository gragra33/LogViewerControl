Imports System.Runtime.CompilerServices
Imports LogViewerVB.Core
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Logging
Imports NLog.Target.LogViewVB.Core
Imports WinFormsNLogVBNoDI.DataStores

Public Module ServicesExtension

    <Extension>
    Public Function AddNLogTargetsNoDI(builder As ILoggingBuilder, config As IConfiguration) As ILoggingBuilder

        ' We need to use a shared instance of the DataStore to pass to the LogViewerControl
        builder.Services.AddSingleton(MainControlsDataStore.DataStore)

        ' call core NLog ServiceExtension initializer
        builder.AddNLogTargets(config)

        Return builder

    End Function

    <Extension>
    Public Function AddNLogTargetsNoDI(builder As ILoggingBuilder, config As IConfiguration, configure As Action(Of DataStoreLoggerConfiguration)) As ILoggingBuilder

        builder.AddNLogTargetsNoDI(config)
        builder.Services.Configure(configure)
        Return builder

    End Function

End Module