using AvaloniaNlogNoDI.DataStores;
using LogViewer.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Target.LogView.Core.Extensions;
using System;

namespace AvaloniaNlogNoDI.Extensions;

public static class ServicesExtension
{
    public static ILoggingBuilder AddNLogTargetsNoDI(this ILoggingBuilder builder, IConfiguration config)
    {
        // We need to use a shared instance of the DataStore to pass to the LogViewerControl
        builder.Services.AddSingleton(MainControlsDataStore.DataStore);

        // call core NLog ServiceExtension initializer
        builder.AddNLogTargets(config);

        return builder;
    }

    public static ILoggingBuilder AddNLogTargetsNoDI(this ILoggingBuilder builder, IConfiguration config, Action<DataStoreLoggerConfiguration> configure)
    {
        builder.AddNLogTargetsNoDI(config);
        builder.Services.Configure(configure);
        return builder;
    }
}