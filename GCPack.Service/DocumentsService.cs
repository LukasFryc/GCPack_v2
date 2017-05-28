using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Model;

namespace GCPack.Service
{
    public class DocumentsService : Interfaces.IDocumentsService
    {
        public RizenyDokument GetDocument(int documentId)
        {
            ICollection<Stredisko> strediska = new HashSet<Stredisko>();
            strediska.Add(new Stredisko() { Nazev = "stredisko 1" });
            strediska.Add(new Stredisko() { Nazev = "stredisko 2" });
            strediska.Add(new Stredisko() { Nazev = "stredisko 3" });
            strediska.Add(new Stredisko() { Nazev = "stredisko 4" });

            ICollection<Uzivatel> uzivatele = new HashSet<Uzivatel>();
            uzivatele.Add(new Uzivatel() { FirstName = "lukas", LastName = "Fryc" });
            uzivatele.Add(new Uzivatel() { FirstName = "david", LastName = "navratil" });
            uzivatele.Add(new Uzivatel() { FirstName = "petr", LastName = "novak" });
            uzivatele.Add(new Uzivatel() { FirstName = "karel", LastName = "ctvrty" });
            RizenyDokument dokument = new RizenyDokument()
            {
                Nazev = "Dokument prvni",
                Strediska = strediska,
                Uzivatele = uzivatele,
                Popis = "Je docela dobře možné, že velký incident by mohl vést k nedostatku paliva, a to by byla hospodářská katastrofa,“ řekl listu The Sun nejmenovaný vysoce postavený zdroj námořnictva a dodal: „Hrozba proti tankerům s palivem se objevila před několika lety a od té doby stále provádíme výcvik, abychom jí čelili."
            };
            return dokument;
        }
    }
}
