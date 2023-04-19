using log4net;
using log4net.Core;

namespace Microsoft.Extensions.Logging.Log4Net.AspNetCore.Entities;

public interface IEventIDLog : ILog
{
    void Log(EventId eventId, LoggingEvent loggingEvent);
}
