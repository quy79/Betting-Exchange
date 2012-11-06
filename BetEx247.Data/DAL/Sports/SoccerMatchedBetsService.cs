using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SoccerMatchedBetsService : ISoccerMatchedBetsService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Soccer_MatchedBets> SoccerMatchedBetses()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_MatchedBets.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public  List<Soccer_MatchedBets> SoccerMatchedBetses(int matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_MatchedBets.Where(w => w.ID == matchID).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Soccer_MatchedBets SoccerMatchedBets(int ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Soccer_MatchedBets.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
         public bool Insert(Soccer_MatchedBets soccerMatchedBets)
        {
            _context.AddToSoccer_MatchedBets(soccerMatchedBets);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
         public bool Update(Soccer_MatchedBets soccerMatchedBets)
        {
            Soccer_MatchedBets _obj = new Soccer_MatchedBets();
            _obj = _context.Soccer_MatchedBets.Where(w => w.ID == soccerMatchedBets.ID).SingleOrDefault();
            if (_obj != null) // Update
            {
                _obj = soccerMatchedBets;
                int result = _context.SaveChanges();
                return result > 0 ? true : false;
            }
            else //Insert
            {
                return Insert(soccerMatchedBets);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
        public bool Delete(Soccer_MatchedBets soccerMatchedBets)
        {
            _context.DeleteObject(soccerMatchedBets);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        public IQueryable<Soccer_MatchedBets> Table
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Soccer_MatchedBets> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_MatchedBets.ToList();

                return list;
            }
        }



        void IBase<Soccer_MatchedBets>.Insert(Soccer_MatchedBets entity)
        {
            throw new NotImplementedException();
        }
    }
}
