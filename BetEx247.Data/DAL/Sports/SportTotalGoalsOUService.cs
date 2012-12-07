using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SportTotalGoalsOUService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Sports_TotalOU> SportTotalGoalsOUs()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_TotalOU.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public List<Sports_TotalOU> SportTotalGoalsOUs(long sportID, long countryID, long LeagueID, string matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_TotalOU.Where(w => w.MatchID == matchID & w.SportID == sportID & w.CountryID == countryID & w.LeagueID == LeagueID).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Sports_TotalOU SportTotalGoalsOU(Guid ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Sports_TotalOU.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SportCorrectScores"></param>
        /// <returns></returns>
        public bool Insert(Sports_TotalOU SportTotalGoalsOU)
        {
            SportTotalGoalsOU.ID = Guid.NewGuid();
            _context.AddToSports_TotalOU(SportTotalGoalsOU);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SportCorrectScores"></param>
        /// <returns></returns>
         public bool Update(Sports_TotalOU SportTotalGoalsOU){
        
            Sports_TotalOU _obj = new Sports_TotalOU();
            _obj = _context.Sports_TotalOU.Where(w => w.MatchID == SportTotalGoalsOU.MatchID & w.UnderPrice == SportTotalGoalsOU.UnderPrice & w.OverPrice == SportTotalGoalsOU.OverPrice/* & w.OU == SportTotalGoalsOU.OU */& w.MarketCloseTime == SportTotalGoalsOU.MarketCloseTime).SingleOrDefault();
            if (_obj != null) // Update
            {
                _obj = SportTotalGoalsOU;
                int result = _context.SaveChanges();
                return result > 0 ? true : false;
            }
            else //Insert
            {
                return Insert(SportTotalGoalsOU);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SportCorrectScores"></param>
        /// <returns></returns>
         public bool Delete(Sports_TotalOU SportTotalGoalsOU)
        {
            _context.DeleteObject(SportTotalGoalsOU);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
       
    }
}
