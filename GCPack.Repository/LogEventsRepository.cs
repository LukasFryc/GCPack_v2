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

        public void LogEvent(int UserId, LogEventType LogEventType, int ResourceId, string Description)
        {
            // text se naplni nazvem dokumentu a jeho verzi a nebo jmenem uzivatele
            // kdyz se pak vymaze objekt z db, tak text Ti napovi kdo co vymazal, protoze
            // jinak zustane jen ResourceId s odkazem na vymazany objekt 
            string Text = "";
            using (GCPackContainer db = new GCPackContainer())
            {
                db.LogEvents.Add(new LogEvent() {
                    Date = DateTime.Now,
                    LogType = (int)LogEventType,
                    ResourceID = ResourceId,
                    UserID = UserId,
                    Text = Description + Text
                });
                db.SaveChanges();
            }
        }

    }
}
