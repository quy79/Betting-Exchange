using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using BetEx247.Core.XMLObjects.Match.Period.Interface;
//using BetEx247.Plugin.DataManager.XMLObjects.Bet247xSoccerMatch.Interface;
using BetEx247.Data.Model;
namespace BetEx247.Plugin.DataManager.XMLObjects.SoccerMatch
{
     [Serializable]
    public partial class Bet247xSoccerMatch : BetEx247.Data.Model.SoccerMatch
    {
         private List<Soccer_MatchOdds> soccerMatchOdds = new List<Soccer_MatchOdds>();
         private List<Soccer_AsianHandicap> soccerAsianHandicaps = new List<Soccer_AsianHandicap>();
         private List<Soccer_TotalGoalsOU> soccerTotalGoalsOUs =  new List<Soccer_TotalGoalsOU>();
         private List<Soccer_CorrectScores> soccerCorrectScores =  new List<Soccer_CorrectScores>();
         private List<Soccer_DrawNoBet> soccerDrawNoBets =  new List<Soccer_DrawNoBet>();

         public  List<Soccer_MatchOdds> Bet247xSoccerMatchOdds
         {
             get { return soccerMatchOdds; }
             set { soccerMatchOdds = value; }
         }
         public  List<Soccer_AsianHandicap> Bet247xSoccerAsianHandicaps
         {
             get { return soccerAsianHandicaps; }
             set { soccerAsianHandicaps = value; }
         }
         public  List<Soccer_TotalGoalsOU> Bet247xSoccerTotalGoalsOUs
         {
             get { return soccerTotalGoalsOUs; }
             set { soccerTotalGoalsOUs = value; }
         }
         public List<Soccer_DrawNoBet> Bet247xSoccerDrawNoBets
         {
             get { return soccerDrawNoBets; }
             set { soccerDrawNoBets = value; }
         }
         public  List<Soccer_CorrectScores> Bet247xSoccerCorrectScores
         {
             get { return soccerCorrectScores; }
             set { soccerCorrectScores = value; }
         }


         public BetEx247.Data.Model.SoccerMatch getMatch()
         {
             BetEx247.Data.Model.SoccerMatch obj = new Data.Model.SoccerMatch();
             obj.ID = ID;
             obj.AwayTeam = AwayTeam;
             obj.HomeTeam = HomeTeam;
             obj.MatchStatusID = MatchStatusID;
            obj.LeagueID = this.LeagueID;
             obj.StartDate = StartDate;
             obj.StartTime = StartTime;
             obj.SportID = SportID;
             obj.CountryID = CountryID;
            // obj.Sports_AsianHandicap=spo
             return obj;
         }
       

         
    }
}
