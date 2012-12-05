using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using BetEx247.Plugin.DataManager.XMLObjects.SportLeague;
using BetEx247.Data.Model;
namespace BetEx247.Plugin.DataManager.XMLObjects.SportCountry
{
    [Serializable()]
    public class Bet247xSportCountry : BetEx247.Data.Model.SportCountry
    {
        //private int id = 0;

        private List<Bet247xSportLeague> bet247xSportLeagues = new List<Bet247xSportLeague>();
      //  Constant.SourceFeedType sportFeedType;
       
        //public int ID
        //{
        //    get { return id; }
        //    set { id = value; }
        //}

        public List<Bet247xSportLeague> Bet247xSportLeagues
        {
            get { return bet247xSportLeagues; }
            set { bet247xSportLeagues = value; }
        }

        public BetEx247.Data.Model.SportCountry getSportCountry()
        {
            BetEx247.Data.Model.SportCountry obj = new Data.Model.SportCountry();
            obj.Country = this.Country;
            obj.ID = this.ID;
            obj.SportID = SportID;
            
            obj.Goalserve_LivescoreFeed = this.Goalserve_LivescoreFeed;
            obj.Goalserve_OddsFeed = this.Goalserve_OddsFeed;
            //obj.Betclick_OddsFeed = this.Betclick_OddsFeed;
            obj.International = this.International;

            return obj;// return this.GetType().BaseType;
        }
    }
}
