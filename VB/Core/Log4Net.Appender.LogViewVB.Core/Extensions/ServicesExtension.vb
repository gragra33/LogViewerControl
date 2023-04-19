Imports System.Runtime.CompilerServices
Imports LogViewerVB.Core
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Logging

Public Module ServicesExtension

    <Extension>
    Public Function AddLog4Net(builder As ILoggingBuilder, config As IConfiguration) As ILoggingBuilder

        builder _
            .ClearProviders() _
            .AddLog4Net(config.GetLog4NetConfiguration())

        Return builder

    End Function

    <Extension>
    Public Function AddLog4Net(builder As ILoggingBuilder, config As IConfiguration, configure As Action(Of DataStoreLoggerConfiguration)) As ILoggingBuilder

        builder.AddLog4Net(config)
        builder.Services.Configure(configure)
        Return builder

    End Function

    <Extension>
    Private Function GetLog4NetConfiguration(configuration As IConfiguration) As Log4NetProviderOptions

        Return configuration _
            .GetSection("Log4NetCore") _
            .Get(Of Log4NetProviderOptions)

    End Function

End Module