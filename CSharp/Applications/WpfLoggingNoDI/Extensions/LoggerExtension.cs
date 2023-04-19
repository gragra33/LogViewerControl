using System;
using LogViewer.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using MsLogger.Core;
using WpfLoggingNoDI.DataStores;

namespace WpfLoggingNoDI.Extensions;

public static class LoggerExtension
{
    public static ILoggingBuilder AddDataStoreLogger(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        // We need to use a shared instance of the DataStore to pass to the LogViewerControl
        builder.Services.AddSingleton(MainControlsDataStore.DataStore);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DataStoreLoggerProvider>());
        return builder;
    }

    public static ILoggingBuilder AddDataStoreLogger(this ILoggingBuilder builder, Action<DataStoreLoggerConfiguration> configure)
    {
        builder.AddDataStoreLogger();
        builder.Services.Configure(configure);
        return builder;
    }
}