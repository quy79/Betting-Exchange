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
        public List<PSV_MYBET> GetMyBetByType(long memberId, string type)
        {
            using (var dba = new BetEXDataContainer())
            {
                var mybet = dba.PSV_MYBET.Where(w => w.BetStatus == type && w.MemberID == memberId).Take(Constant.DefaultRow).ToList();
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

        /// <summary>
        /// Get List Transaction by MemberId
        /// </summary>
        /// <param name="memberId">login memberid</param>
        /// <param name="type">1:Deposit, 2: Withdraw</param>
        /// <returns>list transaction</returns>
        public List<PSV_TRANSACTION> GetTransaction(long memberId, Int16 type, int pageNo, DateTime startDate, DateTime endDate, int recordPerpage, ref int totalRow)
        {
            List<PSV_TRANSACTION> result = new List<PSV_TRANSACTION>();
            totalRow = 0;
            Int16 convertType = type == 14 ? (Int16)1 : (Int16)2;
            using (var dba = new BetEXDataContainer())
            {
                int from = (pageNo - 1) * recordPerpage + 1;
                int to = pageNo * recordPerpage;
                var list = dba.PSV_TRANSACTION.Where(w => w.MemberId == memberId && w.Type == convertType && w.AddedDate>=startDate && w.AddedDate<=endDate).ToList();
                if (list != null)
                {
                    for (int i = 1; i < list.Count; i++)
                    {
                        list[i].Row = i;
                    }
                    totalRow = list.Count;
                    return list.Where(w => w.Row >= from && w.Row <= to).ToList();
                }
            }
            return result;
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
