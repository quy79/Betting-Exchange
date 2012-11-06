using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.DAL.Sports.Interfaces;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports
{
     public partial class BetStatusService : IBetStatusService
    {
         public BetStatusService()
         {
         }
         public List<BetStatu> BetStatus()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.BetStatus.ToList();

                 return list;
             }
         }
         public BetStatu BetStatus(int ID)
         {
             using (var dba = new BetEXDataContainer())
             {
                 BetStatu _sport = dba.BetStatus.Where(w => w.ID == ID).SingleOrDefault();

                 return _sport;
             }
         }
         public BetStatu BetStatus(String name)
         {
             using (var dba = new BetEXDataContainer())
             {
                 BetStatu _sport = dba.BetStatus.Where(w => w.Status == name).SingleOrDefault();

                 return _sport;
             }
         }
         public bool Delete(BetStatu sport)
         {
             return false;
         }
         public bool Insert(BetStatu status)
         {
             using (var dba = new BetEXDataContainer())
             {
                 dba.AddToBetStatus(status);
                 return true;
             }
         }
         public bool Update(BetStatu sport)
         {
             return false;
         }
         public IQueryable<BetStatu> Table
         {
             get { throw new NotImplementedException(); }
         }

         public IList<BetStatu> GetAll()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.BetStatus.ToList();

                 return list;
             }
         }

         public List<BetStatu> MatchStatus()
         {
             throw new NotImplementedException();
         }

         IList<Sport> IBase<Sport>.GetAll()
         {
             throw new NotImplementedException();
         }

         public void Insert(Sport entity)
         {
             throw new NotImplementedException();
         }

         public bool Update(Sport entity)
         {
             throw new NotImplementedException();
         }

         public bool Delete(Sport entity)
         {
             throw new NotImplementedException();
         }

         IQueryable<Sport> IBase<Sport>.Table
         {
             get { throw new NotImplementedException(); }
         }

         List<BetStatu> IBetStatusService.MatchStatus()
         {
             throw new NotImplementedException();
         }

         BetStatu IBetStatusService.BetStatus(int ID)
         {
             throw new NotImplementedException();
         }

         BetStatu IBetStatusService.BetStatus(string name)
         {
             throw new NotImplementedException();
         }


         void IBase<Sport>.Insert(Sport entity)
         {
             throw new NotImplementedException();
         }

         bool IBase<Sport>.Update(Sport entity)
         {
             throw new NotImplementedException();
         }

         bool IBase<Sport>.Delete(Sport entity)
         {
             throw new NotImplementedException();
         }
    }
}
