using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BetEx247.Core.Infrastructure;
using BetEx247.Data.DAL;
using BetEx247.Core;
using System.Web.UI;
using BetEx247.Data.Model;
using BetEx247.Plugin.DataManager;
using BetEx247.Plugin.DataManager.XMLObjects.Sport;
using BetEx247.Core.Common.Utils;

namespace BetEx247.Web.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/
        SportsDataRenderManager renderMgr;

        public ActionResult Index()
        {
            return View();
        }

        //[OutputCache(Duration = 1800, Location = OutputCacheLocation.Client, VaryByParam = "none")]
        //public JsonResult getAllSport()
        //{
        //    renderMgr = new SportsDataRenderManager();
        //    List<Bet247xSport> sportList = renderMgr.refreshData();         
        //    var test = sportList.SelectMany(a => a.Bet247xSoccerCountries.SelectMany(b=> 
        //        b.Bet247xSoccerLeagues.Select(c=>
        //            new { sid = a.ID, si = a.Is_Soccer == true ? 1 : 0, sn = a.SportName,sc=a.Bet247xSoccerCountries.Count, cid = c.CountryID, cn = b.Country, ci = b.International == true ? 1 : 0, cl=b.Bet247xSoccerLeagues.Count, lId = c.ID, ln = c.LeagueName_WebDisplay }))).ToList();
        //    //TimeSpan ts = new TimeSpan(0, 30, 0);
        //    //CommonHelper.SetCookie("testet", test.ToString(),ts);
        //    return Json(test, JsonRequestBehavior.AllowGet);
        //}

        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Client, VaryByParam = "none")]
        public List<Bet247xSport> getData()
        {
            renderMgr = new SportsDataRenderManager();
            List<Bet247xSport> sportList = renderMgr.refreshData();
            return sportList;
        }

        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Client, VaryByParam = "none")]
        public JsonResult getAllSport(int? id)
        {
            if (id == 1)
            {
                //renderMgr = new SportsDataRenderManager();
                //List<Bet247xSport> sportList = renderMgr.refreshData();
                //var soccer = sportList.SelectMany(a => a.Bet247xSoccerCountries.SelectMany(b =>
                //b.Bet247xSoccerLeagues.Select(c =>
                // new { sid = a.ID, si = a.Is_Soccer == true ? 1 : 0, sn = a.SportName, sc = a.Bet247xSoccerCountries.Count, cid = c.CountryID, cn = b.Country, ci = b.International == true ? 1 : 0, cl = b.Bet247xSoccerLeagues.Count, lId = c.ID, ln = c.LeagueName_WebDisplay }))).ToList();
                //var returnData = soccer.Where(w => w.sid == id);
                ////TimeSpan ts = new TimeSpan(0, 30, 0);
                ////CommonHelper.SetCookie("testet", test.ToString(),ts);
                //return Json(returnData, JsonRequestBehavior.AllowGet);

                using (var dba = new BetEXDataContainer())
                {
                    var lst = dba.PSV_SOCCER_LEAGUE.Select(w => new { sid = w.sid, sn = w.sn, sc = w.sc, cid = w.cid, cn = w.cn, cl = w.lc, lid = w.lid, ln = w.ln }).OrderBy(z=>z.cid).ToList();
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
            }
            //return Json(null, JsonRequestBehavior.AllowGet);
            else
            {
                //renderMgr = new SportsDataRenderManager();
                //List<Bet247xSport> sportList = renderMgr.refreshData();
                //var other = sportList.SelectMany(a => a.Bet247xSportCountries.SelectMany(b =>
                //b.Bet247xSportLeagues.Select(c =>
                //   new { sid = a.ID, si = a.Is_Soccer == true ? 1 : 0, sn = a.SportName, sc = a.Bet247xSoccerCountries.Count, cid = c.CountryID, cn = b.Country, ci = b.International == true ? 1 : 0, cl = b.Bet247xSportLeagues.Count, lId = c.ID, ln = c.LeagueName }))).ToList();

                //var returnDataother = other.Where(w => w.sid == id);
                ////TimeSpan ts = new TimeSpan(0, 30, 0);
                ////CommonHelper.SetCookie("testet", test.ToString(),ts);
                //return Json(returnDataother, JsonRequestBehavior.AllowGet);
                using (var dba = new BetEXDataContainer())
                {
                    var lst = dba.PSV_SPORT_LUAGUE.Where(z=>z.sid==id).Select(w => new { sid = w.sid, sn = w.sn, sc = w.sc, cid = w.cid, cn = w.cn, cl = w.lc, lid = w.lid, ln = w.ln }).OrderBy(x=>x.cid).ToList();
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
            }
        }


        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Client, VaryByParam = "none")]
        public JsonResult getTopEvent()
        {
            List<PSV_TOP_EVENT> list = new List<PSV_TOP_EVENT>();
            list = IoC.Resolve<IGuiService>().GetTopEvent();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Client, VaryByParam = "none")]
        public JsonResult getSport()
        {
            Sport sport = IoC.Resolve<IGuiService>().GetSport(Constant.SportType.SOCCER);
            return Json(sport, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Client, VaryByParam = "none")]
        public JsonResult getListSport()
        {
            List<Sport> sport = IoC.Resolve<IGuiService>().GetAllSport(null);
            var result = sport.Select(w => new { id = w.ID, sn = w.SportName }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Client, VaryByParam = "none")]
        public ActionResult MyAccount()
        {
            return PartialView("MyAccount");
        }

        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Client, VaryByParam = "none")]
        public ActionResult MyAccountIndex()
        {
            return PartialView("MyAccountIndex");
        }
    }
}
