using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SoccerWinningMarginService : ISoccerWinningMarginService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Soccer_WinningMargin> SoccerWinningMargins()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_WinningMargin.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public  List<Soccer_WinningMargin> SoccerWinningMargins(int matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_WinningMargin.Where(w => w.ID == matchID).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Soccer_WinningMargin SoccerWinningMargin(int ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Soccer_WinningMargin.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerWinningMargin"></param>
        /// <returns></returns>
         public bool Insert(Soccer_WinningMargin soccerWinningMargin)
        {
            _context.AddToSoccer_WinningMargin(soccerWinningMargin);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerWinningMargin"></param>
        /// <returns></returns>
         public bool Update(Soccer_WinningMargin soccerWinningMargin)
        {
            Soccer_WinningMargin _obj = new Soccer_WinningMargin();
            _obj = _context.Soccer_WinningMargin.Where(w => w.ID == soccerWinningMargin.ID).SingleOrDefault();
            if (_obj != null) // Update
            {
                _obj = soccerWinningMargin;
                int result = _context.SaveChanges();
                return result > 0 ? true : false;
            }
            else //Insert
            {
                return Insert(soccerWinningMargin);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerWinningMargin"></param>
        /// <returns></returns>
        public bool Delete(Soccer_WinningMargin soccerWinningMargin)
        {
            _context.DeleteObject(soccerWinningMargin);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        public IQueryable<Soccer_WinningMargin> Table
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Soccer_WinningMargin> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_WinningMargin.ToList();

                return list;
            }
        }


        void IBase<Soccer_WinningMargin>.Insert(Soccer_WinningMargin entity)
        {
            throw new NotImplementedException();
        }
    }
}
