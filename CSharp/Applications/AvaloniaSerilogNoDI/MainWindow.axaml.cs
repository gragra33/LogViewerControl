using Avalonia.Controls;
using AvaloniaSerilogNoDI.DataStores;
using AvaloniaSerilogNoDI.Helpers;
using LogViewer.Core;
using Microsoft.Extensions.Logging;
using RandomLogging.Service;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;

namespace AvaloniaSerilogNoDI;

public partial class MainWindow : Window, ILogDataStoreImpl
{
    #region Constructors

    public MainWindow()
    {
        InitializeComponent();

        // Initialize _service and pass in the Logger
        _service = new(new Logger<RandomLoggingService>(LoggingHelper.Factory));

        // Get the Launch mode
        bool isDevelopment = string.Equals(Environment.GetEnvironmentVariable("DOTNET_MODIFIABLE_ASSEMBLIES"), "debug",
            StringComparison.InvariantCultureIgnoreCase);

        // initialize a logger & EventId
        Logger<MainWindow> logger = new Logger<MainWindow>(LoggingHelper.Factory);
        EventId eventId = new EventId(id: 0, name: Assembly.GetEntryAssembly()!.GetName().Name);

        // log a test pattern for each log level
        logger.TestPattern(eventId: eventId);

        // log that we have started...
        logger.Emit(eventId, LogLevel.Information, $"Running in {(isDevelopment ? "Debug" : "Release")} mode");

        // Start generating log entries
        _ = _service.StartAsync(CancellationToken.None);

        // manually wire up the logging to the view ... the control will show backlog entries...
        DataStore = MainControlsDataStore.DataStore;

        // we can't bind the controls' DataContext to a static object, so assign the DataStore to the Window
        // and pass a reference to the Window itself
        LogViewerControl.DataContext = this;

        // Listen for when the app is closing
        Window.Closing += OnClosing;
    }

    #endregion

    #region Fields

    private readonly RandomLoggingService? _service;

    #endregion
    
    #region Properties
    
    public ILogDataStore DataStore { get; init;  }

    #endregion

    #region Methods
    
    // flush logs and clean up
    private void OnClosing(object? sender, CancelEventArgs e)
    {
        Window.Closing -= OnClosing;

        _ = _service?.StopAsync();
        LoggingHelper.CloseAndFlush();
    }

    #endregion
}