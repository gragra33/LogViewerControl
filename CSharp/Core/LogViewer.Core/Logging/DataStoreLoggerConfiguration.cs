using System.Drawing;
using Microsoft.Extensions.Logging;

namespace LogViewer.Core;

public class DataStoreLoggerConfiguration
{
    #region Properties
    
    public EventId EventId { get; set; }

    public Dictionary<LogLevel, LogEntryColor> Colors { get; } = new()
    {
        [LogLevel.Trace] = new() { Foreground = Color.DarkGray },
        [LogLevel.Debug] = new() { Foreground = Color.Gray },
        [LogLevel.Information] = new(),
        [LogLevel.Warning] = new() { Foreground = Color.Orange},
        [LogLevel.Error] = new() { Foreground = Color.White, Background = Color.OrangeRed },
        [LogLevel.Critical] = new() { Foreground=Color.White, Background = Color.Red },
        [LogLevel.None] = new(),
    };

    #endregion
}