using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BetEx247.Core.Infrastructure;
using BetEx247.Data.DAL;
using BetEx247.Data.Model;

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

        public ActionResult ByLeague(long? id)
        {
            ViewBag.ListSoccerLive = IoC.Resolve<IGuiService>().LiveInMatches(true);
            ViewBag.ListSoccerComming = IoC.Resolve<IGuiService>().UpCommingMatches(true, id);
            if (id != null)
            {
                SoccerCountry country = IoC.Resolve<IGuiService>().GetCountryByLeage(id.Value);
                ViewBag.SoccerCountries = country;
                ViewBag.AllTournaments = IoC.Resolve<IGuiService>().GetTournamentByCountry(country.ID);
            }
            return View();
        }

        public ActionResult ByCountry(int? id)
        {
            ViewBag.ListSoccerLive = IoC.Resolve<IGuiService>().LiveInMatches(true);
            ViewBag.ListSoccerComming = IoC.Resolve<IGuiService>().UpCommingMatches(true, id);
            if (id != null)
            {
                SoccerCountry country = IoC.Resolve<IGuiService>().GetCountryByCountry(id.Value);
                ViewBag.SoccerCountries = country;
                ViewBag.AllTournaments = IoC.Resolve<IGuiService>().GetTournamentByCountry(id.Value);
            }
            return View();
        }
    }
}
