using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports.Interfaces
{
    public partial interface ISportOutRightService : IBase<Sports_Outright>   
    {
        List<Sports_Outright> SportsOutrights();
        List<Sports_Outright> SportsOutrights(int matchID);
        Sports_Outright SportsOutright(int ID);
        //bool Insert(Sports_Outright sportsOutright);
        //bool Update(Sports_Outright sportsOutright);
        //bool Delete(Sports_Outright sportsOutright);
    }
}
