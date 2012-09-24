using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using BetEx247.Core.XMLObjects.Sport.Interface;
using BetEx247.Core.XMLObjects.Sport;
using BetEx247.Core.XMLObjects.Match.Interface;
using BetEx247.Core.XMLObjects.Match;
using BetEx247.Core.XMLObjects.League.Interface;
using BetEx247.Core.XMLObjects.League;
using BetEx247.Core.XMLObjects.Match.Period.Interface;
using BetEx247.Core.XMLObjects.Match.Period;
using BetEx247.Core.XMLObjects.Match.Period.MoneyLine.Interface;
using BetEx247.Core.XMLObjects.Match.Period.MoneyLine;
using BetEx247.Core.XMLObjects.Match.Period.Spread.Interface;
using BetEx247.Core.XMLObjects.Match.Period.Spread;
using BetEx247.Core.XMLObjects.Match.Period.Total.Interface;
using BetEx247.Core.XMLObjects.Match.Period.Total;

using BetEx247.Core;
//using BetEx247.Core.XML;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;

namespace BetEx247.Plugin.XMLParser
{

    /// <summary>
    /// This classs is a container to store all data object of sports and their details.
    /// </summary>
    public class XMLObjectsManager : IGeneric
    {
        private DateTime fdTime;
        private List<ISport> sports;
        private Constant.SourceFeedType sourceFeed;
        /// <summary>
        /// Init without paramater
        /// </summary>
        public XMLObjectsManager() { }
        /// <summary>
        /// Init with only Time of XML feed
        /// </summary>
        /// <param name="fdTime"></param>
        XMLObjectsManager(DateTime fdTime)
        {
            FDTime = fdTime;
        }
        /// <summary>
        /// Init with Time and Sports
        /// </summary>
        /// <param name="fdTime"></param>
        /// <param name="sports"></param>
        XMLObjectsManager(DateTime fdTime, List<ISport> sports)
        {
            FDTime = fdTime;
            Sports = sports;
        }
        /// <summary>
        /// Init with Time, Sport and Which Feed of xml
        /// </summary>
        /// <param name="fdTime"></param>
        /// <param name="sports"></param>
        /// <param name="sourceFeed"></param>
        XMLObjectsManager(DateTime fdTime, List<ISport> sports, Constant.SourceFeedType sourceFeed)
        {
            FDTime = fdTime;
            Sports = sports;
            SourceFeed = sourceFeed;
        }

