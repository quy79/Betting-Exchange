using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using BetEx247.Core.XMLObjects.Match.Period.Interface;
//using BetEx247.Plugin.DataManager.XMLObjects.Bet247xSportMatch.Interface;
using BetEx247.Data.Model;
namespace BetEx247.Plugin.DataManager.XMLObjects.SportMatch
{
     [Serializable]
     public partial class Bet247xSportMatch : BetEx247.Data.Model.SportsMatch
    {
         private List<Sports_MoneyLine> sportMatchOdds = new List<Sports_MoneyLine>();
         private List<Sports_AsianHandicap> sportAsianHandicaps = new List<Sports_AsianHandicap>();
         private List<Sports_TotalOU> sportTotalGoalsOUs = new List<Sports_TotalOU>();


         public List<Sports_MoneyLine> Bet247xSportMatchOdds
         {
             get { return sportMatchOdds; }
             set { sportMatchOdds = value; }
         }
         public List<Sports_AsianHandicap> Bet247xSportAsianHandicaps
         {
             get { return sportAsianHandicaps; }
             set { sportAsianHandicaps = value; }
         }
         public List<Sports_TotalOU> Bet247xSportTotalGoalsOUs
         {
             get { return sportTotalGoalsOUs; }
             set { sportTotalGoalsOUs = value; }
         }
       


         public BetEx247.Data.Model.SportsMatch getMatch()
         {
             BetEx247.Data.Model.SportsMatch obj = new Data.Model.SportsMatch();
             obj.ID = ID;
             obj.AwayTeam = AwayTeam;
             obj.HomeTeam = HomeTeam;
             // obj.MatchStatusID = MatchStatusID;
             obj.LeagueID = this.LeagueID;
             obj.StartDateTime = StartDateTime;
             //obj.MarketCloseTime = MarketCloseTime;
             obj.MatchStatus = MatchStatus;
             obj.UpdateTime = DateTime.Now;
             obj.SportID = SportID;
             obj.CountryID = CountryID;
             obj.Settled = false;
             obj.Updated = false;
             return obj;
         }
       

         
    }
}


         
    

