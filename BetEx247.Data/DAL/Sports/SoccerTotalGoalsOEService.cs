using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SoccerTotalGoalsOEService : ISoccerTotalGoalsOEService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Soccer_TotalGoalsOE> SoccerTotalGoalsOEs()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_TotalGoalsOE.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public  List<Soccer_TotalGoalsOE> SoccerTotalGoalsOEs(int matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_TotalGoalsOE.Where(w => w.ID == matchID).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Soccer_TotalGoalsOE SoccerTotalGoalsOE(int ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Soccer_TotalGoalsOE.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
         public bool Insert(Soccer_TotalGoalsOE soccerTotalGoalsOE)
        {
            _context.AddToSoccer_TotalGoalsOE(soccerTotalGoalsOE);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
         public bool Update(Soccer_TotalGoalsOE soccerTotalGoalsOE)
        {
            Soccer_TotalGoalsOE _obj = new Soccer_TotalGoalsOE();
            _obj = _context.Soccer_TotalGoalsOE.Where(w => w.ID == soccerTotalGoalsOE.ID).SingleOrDefault();
            if (_obj != null) // Update
            {
                _obj = soccerTotalGoalsOE;
                int result = _context.SaveChanges();
                return result > 0 ? true : false;
            }
            else //Insert
            {
                return Insert(soccerTotalGoalsOE);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
        public bool Delete(Soccer_TotalGoalsOE soccerTotalGoalsOE)
        {
            _context.DeleteObject(soccerTotalGoalsOE);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        public IQueryable<Soccer_TotalGoalsOE> Table
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Soccer_TotalGoalsOE> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_TotalGoalsOE.ToList();

                return list;
            }
        }


        void IBase<Soccer_TotalGoalsOE>.Insert(Soccer_TotalGoalsOE entity)
        {
            throw new NotImplementedException();
        }
    }
}
