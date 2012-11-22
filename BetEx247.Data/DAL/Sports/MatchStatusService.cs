using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.DAL.Sports.Interfaces;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports
{
     public partial class MatchStatusService : IMatchStatusService
    {
         public MatchStatusService()
         {
         }
         public List<MatchStatu> MatchStatuses()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.MatchStatus.ToList();

                 return list;
             }
         }
         public MatchStatu MatchStatus(int ID)
         {
             using (var dba = new BetEXDataContainer())
             {
                 MatchStatu _sport = dba.MatchStatus.Where(w => w.ID == ID).SingleOrDefault();

                 return _sport;
             }
         }
         public MatchStatu MatchStatus(String name)
         {
             using (var dba = new BetEXDataContainer())
             {
                 MatchStatu _sport = dba.MatchStatus.Where(w => w.Status == name).SingleOrDefault();

                 return _sport;
             }
         }
         public bool Delete(MatchStatu sport)
         {
             return false;
         }
         public bool Insert(MatchStatu status)
         {
             using (var dba = new BetEXDataContainer())
             {
                 dba.AddToMatchStatus(status);

                 return true;
             }
         }
         public bool Update(MatchStatu sport)
         {
             return false;
         }
         public IQueryable<MatchStatu> Table
         {
             get { throw new NotImplementedException(); }
         }

         public IList<MatchStatu> GetAll()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.MatchStatus.ToList();

                 return list;
             }
         }

         public List<MatchStatu> MatchStatues()
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
