using System.Collections.Generic;
using GCPack.Model;

namespace GCPack.Repository.Interfaces
{
    public interface ILogEventsRepository
    {
        ICollection<LogEventModel> GetLogEvents(LogEventFilter logEventFilter);
        void LogEvent(int UserId, LogEventType LogEventType, int ResourceId);
    }
}