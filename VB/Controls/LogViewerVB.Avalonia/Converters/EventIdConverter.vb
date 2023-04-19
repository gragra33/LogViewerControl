Imports System.Globalization
Imports Avalonia.Data.Converters
Imports Microsoft.Extensions.Logging

Namespace Converters
    Public Class EventIdConverter : Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

            If value Is Nothing Then
                Return "0"
            End If

            Dim eventId As EventId = DirectCast(value, EventId)

            Return eventId.ToString()

        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Return New EventId(0, If(value Is Nothing, String.Empty, value.ToString()))
        End Function

    End Class

End Namespace