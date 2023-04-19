using log4net.Core;

namespace Microsoft.Extensions.Logging.Log4Net.AspNetCore.Entities;

// ref: http://svn.apache.org/viewvc/logging/log4net/trunk/examples/net/2.0/Extensibility/EventIDLogApp/cs/src/
public class EventIDLogImpl : LogImpl, IEventIDLog
{
    public EventIDLogImpl(log4net.Core.ILogger logger) : base(logger) { /* skip */ }

    #region Implementation of IEventIDLog

    public void Log(EventId eventId, LoggingEvent loggingEvent)
    {
        // is the EventId empty?
        if (!(eventId.Id == 0 && string.IsNullOrWhiteSpace(eventId.Name)))
            loggingEvent.Properties[nameof(EventId)] = eventId;

        Logger.Log(loggingEvent);
    }

    #endregion
}