using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SoccerMatchOddsService : ISoccerMatchOddsService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Soccer_MatchOdds> SoccerMatchOddses()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_MatchOdds.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public List<Soccer_MatchOdds> SoccerMatchOddses(Guid matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_MatchOdds.Where(w => w.ID == matchID).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Soccer_MatchOdds SoccerMatchOdds(Guid ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Soccer_MatchOdds.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
         public bool Insert(Soccer_MatchOdds soccerMatchOdds)
        {
            soccerMatchOdds.ID = Guid.NewGuid();
            _context.AddToSoccer_MatchOdds(soccerMatchOdds);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
         public bool Update(Soccer_MatchOdds soccerMatchOdds)
        {
            Soccer_MatchOdds _obj = new Soccer_MatchOdds();
           // _obj = _context.Soccer_MatchOdds.Where(w => w.ID == soccerMatchOdds.ID).SingleOrDefault();
            _obj = _context.Soccer_MatchOdds.Where(w=>w.HomePrice == soccerMatchOdds.HomePrice & w.AwayPrice== soccerMatchOdds.AwayPrice & w.DrawPrice == soccerMatchOdds.DrawPrice &  w.MarketCloseTime == soccerMatchOdds.MarketCloseTime).SingleOrDefault();
  
            if (_obj != null) // Update
            {
                _obj = soccerMatchOdds;
                int result = _context.SaveChanges();
                return result > 0 ? true : false;
            }
            else //Insert
            {
                return Insert(soccerMatchOdds);
            }
         }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
        public bool Delete(Soccer_MatchOdds soccerMatchOdds)
        {
            _context.DeleteObject(soccerMatchOdds);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        public IQueryable<Soccer_MatchOdds> Table
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Soccer_MatchOdds> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_MatchOdds.ToList();

                return list;
            }
        }


        void IBase<Soccer_MatchOdds>.Insert(Soccer_MatchOdds entity)
        {
            throw new NotImplementedException();
        }
    }
}
