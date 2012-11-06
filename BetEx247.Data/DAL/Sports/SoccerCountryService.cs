using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.DAL.Sports.Interfaces;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports
{
    public partial class SoccerCountryService : ISoccerCountryService
    {
        public List<SoccerCountry> SoccerCountries()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.SoccerCountries.ToList();

                 return list;
             }
         }
        public SoccerCountry SoccerCountry(int ID)
         {
             using (var dba = new BetEXDataContainer())
             {
                 SoccerCountry _sport = dba.SoccerCountries.Where(w => w.ID == ID).SingleOrDefault();

                 return _sport;
             }
         }
        public SoccerCountry SoccerCountry(String name)
         {
             using (var dba = new BetEXDataContainer())
             {
                 SoccerCountry _sport = dba.SoccerCountries.Where(w => w.Country == name).SingleOrDefault();

                 return _sport;
             }
         }
        public bool Delete(SoccerCountry sport)
         {
             return false;
         }
         public bool Insert(SoccerCountry country)
         {
             using (var dba = new BetEXDataContainer())
             {
                 SoccerCountry _country = SoccerCountry(country.ID);
                 if (_country==null)
                 {
                     dba.AddToSoccerCountries(country);
                     dba.SaveChanges();
                 }else{
                     Update(country);
                 }
                
                 return true;
             }
         }
         public bool Update(SoccerCountry country)
         {
             using (var dba = new BetEXDataContainer())
             {
                 var _country = dba.SoccerCountries.Where(w => w.ID == country.ID).SingleOrDefault();
                 if (_country != null)
                 {

                     _country.Country = country.Country;
                     _country.ID = country.ID;
                     _country.Goalserve_LivescoreFeed = country.Goalserve_LivescoreFeed;
                     _country.Goalserve_OddsFeed = country.Goalserve_OddsFeed;
                     _country.Betclick_OddsFeed = country.Betclick_OddsFeed;
                     _country.International = country.International;

                     dba.SaveChanges();
                     return true;
                 }
             }
             return false;
         }
         public IQueryable<SoccerCountry> Table
         {
             get { throw new NotImplementedException(); }
         }

         public IList<SoccerCountry> GetAll()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.SoccerCountries.ToList();

                 return list;
             }
         }

         public SoccerCountry SoccerCountry()
         {
             throw new NotImplementedException();
         }

         IList<Country> IBase<Country>.GetAll()
         {
             throw new NotImplementedException();
         }

         public void Insert(Country entity)
         {
             throw new NotImplementedException();
         }

         public bool Update(Country entity)
         {
             throw new NotImplementedException();
         }

         public bool Delete(Country entity)
         {
             throw new NotImplementedException();
         }

         IQueryable<Country> IBase<Country>.Table
         {
             get { throw new NotImplementedException(); }
         }
    }
}
