Imports System.Drawing
Imports System.Reflection
Imports System.Threading
Imports Avalonia
Imports Avalonia.Controls.ApplicationLifetimes
Imports Avalonia.Data.Core
Imports Avalonia.Data.Core.Plugins
Imports Avalonia.Markup.Xaml
Imports LogViewerVB.Avalonia
Imports LogViewerVB.Core
Imports MessageBox.Avalonia
Imports MessageBox.Avalonia.Enums
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports Microsoft.Extensions.Logging
Imports NLog.Target.LogViewVB.Core
Imports RandomLoggingVB.Service

Partial Public Class App
    Inherits Application

    Public Overrides Sub Initialize()
        AvaloniaXamlLoader.Load(Me)
    End Sub

    Public Overrides Sub OnFrameworkInitializationCompleted()

        Dim desktop As IClassicDesktopStyleApplicationLifetime =
                TryCast(ApplicationLifetime, IClassicDesktopStyleApplicationLifetime)

        If desktop IsNot Nothing Then

            ' Line below is needed to remove Avalonia data validation.
            ' Without this line you will get duplicate validations from both Avalonia and CT
            ExpressionObserver.DataValidators.RemoveAll(Function(x) TypeOf x Is DataAnnotationsValidationPlugin)

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
                    .DataContext = service.GetService(Of MainViewModel)
                })

            _host = builder.Build()
            _cancellationTokenSource = New CancellationTokenSource()

            Try
                LogStartingMode()

                ' set and show
                desktop.MainWindow = _host.Services.GetRequiredService(Of MainWindow)()

                ' startup background services
                Dim task As Task = _host.StartAsync(_cancellationTokenSource.Token)

            Catch oce As OperationCanceledException

                ' skip

            Catch ex As Exception

                ShowMessageBox(ex.Message, "Unhandled Error")

            End Try

        End If

        MyBase.OnFrameworkInitializationCompleted()

    End Sub

#Region "Fields"

    Private _host As IHost
    Private _cancellationTokenSource As CancellationTokenSource

#End Region

#Region "Methods"

    Private Sub OnUnhandledException(sender As Object, e As UnhandledExceptionEventArgs)

        ShowMessageBox(DirectCast(e.ExceptionObject, Exception).Message, "Unhandled Error")

    End Sub

    Private Sub ShowMessageBox(title As String, message As String)

        Dim messageBoxStandardWindow = MessageBoxManager _
                .GetMessageBoxStandardWindow(title, message, ButtonEnum.Ok, Enums.Icon.Stop)

        messageBoxStandardWindow.Show()

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