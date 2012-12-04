using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Plugin.DataManager.XMLObjects.SportMatch;
namespace BetEx247.Plugin.DataManager.XMLObjects.SportLeague
{
     [Serializable]
    public partial class Bet247xSportLeague : BetEx247.Data.Model.SportLeague
    {
 
        private string name = string.Empty;
        private List<Bet247xSportMatch> bet247xSportMatches = new List<Bet247xSportMatch>();

        public List<Bet247xSportMatch> Bet247xSportMatches
        {
            get { return bet247xSportMatches; }
            set { bet247xSportMatches = value; }
        }
        public BetEx247.Data.Model.SportLeague getSportLeague()
        {
            BetEx247.Data.Model.SportLeague obj = new Data.Model.SportLeague();
            obj.ID = this.ID;
            obj.CountryID = this.CountryID;
            obj.SportID = SportID;
            obj.LeagueName = this.LeagueName;
         
            //obj.SportMatches = this.SportMatches;
            return obj;
        }
      
        
    }
}
