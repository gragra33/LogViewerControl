Imports Avalonia

Class Program

    Public Shared Sub Main(args As String())

        BuildAvaloniaApp() _
            .StartWithClassicDesktopLifetime(args)

    End Sub

    Public Shared Function BuildAvaloniaApp() As AppBuilder

        Return AppBuilder.Configure(Of App) _
            .UsePlatformDetect() _
            .LogToTrace()

    End Function

End Class