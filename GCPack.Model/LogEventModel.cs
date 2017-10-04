using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    public enum LogEventType
    {
        UserLogin = 1,
        UserLogout = 2,
        DeleteDocument = 3,
        CreateDocument = 4,
        EditDocument = 5,
        ReadDocument = 6,
        CreateUser = 7,
        EditUser = 8,
        DeleteUser = 9
    }

    public class LogEventModel
    {
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
    }
}
