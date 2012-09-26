using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BetEx247.Plugin.XMLParser;
using BetEx247.Core.Infrastructure;
using BetEx247.Data.DAL;
using BetEx247.Core;

namespace BetEx247.Web.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getAllSport()
        {             
            List<BetEx247.Core.XMLObjects.Sport.Interface.ISport> sportList = IoC.Resolve<IGuiService>().GetAllSport();
            return Json(sportList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSport()
        {
            BetEx247.Core.XMLObjects.Sport.Interface.ISport sport = IoC.Resolve<IGuiService>().GetSport(Constant.SportType.SOCCER);
            return Json(sport, JsonRequestBehavior.AllowGet);
        }
    }
}
