using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Model;
using AutoMapper;
using GCPack.Repository.Interfaces;

namespace GCPack.Repository
{
    public class LogEventsRepository : ILogEventsRepository
    {
        public ICollection<LogEventModel> GetLogEvents(LogEventFilter logEventFilter)
        {
            return new HashSet<LogEventModel>();
        }

        public void LogEvent(int UserId, LogEventType LogEventType, int ResourceId)
        {
            using (GCPackContainer db = new GCPackContainer())
            {
                db.LogEvents.Add(new LogEvent() {
                    Date = DateTime.Now,
                    LogType = (int)LogEventType,
                    ResourceID = ResourceId,
                    UserID = UserId
                });
                db.SaveChanges();
            }
        }

    }
}
