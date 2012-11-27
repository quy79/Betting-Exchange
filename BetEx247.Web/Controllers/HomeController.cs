using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;    
using BetEx247.Core;
using BetEx247.Plugin.DownloadFeed;
using BetEx247.Data.Model;
using BetEx247.Data.DAL;
using BetEx247.Core.Common.Extensions;
using BetEx247.Core.Payment;
using BetEx247.Core.Infrastructure;  
using System.Diagnostics;
using BetEx247.Plugin.DataManager.XMLObjects.Sport;
using BetEx247.Plugin.DataManager;

namespace BetEx247.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {                     
            Debug.WriteLine("Init XMLParserObjectManager");
            //XMLParserObjectManager obj = new XMLParserObjectManager();
            //obj.Parse();

            SportsDataRenderManager renderMgr = new SportsDataRenderManager();
            List<Bet247xSport> list = renderMgr.refreshData();

            List<Bet247xSport> sport = list;// obj.Sports;
            ViewBag.SportList = sport;
            Debug.WriteLine("End XMLParserObjectManager ");

            ViewBag.ListSoccerLive = IoC.Resolve<IGuiService>().LiveInMatches(true);
            ViewBag.ListSoccerComming = IoC.Resolve<IGuiService>().UpCommingMatches(true,0,0,0,1);
            ViewBag.AllSport = IoC.Resolve<IGuiService>().GetAllSport(null);

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public ActionResult About(FormCollection collection, byte id)
        {
            string message = string.Empty;
            long transactionPaymentId = 0;
            TransactionPayment transactionPayment = new TransactionPayment();

            if (id != 1)
            {
                transactionPayment = IoC.Resolve<ITransactionPaymentService>().GetTransactionPaymentByUserId(1);
                transactionPayment.TransactionPaymentType = id;
                transactionPayment.MemberIP = Request.UserHostAddress;
                transactionPayment.RecurringTotalCycles = 1;
                transactionPayment.RecurringCycleLength = 7;
            }
            else
            {
                transactionPayment.TransactionPaymentType = id;
                transactionPayment.MemberId = 1;
                transactionPayment.MemberIP = Request.UserHostAddress;
                transactionPayment.MemberEmail = transactionPayment.Customer.Email1;
                transactionPayment.TransactionPaymentTotal = collection["Amount"].ToDecimal();
                transactionPayment.TransactionPaymentStatusId = (int)PaymentStatusEnum.Authorized;
                transactionPayment.PaymentMethodId = 1;
            }
                     
            message = IoC.Resolve<ITransactionPaymentService>().PlaceTransactionPayment(transactionPayment, out transactionPaymentId);

            ViewBag.Message = message;
            return View();
        }
    }
}
