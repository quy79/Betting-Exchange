using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XMLObjects.Match.Period.Spread.Interface;
using BetEx247.Core.XMLObjects.Match.Period.Total.Interface;
using BetEx247.Core.XMLObjects.Match.Period.MoneyLine.Interface;

namespace BetEx247.Core.XMLObjects.Match.Period.Interface
{
    public interface IPeriod
    {
       float Number { get; set; }
       String Description { get; set; }
       DateTime CutoffDateTime { get; set; }
       List<ISpread> Spreads { get; set; }
       List<ITotal> Totals { get; set; }
       List<IMoneyLine> MoneyLines { get; set; }
        
       
    }
}
