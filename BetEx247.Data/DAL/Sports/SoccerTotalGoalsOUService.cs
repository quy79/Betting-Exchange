using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SoccerTotalGoalsOUService : ISoccerTotalGoalsOUService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Soccer_TotalGoalsOU> SoccerTotalGoalsOUs()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_TotalGoalsOU.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public List<Soccer_TotalGoalsOU> SoccerTotalGoalsOUs(Guid matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_TotalGoalsOU.Where(w => w.ID == matchID).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Soccer_TotalGoalsOU SoccerTotalGoalsOU(Guid ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Soccer_TotalGoalsOU.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
         public bool Insert(Soccer_TotalGoalsOU soccerTotalGoalsOU)
        {
            soccerTotalGoalsOU.ID = Guid.NewGuid();
            _context.AddToSoccer_TotalGoalsOU(soccerTotalGoalsOU);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
         public bool Update(Soccer_TotalGoalsOU soccerTotalGoalsOU)
        {
            Soccer_TotalGoalsOU _obj = new Soccer_TotalGoalsOU();
            _obj = _context.Soccer_TotalGoalsOU.Where(w => w.MatchID == soccerTotalGoalsOU.MatchID & w.UnderPrice == soccerTotalGoalsOU.UnderPrice & w.OverPrice == soccerTotalGoalsOU.OverPrice & w.OU == soccerTotalGoalsOU.OU & w.MarketCloseTime == soccerTotalGoalsOU.MarketCloseTime).SingleOrDefault();
            if (_obj != null) // Update
            {
                _obj = soccerTotalGoalsOU;
                int result = _context.SaveChanges();
                return result > 0 ? true : false;
            }
            else //Insert
            {
                return Insert(soccerTotalGoalsOU);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
        public bool Delete(Soccer_TotalGoalsOU soccerTotalGoalsOU)
        {
            _context.DeleteObject(soccerTotalGoalsOU);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        public IQueryable<Soccer_TotalGoalsOU> Table
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Soccer_TotalGoalsOU> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_TotalGoalsOU.ToList();

                return list;
            }
        }


        void IBase<Soccer_TotalGoalsOU>.Insert(Soccer_TotalGoalsOU entity)
        {
            throw new NotImplementedException();
        }
    }
}
