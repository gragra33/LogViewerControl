Imports System.Runtime.CompilerServices
Imports LogViewerVB.Core
Imports LogViewerVB.Core.ViewModels
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting

Public Module ServicesExtension

    <Extension>
    Public Function AddLogViewer(builder As HostApplicationBuilder) As HostApplicationBuilder

        builder.Services.AddSingleton(Of ILogDataStore, Logging.LogDataStore)
        builder.Services.AddSingleton(Of LogViewerControlViewModel)
        builder.Services.AddTransient(Of LogViewerControl)

        Return builder

    End Function

End Module