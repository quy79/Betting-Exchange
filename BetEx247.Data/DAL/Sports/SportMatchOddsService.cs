using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SportMatchOddsService 
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Sports_MoneyLine> SportMatchOddses()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_MoneyLine.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public List<Sports_MoneyLine> SportMatchOddses(long sportID, long countryID, long LeagueID, Guid matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_MoneyLine.Where(w => w.ID == matchID & w.SportID == sportID & w.CountryID == countryID & w.LeagueID == LeagueID).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Sports_MoneyLine SportMatchOdds(Guid ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Sports_MoneyLine.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SportCorrectScores"></param>
        /// <returns></returns>
        public bool Insert(Sports_MoneyLine SportMatchOdds)
        {
            SportMatchOdds.ID = Guid.NewGuid();
            _context.AddToSports_MoneyLine(SportMatchOdds);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SportCorrectScores"></param>
        /// <returns></returns>
        public bool Update(Sports_MoneyLine SportMatchOdds)
        {
            Sports_MoneyLine _obj = new Sports_MoneyLine();
           // _obj = _context.Sport_MatchOdds.Where(w => w.ID == SportMatchOdds.ID).SingleOrDefault();
            _obj = _context.Sports_MoneyLine.Where(w => w.HomePrice == SportMatchOdds.HomePrice & w.AwayPrice == SportMatchOdds.AwayPrice & w.DrawPrice == SportMatchOdds.DrawPrice & w.MarketCloseTime == SportMatchOdds.MarketCloseTime).SingleOrDefault();
  
            if (_obj != null) // Update
            {
                _obj = SportMatchOdds;
                int result = _context.SaveChanges();
                return result > 0 ? true : false;
            }
            else //Insert
            {
                return Insert(SportMatchOdds);
            }
         }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SportCorrectScores"></param>
        /// <returns></returns>
        public bool Delete(Sports_MoneyLine SportMatchOdds)
        {
            _context.DeleteObject(SportMatchOdds);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
      
    }
}
