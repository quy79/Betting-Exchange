using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports.Interfaces
{
    public partial interface ISoccerCorrectScoresService : IBase<Soccer_CorrectScores>   
    {
        List<Soccer_CorrectScores> SoccerCorrectScoreses();
        List<Soccer_CorrectScores> SoccerCorrectScoreses(long matchID);
        Soccer_CorrectScores SoccerCorrectScores(int ID);
       // bool Insert(Soccer_CorrectScores soccerCorrectScores);
       // bool Update(Soccer_CorrectScores soccerCorrectScores);
       // bool Delete(Soccer_CorrectScores soccerCorrectScores);
    }
}
