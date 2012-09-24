using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XMLObjects.Market.Outcome.Interface;

namespace BetEx247.Core.XMLObjects.Market.Interface
{
    public interface IMarket
    {
        int ID { get; set; }
        String Name { get; set; }
        DateTime StartDateTime { get; set; }
        List<IOutcome> Outcomes { get; set; }
    }
}
