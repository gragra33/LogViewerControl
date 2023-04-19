using LogViewer.Core;
using LogViewer.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogViewer.WinForms;

public static class ServicesExtension
{
    public static HostApplicationBuilder AddLogViewer(this HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ILogDataStore, Logging.LogDataStore>();
        builder.Services.AddSingleton<LogViewerControlViewModel>();
        builder.Services.AddTransient<LogViewerControl>();

        return builder;
    }
}