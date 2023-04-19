using Microsoft.Extensions.Configuration;

namespace Common.Core;

public class AppSettings<TOption>
{
    #region Constructors
    
    public AppSettings(IConfigurationSection configSection, string? key = null)
    {
        _configSection = configSection;

        // ReSharper disable once VirtualMemberCallInConstructor
        GetValue(key);
    }

    #endregion

    #region Fields

    protected static AppSettings<TOption>? _appSetting;

    // ReSharper disable once StaticMemberInGenericType
    protected static IConfigurationSection? _configSection;

    #endregion

    #region Properties
    
    public TOption? Value { get; set; }

    #endregion

    #region Methods
    
    public static TOption? Current(string section, string? key = null)
    {
        _appSetting = GetCurrentSettings(section, key);
        return _appSetting.Value;
    }

    public static AppSettings<TOption> GetCurrentSettings(string section, string? key = null)
    {
        string env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        IConfigurationRoot configuration = builder.Build();

        if (string.IsNullOrEmpty(section))
            section = "AppSettings"; // default

        AppSettings<TOption> settings = new AppSettings<TOption>(configuration.GetSection(section), key);

        return settings;
    }

    protected virtual void GetValue(string? key)
    {
        if (key is null)
        {
            // no key, so must be a class/strut object
            Value = Activator.CreateInstance<TOption>();
            _configSection!.Bind(Value);
            return;
        }

        Type optionType = typeof(TOption);

        if ((optionType == typeof(string) ||
             optionType == typeof(int) ||
             optionType == typeof(long) ||
             optionType == typeof(decimal) ||
             optionType == typeof(float) ||
             optionType == typeof(double)) 
            && _configSection != null)
        {
            // we must be retrieving a value
            Value = _configSection.GetValue<TOption>(key);
            return;
        }

        // Could not find a supported type
        throw new InvalidCastException($"Type {typeof(TOption).Name} is invalid");
    }

    #endregion
}