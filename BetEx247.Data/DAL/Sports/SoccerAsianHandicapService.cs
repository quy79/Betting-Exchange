using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
   
     /// <summary>
    /// 
    /// </summary>
    public partial class SoccerAsianHandicapService : ISoccerAsianHandicapService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
       public List<Soccer_AsianHandicap> SoccerAsianHandicaps()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_AsianHandicap.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
       public List<Soccer_AsianHandicap> SoccerAsianHandicaps(Guid matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_AsianHandicap.Where(w => w.ID == matchID).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       public Soccer_AsianHandicap SoccerAsianHandicap(Guid ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Soccer_AsianHandicap.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
         public bool Insert(Soccer_AsianHandicap soccerAsianHandicap)
        {
            soccerAsianHandicap.ID = Guid.NewGuid();
            _context.AddToSoccer_AsianHandicap(soccerAsianHandicap);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
         public bool Update(Soccer_AsianHandicap soccerAsianHandicap)
        {
            Soccer_AsianHandicap _obj = new Soccer_AsianHandicap();
            _obj = _context.Soccer_AsianHandicap.Where(w => w.MatchID == soccerAsianHandicap.MatchID & w.HomePrice == soccerAsianHandicap.HomePrice & w.AwayPrice == soccerAsianHandicap.AwayPrice & w.HomePrice == soccerAsianHandicap.HomePrice & w.MarketCloseTime == soccerAsianHandicap.MarketCloseTime).SingleOrDefault();
            if (_obj != null) // Update
            {
                _obj = soccerAsianHandicap;
                int result = _context.SaveChanges();
                return result > 0 ? true : false;
            }
            else //Insert
            {
                return Insert(soccerAsianHandicap);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
        public bool Delete(Soccer_AsianHandicap soccerAsianHandicap)
        {
            _context.DeleteObject(soccerAsianHandicap);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        public IQueryable<Soccer_AsianHandicap> Table
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Soccer_AsianHandicap> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_AsianHandicap.ToList();

                return list;
            }
        }


        void IBase<Soccer_AsianHandicap>.Insert(Soccer_AsianHandicap entity)
        {
            throw new NotImplementedException();
        }
    }
}

