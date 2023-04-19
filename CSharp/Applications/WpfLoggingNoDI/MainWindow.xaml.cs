using System;
using System.Reflection;
using System.Threading;
using LogViewer.Core;
using Microsoft.Extensions.Logging;
using RandomLogging.Service;
using WpfLoggingNoDI.DataStores;
using WpfLoggingNoDI.Helpers;

namespace WpfLoggingNoDI;

public partial class MainWindow : ILogDataStoreImpl
{
    public MainWindow()
    {
        InitializeComponent();

        // Initialize service and pass in the Logger
        RandomLoggingService service = new(new Logger<RandomLoggingService>(LoggingHelper.Factory));

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
        _ = service.StartAsync(CancellationToken.None);
        
        // manually wire up the logging to the view ... the control will show backlog entries...
        DataStore = MainControlsDataStore.DataStore;

        // we can't bind the controls' DataContext to a static object, so assign the DataStore to the Window
        // and pass a reference to the Window itself
        LogViewerControl.DataContext = this;
    }

    // Passed to the LogViewerControl via the DataContext property as ILogDataStoreImpl
    public ILogDataStore DataStore { get; init;  }
}