using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BetEx247.Core.Common.Utils;
using System.Text;
using BetEx247.Core;
using BetEx247.Data;
using BetEx247.Core.Infrastructure;
using BetEx247.Data.DAL;
using BetEx247.Core.Common.Extensions;

namespace BetEx247.Web.Controllers
{
    public class AjaxController : Controller
    {
        [Authorize]
        public ActionResult Statement()
        {
            DateTime start = DateTime.MaxValue;
            DateTime end = DateTime.MaxValue;

            string period = HttpHelper.GetQueryStringString(Constant.QueryString.Period);
            string startDate = HttpHelper.GetQueryStringString(Constant.QueryString.StartDate);
            string endDate = HttpHelper.GetQueryStringString(Constant.QueryString.EndDate);
            int betCategory = HttpHelper.GetQueryStringInt(Constant.QueryString.BetCategory);
            int betDisplay = HttpHelper.GetQueryStringInt(Constant.QueryString.BetDisplay);
            int pageNo = HttpHelper.GetQueryStringInt(Constant.QueryString.PageNo);
            int recordPerpage = HttpHelper.GetQueryStringInt(Constant.QueryString.RecordPerPage);

            StringBuilder sb = new StringBuilder();

            CommonHelper.DefineSearchDate(period, startDate, endDate, ref start, ref end);

            if (start != DateTime.MaxValue && end != DateTime.MaxValue)
            {
                sb.Append(" and (t.StatementTime between '" + start + "' and '" + end + "') ");
            }

            if (betCategory > 0)
            {
                sb.Append(" and t.CardId=" + betCategory + " ");
            }

            long memberId = SessionManager.USER_ID;
            var lstStatement = IoC.Resolve<IBettingService>().GetStatementByMemberId(memberId, sb.ToString(), pageNo, recordPerpage);
            return View(lstStatement);
        }

        [Authorize]
        public ActionResult MyBetHistory()
        {
            DateTime start = DateTime.MaxValue;
            DateTime end = DateTime.MaxValue;

            int reportType = HttpHelper.GetQueryStringInt(Constant.QueryString.ReportType);
            string period = HttpHelper.GetQueryStringString(Constant.QueryString.Period);
            string startDate = HttpHelper.GetQueryStringString(Constant.QueryString.StartDate);
            string endDate = HttpHelper.GetQueryStringString(Constant.QueryString.EndDate);
            int betCategory = HttpHelper.GetQueryStringInt(Constant.QueryString.BetCategory);
            int pageNo = HttpHelper.GetQueryStringInt(Constant.QueryString.PageNo);
            int recordPerpage = HttpHelper.GetQueryStringInt(Constant.QueryString.RecordPerPage);

            StringBuilder sb = new StringBuilder();

            CommonHelper.DefineSearchDate(period, startDate, endDate, ref start, ref end);

            if (start != DateTime.MaxValue && end != DateTime.MaxValue)
            {
                sb.Append(" and (t.StatementTime between '" + start + "' and '" + end + "') ");
            }

            if (betCategory > 0)
            {
                sb.Append(" and t.CardId=" + betCategory + " ");
            }

            switch (reportType)
            {
                case (int)Constant.MyBetStatus.BETTINGPL:
                    sb.Append(" and t.BetStatusID=" + (int)Constant.MyBetStatus.BETTINGPL + " ");
                    break;
                case (int)Constant.MyBetStatus.CANCELLEDBETS:
                    sb.Append(" and t.BetStatusID=" + (int)Constant.MyBetStatus.CANCELLEDBETS + " ");
                    break;
                case (int)Constant.MyBetStatus.EXPOSURE:
                    sb.Append(" and t.BetStatusID=" + (int)Constant.MyBetStatus.EXPOSURE + " ");
                    break;
                case (int)Constant.MyBetStatus.LAPSEDBETS:
                    sb.Append(" and t.BetStatusID=" + (int)Constant.MyBetStatus.LAPSEDBETS + " ");
                    break;
                case (int)Constant.MyBetStatus.SETTLEDBETS:
                    sb.Append(" and t.BetStatusID=" + (int)Constant.MyBetStatus.SETTLEDBETS + " ");
                    break;
                case (int)Constant.MyBetStatus.UNMATCHEDBETS:
                    sb.Append(" and t.BetStatusID=" + (int)Constant.MyBetStatus.UNMATCHEDBETS + " ");
                    break;
                case (int)Constant.MyBetStatus.UNSETTLEDBETS:
                    sb.Append(" and t.BetStatusID=" + (int)Constant.MyBetStatus.UNSETTLEDBETS + " ");
                    break;
                case (int)Constant.MyBetStatus.VOIDBETS:
                    sb.Append(" and t.BetStatusID=" + (int)Constant.MyBetStatus.VOIDBETS + " ");
                    break;
            }

            long memberId = SessionManager.USER_ID;
            var lstStatement = IoC.Resolve<IBettingService>().GetMyBetByMemberId(memberId, sb.ToString(), pageNo, recordPerpage);
            ViewBag.ReportType = reportType;
            return View(lstStatement);
        }
    }
}
