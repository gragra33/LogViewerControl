using System;
using Microsoft.Extensions.DependencyInjection;

namespace log4net.Appender;

public abstract class ServiceAppenderSkeleton : AppenderSkeleton, IAppenderServiceProvider, IDisposable
{
    private IServiceProvider _serviceProvider;
    IServiceProvider IAppenderServiceProvider.ServiceProvider { set => _serviceProvider = value; }

    protected T ResolveService<T>() where T : class
    {
        if (_serviceProvider == null)
            return default;

        return _serviceProvider.GetService<T>();
    }

    public void Dispose() => _serviceProvider = null;
}
