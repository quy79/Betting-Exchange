using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports.Interfaces
{
    public partial interface ISportAsianHandicapService : IBase<Sports_AsianHandicap>   
    {
        List<Sports_AsianHandicap> SportsAsianHandicaps();
        List<Sports_AsianHandicap> SportsAsianHandicaps(int matchID);
        Sports_AsianHandicap SportsAsianHandicap(int ID);
       // bool Insert(Sports_AsianHandicap sportsAsianHandicap);
        //bool Update(Sports_AsianHandicap sportsAsianHandicap);
        //bool Delete(Sports_AsianHandicap sportsAsianHandicap);
    }
}
