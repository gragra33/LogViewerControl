namespace Common.Core.Extensions;

public static class ServicesExtension
{
    public static TModel? TryGetService<TModel>(this IServiceProvider serviceProvider) where TModel : class
    {
        try
        {
            return (TModel?)serviceProvider.GetService(typeof(TModel));
        }
        catch (ObjectDisposedException)
        {
            // ignore as we do not care...
        }

        return default;
    }
}