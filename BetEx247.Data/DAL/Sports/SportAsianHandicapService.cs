using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SportAsianHandicapService : ISportAsianHandicapService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Sports_AsianHandicap> SportsAsianHandicaps()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_AsianHandicap.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public List<Sports_AsianHandicap> SportsAsianHandicaps(Guid matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_AsianHandicap.Where(w => w.ID == matchID).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Sports_AsianHandicap SportsAsianHandicap(Guid ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Sports_AsianHandicap.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
        public bool Insert(Sports_AsianHandicap soccerCorrectScores)
        {
            _context.AddToSports_AsianHandicap(soccerCorrectScores);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
        public bool Update(Sports_AsianHandicap soccerCorrectScores)
        {
            Sports_AsianHandicap _obj = new Sports_AsianHandicap();
            _obj = _context.Sports_AsianHandicap.Where(w => w.ID == soccerCorrectScores.ID).SingleOrDefault();
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
        public bool Delete(Sports_AsianHandicap soccerCorrectScores)
        {
            _context.DeleteObject(soccerCorrectScores);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        public IQueryable<Sports_AsianHandicap> Table
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Sports_AsianHandicap> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_AsianHandicap.ToList();

                return list;
            }
        }


        void IBase<Sports_AsianHandicap>.Insert(Sports_AsianHandicap entity)
        {
            throw new NotImplementedException();
        }
    }
}
