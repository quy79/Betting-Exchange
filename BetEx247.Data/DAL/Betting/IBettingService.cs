using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;

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
        List<PSV_MYBET> GetMyBetByType(long memberId, Int16 type);

        /// <summary>
        /// Get Bet for User login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <returns>List mybet</returns>
        List<PSV_MYBET> GetMyBetByMemberId(long memberId);

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
        /// <param name="startDate">startDate for search</param>
        /// <param name="endDate">endDate for search</param>
        /// <param name="betCategory">betCategory ex: Snooker, Soccer...</param>
        /// <param name="betDisplay">betDisplay  ex: Adjustment, Bets Only...</param>
        /// <param name="pageNo">pageNo for search</param>
        /// <param name="recordPerpage">recordPerpage for search</param>
        /// <returns>List Statement</returns>
        List<Statement> GetStatementByMemberId(long memberId, DateTime startDate, DateTime endDate, int betCategory, int betDisplay, int pageNo, int recordPerpage);

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
