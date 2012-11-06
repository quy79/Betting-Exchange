using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;

//using BetEx247.Core.XMLObjects.Match.Interface;
//using BetEx247.Plugin.DataManager.XMLObjects.Bet247xSoccerLeague.Interface;
using BetEx247.Plugin.DataManager.XMLObjects.SoccerMatch;
namespace BetEx247.Plugin.DataManager.XMLObjects.SoccerLeague
{
     [Serializable]
    public partial class Bet247xSoccerLeague : BetEx247.Data.Model.SoccerLeague
    {
 
        private string name = string.Empty;
        private List<Bet247xSoccerMatch> bet247xSoccerMatches = new List<Bet247xSoccerMatch>();

        public List<Bet247xSoccerMatch> Bet247xSoccerMatches
        {
            get { return bet247xSoccerMatches; }
            set { bet247xSoccerMatches = value; }
        }
        public BetEx247.Data.Model.SoccerLeague getSoccerLeague()
        {
            BetEx247.Data.Model.SoccerLeague obj = new Data.Model.SoccerLeague();
            obj.ID = this.ID;
            obj.CountryID = this.CountryID;
            obj.SportID = 1;
            obj.LeagueName_Betclick = this.LeagueName_Betclick;
            obj.LeagueName_Goalserve = LeagueName_Goalserve;
            obj.LeagueName_WebDisplay = this.LeagueName_WebDisplay;
            //obj.SoccerMatches = this.SoccerMatches;
            return obj;
        }
      
        
    }
}
