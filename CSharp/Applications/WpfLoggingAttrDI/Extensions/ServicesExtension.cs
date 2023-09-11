using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WpfLoggingAttrDI.Service;

public static class ServicesExtension
{
    public static HostApplicationBuilder AddRandomBackgroundService(this HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<RandomLoggingService>();
        builder.Services.AddHostedService(service => service.GetRequiredService<RandomLoggingService>());

        return builder;
    }
}