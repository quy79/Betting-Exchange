using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using BetEx247.Plugin.DataManager.XMLObjects.SoccerLeague;
using BetEx247.Data.Model;
namespace BetEx247.Plugin.DataManager.XMLObjects.SoccerCountry
{
    [Serializable()]
    public class Bet247xSoccerCountry : BetEx247.Data.Model.SoccerCountry
    {
        //private int id = 0;

        private List<Bet247xSoccerLeague> bet247xSoccerLeagues = new List<Bet247xSoccerLeague>();
      //  Constant.SourceFeedType sportFeedType;
       
        //public int ID
        //{
        //    get { return id; }
        //    set { id = value; }
        //}

        public List<Bet247xSoccerLeague> Bet247xSoccerLeagues
        {
            get { return bet247xSoccerLeagues; }
            set { bet247xSoccerLeagues = value; }
        }

        public BetEx247.Data.Model.SoccerCountry getSoccerCountry()
        {
            BetEx247.Data.Model.SoccerCountry obj = new Data.Model.SoccerCountry();
            obj.Country = this.Country;
            obj.ID = this.ID;
            obj.Goalserve_LivescoreFeed = this.Goalserve_LivescoreFeed;
            obj.Goalserve_OddsFeed = this.Goalserve_OddsFeed;
            obj.Betclick_OddsFeed = this.Betclick_OddsFeed;
            obj.International = this.International;

            return obj;// return this.GetType().BaseType;
        }
    }
}
