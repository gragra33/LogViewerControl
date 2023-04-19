Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.Extensions.Configuration

Public Module ConfigurationExtension

    <Extension>
    Public Function Initialize(builder As IConfigurationBuilder) As IConfigurationBuilder

        Dim env As String = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
        If String.IsNullOrWhiteSpace(env) Then
            env = "Production"
        End If

        Return New ConfigurationBuilder() _
                .SetBasePath(Directory.GetCurrentDirectory()) _
                .AddJsonFile("appsettings.json", optional:=True, reloadOnChange:=True) _
                .AddJsonFile($"appsettings.{env}.json", optional:=True, reloadOnChange:=True) _
                .AddEnvironmentVariables()

    End Function

End Module