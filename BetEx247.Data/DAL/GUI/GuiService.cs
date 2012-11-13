using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;    
using BetEx247.Core;
using BetEx247.Core.Infrastructure;
using BetEx247.Core.Caching;
using BetEx247.Data.Model;

namespace BetEx247.Data.DAL
{
    public class GuiService : IGuiService
    {
        #region xml
        private static GuiService _instance;
        //private XMLParserObjectManager obj;
        private string key = "betex247";

        /// <summary>
        /// Gets a cache manager
        /// </summary>
        public ICacheManager CacheManager
        {
            get
            {
                return new MemoryCacheManager();
            }
        }

        public GuiService()
        {
            //obj = new XMLParserObjectManager();
            //obj.Parse();
            //CacheManager.Set(key, GetAllSport(), 10);
        }

        public static GuiService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GuiService();
                }
                return _instance;
            }
        }

        /// <summary>
        /// get all sport in system
        /// </summary>
        /// <returns>list sport</returns>
        public List<Sport> GetAllSport()
        {
            //if (CacheManager.IsSet(key))
            //    return CacheManager.Get<List<Sport>>(key);
            //return obj.Sports.Where(w => w.Leagues != null).ToList<Sport>();
            
            return null;
        }

        /// <summary>
        /// Get Sport
        /// </summary>
        /// <param name="type">Sport type</param>
        /// <returns>Sport</returns>
        public Sport GetSport(Constant.SportType type)
        {
            //return obj.Sport(type);
            return null;
        }
        #endregion

        #region database
        public IEnumerable<Sport> GetSportData()
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.Sports.ToList();
            }
        }

        public List<SoccerMatch> LiveInMatches(bool isSoccer)
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.PSP_LIVEINPLAYMATCHES(isSoccer).ToList();
            }            
        }

        public List<SoccerMatch> UpCommingMatches(bool isSoccer,long? leagueId,int? countryId,int? sportId)
        {
            using (var dba = new BetEXDataContainer())
            {
                if (leagueId == null || leagueId == 0)
                {
                    return dba.PSP_UPCOMMINGMATCHES(isSoccer,0,0,0).ToList();
                }
                else
                {
                    return dba.PSP_UPCOMMINGMATCHES(isSoccer, leagueId,countryId,sportId).ToList();
                }
            }   
        }

        public SoccerCountry GetCountryByLeage(long leagueId,int countryId,int sportId)
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.PSP_GETCOUNTRYBYLEAGUE(leagueId,countryId,sportId).First();
            }
        }

        public SoccerCountry GetCountryByCountry(int countryId)
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.SoccerCountries.Where(w => w.ID == countryId).SingleOrDefault();
            }
        }

        public List<PSV_ALLTOURNAMENT> GetTournamentByCountry(int countryId)
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.PSP_GETCOUNTLEAGUEBYCOUNTRY(countryId).ToList();
            }
        }

        public SoccerLeague GetSoccerLeague(long leagueId, int countryId, int sportId)
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.SoccerLeagues.Where(w => w.ID == leagueId && w.CountryID == countryId && w.SportID == sportId).SingleOrDefault();
            }
        }
        #endregion
    }
}
