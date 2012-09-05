using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XML;
using System.Xml;
using System.Xml.XPath;
using BetEx247.Core;

namespace BetEx247.Plugin.XMLParser
{
    public class PinnacleSportsParser : IXMLParser
    {
        public static List<Sport> _lstSport;
        public static List<Event> _lstEvent;
        public static List<Match> _lstMatch;
        public static List<Bet> _lstBet;
        public static List<Choice> _lstChoice;

        public PinnacleSportsParser()
        {
            ReadXML();
        }

        public virtual void ReadXML()
        {
            string urlPathSport = Constant.SourceXML.PINNACLESPORTSURL;
            string urlPathLeague = Constant.SourceXML.PINNACLELEAGUEURL;
            string urlPathFeed = Constant.SourceXML.PINNACLEFEEDURL;
            _lstSport = new List<Sport>();
            _lstEvent = new List<Event>();
            _lstMatch = new List<Match>();
            _lstBet = new List<Bet>();
            _lstChoice = new List<Choice>();
            //sport
            XmlTextReader readerSport = new XmlTextReader(urlPathSport);
            // Skip non-significant whitespace  
            readerSport.WhitespaceHandling = WhitespaceHandling.Significant;
            XPathDocument doc = new XPathDocument(readerSport, XmlSpace.Preserve);
            XPathNavigator nav = doc.CreateNavigator();

            XPathExpression exprSport;
            exprSport = nav.Compile("/rsp/sports/sport");
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
                    XPathNavigator _sportNameNavigator = iteratorSport.Current.Clone();
                    _sportId = Convert.ToInt32(_sportNameNavigator.GetAttribute("id", ""));
                    Sport _sport = new Sport();
                    _sport.sportId = Convert.ToInt32(_sportNameNavigator.GetAttribute("id", ""));
                    _sport.sportName = _sportNameNavigator.Value;
                    _lstSport.Add(_sport);
                    //league- event
                    XmlTextReader readerLeague = new XmlTextReader(string.Format(urlPathLeague, _sportId));
                    readerLeague.WhitespaceHandling = WhitespaceHandling.Significant;
                    XPathDocument docLeague = new XPathDocument(readerLeague, XmlSpace.Preserve);
                    XPathNavigator navLeague = docLeague.CreateNavigator();

                    XPathExpression exprLeague;
                    exprLeague = navLeague.Compile("/rsp/leagues/league");
                    XPathNodeIterator iteratorLeague = navLeague.Select(exprLeague);

                    while (iteratorLeague.MoveNext())
                    {
                        XPathNavigator _eventNameNavigator = iteratorLeague.Current.Clone();
                        Event _event = new Event();
                        _eventId = Convert.ToInt32(_eventNameNavigator.GetAttribute("id", ""));
                        _event.eventId = _eventId;
                        _event.sportId = _sportId;
                        _event.eventName = _eventNameNavigator.Value;
                        _lstEvent.Add(_event);
                        //match - home team - awayteam  
                        XmlTextReader readerMatch = new XmlTextReader(string.Format(urlPathFeed, _sportId, _eventId));
                        readerMatch.WhitespaceHandling = WhitespaceHandling.Significant;
                        XPathDocument docMatch = new XPathDocument(readerMatch, XmlSpace.Preserve);
                        XPathNavigator navMatch = docMatch.CreateNavigator();

                        XPathExpression exprematch;
                        exprematch = navMatch.Compile("/rsp/fd/sports/sport/leagues/league");
                        XPathNodeIterator iteratorMatch = navMatch.Select(exprematch);
                        while (iteratorMatch.MoveNext())
                        {
                            XPathNavigator _matchNameNavigator = iteratorMatch.Current.Clone();

                            XPathExpression exprematchEvent;
                            exprematchEvent = _matchNameNavigator.Compile("events/event");
                            XPathNodeIterator iteratorMatchEvent = _matchNameNavigator.Select(exprematchEvent);
                            while (iteratorMatchEvent.MoveNext())
                            {
                                _matchId++;
                                XPathNavigator _matchEventNameNavigator = iteratorMatchEvent.Current.Clone();

                                Match _match = new Match();
                                _match.matchId = _matchId;
                                _match.eventId = _eventId;
                                //_match.nameMatch = _matchNameNavigator.GetAttribute("name", "");
                                _match.homeTeam = _matchEventNameNavigator.SelectSingleNode("homeTeam").SelectSingleNode("name").Value;
                                _match.awayTeam = _matchEventNameNavigator.SelectSingleNode("awayTeam").SelectSingleNode("name").Value;
                                _match.startTime = Convert.ToDateTime(_matchEventNameNavigator.SelectSingleNode("startDateTime").Value);
                                _lstMatch.Add(_match);

                                if (_matchEventNameNavigator.HasChildren)
                                {
                                    XPathExpression exprebet;
                                    exprebet = _matchEventNameNavigator.Compile("periods/period");
                                    XPathNodeIterator iteratorBet = _matchEventNameNavigator.Select(exprebet);
                                    while (iteratorBet.MoveNext())
                                    {
                                        _betId++;
                                        XPathNavigator _betNameNavigator = iteratorBet.Current.Clone();
                                        Bet _bet = new Bet();
                                        _bet.betId = _betId;
                                        _bet.matchId = _matchId;

                                        _bet.betName = _betNameNavigator.SelectSingleNode("description").Value;
                                        if (_bet.betName == "Game")
                                        {
                                            _bet.betCodeID = (long)Constant.BetType.GAME;
                                            _bet.betCodeName = "GAME";
                                        }
                                        else
                                        {
                                            _bet.betCodeID = (long)Constant.BetType.OTHER;
                                            _bet.betCodeName = "OTHER";
                                        }
                                        _lstBet.Add(_bet);

                                        //handicap
                                        XPathExpression exprehandicap;
                                        exprehandicap = _matchEventNameNavigator.Compile("spreads/spread");
                                        XPathNodeIterator iteratorHandicap = _matchEventNameNavigator.Select(exprehandicap);
                                        //total
                                        XPathExpression expretotal;
                                        expretotal = _matchEventNameNavigator.Compile("totals/total");
                                        XPathNodeIterator iteratorTotal = _matchEventNameNavigator.Select(expretotal);
                                        //moneyline
                                        XPathExpression expremoneyline;
                                        expremoneyline = _matchEventNameNavigator.Compile("moneyLine");
                                        XPathNodeIterator iteratorMoneyLine = _matchEventNameNavigator.Select(expremoneyline);

                                        while (iteratorHandicap.MoveNext())
                                        {
                                            _choiceId++;
                                            XPathNavigator _choiceNameNavigator = iteratorHandicap.Current.Clone();
                                            Choice _choice = new Choice();
                                            _choice.choiceId = _choiceId;
                                            _choice.betId = _betId;
                                            _choice.choiceCodeId = (long)Constant.ChoiceType.HANDICAP;
                                            _choice.choiceCodeName = "HANDICAP";
                                            _choice.choiceName = "HANDICAP";
                                            _choice.awaySpread = _choiceNameNavigator.SelectSingleNode("awaySpread").Value;
                                            _choice.awayPrice = _choiceNameNavigator.SelectSingleNode("awayPrice").Value;
                                            _choice.homeSpread = _choiceNameNavigator.SelectSingleNode("homeSpread").Value;
                                            _choice.homePrice = _choiceNameNavigator.SelectSingleNode("homePrice").Value;
                                            _lstChoice.Add(_choice);
                                        }

                                        while (iteratorTotal.MoveNext())
                                        {
                                            _choiceId++;
                                            XPathNavigator _choiceNameNavigator = iteratorTotal.Current.Clone();
                                            Choice _choice = new Choice();
                                            _choice.choiceId = _choiceId;
                                            _choice.betId = _betId;
                                            _choice.choiceCodeId = (long)Constant.ChoiceType.TOTAL;
                                            _choice.choiceCodeName = "TOTAL";
                                            _choice.choiceName = "TOTAL";
                                            _choice.points = _choiceNameNavigator.SelectSingleNode("points").Value;
                                            _choice.overPrice = _choiceNameNavigator.SelectSingleNode("overPrice").Value;
                                            _choice.underPrice = _choiceNameNavigator.SelectSingleNode("underPrice").Value;
                                            _lstChoice.Add(_choice);
                                        }

                                        while (iteratorMoneyLine.MoveNext())
                                        {
                                            _choiceId++;
                                            XPathNavigator _choiceNameNavigator = iteratorMoneyLine.Current.Clone();
                                            Choice _choice = new Choice();
                                            _choice.choiceId = _choiceId;
                                            _choice.betId = _betId;
                                            _choice.choiceCodeId = (long)Constant.ChoiceType.MONEY_LINE;
                                            _choice.choiceCodeName = "MONEY_LINE";
                                            _choice.choiceName = "MONEY_LINE";
                                            _choice.awayPrice = _choiceNameNavigator.SelectSingleNode("awayPrice").Value;
                                            _choice.homePrice = _choiceNameNavigator.SelectSingleNode("homePrice").Value;
                                            _choice.drawPrice = _choiceNameNavigator.SelectSingleNode("drawPrice").Value;
                                            _lstChoice.Add(_choice);
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
                //throw new Exception(ex.Message);
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
