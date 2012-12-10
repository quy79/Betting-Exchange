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

        public ActionResult AllMarket(string id)
        {
            //long memberId = SessionManager.USER_ID;
            ViewBag.Type = 12;
            Guid gid = Guid.Parse(id);
            List<Soccer_DrawNoBet> drawOdds = IoC.Resolve<IGuiService>().getSoccerDrawNoBet(gid);
            List<Soccer_MatchOdds> matchOdds = IoC.Resolve<IGuiService>().getSoccerMatchOdd(gid);
            SoccerMatch match = IoC.Resolve<IGuiService>().getSoccerMatch(gid);

            ViewBag.MatchOdd = matchOdds;
            ViewBag.DrawNoBetOdd = drawOdds;
            ViewBag.Match = match;
                        
            return View();
        }

    }
}
