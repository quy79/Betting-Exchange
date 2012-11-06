using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports.Interfaces
{
    public partial interface ISoccerWinningMarginService : IBase<Soccer_WinningMargin>   
    {
        List<Soccer_WinningMargin> SoccerWinningMargins();
        List<Soccer_WinningMargin> SoccerWinningMargins(int matchID);
        Soccer_WinningMargin SoccerWinningMargin(int ID);
        //bool Insert(Soccer_WinningMargin soccerWinningMargin);
        //bool Update(Soccer_WinningMargin soccerWinningMargin);
        //bool Delete(Soccer_WinningMargin soccerWinningMargin);
    }
}
