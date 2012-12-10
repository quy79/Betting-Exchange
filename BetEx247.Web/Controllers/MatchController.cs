using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BetEx247.Data.Model;
using BetEx247.Web.Models;
using BetEx247.Core.Infrastructure;
using BetEx247.Data.DAL;
using BetEx247.Data.DAL.Sports.Interfaces;
using BetEx247.Core.Common.Extensions;
using BetEx247.Core.Common.Utils;
using BetEx247.Core;
using BetEx247.Data;
using BetEx247.Core.Payment;
using System.Text;
using CaptchaMvc.Attributes;
using CaptchaMvc.Infrastructure;
using ChilkatEmail;

namespace BetEx247.Web.Controllers
{
    public class MatchController : Controller
    {
        //
        // GET: /Match/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllMarket(string mid)
        {
            long memberId = SessionManager.USER_ID;
            ViewBag.Type = 12;
            
            //using (var dba = new BetEXDataContainer())
            //{
            //    List<PSV_MATCHES> soccerMatchList = dba.PSV_MATCHES.Where(w => w.ID == mid).ToList();
            //    ViewBag.SoccerMatchInfo = soccerMatchList;
            //}

            
            return View();
        }

    }
}
