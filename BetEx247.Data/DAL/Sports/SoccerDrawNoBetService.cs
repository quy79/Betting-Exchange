using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SoccerDrawNoBetService : ISoccerDrawNoBetService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Soccer_DrawNoBet> SoccerDrawNoBets()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_DrawNoBet.ToList();

                return list;
            }
        }
        public  List<Soccer_DrawNoBet> SoccerDrawNoBets(long matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_DrawNoBet.Where(w => w.ID == matchID).ToList();

                return list;
            }
        }
        public Soccer_DrawNoBet SoccerDrawNoBet(long ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Soccer_DrawNoBet.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
         public bool Insert(Soccer_DrawNoBet soccerDrawNoBet)
        {
            _context.AddToSoccer_DrawNoBet(soccerDrawNoBet);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
         public bool Update(Soccer_DrawNoBet soccerDrawNoBet)
        {
            Soccer_DrawNoBet _obj = new Soccer_DrawNoBet();
            _obj = _context.Soccer_DrawNoBet.Where(w => w.ID == soccerDrawNoBet.ID).SingleOrDefault();
            if (_obj != null) // Update
            {
                _obj = soccerDrawNoBet;
                int result = _context.SaveChanges();
                return result > 0 ? true : false;
            }
            else //Insert
            {
                return Insert(soccerDrawNoBet);
            }
        }
        public bool Delete(Soccer_DrawNoBet soccerDrawNoBet)
        {
            _context.DeleteObject(soccerDrawNoBet);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        public IQueryable<Soccer_DrawNoBet> Table
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Soccer_DrawNoBet> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Soccer_DrawNoBet.ToList();

                return list;
            }
        }


        void IBase<Soccer_DrawNoBet>.Insert(Soccer_DrawNoBet entity)
        {
            throw new NotImplementedException();
        }
    }
}
