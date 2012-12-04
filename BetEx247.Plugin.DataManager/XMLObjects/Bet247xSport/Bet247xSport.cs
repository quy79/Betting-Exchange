using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using BetEx247.Plugin.DataManager.XMLObjects.Bet247xSport;
using BetEx247.Plugin.DataManager.XMLObjects.SoccerCountry;
using BetEx247.Plugin.DataManager.XMLObjects.SoccerLeague;
using BetEx247.Plugin.DataManager.XMLObjects.SportCountry;
using BetEx247.Plugin.DataManager.XMLObjects.SportLeague;
namespace BetEx247.Plugin.DataManager.XMLObjects.Sport
{
    [Serializable()]
    public class Bet247xSport : BetEx247.Data.Model.Sport
    {
        private List<Bet247xSoccerCountry> bet247xSoccerCountries = new List<Bet247xSoccerCountry>();

        private List<Bet247xSportCountry> bet247xSportCountries = new List<Bet247xSportCountry>();
        
        public List<Bet247xSportCountry> Bet247xSportCountries
        {
            get { return bet247xSportCountries; }
            set { bet247xSportCountries = value; }
        }
       // private List<IBet247xSportLeague> bet247xSportLeagues = new List<IBet247xSportLeague>();
       private bool is_Soccer;
        public List<Bet247xSoccerCountry> Bet247xSoccerCountries
        {
            get { return bet247xSoccerCountries; }
            set { bet247xSoccerCountries = value; }
        }

        /*public List<IBet247xSportLeague> Bet247xSportLeagues 
        {
            get { return bet247xSportLeagues; }
            set { bet247xSportLeagues = value; }
        }*/
       public  bool Is_Soccer
        {
            get { return is_Soccer; }
            set { is_Soccer = value; }
        }
        public BetEx247.Data.Model.Sport getSport(){
            BetEx247.Data.Model.Sport obj = new Data.Model.Sport();
            obj.ID = ID;
            obj.SportName = SportName;
            return obj;
        }
       
    }
}
