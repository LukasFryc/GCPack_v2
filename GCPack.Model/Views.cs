using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    public class RizenyDokument
    {
        public int ID { get; set; }
        public int ID_TypuRizenehoDokumentu { get; set; }
        public string Nazev { get; set; }
        public ICollection<Stredisko> Strediska { get; set; }
        public ICollection<Uzivatel> Uzivatele { get; set; }
        public string Popis { get; set; }
    }

    public class Uzivatel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ID { get; set; }
    }

    public class Stredisko
    {
        public int ID { get; set; }
        public string Nazev { get; set; }
    }

    public class XXX {
        public void metoda()
        {
            RizenyDokument doc = new RizenyDokument();
            doc.Strediska.Add(new Stredisko() );
        }
    }

}
