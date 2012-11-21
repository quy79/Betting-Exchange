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
    public partial class SoccerCorrectScoresService : ISoccerCorrectScoresService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Soccer_CorrectScores> SoccerCorrectScoreses()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_CorrectScores.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public List<Soccer_CorrectScores> SoccerCorrectScoreses(long sportID, long countryID, long LeagueID, Guid matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_CorrectScores.Where(w => w.ID == matchID & w.SportID == sportID & w.CountryID == countryID & w.LeagueID == LeagueID).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Soccer_CorrectScores SoccerCorrectScores(Guid ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Soccer_CorrectScores.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
         public bool Insert(Soccer_CorrectScores soccerCorrectScores)
        {

            soccerCorrectScores.ID = Guid.NewGuid();
            _context.AddToSoccer_CorrectScores(soccerCorrectScores);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
         public bool Update(Soccer_CorrectScores soccerCorrectScores)
        {
            Soccer_CorrectScores _obj = new Soccer_CorrectScores();
            _obj = _context.Soccer_CorrectScores.Where(w => w.MatchID == soccerCorrectScores.MatchID & w.CorrectScore == soccerCorrectScores.CorrectScore & w.Price == soccerCorrectScores.Price).SingleOrDefault();
            if (_obj != null) // Update
            {
                _obj = soccerCorrectScores;
                int result = _context.SaveChanges();
                return result > 0 ? true : false;
            }
            else //Insert
            {
                return Insert(soccerCorrectScores);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
        public bool Delete(Soccer_CorrectScores soccerCorrectScores){
            _context.DeleteObject(soccerCorrectScores);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        public IQueryable<Soccer_CorrectScores> Table
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Soccer_CorrectScores> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_CorrectScores.ToList();

                return list;
            }
        }


        void IBase<Soccer_CorrectScores>.Insert(Soccer_CorrectScores entity)
        {
            throw new NotImplementedException();
        }
    }
}
