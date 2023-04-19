using System;

namespace log4net.Appender;

internal interface IAppenderServiceProvider
{
    IServiceProvider ServiceProvider { set; }
}