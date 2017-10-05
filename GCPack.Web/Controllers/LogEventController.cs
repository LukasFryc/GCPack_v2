using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GCPack.Service;
using GCPack.Model;
using GCPack.Service.Interfaces;

namespace GCPack.Web.Controllers
{
    public class LogEventController : Controller
    {
        public ActionResult DocumentTypesIndex()
        {
            return View();
        }
    }
}