        /// <summary>
        /// Time of the XML feed
        /// </summary>
        public DateTime FDTime
        {
            set { fdTime = value; }
            get { return fdTime; }
        }
        /// <summary>
        /// All sports
        /// </summary>
        public List<ISport> Sports
        {
            set { sports = value; }
            get { return sports; }
        }
        /// <summary>
        /// Which XML feed Url where we get data
        /// </summary>
        public Constant.SourceFeedType SourceFeed
        {
            set { sourceFeed = value; }
            get { return sourceFeed; }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Parse()
        {
            Sports = XMLParse();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<ISport> XMLParse()
        {
            List<ISport> sport = new List<ISport>();
            string urlPathSport = string.Empty;
            string urlPathLeague = string.Empty;
            string urlPathFeed = string.Empty;

            urlPathSport = Constant.SourceXML.PINNACLESPORTSURL;
            if (urlPathSport.IndexOf("http://") > -1)
            {
                urlPathLeague = Constant.SourceXML.PINNACLELEAGUEURL;
                urlPathFeed = Constant.SourceXML.PINNACLEFEEDURL;
            }
            else
            {
                string[] arrPlitUrl = urlPathSport.Split('_');
                string timetick = arrPlitUrl[2].Replace(".xml", "");
                urlPathLeague = arrPlitUrl[0] + "_league_{0}_" + timetick + ".xml";
                urlPathFeed = arrPlitUrl[0] + "_feed_{0}_{1}_" + timetick + ".xml";
            }

            //  _lstSport = new List<Sport>();
            //_lstEvent = new List<Event>();
            // _lstMatch = new List<Match>();
            // _lstBet = new List<Bet>();
            // _lstChoice = new List<Choice>();
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
                // int _sportId = 0;
                // int _leagueId = 0;
                // int _matchId = 0;
                //long _betId = 0;
                // long _choiceId = 0;
                // Loop all sports
                while (iteratorSport.MoveNext())
                {
                    XPathNavigator _sportNameNavigator = iteratorSport.Current.Clone();
                    int feedContentSport = Convert.ToInt32(_sportNameNavigator.GetAttribute("feedContents", ""));
                    if (feedContentSport > 0)
                    {
                        //_sportId = Convert.ToInt32(_sportNameNavigator.GetAttribute("id", ""));
                        ISport _sport = new Sport();
                        _sport.ID = Convert.ToInt32(_sportNameNavigator.GetAttribute("id", ""));
                        _sport.Name = _sportNameNavigator.Value;
                        //Add sport to List
                        // sports.Add(_sport);
                        //league- event
                        XmlTextReader readerLeague = new XmlTextReader(string.Format(urlPathLeague, _sport.ID));
                        readerLeague.WhitespaceHandling = WhitespaceHandling.Significant;
                        XPathDocument docLeague = new XPathDocument(readerLeague, XmlSpace.Preserve);
                        XPathNavigator navLeague = docLeague.CreateNavigator();

                        XPathExpression exprLeague;
                        exprLeague = navLeague.Compile("/rsp/leagues/league");
                        XPathNodeIterator iteratorLeague = navLeague.Select(exprLeague);
                        // Loop all Leagues in each sport
                        while (iteratorLeague.MoveNext())
                        {
                            XPathNavigator _eventNameNavigator = iteratorLeague.Current.Clone();
                            int feedContentLeague = Convert.ToInt32(_sportNameNavigator.GetAttribute("feedContents", ""));
                            if (feedContentLeague > 0)
                            {
                                ILeague _league = new League();
                                _league.ID = Convert.ToInt32(_eventNameNavigator.GetAttribute("id", ""));
                                _league.Name = _eventNameNavigator.Value;
                                //Add the current League to the latest sport
                                sports[sports.Count - 1].Leagues.Add(_league);

                                //match - home team - awayteam  
                                XmlTextReader readerMatch = new XmlTextReader(string.Format(urlPathFeed, _sport.ID, _league.ID));
                                readerMatch.WhitespaceHandling = WhitespaceHandling.Significant;
                                XPathDocument docMatch = new XPathDocument(readerMatch, XmlSpace.Preserve);
                                XPathNavigator navMatch = docMatch.CreateNavigator();

                                XPathExpression exprematch;
                                exprematch = navMatch.Compile("/rsp/fd/sports/sport/leagues/league");
                                XPathNodeIterator iteratorMatch = navMatch.Select(exprematch);
                                // Loop in each League
                                while (iteratorMatch.MoveNext())
                                {
                                    XPathNavigator _matchNameNavigator = iteratorMatch.Current.Clone();

                                    XPathExpression exprematchEvent;
                                    exprematchEvent = _matchNameNavigator.Compile("events/event");
                                    XPathNodeIterator iteratorMatchEvent = _matchNameNavigator.Select(exprematchEvent);
                                    while (iteratorMatchEvent.MoveNext())
                                    {
                                        //  _matchId++;
                                        XPathNavigator _matchEventNameNavigator = iteratorMatchEvent.Current.Clone();
                                        IMatch _match = new Match();
                                        _match.ID = Convert.ToInt32(_matchEventNameNavigator.GetAttribute("id", "")); ;
                                        //_match.eventId = _eventId;
                                        //_match.nameMatch = _matchNameNavigator.GetAttribute("name", "");
                                        _match.HomeTeam = _matchEventNameNavigator.SelectSingleNode("homeTeam").SelectSingleNode("name").Value;
                                        _match.AwayTeam = _matchEventNameNavigator.SelectSingleNode("awayTeam").SelectSingleNode("name").Value;
                                        _match.StartDateTime = Convert.ToDateTime(_matchEventNameNavigator.SelectSingleNode("startDateTime").Value);
                                        // Add the match into current League


                                        // Loop all in Periods
                                        if (_matchEventNameNavigator.HasChildren)
                                        {
                                            XPathExpression exprebet;
                                            exprebet = _matchEventNameNavigator.Compile("periods/period");
                                            XPathNodeIterator iteratorBet = _matchEventNameNavigator.Select(exprebet);
                                            //Loop each period
                                            while (iteratorBet.MoveNext())
                                            {
                                                //  _betId++;
                                                XPathNavigator _betNameNavigator = iteratorBet.Current.Clone();
                                                IPeriod _period = new Period();
                                                _period.Description = _betNameNavigator.SelectSingleNode("description").Value; ;

                                                // Add period to Match

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
                                                    XPathNavigator _choiceNameNavigator = iteratorHandicap.Current.Clone();
                                                    ISpread _spread = new Spread();
                                                    _spread.AwaySpread = float.Parse(_choiceNameNavigator.SelectSingleNode("awaySpread").Value);
                                                    _spread.AwayPrice = float.Parse(_choiceNameNavigator.SelectSingleNode("awayPrice").Value);
                                                    _spread.HomeSpread = float.Parse(_choiceNameNavigator.SelectSingleNode("homeSpread").Value);
                                                    _spread.HomePrice = float.Parse(_choiceNameNavigator.SelectSingleNode("homePrice").Value);
                                                    _period.Spreads.Add(_spread);
                                                }

                                                while (iteratorTotal.MoveNext())
                                                {
                                                    XPathNavigator _choiceNameNavigator = iteratorTotal.Current.Clone();
                                                    ITotal _total = new Total();
                                                    _total.Point = float.Parse(_choiceNameNavigator.SelectSingleNode("points").Value);
                                                    _total.OverPrice = float.Parse(_choiceNameNavigator.SelectSingleNode("overPrice").Value);
                                                    _total.UnderPrice = float.Parse(_choiceNameNavigator.SelectSingleNode("underPrice").Value);
                                                    _period.Totals.Add(_total);

                                                }

                                                while (iteratorMoneyLine.MoveNext())
                                                {
                                                    XPathNavigator _choiceNameNavigator = iteratorMoneyLine.Current.Clone();
                                                    IMoneyLine _moneyline = new MoneyLine();

                                                    _moneyline.AwayPrice = float.Parse(_choiceNameNavigator.SelectSingleNode("awayPrice").Value);
                                                    _moneyline.HomePrice = float.Parse(_choiceNameNavigator.SelectSingleNode("homePrice").Value);
                                                    _moneyline.DrawPrice = float.Parse(_choiceNameNavigator.SelectSingleNode("drawPrice").Value);
                                                    _period.MoneyLines.Add(_moneyline);
                                                }

                                                _match.Periods.Add(_period);
                                            }

                                            _league.Matches.Add(_match);

                                        }


                                    }


                                }

                                _sport.Leagues.Add(_league);
                            }


                        }
                        sport.Add(_sport);

                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                sports = null;
            }
            return sport;
        }

        public XMLObjectsManager(SerializationInfo info, StreamingContext ctxt)
        {
            //this.make = (string)info.GetValue("Make", typeof(string));
            //this.model = (string)info.GetValue("Model",typeof(string));
            //this.year = (string)info.GetValue("Year", typeof(int));
            //this.owner = (Owner)info.GetValue("Owner", typeof(Owner));
            this.Sports = (List<ISport>)info.GetValue("Sports", typeof(List<ISport>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //info.AddValue("Make", this.make);
            //info.AddValue("Model", this.model);
            //info.AddValue("Make", this.year);
            //info.AddValue("Owner", this.owner);
            info.AddValue("Sports", this.Sports);
        }
    }

}
 


