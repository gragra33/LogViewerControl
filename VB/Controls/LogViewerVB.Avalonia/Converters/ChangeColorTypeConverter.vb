Imports System.Globalization
Imports Avalonia.Data.Converters
Imports Avalonia.Media
Imports SysDrawColor = System.Drawing.Color

Namespace Converters

    Public Class ChangeColorTypeConverter : Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

            Dim sysDrawColor As SysDrawColor = DirectCast(value, SysDrawColor)
            Return New SolidColorBrush(Color.FromArgb(
                sysDrawColor.A,
                sysDrawColor.R,
                sysDrawColor.G,
                sysDrawColor.B))

        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException
        End Function

    End Class

End Namespace