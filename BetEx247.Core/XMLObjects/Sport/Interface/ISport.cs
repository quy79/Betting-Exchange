using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using BetEx247.Core.XMLObjects.League.Interface;
using BetEx247.Core;

namespace BetEx247.Core.XMLObjects.Sport.Interface
{
    public interface ISport 
    {
        int ID { get; set; }
        String Name { get; set; }
        List<ILeague> Leagues { get; set; }
        Constant.SourceFeedType SportFeedType { get; set; }
        // type of sport
    }
}
