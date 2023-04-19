using System.Reflection;
using Microsoft.Extensions.Logging;
using RandomLogging.Service;
using WinFormsNLogNoDI.DataStores;
using WinFormsNLogNoDI.Helpers;

namespace WinFormsNLogNoDI;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        // Initialize service and pass in the Logger
        RandomLoggingService service = new(new Logger<RandomLoggingService>(LoggingHelper.Factory));

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
        _ = service.StartAsync(CancellationToken.None);

        // manually wire up the logging to the view ... the control will show backlog entries...
        LogViewerControl.RegisterLogDataStore(MainControlsDataStore.DataStore);
    }
}