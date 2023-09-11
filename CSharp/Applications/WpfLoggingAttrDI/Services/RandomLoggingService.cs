using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WpfLoggingAttrDI.Service;

public class RandomLoggingService : BackgroundService
{
    #region Constructors

    public RandomLoggingService(ILogger<RandomLoggingService> logger)
    {
        _logger = logger;
        _eventNames = RandomServiceLog.Events.Keys.ToList();
    }

    #endregion

    #region Fields

    #region Injected

    private readonly ILogger _logger;

    #endregion

    // ChatGPT generated lists

    private readonly List<string> _messages = new()
    {
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
    };

    private readonly IReadOnlyList<string> _eventNames;

    private readonly List<string> _errorMessages = new()
    {
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
    };

    private readonly Random _random = new();
    private static readonly EventId EventId = new(id: 0x1A4, name: "RandomLoggingService");

    #endregion

    #region BackgroundService

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ApplicationLog.Emit(_logger, LogLevel.Information, "Started");

        while (!stoppingToken.IsCancellationRequested)
        {
            // wait for a pre-determined interval
            await Task.Delay(1000, stoppingToken).ConfigureAwait(false);
            
            if (stoppingToken.IsCancellationRequested)
                return;

            // heartbeat logging
            GenerateLogEntry();
        }
  
        ApplicationLog.Emit(_logger, LogLevel.Information, "Stopped");
    }

    public Task StartAsync()
        => StartAsync(CancellationToken.None);

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();

        ApplicationLog.Emit(_logger, LogLevel.Information, "Starting");

        await base.StartAsync(cancellationToken).ConfigureAwait(false);
    }

    public Task StopAsync()
        => StopAsync(CancellationToken.None);

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        ApplicationLog.Emit(_logger, LogLevel.Information, "Stopping");
        await base.StopAsync(cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region Methods

    private void GenerateLogEntry()
    {
        LogLevel level = _random.Next(0, 100) switch
        {
            < 50 => LogLevel.Information,
            < 65 => LogLevel.Debug,
            < 75 => LogLevel.Trace,
            < 85 => LogLevel.Warning,
            < 95 => LogLevel.Error,
            _ => LogLevel.Critical
        };

        if (level < LogLevel.Error)
        {
            RandomServiceLog.Emit(_logger, GenerateEventId(), level, message: GetMessage());
            return;
        }

        RandomServiceLog.Emit(_logger, GenerateEventId(), level, message: GetMessage(),
            new Exception(_errorMessages[_random.Next(0, _errorMessages.Count)]));
    }

    private EventId GenerateEventId()
    {
        int index = _random.Next(0, _eventNames.Count);
        return new EventId(id: 0x1A4 + index, name: _eventNames[index]);
    }

    private string GetMessage()
        => _messages[_random.Next(0, _messages.Count)];

    #endregion
}