using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XMLObjects.Match.Interface;

namespace BetEx247.Core.XMLObjects.League.Interface
{
    public interface ILeague
    {
        int ID { get; set; }
        String Name { get; set; }
        List<IMatch> Matches { get; set; }
    }
}
