Imports System.Runtime.CompilerServices
Imports LogViewerVB.Core
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Logging
Imports NLog.Extensions.Logging
Imports MsLogLevel = Microsoft.Extensions.Logging.LogLevel

Public Module ServicesExtension

    <Extension>
    Public Function AddNLogTargets(builder As ILoggingBuilder, config As IConfiguration) As ILoggingBuilder

        LogManager _
            .Setup() _
            .SetupExtensions(
                Sub(extensionBuilder) _
                    extensionBuilder.RegisterTarget(Of DataStoreLoggerTarget)("DataStoreLogger"))

        ' Load NLog settings from appsettings*.json
        ' custom options for capturing the EventId information
        ' https://nlog-project.org/2021/08/25/nlog-5-0-preview1-ready.html#nlogextensionslogging-changes-capture-of-eventid
        builder _
            .ClearProviders() _
            .SetMinimumLevel(MsLogLevel.Trace) _
            .AddNLog(config,
                New NLogProviderOptions With
                {
                    .IgnoreEmptyEventId = False,
                    .CaptureEventId = EventIdCaptureType.Legacy
                })

        Return builder

    End Function

    <Extension>
    Public Function AddNLogTargets(builder As ILoggingBuilder, config As IConfiguration, configure As Action(Of DataStoreLoggerConfiguration)) As ILoggingBuilder

        builder.AddNLogTargets(config)
        builder.Services.Configure(configure)
        Return builder

    End Function

End Module