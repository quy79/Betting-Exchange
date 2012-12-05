using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.DAL.Sports.Interfaces;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SportLeagueService 
    {
        public List<SportLeague> SportLeagues()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.SportLeagues.ToList();

                 return list;
             }
         }
        public SportLeague SportLeague(long ID)
         {
             using (var dba = new BetEXDataContainer())
             {
                 SportLeague _sport = dba.SportLeagues.Where(w => w.ID == ID).SingleOrDefault();

                 return _sport;
             }
         }
        public SportLeague SportLeague(long ID, long sportID, long countryID)
        {
            using (var dba = new BetEXDataContainer())
            {
                SportLeague _sport = dba.SportLeagues.Where(w => w.ID == ID & w.SportID == sportID & w.CountryID == countryID).SingleOrDefault();

                return _sport;
            }
        }
        public List<SportLeague> SportLeagues( long sportID, long countryID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list  = dba.SportLeagues.Where(w => w.SportID == sportID & w.CountryID == countryID).ToList();

                return list;
            }
        }
        public SportLeague GoalServeSportLeague(long countryID,String name)
         {
             using (var dba = new BetEXDataContainer())
             {
                 var _sport = dba.SportLeagues.Where(w => name.Contains(w.LeagueName) & w.CountryID ==countryID).ToList();

                 return _sport.Count==0?null:_sport[0];
             }
         }

        public long GoalServeSportLeagueMaxIdByCountry(long countryID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var _sport = dba.SportLeagues.Where(w => w.CountryID == countryID ).ToList();
                long maxID = 0;
                foreach (SportLeague sl in _sport)
                {
                    if(sl.ID>maxID){
                        maxID = sl.ID;
                    }
                }
                return maxID;
            }
        }

        public bool Delete(SportLeague sport)
         {
             return false;
         }
        public void Insert(SportLeague league)
         {
             using (var dba = new BetEXDataContainer())
             {

                 SportLeague _league = SportLeague(league.ID, (long)league.SportID, (long)league.CountryID);
                 if (_league == null)
                 {

                     dba.AddToSportLeagues(league);
                     dba.SaveChanges();
                 }
                 else
                 {
                     Update(_league);
                 }
                
             }
         }
        public bool Update(SportLeague league)
         {
             using (var dba = new BetEXDataContainer())
             {
                 var _league = SportLeague(league.ID, league.SportID, league.CountryID);
                 if (_league != null)
                 {

                     _league.ID = league.ID;
                     _league.CountryID = league.CountryID;
                     _league.SportID = league.SportID;
                     _league.LeagueName = league.LeagueName;
                    

                     dba.SaveChanges();
                     return true;
                 }
             }
             return false;
         }
        public IQueryable<SportLeague> Table
         {
             get { throw new NotImplementedException(); }
         }

        public IList<SportLeague> GetAll()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.SportLeagues.ToList();

                 return list;
             }
         }

     

        

       
    }
}
