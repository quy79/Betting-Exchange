using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports.Interfaces
{
    public partial interface ISoccerMatchOddsService : IBase<Soccer_MatchOdds>   
    {
        List<Soccer_MatchOdds> SoccerMatchOddses();
        List<Soccer_MatchOdds> SoccerMatchOddses(long matchID);
        Soccer_MatchOdds SoccerMatchOdds(int ID);
        //bool Insert(Soccer_MatchOdds soccerMatchOdds);
        //bool Update(Soccer_MatchOdds soccerMatchOdds);
        //bool Delete(Soccer_MatchOdds soccerMatchOdds);
    }
}
