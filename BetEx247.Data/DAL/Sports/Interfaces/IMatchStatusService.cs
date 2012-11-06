using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports.Interfaces
{
    public partial interface IMatchStatusService : IBase<Sport>   
    {
        List<MatchStatu> MatchStatues();
        MatchStatu MatchStatus(int ID);
        MatchStatu MatchStatus(String name);

    }
}
