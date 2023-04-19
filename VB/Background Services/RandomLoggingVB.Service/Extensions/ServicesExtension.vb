Imports System.Runtime.CompilerServices
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting

Public Module ServicesExtension

    <Extension>
    Public Function AddRandomBackgroundService(builder As HostApplicationBuilder) As HostApplicationBuilder

        builder.Services.AddSingleton(Of RandomLoggingService)
        builder.Services.AddHostedService(Function(service) service.GetRequiredService(Of RandomLoggingService))

        Return builder

    End Function

End Module