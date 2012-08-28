using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XML;
using System.Xml;
using System.Xml.XPath;

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
            string urlPath = "D:/Project/PN Technologies/BetEx247/BetEx247.Web/App_Data/feed.xml";
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
                                            _bet.betCode = _betNameNavigator.GetAttribute("tid", "");
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
