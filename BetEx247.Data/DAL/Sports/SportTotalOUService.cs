using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SportTotalOUService : ISportTotalOUService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
       public List<Sports_TotalOU> SportsTotalOUs()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_TotalOU.ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
       public List<Sports_TotalOU> SportsTotalOUs(Guid matchID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Sports_TotalOU.Where(w => w.ID == matchID).ToList();

                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       public Sports_TotalOU SportsTotalOU(Guid ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Sports_TotalOU.Where(w => w.ID == ID).SingleOrDefault();

                return obj;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
       public bool Insert(Sports_TotalOU soccerCorrectScores)
        {
            _context.AddToSports_TotalOU(soccerCorrectScores);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerCorrectScores"></param>
        /// <returns></returns>
       public bool Update(Sports_TotalOU soccerCorrectScores)
        {
            Sports_TotalOU _obj = new Sports_TotalOU();
            _obj = _context.Sports_TotalOU.Where(w => w.ID == soccerCorrectScores.ID).SingleOrDefault();
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
       public bool Delete(Sports_TotalOU soccerCorrectScores)
        {
            _context.DeleteObject(soccerCorrectScores);
            int result = _context.SaveChanges();
            return result > 0 ? true : false;
        }
       public IQueryable<Sports_TotalOU> Table
       {
           get { throw new NotImplementedException(); }
       }

       public IList<Sports_TotalOU> GetAll()
       {
           using (var dba = new BetEXDataContainer())
           {
               var list = dba.Sports_TotalOU.ToList();

               return list;
           }
       }


       void IBase<Sports_TotalOU>.Insert(Sports_TotalOU entity)
       {
           throw new NotImplementedException();
       }
    }
}
