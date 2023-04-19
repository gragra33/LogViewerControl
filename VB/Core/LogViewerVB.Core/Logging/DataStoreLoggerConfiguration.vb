Imports System.Drawing
Imports Microsoft.Extensions.Logging

Public Class DataStoreLoggerConfiguration

#Region "Properties"

    Public Property EventId As EventId

    Public Property Colors As Dictionary(Of LogLevel, LogEntryColor) = New Dictionary(Of LogLevel, LogEntryColor) From
    {
        {LogLevel.Trace, New LogEntryColor() With {.Foreground = Color.DarkGray}},
        {LogLevel.Debug, New LogEntryColor() With {.Foreground = Color.Gray}},
        {LogLevel.Information, New LogEntryColor()},
        {LogLevel.Warning, New LogEntryColor() With {.Foreground = Color.Orange}},
        {LogLevel.Error, New LogEntryColor() With {.Foreground = Color.White, .Background = Color.OrangeRed}},
        {LogLevel.Critical, New LogEntryColor() With {.Foreground = Color.White, .Background = Color.Red}},
        {LogLevel.None, New LogEntryColor()}
    }

#End Region

End Class