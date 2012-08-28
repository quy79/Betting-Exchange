using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XML;

namespace BetEx247.Plugin.XMLParser
{
    public interface IXMLParser
    {
        void ReadXML();
        List<Sport> getAllSport(); 
        List<Event> getAllEvent();
        List<Match> getAllMatch();
        List<Bet> getAllBet();
        List<Choice> getAllChoice();
    }
}
