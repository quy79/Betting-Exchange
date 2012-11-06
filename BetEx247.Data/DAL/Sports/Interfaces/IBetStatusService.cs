using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports.Interfaces
{
    public partial interface IBetStatusService : IBase<Sport>   
    {
        List<BetStatu> MatchStatus();
        BetStatu BetStatus(int ID);
        BetStatu BetStatus(String name);

    }
}
