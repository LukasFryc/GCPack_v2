using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Model;
using GCPack.Service.Interfaces;
using GCPack.Repository.Interfaces;

namespace GCPack.Service
{
    public class LogEventsService : ILogEventsService
    {
        private readonly ILogEventsRepository logEventsRepository;

        public LogEventsService(ILogEventsRepository logEventsRepository)
        {
            this.logEventsRepository = logEventsRepository;
        }

        public ICollection<LogEventModel> GetLogEvents(LogEventFilter logEventFilter)
        {
            return logEventsRepository.GetLogEvents(logEventFilter);
        }

        public void LogEvent(int UserId, LogEventType LogEventType, int ResourceId)
        {
            logEventsRepository.LogEvent(UserId, LogEventType, ResourceId);
        }


    }
}
