using System.Collections.Generic;
using GCPack.Model;


namespace GCPack.Service.Interfaces
{
    public interface ILogEventsService
    {
        ICollection<LogEventModel> GetLogEvents(LogEventFilter logEventFilter);
        void LogEvent(int UserId, LogEventType LogEventType, int ResourceId);
        void LogEvent(int UserId, LogEventType LogEventType, int ResourceId, string description);
    }
}