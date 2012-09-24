using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core;
using BetEx247.Core.XMLObjects.Match.Interface;
using BetEx247.Core.XMLObjects.Market.Interface;
namespace BetEx247.Core.XMLObjects.League.Interface
{
    public interface ILeague
    {
        Constant.SourceFeedType SourceFeedType { get; set; }
        int ID { get; set; }
        String Name { get; set; }
        List<IMatch> Matches { get; set; }
       // List<IMarket> Markets { get; set; }
    }
}
