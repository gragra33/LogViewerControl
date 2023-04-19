using LogViewer.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace MsLogger.Core;

public static class ServicesExtension
{
    public static ILoggingBuilder AddDefaultDataStoreLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DataStoreLoggerProvider>());
        return builder;
    }

    public static ILoggingBuilder AddDefaultDataStoreLogger(this ILoggingBuilder builder, Action<DataStoreLoggerConfiguration> configure)
    {
        builder.AddDefaultDataStoreLogger();
        builder.Services.Configure(configure);
        return builder;
    }
}