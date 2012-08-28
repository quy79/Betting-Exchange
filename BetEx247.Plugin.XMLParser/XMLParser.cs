using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XML;
using BetEx247.Core;

namespace BetEx247.Plugin.XMLParser
{
    public class XMLParser
    {               
        IXMLParser parser;
        public XMLParser(string parserSource)
        {
            switch (parserSource)
            {
                case Constant.SourceXML.BETCLICK:
                    parser = new BetclickParser();
                    break;
                case Constant.SourceXML.PINNACLESPORTS:
                    parser = new PinnacleSportsParser();
                    break;
                case Constant.SourceXML.TITANBET:
                    parser = new CachefeedsParser();
                    break;
                default:
                    parser = new CachefeedsParser();
                    break;
            }
            parser =new CachefeedsParser();
        }
       
        public List<Sport> getAllSport()
        {
            return parser.getAllSport();
        }
            
        public List<Event> getAllEvent()
        {
            return parser.getAllEvent();
        }

        public List<Match> getAllMatch()
        {
            return parser.getAllMatch();
        }
    
        public  List<Bet> getAllBet()
        {
            return parser.getAllBet();
        }

        public List<Choice> getAllChoice()
        {
            return parser.getAllChoice();
        } 
    }
}
