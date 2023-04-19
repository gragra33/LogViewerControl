Imports LogViewerVB.Core
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.Logging
Imports Serilog
Imports Serilog.Sinks.LogViewVB.Core
Imports WinFormsSerilogVBNoDI.DataStores

Namespace Helpers

    ' application-wide DataStoreLogger Factory ... returns a wired up Logger instance
    Public Module LoggingHelper

#Region "Constructors"

        Sub New()

            Dim configuration As IConfigurationRoot = New ConfigurationBuilder() _
                .Initialize() _
                .Build()

            'Log.Logger = New LoggerConfiguration() _
            '.ReadFrom.Configuration(configuration) _
            '.WriteTo.DataStoreLoggerSink(Function() MainControlsDataStore.DataStore) _
            '.CreateLogger()

            Log.Logger = New LoggerConfiguration() _
                    .ReadFrom.Configuration(configuration) _
                    .WriteTo.DataStoreLoggerSink(
                        Function() MainControlsDataStore.DataStore,
                        Sub(options)

                            options.Colors(LogLevel.Trace) = New LogEntryColor() With
                            {
                                .Foreground = Color.White,
                                .Background = Color.DarkGray
                            }

                            options.Colors(LogLevel.Debug) = New LogEntryColor() With
                            {
                                .Foreground = Color.White,
                                .Background = Color.Gray
                            }

                            options.Colors(LogLevel.Information) = New LogEntryColor() With
                            {
                                .Foreground = Color.White,
                                .Background = Color.DodgerBlue
                            }

                            options.Colors(LogLevel.Warning) = New LogEntryColor() With
                            {
                                .Foreground = Color.White,
                                .Background = Color.Orchid
                            }

                        End Sub) _
                    .CreateLogger()

            ' wire up the loggers
            Factory = LoggerFactory.Create(Sub(LoggingBuilder) LoggingBuilder.AddSerilog(Log.Logger))

        End Sub

#End Region

#Region "Properties"

        Public ReadOnly Property Factory As ILoggerFactory

#End Region

#Region "Methods"

        Friend Sub CloseAndFlush()

            Log.CloseAndFlush()

        End Sub

#End Region

    End Module

End Namespace