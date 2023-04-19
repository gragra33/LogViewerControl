Imports Microsoft.Extensions.Logging

Public Class LogModel

    Public Property Timestamp As Date

    Public Property LogLevel As LogLevel

    Public Property EventId As EventId

    Public Property State As Object

    Public Property Exception As String

    Public Property Color As LogEntryColor

End Class