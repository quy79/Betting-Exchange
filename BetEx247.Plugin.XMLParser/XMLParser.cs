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
        }
       
        public List<Sport> AllSport
        {
            get { return parser.getAllSport(); }
        }
            
        public List<Event> AllEvent
        {
            get { return parser.getAllEvent(); }
        }

        public List<Match> AllMatch
        {
           get{ return parser.getAllMatch();}
        }
    
        public  List<Bet> AllBet
        {
            get{return parser.getAllBet();}
        }

        public List<Choice> AllChoice
        {
            get { return parser.getAllChoice(); }
        } 
    }
}
