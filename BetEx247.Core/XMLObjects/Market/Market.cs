using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XMLObjects.Market.Interface;
using BetEx247.Core.XMLObjects.Market.Outcome.Interface;

namespace BetEx247.Core.XMLObjects.Market
{
     [Serializable]
    public class Market:IMarket
    {
         private int id = 0;
        private string name = string.Empty;
        private List<IOutcome> outcomes;
        private DateTime startDateTime;
        public Market()
        {
        }
         public int ID
        {
            get { return id; }
            set { id = value; }
        }
         public String Name
        {
            get { return name; }
            set { name = value; }
        }
         public List<IOutcome> Outcomes
        {
            get { return outcomes; }
            set { outcomes = value; }
        }
         public DateTime StartDateTime
         {
             get { return startDateTime; }
             set { startDateTime = value; }
         }

    }
}
