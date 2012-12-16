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
             String id = _soccerMatch.ID.ToString();
             using (var dba = new BetEXDataContainer())
             {
                 var mb = dba.MyBets.Where(w => w.MatchID == id & w.LeagueID == _soccerMatch.LeagueID & w.CountryID == _soccerMatch.CountryID & w.SportID == _soccerMatch.SportID).ToList();

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
                 var mb = dba.MyBets.Where(w => w.ID == ID & w.BL==bl &w.LeagueID==leagueID&w.CountryID==countryID&w.SportID==sportID ).SingleOrDefault();

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

                     _mb.BetStatus = mb.BetStatus;
                     _mb.BL = mb.BL;
                     _mb.CancelledTime = mb.CancelledTime;
                     _mb.Commission = mb.Commission;
                     _mb.Exposure = mb.Exposure;
                     _mb.GrossProfit = mb.GrossProfit;
                     _mb.GrossWinning = mb.GrossWinning;
                     _mb.IsDraw = mb.IsDraw;
                     _mb.IsWon = mb.IsWon;
                     _mb.Liability = mb.Liability;
                     _mb.MyEntrant = mb.MyEntrant;
                     _mb.NetProfit = mb.NetProfit;
                     _mb.Payouts = mb.Payouts;
                     _mb.PointsRefunded = mb.PointsRefunded;
                     _mb.Stake = mb.Stake;
                     _mb.SubmitedTime = mb.SubmitedTime;
                     


                     dba.SaveChanges();
                     return true;
                 }
             }
             return false;
         }
        
    }
}
