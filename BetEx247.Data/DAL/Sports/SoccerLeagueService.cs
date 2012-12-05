using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.DAL.Sports.Interfaces;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SoccerLeagueService : ISoccerLeagueService
    {
        public List<SoccerLeague> SoccerLeagues()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.SoccerLeagues.ToList();

                 return list;
             }
         }
        public SoccerLeague SoccerLeague(long ID)
         {
             using (var dba = new BetEXDataContainer())
             {
                 SoccerLeague _sport = dba.SoccerLeagues.Where(w => w.ID == ID).SingleOrDefault();

                 return _sport;
             }
         }
        public SoccerLeague SoccerLeague(long ID, long sportID, long countryID)
        {
            using (var dba = new BetEXDataContainer())
            {
                SoccerLeague _sport = dba.SoccerLeagues.Where(w => w.ID == ID & w.SportID == sportID & w.CountryID == countryID).SingleOrDefault();

                return _sport;
            }
        }
        public List<SoccerLeague> SoccerLeagues( long sportID, long countryID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var list  = dba.SoccerLeagues.Where(w => w.SportID == sportID & w.CountryID == countryID).ToList();

                return list;
            }
        }
        public SoccerLeague GoalServeSoccerLeague(long countryID,String name)
         {
             using (var dba = new BetEXDataContainer())
             {
                 var _sport = dba.SoccerLeagues.Where(w => name.Contains(w.LeagueName_Goalserve) & w.CountryID ==countryID).ToList();

                 return _sport.Count==0?null:_sport[0];
             }
         }

        public long GoalServeSoccerLeagueMaxIdByCountry(long countryID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var _sport = dba.SoccerLeagues.Where(w => w.CountryID == countryID ).ToList();
                long maxID = 0;
                foreach (SoccerLeague sl in _sport)
                {
                    if(sl.ID>maxID){
                        maxID = sl.ID;
                    }
                }
                return maxID;
            }
        }
        public SoccerLeague BetClickSoccerLeague(String name)
        {
            using (var dba = new BetEXDataContainer())
            {
                SoccerLeague _sport = dba.SoccerLeagues.Where(w => w.LeagueName_Betclick == name).SingleOrDefault();

                return _sport;
            }
        }
        public bool Delete(SoccerLeague sport)
         {
             return false;
         }
        public void Insert(SoccerLeague league)
         {
             using (var dba = new BetEXDataContainer())
             {

                 SoccerLeague _league = SoccerLeague(league.ID, (long)league.SportID, (long)league.CountryID);
                 if (_league == null)
                 {

                     dba.AddToSoccerLeagues(league);
                     dba.SaveChanges();
                 }
                 else
                 {
                     Update(_league);
                 }
                
             }
         }
        public bool Update(SoccerLeague league)
         {
             using (var dba = new BetEXDataContainer())
             {
                 var _league = SoccerLeague(league.ID, league.SportID, league.CountryID);
                 if (_league != null)
                 {

                     _league.ID = league.ID;
                     _league.CountryID = league.CountryID;
                     _league.SportID = 1;
                     _league.LeagueName_Betclick = league.LeagueName_Betclick;
                     _league.LeagueName_Goalserve = league.LeagueName_Goalserve;
                     _league.LeagueName_WebDisplay = league.LeagueName_WebDisplay;

                     dba.SaveChanges();
                     return true;
                 }
             }
             return false;
         }
        public IQueryable<SoccerLeague> Table
         {
             get { throw new NotImplementedException(); }
         }

        public IList<SoccerLeague> GetAll()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.SoccerLeagues.ToList();

                 return list;
             }
         }

        public SoccerLeague SoccerCountry()
         {
             throw new NotImplementedException();
         }

        IList<SoccerLeague> IBase<SoccerLeague>.GetAll()
         {
             throw new NotImplementedException();
         }

        

       
    }
}
