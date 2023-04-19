Imports System.Threading
Imports LogViewerVB.Core
Imports Microsoft.Extensions.Hosting
Imports Microsoft.Extensions.Logging

Public Class RandomLoggingService : Inherits BackgroundService

#Region "Constructors"

    Public Sub New(logger As ILogger(Of RandomLoggingService))

        _logger = logger

    End Sub

#End Region

#Region "Fields"

#Region "Injects"

    Private _logger As ILogger

#End Region

    ' ChatGPT generated lists

    Private ReadOnly _messages As List(Of String) = New List(Of String) From {
        "Bringing your virtual world to life!",
        "Preparing a new world of adventure for you.",
        "Calculating the ideal balance of work and play.",
        "Generating endless possibilities for you to explore.",
        "Crafting the perfect balance of life and love.",
        "Assembling a world of endless exploration.",
        "Bringing your imagination to life one pixel at a time.",
        "Creating a world of endless creativity and inspiration.",
        "Designing the ultimate dream home for you to live in.",
        "Preparing for the ultimate life simulation experience.",
        "Loading up your personalized world of dreams and aspirations.",
        "Building a new neighborhood full of excitement and adventure.",
        "Creating a world full of surprise and wonder.",
        "Generating the ultimate adventure for you to embark on.",
        "Assembling a community full of life and energy.",
        "Crafting the perfect balance of laughter and joy.",
        "Bringing your digital world to life with endless possibilities.",
        "Calculating the perfect formula for happiness and success.",
        "Generating a world of endless imagination and creativity.",
        "Designing a world that's truly one-of-a-kind for you."
    }

    Private ReadOnly _eventNames As List(Of String) = New List(Of String)() From {
            "OnButtonClicked",
            "OnMenuItemSelected",
            "OnWindowResized",
            "OnDataLoaded",
            "OnFormSubmitted",
            "OnTabChanged",
            "OnItemSelected",
            "OnValidationFailed",
            "OnNotificationReceived",
            "OnApplicationStarted",
            "OnUserLoggedIn",
            "OnUploadStarted",
            "OnDownloadCompleted",
            "OnProgressUpdated",
            "OnNetworkErrorOccurred",
            "OnPaymentSuccessful",
            "OnProfileUpdated",
            "OnSearchCompleted",
            "OnFilterChanged",
            "OnLanguageChanged"
        }

    Private ReadOnly _errorMessages As List(Of String) = New List(Of String)() From {
            "Error: Could not connect to the server. Please check your internet connection.",
            "Warning: Your computer's operating system is not compatible with this software.",
            "Error: Insufficient memory. Please close other programs and try again.",
            "Warning: Your graphics card drivers may be outdated. Please update them before playing.",
            "Error: The installation file is corrupt. Please download a new copy.",
            "Warning: Your computer may be running too hot. Please check the temperature and cooling system.",
            "Error: The required DirectX version is not installed on your computer.",
            "Warning: Your sound card may not be supported. Please check the system requirements.",
            "Error: The installation directory is full. Please free up space and try again.",
            "Warning: Your computer's power supply may not be sufficient. Please check the requirements.",
            "Error: The installation process was interrupted. Please restart the setup.",
            "Warning: Your antivirus software may interfere with the game. Please add it to the exception list.",
            "Error: The required Microsoft library is not installed.",
            "Warning: Your input devices may not be compatible. Please check the system requirements.",
            "Error: The installation process failed. Please contact support for assistance.",
            "Warning: Your network speed may cause lag and disconnections.",
            "Error: The setup file is not compatible with your operating system.",
            "Warning: Your computer's resolution may cause display issues.",
            "Error: The required Microsoft .NET Framework is not installed on your computer.",
            "Warning: Your keyboard layout may cause input errors. Please check the settings."
        }

    Private ReadOnly _random As Random = New Random()
    Private Shared ReadOnly EventId As EventId = New EventId(id:=&H1A4, name:="RandomLoggingService")

#End Region

#Region "BackgroundService"

    Protected Overrides Async Function ExecuteAsync(ByVal stoppingToken As CancellationToken) As Task

        _logger.Emit(EventId, LogLevel.Information, "Started")

        While Not stoppingToken.IsCancellationRequested

            ' wait for a pre-determined interval
            Await Task.Delay(1000, stoppingToken).ConfigureAwait(False)

            ' heartbeat logging
            GenerateLogEntry()

        End While

        _logger.Emit(EventId, LogLevel.Information, "Stopped")

    End Function

    Public Overrides Async Function StartAsync(ByVal cancellationToken As CancellationToken) As Task

        Await Task.Yield()

        _logger.Emit(EventId, LogLevel.Information, "Starting")
        Await MyBase.StartAsync(cancellationToken).ConfigureAwait(False)

    End Function

    Public Overrides Async Function StopAsync(ByVal cancellationToken As CancellationToken) As Task

        _logger.Emit(EventId, LogLevel.Information, "Stopping")
        Await MyBase.StopAsync(cancellationToken).ConfigureAwait(False)

    End Function

#End Region

#Region "Methods"

    Private Sub GenerateLogEntry()

        Dim level As LogLevel

        Select Case _random.Next(0, 100)
            Case < 50 : level = LogLevel.Information
            Case < 65 : level = LogLevel.Debug
            Case < 75 : level = LogLevel.Trace
            Case < 85 : level = LogLevel.Warning
            Case < 95 : level = LogLevel.Error
            Case Else : level = LogLevel.Critical
        End Select

        If level < LogLevel.Error Then
            _logger.Emit(GenerateEventId(), level, GetMessage())
            Return
        End If

        _logger.Emit(GenerateEventId(), level, GetMessage(),
                     New Exception(_errorMessages(_random.Next(0, _errorMessages.Count))))

    End Sub

    Private Function GenerateEventId() As EventId

        Dim index As Integer = _random.[Next](0, _eventNames.Count)
        Return New EventId(id:=&H1A4 + index, name:=_eventNames(index))

    End Function

    Private Function GetMessage() As String

        Return _messages(_random.[Next](0, _messages.Count))

    End Function

#End Region

End Class