using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCPack.Model;
using GCPack.Repository.Interfaces;

namespace GCPack.Service
{
    public class DocumentsService : Interfaces.IDocumentsService
    {
        readonly IDocumentsRepository documentsRepository;

        public DocumentsService(IDocumentsRepository documentsRepository)
        {
            this.documentsRepository = documentsRepository;
        }
        public RizenyDokument GetDocument(int documentId)
        {
            return new RizenyDokument();
        }


        public DocumentModel AddDocument(DocumentModel document)
        {
            // pokud je dokument ve stavu n


            return new DocumentModel();

        }

        public DocumentModel EditDocument(DocumentModel document)
        {
            // zjistit v jakem stavu je dokument pro posilani emailu pridanym nebo odstranenym uzivatelum

            // nacist uzivatele kteri se smazou

            // nacist uzivatele kteri se pridavaji



            return new DocumentModel();

        }

    }
}
