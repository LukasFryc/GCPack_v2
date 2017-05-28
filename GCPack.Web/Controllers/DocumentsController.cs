using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GCPack.Web.Filters;
using GCPack.Model;
using GCPack.Service.Interfaces;

namespace GCPack.Web.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IDocumentsService documentService;
        public DocumentsController(IDocumentsService documentService)
        {
            this.documentService = documentService;
        }


        // GET: Documents
        [AuthorizeAttributeGC(Roles = "user,admin")]
        public ActionResult Index()
        {
            RizenyDokument dokument = documentService.GetDocument(0);
            return View(dokument);
        }

        [AuthorizeAttributeGC(Roles = "user,admin")]
        public ActionResult Documents()
        {
            RizenyDokument dokument = documentService.GetDocument(0);
            return View(dokument);
        }


    }
}