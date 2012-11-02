using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Core;
using System.Data;
using BetEx247.Core.Infrastructure;

namespace BetEx247.Data.DAL
{
    public partial class BettingService:IBettingService
    {
        /// <summary>
        /// Get Bet for User login follow bettype(Betting P/L,Settled Bets, Cancelled Bets, Lapsed Bets, Void Bets)
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <param name="type">type</param>
        /// <returns>List mybet</returns>
        public List<PSV_MYBET> GetMyBetByType(long memberId, Int16 type)
        {
            using (var dba = new BetEXDataContainer())
            {
                var mybet = dba.PSV_MYBET.Where(w => w.BetStatusID == type && w.MemberID == memberId).Take(Constant.DefaultRow).ToList();
                return mybet;
            }
        }

        /// <summary>
        /// Get Bet for User login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <returns>List mybet</returns>
        public List<PSV_MYBET> GetMyBetByMemberId(long memberId)
        {
            using (var dba = new BetEXDataContainer())
            {
                var mybet = dba.PSV_MYBET.Where(w => w.MemberID == memberId).ToList();
                return mybet;
            }
        }

        /// <summary>
        /// Get Bet for User login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <param name="sWhere">where clause for search</param>
        /// <param name="pageNo">pageNo for search</param>
        /// <param name="recordPerpage">recordPerpage for search</param>
        /// <returns>List psv_mybet</returns>
        public List<PSV_MYBET> GetMyBetByMemberId(long memberId, string sWhere, int pageNo, int recordPerpage)
        {
            using (var dba = new BetEXDataContainer())
            {
                var mybet = dba.PSP_SEARCHMYBETS(memberId, sWhere, pageNo, recordPerpage).ToList();
                return mybet;
            }
        }

        /// <summary>
        /// Get Total Row for Searh History User login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <param name="sWhere">where clause for search</param>
        /// <returns>number row</returns>
        public int CountRowBetByMemberId(long memberId, string sWhere)
        {
            using (var dba = new BetEXDataContainer())
            {
                var result = dba.PSP_SEARCHMYBETSPAGESIZE(memberId, sWhere).First<int?>();
                return result.Value;
            }
        }

        /// <summary>
        /// Get Statement for User login                                                    
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <returns>List mybet</returns>
        public List<Statement> GetStatementByMemberId(long memberId)
        {
            using (var dba = new BetEXDataContainer())
            {
                var statement = dba.Statements.Where(w => w.MemberId == memberId).ToList();
                return statement;
            }
        }

        /// <summary>
        /// Get Statement for User login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <returns>DataTable Statement</returns>
        public DataTable GetStatementByMemberId(long memberId, string sWhere, int pageNo, int recordPerpage)
        {
            DataTable dt = new DataTable("Statement");
            DataFactory dataFactory = new DataFactory();
            dt = dataFactory.ExecuteReader("PSP_SEARCHSTATEMENT", "@memberId", memberId, "@where", sWhere, "@pageNo", pageNo, "@sRecordsPerPage", recordPerpage);
            return dt;

            //using (var dba = new BetEXDataContainer())
            //{
            //    var statement = dba.PSP_SEARCHSTATEMENT(memberId, sWhere, pageNo, recordPerpage).ToList();                
            //    return statement;
            //}
        }

        /// <summary>
        /// Get Total rowfor search statement by user login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <param name="sWhere">where clause for search</param>
        /// <returns>number row</returns>
        public int CountRowStatementByMemberId(long memberId, string sWhere)
        {
            using (var dba = new BetEXDataContainer())
            {
                var rowCount = dba.PSP_SEARCHSTATEMENTPAGESIZE(memberId, sWhere).First<int?>();
                return rowCount.Value;
            }
        }

        /// <summary>
        /// Get Bet for User login
        /// </summary>
        /// <param name="memberId">memberId</param>
        /// <param name="dateFrom">date start to search</param>
        /// <param name="dateTo">date end to search</param>
        /// <returns>List mybet</returns>
        public List<PSV_MYBET> GetMyBetByMemberId(long memberId, DateTime? dateFrom, DateTime? dateTo)
        {
            var mybet= new List<PSV_MYBET>();
            using (var dba = new BetEXDataContainer())
            {
                if (dateFrom == null && dateTo == null)
                {
                    mybet = GetMyBetByMemberId(memberId);
                }
                else
                {
                    mybet = dba.PSV_MYBET.Where(w => w.MemberID == memberId && w.SubmitedTime>=dateFrom && w.SubmitedTime<=dateTo).ToList();
                }
                return mybet;
            }
        }

        #region base menthod
        public IList<MyBet> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Insert(MyBet entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(MyBet entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(MyBet entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<MyBet> Table
        {
            get { throw new NotImplementedException(); }
        }
        #endregion
    }
}
