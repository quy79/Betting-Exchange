using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using BetEx247.Core.XML;
using BetEx247.Plugin.XMLParser;
using BetEx247.Core;
using BetEx247.Plugin.DownloadFeed;
using BetEx247.Data.Model;
using BetEx247.Data.DAL;
using BetEx247.Core.Common.Extensions;
using BetEx247.Core.Payment;
using BetEx247.Core.Infrastructure;
using BetEx247.Core.XMLObjects.Sport.Interface;
using System.Diagnostics;

namespace BetEx247.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {                     
            Debug.WriteLine("Init XMLParserObjectManager");
            XMLParserObjectManager obj = new XMLParserObjectManager();
            obj.Parse();
            List<BetEx247.Core.XMLObjects.Sport.Interface.ISport> sport=  obj.Sports;
            ViewBag.SportList = sport;
            Debug.WriteLine("End XMLParserObjectManager ");

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
            transactionPayment.TransactionPaymentType = id;
            transactionPayment.MemberId = 1;
            transactionPayment.MemberIP = Request.UserHostAddress;
            transactionPayment.MemberEmail = transactionPayment.Customer.Email1;
            transactionPayment.TransactionPaymentTotal = collection["Amount"].ToDecimal();
            transactionPayment.TransactionPaymentStatusId = (int)PaymentStatusEnum.Authorized;
            transactionPayment.PaymentMethodId = 1;
            if (id != 1)
            {
                transactionPayment.RecurringTotalCycles = 1;
                transactionPayment.RecurringCycleLength = 7;
            }
                     
            message = IoC.Resolve<ITransactionPaymentService>().PlaceTransactionPayment(transactionPayment, out transactionPaymentId);

            ViewBag.Message = message;
            return View();
        }
    }
}
