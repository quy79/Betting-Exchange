using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using System.Data;

namespace BetEx247.Data.DAL
{
    public partial interface IBettingService:IBase<MyBet>
    {
        /// <summary>
        /// Get Bet for User login follow bettype(Betting P/L,Settled Bets, Cancelled Bets, Lapsed Bets, Void Bets)
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <param name="type">type</param>
        /// <returns>List mybet</returns>
        List<PSV_MYBET> GetMyBetByType(long memberId, string type);

        /// <summary>
        /// Get Bet for User login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <returns>List mybet</returns>
        List<PSV_MYBET> GetMyBetByMemberId(long memberId);

        /// <summary>
        /// Get Bet for User login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <param name="sWhere">where clause for search</param>
        /// <param name="pageNo">pageNo for search</param>
        /// <param name="recordPerpage">recordPerpage for search</param>
        /// <returns>List psv_mybet</returns>
        List<PSV_MYBET> GetMyBetByMemberId(long memberId, string sWhere, int pageNo, int recordPerpage);

        /// <summary>
        /// Get Total Row for Searh History User login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <param name="sWhere">where clause for search</param>
        /// <returns>number row</returns>
        int CountRowBetByMemberId(long memberId, string sWhere);

        /// <summary>
        /// Get Statement for User login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <returns>List Statement</returns>
        List<Statement> GetStatementByMemberId(long memberId);

        /// <summary>
        /// Search statement for user login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <param name="sWhere">where clause for search</param>
        /// <param name="pageNo">pageNo for search</param>
        /// <param name="recordPerpage">recordPerpage for search</param>
        /// <returns>DataTable Statement</returns>
        DataTable GetStatementByMemberId(long memberId, string sWhere, int pageNo, int recordPerpage);

        /// <summary>
        /// Get Total rowfor search statement by user login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <param name="sWhere">where clause for search</param>
        /// <returns>number row</returns>
        int CountRowStatementByMemberId(long memberId, string sWhere);

        /// <summary>
        /// Get Bet for User login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <param name="dateFrom">date start to search</param>
        /// <param name="dateTo">date end to search</param>
        /// <returns>List mybet</returns>
        List<PSV_MYBET> GetMyBetByMemberId(long memberId,DateTime? dateFrom, DateTime? dateTo);
    }
}
