using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XMLObjects.Match.Period.Interface;
using BetEx247.Core.XMLObjects.Market.Interface;
namespace BetEx247.Core.XMLObjects.Match.Interface
{
    public interface IMatch
    {
        int ID { get; set; }
        DateTime StartDateTime { get; set; }
        String Name { get; set; }
        bool LiveID { get; set; }
        String Status { get; set; }
        String HomeTeam { get; set; }
        String AwayTeam { get; set; }
        List<IPeriod> Periods { get; set; }
        List<IMarket> Markets { get; set; }
    }
}
