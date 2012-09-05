using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XML;
using System.Xml;
using System.Xml.XPath;
using BetEx247.Core;
using BetEx247.Core.Common.Extensions;

namespace BetEx247.Plugin.XMLParser
{
    public class CachefeedsParser:IXMLParser
    {
         public static List<Sport> _lstSport;
        public static List<Event> _lstEvent;
        public static List<Match> _lstMatch;
        public static List<Bet> _lstBet;
        public static List<Choice> _lstChoice;

        public CachefeedsParser()
        {
            ReadXML();
        }

        public virtual void ReadXML()
        {
            string urlPath = Constant.SourceXML.TITABETURL;
            _lstSport = new List<Sport>();
            _lstEvent = new List<Event>();
            _lstMatch = new List<Match>();
            _lstBet = new List<Bet>();
            _lstChoice = new List<Choice>();

            XmlTextReader reader = new XmlTextReader(urlPath);
            // Skip non-significant whitespace  
            reader.WhitespaceHandling = WhitespaceHandling.Significant;
            XPathDocument doc = new XPathDocument(reader, XmlSpace.Preserve);
            XPathNavigator nav = doc.CreateNavigator();

            XPathExpression exprSport;
            exprSport = nav.Compile("/bookmaker/sport");
            XPathNodeIterator iteratorSport = nav.Select(exprSport);
            try
            {
                int _sportId = 0;
                int _eventId = 0;
                long _matchId = 0;
                long _betId = 0;
                long _choiceId = 0;

                while (iteratorSport.MoveNext())
                {
                    _sportId++;
                    XPathNavigator _sportNameNavigator = iteratorSport.Current.Clone();
                    Sport _sport = new Sport();
                    _sport.sportId = _sportId;
                    _sport.sportName = _sportNameNavigator.GetAttribute("name", "");
                    _lstSport.Add(_sport);

                    if (_sportNameNavigator.HasChildren)
                    {
                        XPathExpression exprevent;
                        exprevent = _sportNameNavigator.Compile("group");
                        XPathNodeIterator iteratorEvent = _sportNameNavigator.Select(exprevent);
                        while (iteratorEvent.MoveNext())
                        {
                            _eventId++;
                            XPathNavigator _eventNameNavigator = iteratorEvent.Current.Clone();
                            Event _event = new Event();
                            _event.eventId = _eventId;
                            _event.sportId = _sportId;
                            _event.eventName = _eventNameNavigator.GetAttribute("name", "");
                            _lstEvent.Add(_event);

                            if (_eventNameNavigator.HasChildren)
                            {
                                XPathExpression exprematch;
                                exprematch = _eventNameNavigator.Compile("event");
                                XPathNodeIterator iteratorMatch = _eventNameNavigator.Select(exprematch);
                                while (iteratorMatch.MoveNext())
                                {
                                    _matchId++;
                                    XPathNavigator _matchNameNavigator = iteratorMatch.Current.Clone();
                                    Match _match = new Match();
                                    _match.matchId = _matchId;
                                    _match.eventId = _eventId;
                                    string[] league = _matchNameNavigator.GetAttribute("name", "").Split('-');
                                    if (league.Length > 1)
                                    {
                                        _match.homeTeam = league[0].Trim();
                                        _match.awayTeam = league[1].Trim();
                                    }
                                    else
                                    {
                                        _match.homeTeam = _matchNameNavigator.GetAttribute("name", "");
                                    }
                                    _match.startTime = Convert.ToDateTime(_matchNameNavigator.GetAttribute("date", ""));
                                    _lstMatch.Add(_match);

                                    if (_matchNameNavigator.HasChildren)
                                    {
                                        XPathExpression exprebet;
                                        exprebet = _matchNameNavigator.Compile("market");
                                        XPathNodeIterator iteratorBet = _matchNameNavigator.Select(exprebet);
                                        while (iteratorBet.MoveNext())
                                        {
                                            _betId++;
                                            XPathNavigator _betNameNavigator = iteratorBet.Current.Clone();
                                            Bet _bet = new Bet();
                                            _bet.betId = _betId;
                                            _bet.matchId = _matchId;
                                            _bet.betName = _betNameNavigator.GetAttribute("name", "");
                                            //_bet.betCodeName = _betNameNavigator.GetAttribute("tid", "");   
                                            long siteCodeId = _betNameNavigator.GetAttribute("tid", "").ToInt64();

                                            int[] vals = (int[])Enum.GetValues(typeof(Constant.BetType));
                                            string[] names = Enum.GetNames(typeof(Constant.BetType));

                                            switch (siteCodeId)
                                            {
                                                case 154:
                                                    _bet.betCodeID = vals[0];
                                                    _bet.betCodeName = names[0];
                                                    break;
                                                case 1:
                                                    _bet.betCodeID = vals[1];
                                                    _bet.betCodeName = names[1];
                                                    break;
                                                case 67:
                                                    _bet.betCodeID = vals[2];
                                                    _bet.betCodeName = names[2];
                                                    break;
                                                case 120:
                                                    _bet.betCodeID = vals[3];
                                                    _bet.betCodeName = names[3];
                                                    break;
                                                case 5:
                                                    _bet.betCodeID = vals[4];
                                                    _bet.betCodeName = names[4];
                                                    break;
                                                case 2:
                                                    _bet.betCodeID = vals[5];
                                                    _bet.betCodeName = names[5];
                                                    break;
                                                case 6:
                                                    _bet.betCodeID = vals[6];
                                                    _bet.betCodeName = names[6];
                                                    break;
                                                case 277:
                                                    _bet.betCodeID = vals[7];
                                                    _bet.betCodeName = names[7];
                                                    break;
                                                case 278:
                                                    _bet.betCodeID = vals[8];
                                                    _bet.betCodeName = names[8];
                                                    break;
                                                case 28:
                                                    _bet.betCodeID = vals[9];
                                                    _bet.betCodeName = names[9];
                                                    break;
                                                case 78:
                                                    _bet.betCodeID = vals[10];
                                                    _bet.betCodeName = names[10];
                                                    break;
                                                case 39:
                                                    _bet.betCodeID = vals[11];
                                                    _bet.betCodeName = names[11];
                                                    break;
                                                case 269:
                                                    _bet.betCodeID = vals[12];
                                                    _bet.betCodeName = names[12];
                                                    break;
                                                case 439:
                                                    _bet.betCodeID = vals[13];
                                                    _bet.betCodeName = names[13];
                                                    break;
                                                case 155:
                                                    _bet.betCodeID = vals[14];
                                                    _bet.betCodeName = names[14];
                                                    break;
                                                case 433:
                                                    _bet.betCodeID = vals[15];
                                                    _bet.betCodeName = names[15];
                                                    break;
                                                case 434:
                                                    _bet.betCodeID = vals[16];
                                                    _bet.betCodeName = names[16];
                                                    break;
                                                case 463:
                                                    _bet.betCodeID = vals[17];
                                                    _bet.betCodeName = names[17];
                                                    break;
                                                case 464:
                                                    _bet.betCodeID = vals[18];
                                                    _bet.betCodeName = names[18];
                                                    break;
                                                case 37:
                                                    _bet.betCodeID = vals[19];
                                                    _bet.betCodeName = names[19];
                                                    break;
                                                case 5000:
                                                    _bet.betCodeID = vals[20];
                                                    _bet.betCodeName = names[20];
                                                    break;
                                                case 5001:
                                                    _bet.betCodeID = vals[21];
                                                    _bet.betCodeName = names[21];
                                                    break;
                                                default:
                                                    break;
                                            }    
                                            
                                            _lstBet.Add(_bet);

                                            if (_betNameNavigator.HasChildren)
                                            {
                                                XPathExpression exprechoice;
                                                exprechoice = _betNameNavigator.Compile("outcome");
                                                XPathNodeIterator iteratorChoice = _betNameNavigator.Select(exprechoice);
                                                while (iteratorChoice.MoveNext())
                                                {
                                                    _choiceId++;
                                                    XPathNavigator _choiceNameNavigator = iteratorChoice.Current.Clone();
                                                    Choice _choice = new Choice();
                                                    _choice.choiceId = _choiceId;
                                                    _choice.betId = _betId;
                                                    _choice.choiceCodeId = _bet.betCodeID;
                                                    _choice.choiceCodeName = _bet.betCodeName;
                                                    _choice.choiceName = _choiceNameNavigator.GetAttribute("name", "");
                                                    _choice.odd = _choiceNameNavigator.GetAttribute("odd", "");
                                                    _choice.american_odd = _choiceNameNavigator.GetAttribute("american_odds", "");
                                                    _choice.fra_odd = _choiceNameNavigator.GetAttribute("fra_odds", "");
                                                    _lstChoice.Add(_choice);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Sport> getAllSport()
        {
            return _lstSport; 
        }

        public List<Event> getAllEvent()
        {
            return _lstEvent;
        }

        public List<Match> getAllMatch()
        {
            return _lstMatch;
        }

        public List<Bet> getAllBet()
        {
            return _lstBet;
        }

        public List<Choice> getAllChoice()
        {
            return _lstChoice;
        }
    }
}
