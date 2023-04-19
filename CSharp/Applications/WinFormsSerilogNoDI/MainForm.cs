using System.Reflection;
using Microsoft.Extensions.Logging;
using RandomLogging.Service;
using WinFormsSerilogNoDI.DataStores;
using WinFormsSerilogNoDI.Helpers;

namespace WinFormsSerilogNoDI;

public partial class MainForm : Form
{
    #region Constructors

    public MainForm()
    {
        InitializeComponent();

        // Initialize _service and pass in the Logger
        _service = new(new Logger<RandomLoggingService>(LoggingHelper.Factory));

        // Get the Launch mode
        bool isDevelopment = string.Equals(Environment.GetEnvironmentVariable("DOTNET_MODIFIABLE_ASSEMBLIES"), "debug",
                                           StringComparison.InvariantCultureIgnoreCase);

        // initialize a logger & EventId
        Logger<MainForm> logger = new Logger<MainForm>(LoggingHelper.Factory);
        EventId eventId = new EventId(id: 0, name: Assembly.GetEntryAssembly()!.GetName().Name);

        // log a test pattern for each log level
        logger.TestPattern(eventId: eventId);

        // log that we have started...
        logger.Emit(eventId, LogLevel.Information, $"Running in {(isDevelopment ? "Debug" : "Release")} mode");

        // Start generating log entries
        _ = _service.StartAsync(CancellationToken.None);

        // manually wire up the logging to the view ... the control will show backlog entries...
        LogViewerControl.RegisterLogDataStore(MainControlsDataStore.DataStore);

        // Listen for when the app is closing
        Closing += OnClosing;
    }

    #endregion

    #region Fields

    private readonly RandomLoggingService? _service;

    #endregion
    
    #region Methods
    
    // flush logs and clean up
    private void OnClosing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        Closing -= OnClosing;

        _ = _service?.StopAsync();
        LoggingHelper.CloseAndFlush();
    }

    #endregion
}