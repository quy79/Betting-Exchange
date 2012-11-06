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

        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Client, VaryByParam = "none")]
        public JsonResult getAllSport()
        {
            renderMgr = new SportsDataRenderManager();
            List<Bet247xSport> sportList = renderMgr.refreshData();

            //List<Sport> sportList = IoC.Resolve<IGuiService>().GetAllSport();

            return Json(sportList, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Client, VaryByParam = "none")]
        public JsonResult getSport()
        {
            Sport sport = IoC.Resolve<IGuiService>().GetSport(Constant.SportType.SOCCER);
            return Json(sport, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Client, VaryByParam = "none")]
        public ActionResult MyAccount()
        {
            return PartialView("MyAccount");
        }
    }
}
