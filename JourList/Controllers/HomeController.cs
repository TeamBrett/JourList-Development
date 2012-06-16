using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JourList.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "When life needs a little order";

            return View();
        }

        public ActionResult Scoreboard()
        {
            ViewBag.Message = "Am I winning yet?";

            return View();
        }

        public ActionResult Today()
        {
            ViewBag.Message = "What did you do today?";

            return View();
        }

        public ActionResult Journal()
        {
            ViewBag.Message = "Life Recorded";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
