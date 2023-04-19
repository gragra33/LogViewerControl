Imports System.Reflection
Imports System.Threading
Imports CommonVB.Core
Imports LogViewerVB.Core
Imports LogViewerVB.WinForms
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports Microsoft.Extensions.Logging
Imports RandomLoggingVB.Service
Imports Serilog
Imports Serilog.Sinks.LogViewVB.Core

Namespace My

    Partial Friend Class MyApplication

#Region "Fields"

        Private Shared _host As IHost
        Private Shared _cancellationTokenSource As CancellationTokenSource

#End Region

#Region "Methods"

        Protected Overrides Function OnStartup(eventArgs As ApplicationServices.StartupEventArgs) As Boolean

            InitializeDI()

            _cancellationTokenSource = New CancellationTokenSource()

            Try
                LogStartingMode()

                ' startup background services
                Dim task As Task = _host.StartAsync(_cancellationTokenSource.Token)

            Catch oce As OperationCanceledException

                ' skip

            Catch ex As Exception

                Serilog.Log.Fatal(ex, "Application terminated unexpectedly")

                MessageBox.Show(ex.Message, $"Unhandled Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)

                CleanUp()
                Return False

            End Try

            Return MyBase.OnStartup(eventArgs)

        End Function

        ' Used my My Project\Application.Designer.vb > OnCreateMainForm() method to load the form
        Protected Function GetMainForm() As Form

            Return _host.Services.GetRequiredService(Of MainForm)

        End Function

        Protected Overrides Sub OnShutdown()

            CleanUp()
            MyBase.OnShutdown()

        End Sub

        Private Sub InitializeDI()

            Dim builder As HostApplicationBuilder = Host.CreateApplicationBuilder()

            ' 
            ' Note: For information on launch profiles for debugging,
            '     see article: https :  //www.codeproject.com/Articles/5354478/NET-App-Settings-Demystified-Csharp-VB
            ' 

            ' Random Logging Service
            builder.AddRandomBackgroundService()

            ' visual debugging tools
            builder.AddLogViewer()

            Dim services As IServiceCollection = builder.Services

            services.AddLogging(
            Sub(cfg)

                ' Use Default Colors
                'Serilog.Log.Logger = New LoggerConfiguration() _
                '    .ReadFrom.Configuration(builder.Configuration) _
                '    .WriteTo.DataStoreLoggerSink(
                '    Function() _host.Services.TryGetService(Of ILogDataStore)) _
                '.CreateLogger()

                ' Use Custom Colors
                Serilog.Log.Logger = New LoggerConfiguration() _
                        .ReadFrom.Configuration(builder.Configuration) _
                        .WriteTo.DataStoreLoggerSink(
                            Function() _host.Services.TryGetService(Of ILogDataStore),
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

                cfg.ClearProviders() _
                    .AddSerilog(Serilog.Log.Logger)

            End Sub)

            services _
                .AddSingleton(Of MainControlsDataStore) _
                .AddSingleton(Of MainForm)

            _host = builder.Build()
            _cancellationTokenSource = New CancellationTokenSource()

        End Sub

        Private Sub LogStartingMode()

            ' Get the Launch mode
            Dim isDevelopment As Boolean = String.Equals(Environment.GetEnvironmentVariable("DOTNET_MODIFIABLE_ASSEMBLIES"), "debug",
                                                        StringComparison.InvariantCultureIgnoreCase)

            ' initialize a logger & EventId
            Dim logger As ILogger(Of Application) = _host.Services.GetRequiredService(Of ILogger(Of Application))
            Dim eventId As EventId = New EventId(id:=0, name:=Assembly.GetEntryAssembly.GetName.Name)

            ' log a test pattern for each log level
            logger.TestPattern(eventId)

            ' log that we have started...
            logger.Emit(eventId, LogLevel.Information, $"Running in {If(isDevelopment, "Debug", "Release")} mode")

        End Sub

        Private Sub CleanUp()

            ' tell the background services that we are shutting down
            Dim task As Task = _host.StopAsync(_cancellationTokenSource.Token)

            ' flush logs
            Serilog.Log.CloseAndFlush()

        End Sub

#End Region

    End Class

End Namespace