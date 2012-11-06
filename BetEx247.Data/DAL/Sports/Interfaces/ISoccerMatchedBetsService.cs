using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports.Interfaces
{
    public partial interface ISoccerMatchedBetsService : IBase<Soccer_MatchedBets>   
    {
        List<Soccer_MatchedBets> SoccerMatchedBetses();
        List<Soccer_MatchedBets> SoccerMatchedBetses(int matchID);
        Soccer_MatchedBets SoccerMatchedBets(int ID);
        //bool Insert(Soccer_MatchedBets soccerMatchedBets);
        //bool Update(Soccer_MatchedBets soccerMatchedBets);
        //bool Delete(Soccer_MatchedBets soccerMatchedBets);
    }
}
