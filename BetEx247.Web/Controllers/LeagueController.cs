using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetEx247.Web.Controllers
{
    public class LeagueController : Controller
    {
        //
        // GET: /League/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ByLeague()
        {
            return View();
        }

        public ActionResult ByCountry()
        {
            return View();
        }
    }
}
