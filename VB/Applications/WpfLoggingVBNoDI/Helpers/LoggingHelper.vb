Imports CommonVB.Core
Imports LogViewerVB.Core
Imports Microsoft.Extensions.Logging
Imports System.Drawing

Namespace Helpers

    ' application-wide DataStoreLogger Factory ... returns a wired up Logger instance
    Public Module LoggingHelper

#Region "Constructors"

        Sub New()

            ' retrieve the log level from 'appsettings'
            Dim value As String = AppSettings(Of String).Current("Logging:LogLevel", "Default")
            If String.IsNullOrWhiteSpace(value) Then
                value = "Information"
            End If

            Dim logLevel As LogLevel
            If Not [Enum].TryParse(value, logLevel) Then
                logLevel = LogLevel.Information
            End If

            ' wire up the loggers
            Factory = LoggerFactory.Create(
                Sub(builder)

                    ' visual debugging tools
                    builder.AddDataStoreLogger()

                    ' uncomment to use custom logging colors (note: System.Drawing namespace)
                    '
                    'builder.AddDataStoreLogger(
                    '    Sub(options)

                    '        options.Colors(LogLevel.Trace) = New LogEntryColor() With
                    '        {
                    '            .Foreground = Color.White,
                    '            .Background = Color.DarkGray
                    '        }

                    '        options.Colors(LogLevel.Debug) = New LogEntryColor() With
                    '        {
                    '            .Foreground = Color.White,
                    '            .Background = Color.Gray
                    '        }

                    '        options.Colors(LogLevel.Information) = New LogEntryColor() With
                    '        {
                    '            .Foreground = Color.White,
                    '            .Background = Color.DodgerBlue
                    '        }

                    '        options.Colors(LogLevel.Warning) = New LogEntryColor() With
                    '        {
                    '            .Foreground = Color.White,
                    '            .Background = Color.Orchid
                    '        }

                    '    End Sub)


                    ' example of adding other loggers...
                    builder.AddSimpleConsole(
                            Sub(options)
                                options.SingleLine = True
                                options.TimestampFormat = "hh:mm:ss "
                            End Sub)

                    ' note:
                    '   * The IDE will automatically add the Debugger Logger, even though Not visible
                    '   * Adding the DebugLogger Is useful for remote debugging
                    ' .AddDebug()

                    ' set minimum log level from 'appsettings'
                    builder.SetMinimumLevel(logLevel)

                End Sub)

        End Sub

#End Region

#Region "Properties"

        Public ReadOnly Property Factory As ILoggerFactory

#End Region

    End Module

End Namespace