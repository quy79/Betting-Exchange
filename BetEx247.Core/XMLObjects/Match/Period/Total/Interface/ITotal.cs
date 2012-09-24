using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.XMLObjects.Match.Period.Total.Interface
{
    public interface ITotal
    {
       float Point { get; set; }
       float OverPrice { get; set; }
       float UnderPrice { get; set; }

    }
}
