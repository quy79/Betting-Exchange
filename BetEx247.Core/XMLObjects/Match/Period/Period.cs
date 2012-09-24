using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XMLObjects.Match.Period.Interface;
using BetEx247.Core.XMLObjects.Match.Period.Spread.Interface;
using BetEx247.Core.XMLObjects.Match.Period.Total.Interface;
using BetEx247.Core.XMLObjects.Match.Period.MoneyLine.Interface;

namespace BetEx247.Core.XMLObjects.Match.Period
{
    /// <summary>
    /// Implement Period class
    /// </summary>
    /// 
     [Serializable]
   public class Period:IPeriod
    {
       private float number;
       private String description;
       private DateTime cutoffDateTime;
       private List<ISpread> spreads;
       private List<ITotal> totals;
       private List<IMoneyLine> moneyLines;

       public Period()
       {
       }
        public float Number
        {
            set { number = value; }
            get { return number; }
        }
        public String Description
        {
            set { description = value; }
            get { return description; }
        }
        public DateTime CutoffDateTime
        {
            set { cutoffDateTime = value; }
            get { return cutoffDateTime; }
        }
        public List<ISpread> Spreads
        {
            set { spreads = value; }
            get { return spreads; }
        }
        public List<ITotal> Totals
        {
            set { totals = value; }
            get { return totals; }
        }
        public List<IMoneyLine> MoneyLines
        {
            set { moneyLines = value; }
            get { return moneyLines; }
        }
    }
}
