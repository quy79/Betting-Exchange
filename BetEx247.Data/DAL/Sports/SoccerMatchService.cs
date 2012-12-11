using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SoccerMatchService : ISoccerMatchService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SoccerMatch> SoccerMatches()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.SoccerMatches.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public List<SoccerMatch> SoccerMatches(Guid matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.SoccerMatches.Where(w => w.ID == matchID).ToList();

                return list;
            }
        }
        public List<SoccerMatch> SoccerMatches(long sportID, long countryID, long leagueID, String status)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.SoccerMatches.Where(w => w.LeagueID == leagueID & w.CountryID == countryID & w.SportID == sportID & w.MatchStatus.Equals(status)).ToList();

                return list;
            }
        }

        public List<SoccerMatch> SoccerMatches4Settle(bool settled)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.SoccerMatches.Where(w => w.Settled == settled).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public SoccerMatch SoccerMatch(Guid ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.SoccerMatches.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SoccerMatch SoccerMatch(long leagueID, String homeTeam, String awayTeam, DateTime startDate)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.SoccerMatches.Where(w => w.LeagueID == leagueID & w.HomeTeam.Equals(homeTeam) & w.AwayTeam.Equals(awayTeam) & w.StartDateTime == startDate).ToList();

                return obj.Count == 0 ? null : obj[0];
            }
        }


        public Guid SoccerMatch(SoccerMatch soccerMatch)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.SoccerMatches.Where(w => w.LeagueID == soccerMatch.LeagueID & w.HomeTeam.Equals(soccerMatch.HomeTeam) & w.AwayTeam.Equals(soccerMatch.AwayTeam) & w.StartDateTime == soccerMatch.StartDateTime).ToList();

                return obj.Count == 0 ? Guid.Empty : obj[0].ID;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
        public bool Insert(SoccerMatch soccerMatch)
        {
            soccerMatch.ID = Guid.NewGuid();
            _context.AddToSoccerMatches(soccerMatch);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
        public bool Update(SoccerMatch soccerMatch)
        {
            SoccerMatch _obj = new SoccerMatch();

            //_obj = _context.SoccerMatches.Where(w => w.StartDateTime == soccerMatch.StartDateTime & w.AwayTeam == soccerMatch.AwayTeam & w.HomeTeam == soccerMatch.HomeTeam & w.SportID == soccerMatch.SportID & w.LeagueID == soccerMatch.LeagueID & w.CountryID == soccerMatch.CountryID).SingleOrDefault();
           // We must add column to monitor the match if the date change. 
            _obj = _context.SoccerMatches.Where(w => w.AwayTeam == soccerMatch.AwayTeam & w.HomeTeam == soccerMatch.HomeTeam & w.SportID == soccerMatch.SportID & w.LeagueID == soccerMatch.LeagueID & w.CountryID == soccerMatch.CountryID).SingleOrDefault();
            if (_obj != null) // Update
            {
                _obj = soccerMatch;
                int result = _context.SaveChanges();
                return result > 0 ? true : false;
            }
            else //Insert
            {
                return Insert(soccerMatch);
            }
        }

        public void Update4Settle(SoccerMatch soccerMatch)
        {
            SoccerMatch _obj = new SoccerMatch();

            _obj = _context.SoccerMatches.Where(w => w.ID == soccerMatch.ID & w.AwayTeam == soccerMatch.AwayTeam & w.HomeTeam == soccerMatch.HomeTeam & w.SportID == soccerMatch.SportID & w.LeagueID == soccerMatch.LeagueID & w.CountryID == soccerMatch.CountryID).SingleOrDefault();
            if (_obj != null) // Update
            {
                _obj = soccerMatch;
                int result = _context.SaveChanges();
                //  return result > 0 ? true : false;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
        public bool Delete(SoccerMatch soccerMatch)
        {
            _context.DeleteObject(soccerMatch);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        public IQueryable<SoccerMatch> Table
        {
            get { throw new NotImplementedException(); }
        }

        public IList<SoccerMatch> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.SoccerMatches.ToList();

                return list;
            }
        }


        void IBase<SoccerMatch>.Insert(SoccerMatch entity)
        {
            throw new NotImplementedException();
        }
    }
}
