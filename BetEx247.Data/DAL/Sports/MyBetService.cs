using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.DAL.Sports.Interfaces;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports
{
     public partial class MyBetService 
    {
         public MyBetService()
         {
         }
         public List<MyBet> MyBets()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.MyBets.ToList();

                 return list;
             }
         }
         public MyBet MyBet(Guid ID)
         {
             using (var dba = new BetEXDataContainer())
             {
                 MyBet mb = dba.MyBets.Where(w => w.ID == ID).SingleOrDefault();

                 return mb;
             }
         }

         public List<MyBet> MyBetExposuresList(Guid ID)
         {
             using (var dba = new BetEXDataContainer())
             {
                 var mb = dba.MyBets.Where(w => w.ID != ID).ToList();

                 return mb;
             }
         }

         public List<MyBet> MyBetsByMatch(SoccerMatch _soccerMatch)
         {
             using (var dba = new BetEXDataContainer())
             {
                 var mb = dba.MyBets.Where(w => w.MatchID == _soccerMatch.ID.ToString() & w.LeagueID == _soccerMatch.LeagueID & w.CountryID == _soccerMatch.CountryID & w.SportID == _soccerMatch.SportID).ToList();

                 return mb;
             }
         }
         public List<MyBet> MyBetExposuresList(Guid ID, String match)
         {
             using (var dba = new BetEXDataContainer())
             {
                 var mb = dba.MyBets.Where(w => w.ID != ID & w.BetStatus.Equals(match)).ToList();

                 return mb;
             }
         }

        
         public MyBet MyBet(Guid ID, String bl,long leagueID,long countryID,long sportID)
         {
             using (var dba = new BetEXDataContainer())
             {
                 var mb = dba.MyBets.Where(w => w.ID != ID & w.BL==bl &w.LeagueID==leagueID&w.CountryID==countryID&w.SportID==sportID ).SingleOrDefault();

                 return mb;
             }
         }

         public bool Update(MyBet mb)
         {
            using (var dba = new BetEXDataContainer())
             {
                 var _mb = dba.MyBets.Where(w => w.ID == mb.ID).SingleOrDefault();
                 if (_mb != null)
                 {

                     _mb = mb;
                     
                     dba.SaveChanges();
                     return true;
                 }
             }
             return false;
         }
        
    }
}
