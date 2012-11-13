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

        public ActionResult ByLeague(long? id,int? cid,int? sid)
        {
            ViewBag.tempLeague = id;
            ViewBag.ListSoccerLive = IoC.Resolve<IGuiService>().LiveInMatches(true);
            ViewBag.ListSoccerComming = IoC.Resolve<IGuiService>().UpCommingMatches(true, id,cid,sid);
            if (id != null)
            {
                SoccerCountry country = IoC.Resolve<IGuiService>().GetCountryByLeage(id.Value,cid.Value,sid.Value);
                ViewBag.SoccerCountries = country;
                ViewBag.AllTournaments = IoC.Resolve<IGuiService>().GetTournamentByCountry(country.ID);
                ViewBag.LeagueDetail = IoC.Resolve<IGuiService>().GetSoccerLeague(id.Value, cid.Value, sid.Value); 
            }
            return View();
        }

        public ActionResult ByCountry(int? id)
        {                               
            ViewBag.ListSoccerLive = IoC.Resolve<IGuiService>().LiveInMatches(true);
            ViewBag.ListSoccerComming = IoC.Resolve<IGuiService>().UpCommingMatches(true, id,0,0);
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
