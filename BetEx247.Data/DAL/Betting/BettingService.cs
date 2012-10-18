using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;

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
                var mybet = dba.PSV_MYBET.Where(w => w.BetStatusID == type && w.MemberID == memberId).ToList();
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
        /// <returns>List mybet</returns>
        public List<Statement> GetStatementByMemberId(long memberId, DateTime startDate, DateTime endDate, int betCategory, int betDisplay, int pageNo, int recordPerpage)
        {
            using (var dba = new BetEXDataContainer())
            {
                var statement = dba.PSP_SEARCHSTATEMENT(memberId, startDate, endDate, betCategory, betDisplay, pageNo, recordPerpage).ToList();
                return statement;
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
