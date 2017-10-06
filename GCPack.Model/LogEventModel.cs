using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    // sem si zapises vsechny udalosti ktere chces logovat - ok ?
    // pak se napise fce ktera podle techto udalosti bude plnit log
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

    // to je to co se vraci klientovi - Datum - Kdo - Co
    // v tom Co je zaznamenano o jaky dokument / uzivatele se jedna v textove forme
    // je to kvuli tomu, ze pokud by jsi vymazal nejaky zaznam z databaze, tak by
    // v logu bylo pouze referencni ID a uz by jsi nezjistil co bylo vymazano
    // proto to napiseme primo do textu logu 
    public class LogEventModel
    {
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
    }
}
