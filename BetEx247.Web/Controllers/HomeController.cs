using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BetEx247.Core.XML;
using BetEx247.Plugin.XMLParser;
using BetEx247.Core;
using BetEx247.Plugin.DownloadFeed;

namespace BetEx247.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            List<Sport> lstSport = new List<Sport>();
            List<Event> lstEvent = new List<Event>();
            List<Match> lstMatch = new List<Match>();
            List<Bet> lstBet = new List<Bet>();
            List<Choice> lstChoice = new List<Choice>();

            DownloadXMLFeed downloadfeed = new DownloadXMLFeed(Constant.SourceXML.PINNACLESPORTS);
            downloadfeed.DownloadXML();
            XMLParser parser = new XMLParser(Constant.SourceXML.TITANBET);
            lstSport = parser.AllSport;
            lstEvent = parser.AllEvent;
            lstMatch = parser.AllMatch;
            lstBet = parser.AllBet;
            lstChoice = parser.AllChoice;

            ViewBag.lstEvent = lstEvent;
            ViewBag.lstMatch = lstMatch;
            ViewBag.lstBet = lstBet;
            ViewBag.lstChoice = lstChoice;

            return View(lstSport);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
