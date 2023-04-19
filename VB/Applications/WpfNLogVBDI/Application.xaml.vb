Imports System.Drawing
Imports System.Reflection
Imports System.Threading
Imports LogViewerVB.Core
Imports LogViewerVB.Wpf
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports Microsoft.Extensions.Logging
Imports NLog.Target.LogViewVB.Core
Imports RandomLoggingVB.Service

Class Application

#Region "Constructors"

    Public Sub New()

        ' catch all unhandled errors
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf OnUnhandledException

        Dim builder As HostApplicationBuilder = Host.CreateApplicationBuilder()

        ' 
        ' Note: For information on launch profiles for debugging,
        '     see article: https :  //www.codeproject.com/Articles/5354478/NET-App-Settings-Demystified-Csharp-VB
        ' 

        ' Random Logging Service
        builder.AddRandomBackgroundService()

        ' visual debugging tools
        builder.AddLogViewer()

        ' Nlog Target
        'builder.Logging.AddNLogTargets(builder.Configuration)

        ' uncomment to use custom logging colors (note: System.Drawing namespace)
        '
        builder.Logging.AddNLogTargets(
            builder.Configuration,
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

            End Sub)

        Dim services As IServiceCollection = builder.Services

        services _
            .AddSingleton(Of MainViewModel) _
            .AddSingleton(Of MainWindow)(
                Function(service) New MainWindow() With
                {
                    .DataContext = service.GetRequiredService(Of MainViewModel)
                })

        _host = builder.Build()
        _cancellationTokenSource = New CancellationTokenSource()

    End Sub

#End Region

#Region "Fields"

    Private ReadOnly _host As IHost
    Private ReadOnly _cancellationTokenSource As CancellationTokenSource

#End Region

#Region "Methods"

    Private Sub OnUnhandledException(sender As Object, e As UnhandledExceptionEventArgs)

        MessageBox.Show(
            DirectCast(e.ExceptionObject, Exception).Message,
            "Unhandled Error",
            MessageBoxButton.OK,
            MessageBoxImage.Stop)

    End Sub

    Protected Overrides Sub OnStartup(e As StartupEventArgs)

        Try
            LogStartingMode()

            ' set and show
            MainWindow = _host.Services.GetRequiredService(Of MainWindow)()
            MainWindow.Show()

            ' startup background services
            Dim task As Task = _host.StartAsync(_cancellationTokenSource.Token)

        Catch oce As OperationCanceledException

            ' skip

        Catch ex As Exception

            MessageBox.Show(ex.Message, "Unhandled Error", MessageBoxButton.OK, MessageBoxImage.[Stop])

        End Try

    End Sub

    Protected Overrides Sub OnExit(e As ExitEventArgs)

        ' tell the background services that we are shutting down
        _host.StopAsync(_cancellationTokenSource.Token)

        MyBase.OnExit(e)

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

#End Region

End Class