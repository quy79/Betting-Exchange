using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.XMLObjects.Market.Outcome.Interface
{
   public  interface IOutcome
    {
        int ID { get; set; }
        String Name { get; set; }
        float Odds { get; set; }
    }
}
