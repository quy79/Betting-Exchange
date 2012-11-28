using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

using BetEx247.Plugin.DataManager.XMLObjects.Sport;

using BetEx247.Plugin.DataManager.XMLObjects.SoccerCountry;

using BetEx247.Plugin.DataManager.XMLObjects.SoccerLeague;
using BetEx247.Plugin.DataManager.XMLObjects;
using BetEx247.Plugin.DataManager.XMLObjects.SoccerMatch;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using BetEx247.Core;
using BetEx247.Data.Model;
using BetEx247.Core.Common.Utils;
using BetEx247.Data.DAL.Sports;
namespace BetEx247.Plugin.DataManager
{
    [Serializable()]
    public class SportsDataRenderManager
    {
        List<Bet247xSport> sports = new System.Collections.Generic.List<Bet247xSport>();
        public SportsDataRenderManager()
        {
        }
        public void CollectInfoToSerialize()
        {
            SportService sportSvr = new SportService();
            List<Sport> _sports1 = sportSvr.Sports();
            foreach (Sport sp in _sports1)
            {
                Bet247xSport _bet247xSport = new DataManager.XMLObjects.Sport.Bet247xSport()
                {
                    ID = sp.ID,
                    SportName = sp.SportName
                };
                // _bet247xSport = sp;
                if (_bet247xSport.ID == 1) //Soccer
                    loadCountry(ref _bet247xSport);
                sports.Add(_bet247xSport);
            }
            SerializeObject(sports);

        }
        public List<Bet247xSport> refreshData()
        {

            List<Bet247xSport> _sports = this.DeSerializeObject();
            if (_sports == null && _sports.Count>0)
            {
                SportService sportSvr = new SportService();
                List<Sport> _sports1 = sportSvr.Sports();
                foreach (Sport sp in _sports1)
                {
                    Bet247xSport _bet247xSport = new DataManager.XMLObjects.Sport.Bet247xSport()
                    {
                        ID = sp.ID,
                        SportName = sp.SportName
                    };
                    // _bet247xSport = sp;
                    if (_bet247xSport.ID == 1) //Soccer
                        loadCountry(ref _bet247xSport);
                    if (_bet247xSport.ID == 1) //Soccer
                        loadCountry(ref _bet247xSport);
                    if (_bet247xSport.ID == 1) //Soccer
                        loadCountry(ref _bet247xSport);
                    if (_bet247xSport.ID == 1) //Soccer
                        loadCountry(ref _bet247xSport);
                    sports.Add(_bet247xSport);
                }
                return sports;
            }
            else
            {

                this.sports = _sports;
            }
            return sports;

        }
        void loadCountry(ref  Bet247xSport _bet247xSport)
        {
            List<Bet247xSoccerCountry> _soccerCountries = new List<Bet247xSoccerCountry>();

            SoccerCountryService _soccerCountrySvr = new SoccerCountryService();
            List<SoccerCountry> _countries = _soccerCountrySvr.SoccerCountries();

            foreach (SoccerCountry sp in _countries)
            {
                Bet247xSoccerCountry _obj = new DataManager.XMLObjects.SoccerCountry.Bet247xSoccerCountry()
                {
                    ID = sp.ID,
                    Country = sp.Country,
                    // Bet247xSoccerLeagues = sp.Bet247xSoccerLeagues,
                    Betclick_OddsFeed = sp.Betclick_OddsFeed,
                    Goalserve_LivescoreFeed = sp.Goalserve_LivescoreFeed,
                    Goalserve_OddsFeed = sp.Goalserve_OddsFeed,
                    International = sp.International
                };
                //  _obj = (Bet247xSoccerCountry)sp;
                loadLeague(_bet247xSport, ref _obj);
                _soccerCountries.Add(_obj);
            }
            _bet247xSport.Bet247xSoccerCountries.AddRange(_soccerCountries);
        }

        void loadLeague(Bet247xSport _bet247xSport, ref  Bet247xSoccerCountry _country)
        {
            List<Bet247xSoccerLeague> _soccerLeagues = new List<Bet247xSoccerLeague>();

            SoccerLeagueService _soccerLeagueSvr = new SoccerLeagueService();
            List<SoccerLeague> _leagues = _soccerLeagueSvr.SoccerLeagues(_bet247xSport.ID, _country.ID);

            foreach (SoccerLeague sp in _leagues)
            {
                Bet247xSoccerLeague _obj = new DataManager.XMLObjects.SoccerLeague.Bet247xSoccerLeague()
                {
                    ID = sp.ID,
                    CountryID = sp.CountryID,
                    SportID = sp.SportID,
                    LeagueName_Betclick = sp.LeagueName_Betclick,
                    LeagueName_Goalserve = sp.LeagueName_Goalserve,
                    LeagueName_WebDisplay = sp.LeagueName_WebDisplay
                };
                //_obj = (Bet247xSoccerLeague)sp;
                _soccerLeagues.Add(_obj);
            }
            _country.Bet247xSoccerLeagues.AddRange(_soccerLeagues);
        }

