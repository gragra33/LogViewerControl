using LogViewer.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using WinFormsLog4NetNoDI.DataStores;
using Log4Net.Appender.LogView.Core;

namespace WinFormsLog4NetNoDI.Extensions;

public static class ServicesExtension
{
    public static ILoggingBuilder AddLog4NetNoDI(this ILoggingBuilder builder, IConfiguration config)
    {
        // We need to use a shared instance of the DataStore to pass to the LogViewerControl
        builder.Services.AddSingleton(MainControlsDataStore.DataStore);

        // call core Log4Net ServiceExtension initializer
        builder.AddLog4Net(config);

        return builder;
    }

    public static ILoggingBuilder AddLog4NetNoDI(this ILoggingBuilder builder, IConfiguration config, Action<DataStoreLoggerConfiguration> configure)
    {
        builder.AddLog4NetNoDI(config);
        builder.Services.Configure(configure);
        return builder;
    }
}