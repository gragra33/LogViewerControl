Imports System.Runtime.CompilerServices

Public Module ServicesExtension

    <Extension>
    Public Function TryGetService(Of TModel As Class)(serviceProvider As IServiceProvider) As TModel

        Try

            Return serviceProvider.GetService(GetType(TModel))

        Catch ex As ObjectDisposedException

            ' ignore as we do not care...

        End Try

        Return Nothing

    End Function

End Module