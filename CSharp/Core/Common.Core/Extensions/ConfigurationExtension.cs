using Microsoft.Extensions.Configuration;

namespace Common.Core.Extensions;

public static class ConfigurationExtension
{
    public static IConfigurationBuilder Initialize(this IConfigurationBuilder builder)
    {
        string env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        return builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}