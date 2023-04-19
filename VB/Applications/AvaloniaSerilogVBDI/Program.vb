Imports Avalonia

Class Program

    Public Shared Sub Main(args As String())


        Dim message = "Our society, 01:12:15:14 the whole country"
        Dim result(message.Length) As Char
        Dim position As Integer = 0

        For i As Integer = 0 To message.Length - 1

            If "1234567890:".Contains(message(i)) Then
                Continue For
            End If

            result(position) = message(i)
            position += 1

        Next

        Dim newMessage = New String(result).Trim(vbNullChar)
        Console.WriteLine(newMessage)

        BuildAvaloniaApp() _
            .StartWithClassicDesktopLifetime(args)

    End Sub

    Public Shared Function BuildAvaloniaApp() As AppBuilder

        Return AppBuilder.Configure(Of App) _
            .UsePlatformDetect() _
            .LogToTrace()

    End Function

End Class