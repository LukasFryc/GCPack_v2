using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Service.Interfaces;

namespace GCPack.Service
{
    /* tato sluzba slouzi k tomu, aby se zasilaly upozorneni pri
     expiraci smluv pri techto datumech
        -   uplnutí termínu na seznámení
        -   X dnů před termínem příštího přezkoumání
        -   X dnů před termínem ukončení platnosti
    */

    public class WarningService
    {
        IDocumentsService documentsService;
        IMailService mailService;

        public WarningService(IDocumentsService documentsService, IMailService mailService)
        {
            this.documentsService = documentsService;
            this.mailService = mailService;
        }

        // tato metoda se spousti pro overeni a odeslani emailu pro vsechny udalosti
        public void CheckEvents()
        {
            CheckReadExpiration();
            CheckEndDate();
            CheckNextRevision();
        }

        // vsem spravcum dokumentu u kterych se blizi konec platnosti
        private void CheckEndDate()
        {

        }

        // vsem spravcum dokumentu u kterych se blizi dalsi revize
        private void CheckNextRevision()
        {

        }

        // vsem uzivatelum kteri jsou prirazeni k dokumentu a nejsou seznameni
        // k urcitemu datumu
        private void CheckReadExpiration()
        {

        }

    }
}
