using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SportMatchService 
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<SportsMatch> SportMatches()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.SportsMatches.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public List<SportsMatch> SportMatches(Guid matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.SportsMatches.Where(w => w.ID == matchID).ToList();

                return list;
            }
        }
        public List<SportsMatch> SportMatches(long sportID,long countryID,long leagueID , String status)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.SportsMatches.Where(w => w.LeagueID == leagueID &w.CountryID==countryID&w.SportID==sportID & w.MatchStatus.Equals(status)).ToList();

                return list;
            }
        }

        public List<SportsMatch> SportMatches4Settle(bool settled)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.SportsMatches.Where(w => w.Settled == settled).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public SportsMatch SportMatch(Guid ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.SportsMatches.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SportsMatch SportMatch(long leagueID, String homeTeam, String awayTeam, DateTime startDate)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.SportsMatches.Where(w => w.LeagueID == leagueID & w.HomeTeam.Equals(homeTeam) & w.AwayTeam.Equals(awayTeam) & w.StartDateTime == startDate ).ToList();

                return obj.Count==0?null:obj[0];
            }
        }


        public Guid SportMatch(SportsMatch SportMatch)
        {
            using (var dba = new BetEXDataContainer())

            {
                var obj = dba.SportsMatches.Where(w => w.LeagueID == SportMatch.LeagueID & w.HomeTeam.Equals(SportMatch.HomeTeam) & w.AwayTeam.Equals(SportMatch.AwayTeam) & w.StartDateTime == SportMatch.StartDateTime).ToList();

                return  obj.Count==0?Guid.Empty:obj[0].ID;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SportsCorrectScores"></param>
        /// <returns></returns>
         public bool Insert(SportsMatch SportsMatch)
        {
            SportsMatch.ID = Guid.NewGuid();
            _context.AddToSportsMatches(SportsMatch);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SportsCorrectScores"></param>
        /// <returns></returns>
         public bool Update(SportsMatch SportsMatch)
        {
            SportsMatch _obj = new SportsMatch();

            _obj = _context.SportsMatches.Where(w => w.StartDateTime == SportsMatch.StartDateTime & w.AwayTeam == SportsMatch.AwayTeam & w.HomeTeam == SportsMatch.HomeTeam & w.SportID == SportsMatch.SportID & w.LeagueID == SportsMatch.LeagueID & w.CountryID == SportsMatch.CountryID).SingleOrDefault();
            if (_obj != null) // Update
            {
                _obj = SportsMatch;
                int result = _context.SaveChanges();
                return result > 0 ? true : false;
            }
            else //Insert
            {
                return Insert(SportsMatch);
            }
        }

         public void Update4Settle(SportsMatch SportsMatch)
         {
             SportsMatch _obj = new SportsMatch();

             _obj = _context.SportsMatches.Where(w => w.ID == SportsMatch.ID & w.AwayTeam == SportsMatch.AwayTeam & w.HomeTeam == SportsMatch.HomeTeam & w.SportID == SportsMatch.SportID & w.LeagueID == SportsMatch.LeagueID & w.CountryID == SportsMatch.CountryID).SingleOrDefault();
             if (_obj != null) // Update
             {
                 _obj = SportsMatch;
                 int result = _context.SaveChanges();
               //  return result > 0 ? true : false;
             }
            
         }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SportsCorrectScores"></param>
        /// <returns></returns>
        public bool Delete(SportsMatch SportsMatch)
        {
            _context.DeleteObject(SportsMatch);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        public IQueryable<SportsMatch> Table
        {
            get { throw new NotImplementedException(); }
        }

        public IList<SportsMatch> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.SportsMatches.ToList();

                return list;
            }
        }


    }
}
