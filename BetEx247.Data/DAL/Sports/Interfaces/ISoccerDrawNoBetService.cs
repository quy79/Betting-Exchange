using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports.Interfaces
{
    public partial interface ISoccerDrawNoBetService : IBase<Soccer_DrawNoBet>   
    
    {
        List<Soccer_DrawNoBet> SoccerDrawNoBets();
        List<Soccer_DrawNoBet> SoccerDrawNoBets(long matchID);
        Soccer_DrawNoBet SoccerDrawNoBet(long  ID);
        //bool Insert(Soccer_DrawNoBet soccerDrawNoBet);
        //bool Update(Soccer_DrawNoBet soccerDrawNoBet);
        //bool Delete(Soccer_DrawNoBet soccerDrawNoBet);
    }
}
