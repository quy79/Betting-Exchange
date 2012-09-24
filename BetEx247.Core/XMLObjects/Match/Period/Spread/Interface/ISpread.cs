using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.XMLObjects.Match.Period.Spread.Interface
{
   public  interface ISpread
    {
       float AwaySpread { get; set; }
       float AwayPrice { get; set; }
       float HomeSpread { get; set; }
       float HomePrice { get; set; }


       
    }
}
