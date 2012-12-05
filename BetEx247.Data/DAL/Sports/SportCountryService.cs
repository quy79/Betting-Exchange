using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.DAL.Sports.Interfaces;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SportCountryService 
    {
        public List<SportCountry> SportCountries()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.SportCountries.ToList();

                 return list;
             }
         }
        public SportCountry SportCountry(long ID)
         {
             using (var dba = new BetEXDataContainer())
             {
                 SportCountry _sport = dba.SportCountries.Where(w => w.ID == ID).SingleOrDefault();

                 return _sport;
             }
         }
        public SportCountry SportCountry(long ID,long sportID)
        {
            using (var dba = new BetEXDataContainer())
            {
                SportCountry _sport = dba.SportCountries.Where(w => w.ID == ID & w.SportID == sportID).SingleOrDefault();

                return _sport;
            }
        }
        public SportCountry SportCountry(String name)
         {
             using (var dba = new BetEXDataContainer())
             {
                 SportCountry _sport = dba.SportCountries.Where(w => w.Country == name).SingleOrDefault();

                 return _sport;
             }
         }
        public bool Delete(SportCountry sport)
         {
             return false;
         }
         public bool Insert(SportCountry country)
         {
             using (var dba = new BetEXDataContainer())
             {
                 SportCountry _country = SportCountry(country.ID, country.SportID);
                 if (_country==null)
                 {
                     dba.AddToSportCountries(country);
                     dba.SaveChanges();
                 }else{
                     Update(country);
                 }
                
                 return true;
             }
         }
         public bool Update(SportCountry country)
         {
             using (var dba = new BetEXDataContainer())
             {
                 var _country = dba.SportCountries.Where(w => w.ID == country.ID & w.SportID == country.SportID).SingleOrDefault();
                 if (_country != null)
                 {

                     _country.Country = country.Country;
                     _country.ID = country.ID;
                     _country.Goalserve_LivescoreFeed = country.Goalserve_LivescoreFeed;
                     _country.Goalserve_OddsFeed = country.Goalserve_OddsFeed;
                  
                     _country.International = country.International;

                     dba.SaveChanges();
                     return true;
                 }
             }
             return false;
         }
      

    }
}
