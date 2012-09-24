using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.XMLObjects.Match.Period.MoneyLine.Interface
{
    public interface IMoneyLine
    {
       float AwayPrice { get; set; }
       float HomePrice { get; set; }
       float DrawPrice { get; set; }
    }
}
