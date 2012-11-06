using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.DAL.Sports.Interfaces;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports
{
     public partial class SportService : ISportService
    {
         public SportService()
         {
         }
         public List<Sport> Sports()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.Sports.ToList();

                 return list;
             }
         }
         public Sport Sport(int ID)
         {
             using (var dba = new BetEXDataContainer())
             {
                 Sport _sport  = dba.Sports.Where(w => w.ID == ID).SingleOrDefault();

                 return _sport;
             }
         }
         public Sport Sport(String name)
         {
             using (var dba = new BetEXDataContainer())
             {
                 Sport _sport = dba.Sports.Where(w => w.SportName == name).SingleOrDefault();

                 return _sport;
             }
         }
         public bool Delete(Sport sport)
         {
             using (var dba = new BetEXDataContainer())
             {

                 dba.DeleteObject(sport);
                 return true;
             }
         }
         public bool Insert(Sport sport)
         {
             using (var dba = new BetEXDataContainer())
             {
                 Sport _sport = dba.Sports.Where(w => w.ID == sport.ID).SingleOrDefault();
                 if (_sport == null)
                 {
                     dba.AddToSports(sport);
                     dba.SaveChanges();
                 }
                 else
                 {
                     Update(sport);
                 }
                
                 
                 return true;
             }

             
         }
         public bool Update(Sport sport)
         {
            using (var dba = new BetEXDataContainer())
             {
                 var _sport = dba.Sports.Where(w => w.ID == sport.ID).SingleOrDefault();
                 if (_sport != null)
                 {

                     _sport.SportName = sport.SportName;
                     
                     dba.SaveChanges();
                     return true;
                 }
             }
             return false;
         }
         public IQueryable<Sport> Table
         {
             get { throw new NotImplementedException(); }
         }

         public IList<Sport> GetAll()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.Sports.ToList();

                 return list;
             }
         }


         void IBase<Sport>.Insert(Sport entity)
         {
             throw new NotImplementedException();
         }
    }
}
