using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SportMoneyLineService : ISportMoneyLineService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Sports_MoneyLine> SportsMoneyLines()
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
        public List<Sports_MoneyLine> SportsMoneyLines(int matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_MoneyLine.Where(w => w.ID == matchID).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       public Sports_MoneyLine SportsMoneyLine(int ID)
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
        /// <param name="sportsMoneyLine"></param>
        /// <returns></returns>
        public bool Insert(Sports_MoneyLine sportsMoneyLine)
        {
            _context.AddToSports_MoneyLine(sportsMoneyLine);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sportsMoneyLine"></param>
        /// <returns></returns>
        public bool Update(Sports_MoneyLine sportsMoneyLine)
        {
            Sports_MoneyLine _obj = new Sports_MoneyLine();
            _obj = _context.Sports_MoneyLine.Where(w => w.ID == sportsMoneyLine.ID).SingleOrDefault();
            if (_obj != null) // Update
            {
                _obj = sportsMoneyLine;
                int result = _context.SaveChanges();
                return result > 0 ? true : false;
            }
            else //Insert
            {
                return Insert(sportsMoneyLine);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sportsMoneyLine"></param>
        /// <returns></returns>
        public bool Delete(Sports_MoneyLine sportsMoneyLine)
        {
            _context.DeleteObject(sportsMoneyLine);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        public IQueryable<Sports_MoneyLine> Table
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Sports_MoneyLine> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_MoneyLine.ToList();

                return list;
            }
        }


        void IBase<Sports_MoneyLine>.Insert(Sports_MoneyLine entity)
        {
            throw new NotImplementedException();
        }
    }
}
