using LogViewer.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Log4Net.Appender.LogView.Core;

public static class ServicesExtension
{
    public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, IConfiguration config)
        => builder
            .ClearProviders()
            .AddLog4Net(config.GetLog4NetConfiguration());

    public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, IConfiguration config, Action<DataStoreLoggerConfiguration> configure)
    {
        builder
            .AddLog4Net(config)
            .Services.Configure(configure);

        return builder;
    }

    public static Log4NetProviderOptions? GetLog4NetConfiguration(this IConfiguration configuration)
        => configuration
            .GetSection("Log4NetCore")
            .Get<Log4NetProviderOptions>();
}