        void loadMatch(ref Bet247xSoccerLeague _soccerLeague)
        {
            List<Bet247xSoccerMatch> _soccerMatches = new List<Bet247xSoccerMatch>();
            String _matchStatus = ""; // not started
            SoccerMatchService _soccerMatchSvr = new SoccerMatchService();
            List<SoccerMatch> _matches = _soccerMatchSvr.SoccerMatches(_soccerLeague.SportID, _soccerLeague.CountryID, _soccerLeague.ID, _matchStatus);

            foreach (SoccerMatch sp in _matches)
            {
                Bet247xSoccerMatch _obj = new DataManager.XMLObjects.SoccerMatch.Bet247xSoccerMatch()
                {
                    ID = sp.ID,
                    AwayTeam = sp.AwayTeam,
                    HomeTeam = sp.HomeTeam,
                    MatchStatus = sp.MatchStatus,
                    LeagueID = sp.LeagueID,
                    SportID = sp.SportID,
                    CountryID = sp.CountryID,
                    StartDateTime = sp.StartDateTime

                };

                loadDrawNoBet(ref _obj);
                loadTotalOUBet(ref _obj);
                loadHandicapBet(ref _obj);
                loadMatchOddBet(ref _obj);
                loadCorrectScoreBet(ref _obj);
                _soccerMatches.Add(_obj);
            }





            //Over/Under 1st Half
            //  SoccerTotalGoalsOUService _soccerTotalOUSvr = new SoccerTotalGoalsOUService();
            //  _soccerTotalOUSvr.Insert(_soccer_TotalGoalsOU);

        }
        void loadDrawNoBet(ref Bet247xSoccerMatch _soccerMatch)
        {
            //Home/Away
            SoccerDrawNoBetService _soccerDrawNoBetSvr = new SoccerDrawNoBetService();
            List<Soccer_DrawNoBet> _objs = _soccerDrawNoBetSvr.SoccerDrawNoBets(_soccerMatch.SportID, _soccerMatch.CountryID, _soccerMatch.LeagueID, _soccerMatch.ID);

            _soccerMatch.Bet247xSoccerDrawNoBets.AddRange(_objs);
        }
        void loadTotalOUBet(ref Bet247xSoccerMatch _soccerMatch)
        {
            //Over/Under
            SoccerTotalGoalsOUService _soccerTotalOUSvr = new SoccerTotalGoalsOUService();
            List<Soccer_TotalGoalsOU> _objs = _soccerTotalOUSvr.SoccerTotalGoalsOUs(_soccerMatch.SportID, _soccerMatch.CountryID, _soccerMatch.LeagueID, _soccerMatch.ID);

            _soccerMatch.Bet247xSoccerTotalGoalsOUs.AddRange(_objs);
        }
        void loadHandicapBet(ref Bet247xSoccerMatch _soccerMatch)
        {
            //Handicap
            SoccerAsianHandicapService _soccerHandicapSvr = new SoccerAsianHandicapService();
            List<Soccer_AsianHandicap> _objs = _soccerHandicapSvr.SoccerAsianHandicaps(_soccerMatch.SportID, _soccerMatch.CountryID, _soccerMatch.LeagueID, _soccerMatch.ID);

            _soccerMatch.Bet247xSoccerAsianHandicaps.AddRange(_objs);
        }
        void loadMatchOddBet(ref Bet247xSoccerMatch _soccerMatch)
        {
            //3Way Result 1st Half
            SoccerMatchOddsService _soccerMatchOddSvr = new SoccerMatchOddsService();
            List<Soccer_MatchOdds> _objs = _soccerMatchOddSvr.SoccerMatchOddses(_soccerMatch.SportID, _soccerMatch.CountryID, _soccerMatch.LeagueID, _soccerMatch.ID);

            _soccerMatch.Bet247xSoccerMatchOdds.AddRange(_objs);
        }
        void loadCorrectScoreBet(ref Bet247xSoccerMatch _soccerMatch)
        {
            //Correct
            SoccerCorrectScoresService _soccerCorectSvr = new SoccerCorrectScoresService();
            List<Soccer_CorrectScores> _objs = _soccerCorectSvr.SoccerCorrectScoreses(_soccerMatch.SportID, _soccerMatch.CountryID, _soccerMatch.LeagueID, _soccerMatch.ID);

            _soccerMatch.Bet247xSoccerCorrectScores.AddRange(_objs);
        }

        #region Serialier
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public SportsDataRenderManager(SerializationInfo info, StreamingContext ctxt)
        {

            this.sports = (List<Bet247xSport>)info.GetValue("Sports", typeof(List<Bet247xSport>));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("Sports", this.sports);
        }
        public void SerializeObject(List<Bet247xSport> objectToSerialize)
        {
            if (objectToSerialize != null)
            {
                //Stream stream = File.Open(filename, FileMode.Create);
                BinaryFormatter serializer = new BinaryFormatter();
                //bFormatter.Serialize(stream, objectToSerialize);
                //stream.Close();
                MemoryStream stream = new MemoryStream();
                serializer.Serialize(stream, objectToSerialize);

                string str = System.Convert.ToBase64String(stream.ToArray());
                DataControlStatusService Svr = new DataControlStatusService();
                DataControlStatu status = new DataControlStatu();
                status.ID = 1;
                status.DataObject = str;
                Svr.Insert(status);
            }
        }

        public List<Bet247xSport> DeSerializeObject()
        {
            List<Bet247xSport> objectToSerialize;
            try
            {

                DataControlStatusService Svr = new DataControlStatusService();
                DataControlStatu status = Svr.DataControlStatus(1);



                byte[] str = System.Convert.FromBase64String(status.DataObject);
                MemoryStream stream = new MemoryStream(str);

                //Stream stream = File.Open(filename, FileMode.Open);
                BinaryFormatter bFormatter = new BinaryFormatter();
                objectToSerialize = (List<Bet247xSport>)bFormatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception e)
            {
                return null;
            }
            return objectToSerialize;
        }
        #endregion
    }
}
