using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SportOutRightService : ISportOutRightService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Sports_Outright> SportsOutrights()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_Outright.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public List<Sports_Outright> SportsOutrights(int matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_Outright.Where(w => w.ID == matchID).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Sports_Outright SportsOutright(int ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Sports_Outright.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
        public bool Insert(Sports_Outright soccerCorrectScores)
        {
            _context.AddToSports_Outright(soccerCorrectScores);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
        public bool Update(Sports_Outright soccerCorrectScores)
        {
            Sports_Outright _obj = new Sports_Outright();
            _obj = _context.Sports_Outright.Where(w => w.ID == soccerCorrectScores.ID).SingleOrDefault();
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
        public bool Delete(Sports_Outright soccerCorrectScores)
        {
            _context.DeleteObject(soccerCorrectScores);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        public IQueryable<Sports_Outright> Table
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Sports_Outright> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_Outright.ToList();

                return list;
            }
        }


        void IBase<Sports_Outright>.Insert(Sports_Outright entity)
        {
            throw new NotImplementedException();
        }
    }
}
