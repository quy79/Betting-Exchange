using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports.Interfaces
{
    public partial interface ISoccerMatchService : IBase<SoccerMatch>   
    {
        List<SoccerMatch> SoccerMatches();
        List<SoccerMatch> SoccerMatches(int LeagueID);
        SoccerMatch SoccerMatch(int ID);
        SoccerMatch SoccerMatch(long leagueID, String homeTeam, String awayTeam, DateTime startDate, DateTime startTime);
        //bool Insert(SoccerMatch soccerMatch);
        //bool Update(SoccerMatch soccerMatch);
        //bool Delete(SoccerMatch soccerMatch);
    }
}
