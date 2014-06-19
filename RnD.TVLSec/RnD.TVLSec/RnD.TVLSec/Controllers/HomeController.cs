using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RnD.TVLSec.Helpers;
using RnD.TVLSec.ViewModels;
using RnD.TVLSec.Models;

namespace RnD.TVLSec.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}
