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
using BetEx247.Core.XMLObjects.Market.Interface;
using BetEx247.Core.XMLObjects.Market;
using BetEx247.Core.XMLObjects.Market.Outcome.Interface;
using BetEx247.Core.XMLObjects.Market.Outcome;

using BetEx247.Core;
//using BetEx247.Core.XML;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Configuration;
using System.IO;


namespace BetEx247.Plugin.XMLParser
{

    /// <summary>
    /// This classs is a container to store all data object of sports and their details.
    /// </summary>
    /// 
    [Serializable()]
    public class XMLParserObjectManager : IGeneric
    {
        private DateTime fdTime;
        private List<ISport> sports;
        private Constant.SourceFeedType sourceFeed;
        /// <summary>
        /// Init without paramater
        /// </summary>
        public XMLParserObjectManager() { }
        /// <summary>
        /// Init with only Time of XML feed
        /// </summary>
        /// <param name="fdTime"></param>
        XMLParserObjectManager(DateTime fdTime)
        {
            FDTime = fdTime;
        }
        /// <summary>
        /// Init with Time and Sports
        /// </summary>
        /// <param name="fdTime"></param>
        /// <param name="sports"></param>
        XMLParserObjectManager(DateTime fdTime, List<ISport> sports)
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
        XMLParserObjectManager(DateTime fdTime, List<ISport> sports, Constant.SourceFeedType sourceFeed)
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
        /// 
        /// </summary>
        /// <param name="sportType"></param>
        /// <returns></returns>
        public ISport Sport(Constant.SportType sportType)
        {
            foreach(ISport sp in sports){
                if (sp.ID== (int)sportType)
                {
                    return sp;
                }
            }
            return null;
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
            Debug.WriteLine("Check if Serialized?");
            List<ISport> _sports = this.DeSerializeObject("c:\\git\\outputFile.txt");
            if (_sports == null)
            {
                Debug.WriteLine("Not Sirialized");
                Debug.WriteLine("Begin Parse XML");
               sports= this.XMLParse();
                Debug.WriteLine("End Parse XML");
                this.SerializeObject("c:\\git\\outputFile.txt", this.Sports);
                Debug.WriteLine("Serialized after Parsed");
            }
            else
            {
                Debug.WriteLine("Serialized");
                this.Sports = _sports;
            }

           // Sports = XMLParse();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<ISport> XMLParse1()
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
                // Loop all sports
                while (iteratorSport.MoveNext())
                {
                    XPathNavigator _sportNameNavigator = iteratorSport.Current.Clone();
                    int feedContentSport = Convert.ToInt32(_sportNameNavigator.GetAttribute("feedContents", ""));
                    if (feedContentSport > 0)
                    {
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
        #region Parse XML
        private List<ISport> XMLParse()
        {
            List<ISport> _sports = new List<ISport>();
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


            //sport
            XmlTextReader readerSport = new XmlTextReader(urlPathSport);
            // Skip non-significant whitespace  
            readerSport.WhitespaceHandling = WhitespaceHandling.Significant;
            XPathDocument doc = new XPathDocument(readerSport, XmlSpace.Preserve);
            XPathNavigator nav = doc.CreateNavigator();

            XPathExpression exprSport;
            exprSport = nav.Compile("/rsp/sports/sport");
            XPathNodeIterator iteratorSport = nav.Select(exprSport);
            // try
            // {
            // Loop all sports
            int test = 0;
            while (iteratorSport.MoveNext())
            {
                
                XPathNavigator _sportNameNavigator = iteratorSport.Current.Clone();
                int feedContentSport = Convert.ToInt32(_sportNameNavigator.GetAttribute("feedContents", ""));
                int sportID = int.Parse(_sportNameNavigator.GetAttribute("id", ""));
                /*
                 * Algorithm
                   2. Loop all SPORT in PINACLE
                     VAR SOURCETYPE = null
                    3.  If SPORT=BOXING THEN
                          IF NO DATA from PINACLE THEN
	                          Get from TITABET
		                      SOURCETYPE = TITABET
		                         -GROUPS-LEAGUES
			                        -EVENTS-MATCHES
				                       -MARKETS
				                         -OUTCOMES
	                      //ELSE
	                         // Get data from PINACLE
	                      END IF
	                    END IF
                    3.  If SPORT=CRICKET THEN
                          IF NO DATA from PINACLE THEN
	                          Get from TITABET
		                      SOURCETYPE = TITABET
	                     // ELSE
	                         // Get data from PINACLE
	                      END IF
	                    END IF	
                    3.  If SPORT=SOCCER THEN
                          IF NO DATA from TITABET THEN
	                         // Get from PINACLE
	                     // ELSE
	                          Get data from TITABET
		                      SOURCETYPE = TITABET
	                      END IF
	                    END IF	
                    3.  If SPORT=TENNIS THEN
                          IF NO DATA from PINACLE THEN
	                          Get from TITABET
		                      SOURCETYPE = TITABET
	                     // ELSE
	                         // Get data from PINACLE
	                      END IF
	                    END IF	
                    3.1 IF 	SOURCETYPE== TITABET THEN
       
	   
                           EXIT LOOP
                        END IF 
                    4.Get each LEAGUE of SPORT
                */


                // if (feedContentSport > 0)
                // {

                ISport _sport = new Sport();
                Constant.SportType _sportType = Constant.SportType.EMPTY; ;
                if (sportID == (int)Constant.SportType.BOXING)
                {
                    if (feedContentSport == 0) // No data from Pinacle
                    {
                        _sport = SportTitabetParse(Constant.SportType.BOXING);
                        _sportType = Constant.SportType.BOXING;
                    }
                }
                if (sportID == (int)Constant.SportType.CRICKET)
                {
                    if (feedContentSport == 0) // No data from Pinacle
                    {
                        _sport = SportTitabetParse(Constant.SportType.CRICKET);
                        _sportType = Constant.SportType.CRICKET;
                    }
                    
                }

                if (sportID == (int)Constant.SportType.SOCCER)
                {

                    _sport = SportTitabetParse(Constant.SportType.SOCCER);
                    if (_sport != null)
                    {
                        _sportType = Constant.SportType.SOCCER;
                    }
                }

                if (sportID == (int)Constant.SportType.TENNIS)
                {
                    if (feedContentSport == 0) // No data from Pinacle
                    {
                        _sport = SportTitabetParse(Constant.SportType.TENNIS);
                        _sportType = Constant.SportType.TENNIS;
                    }
                }


                // Check to see if gets data from TITABET
                if (_sportType != Constant.SportType.EMPTY)
                {
                    _sport.SportFeedType = Constant.SourceFeedType.TITANBET;
                    _sports.Add(_sport);
                }
                else
                {
                     _sport = SportPinnacleParse(urlPathLeague, urlPathFeed, iteratorSport);
                     _sport.SportFeedType = Constant.SourceFeedType.PINNACLESPORTS;
                     _sports.Add(_sport);
                }

                //{ // get data from Pinacle
                  //  ISport _sport = SportPinnacleParse(urlPathLeague, urlPathFeed, iteratorSport);
                  //  _sports.Add(_sport);
               // }
                //else // Get from Titabet
                //  {
                //}

                // }
            }

            return _sports;

        }
        #endregion
        
        #region Serialier
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public XMLParserObjectManager(SerializationInfo info, StreamingContext ctxt)
        {

            this.Sports = (List<ISport>)info.GetValue("Sports", typeof(List<ISport>));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("Sports", this.Sports);
        }
        public void SerializeObject(string filename, List<ISport> objectToSerialize)
        {
            if (objectToSerialize != null)
            {
                Stream stream = File.Open(filename, FileMode.Create);
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, objectToSerialize);
                stream.Close();
            }
        }

        public List<ISport> DeSerializeObject(string filename)
        {
            List<ISport> objectToSerialize ;
            try
            {

                Stream stream = File.Open(filename, FileMode.Open);
                BinaryFormatter bFormatter = new BinaryFormatter();
                objectToSerialize = (List<ISport>)bFormatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception e)
            {
                return null;
            }
            return objectToSerialize;
        }
        #endregion
        #region XML Titabet Parser
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sport"></param>
        /// <returns></returns>
        private ISport SportTitabetParse(Constant.SportType sport)
        {
            ISport _sport = new Sport();
            string urlPath = Constant.SourceXML.TITABETURL;
            string mainSport = ConfigurationManager.AppSettings["TITANBETMAINSPORT"].ToString();



            XmlTextReader reader = new XmlTextReader(urlPath);
            // Skip non-significant whitespace  
            reader.WhitespaceHandling = WhitespaceHandling.Significant;
            XPathDocument doc = new XPathDocument(reader, XmlSpace.Preserve);
            XPathNavigator nav = doc.CreateNavigator();

            int sportID = 0;
            /*
                
                Cricket
                Handball
                Horse Racing
                Motor Sports
                Boxing
                Golf
                Tennis

             */
            switch (sport)
            {
                case Constant.SportType.CRICKET:
                    sportID = 33948;
                    break;
                case Constant.SportType.HANDBALL:
                    sportID = 33945;
                    break;
                case Constant.SportType.HORSE_RACING:
                    sportID = 55882;
                    break;
                case Constant.SportType.MOTOR_SPORTS:
                    sportID = 0;
                    break;
                case Constant.SportType.BOXING:
                    sportID = 6638;
                    break;
                case Constant.SportType.GOLF:
                    sportID = 0;
                    break;
                case Constant.SportType.TENNIS:
                    sportID = 2490;
                    break;


            }

            XPathExpression exprSport;
            exprSport = nav.Compile("/bookmaker/sport[@id='" + sportID + "']");
            XPathNodeIterator iteratorSport = nav.Select(exprSport);
            while (iteratorSport.MoveNext())
            {
                XPathNavigator _sportNameNavigator = iteratorSport.Current.Clone();

                _sport.ID = sportID;
                _sport.Name = _sportNameNavigator.GetAttribute("name", "");

                // League
                if (_sportNameNavigator.HasChildren)
                {
                   XPathExpression exprevent;
                            exprevent = _sportNameNavigator.Compile("group");
                            XPathNodeIterator iteratorEvent = _sportNameNavigator.Select(exprevent);
                            while (iteratorEvent.MoveNext())
                            {

                                ILeague _league = LeagueTitabetParse(iteratorEvent, _sport.ID);
                                if(_sport.Leagues==null){
                                    _sport.Leagues = new List<ILeague>();
                                }
                                _sport.Leagues.Add(_league);
                            }

                }
            }
            return _sport;
        }
        private ILeague LeagueTitabetParse( XPathNodeIterator iteratorLeague, int sportID)
        {
            ILeague _league = new League();


            XPathNavigator _eventNameNavigator = iteratorLeague.Current.Clone();

            _league.Name = _eventNameNavigator.GetAttribute("name", "");
            _league.ID = int.Parse(_eventNameNavigator.GetAttribute("id", ""));


            if (_eventNameNavigator.HasChildren)
            {
                XPathExpression exprematch;
                exprematch = _eventNameNavigator.Compile("event");
                XPathNodeIterator iteratorMatch = _eventNameNavigator.Select(exprematch);
                while (iteratorMatch.MoveNext())
                {
                    IMatch _match = MatchTitabetParse(iteratorMatch, sportID);
                    if (_league.Matches == null)
                    {
                        _league.Matches = new List<IMatch>();
                    }
                    _league.Matches.Add(_match);
                }
            }

            return _league;
        }


        private IMatch MatchTitabetParse(XPathNodeIterator iteratorMatch, int sportID)
        {
            IMatch _match = new Match();

            XPathNavigator _matchNameNavigator = iteratorMatch.Current.Clone();

            _match.ID = int.Parse(_matchNameNavigator.GetAttribute("id", ""));
            string[] league = _matchNameNavigator.GetAttribute("name", "").Split('-');
            if (league.Length > 1)
            {
                _match.HomeTeam = league[0].Trim();
                _match.AwayTeam = league[1].Trim();
            }
            else
            {
                _match.HomeTeam = _matchNameNavigator.GetAttribute("name", "");
            }
            _match.StartDateTime = Convert.ToDateTime(_matchNameNavigator.GetAttribute("date", ""));

            if (_matchNameNavigator.HasChildren)
            {
                string OddTypeString = string.Empty;
                switch (sportID)
                {
                    case 1:
                        OddTypeString = Constant.TitanBetOddTypeID.CRICKET;
                        break;
                    case 2:
                        OddTypeString = Constant.TitanBetOddTypeID.HANDBALL;
                        break;
                    case 3:
                        OddTypeString = Constant.TitanBetOddTypeID.HORSERACING;
                        break;
                    case 4:
                        OddTypeString = Constant.TitanBetOddTypeID.MOTORSPORTS;
                        break;
                    case 5:
                        OddTypeString = Constant.TitanBetOddTypeID.BOXING;
                        break;
                    case 6:
                        OddTypeString = Constant.TitanBetOddTypeID.GOLF;
                        break;
                    case 7:
                        OddTypeString = Constant.TitanBetOddTypeID.TENNIS;
                        break;
                    case 8:
                        OddTypeString = Constant.TitanBetOddTypeID.FOOTBALL;
                        break;
                }
                string[] arrOddTypeId = OddTypeString.Split(',');
                foreach (string oddTypeId in arrOddTypeId)
                {
                    XPathExpression exprebet;
                    exprebet = _matchNameNavigator.Compile("market[@tid='" + oddTypeId + "']");
                    XPathNodeIterator iteratorMarket = _matchNameNavigator.Select(exprebet);
                    while (iteratorMarket.MoveNext())
                    {
                        IMarket _market = MarketTitabetParse(iteratorMarket);
                        if (_match.Markets==null)
                        {
                            _match.Markets = new List<IMarket>();
                        }
                        _match.Markets.Add(_market);
                    }
                }
            }
            return _match;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iteratorMarket"></param>
        /// <returns></returns>
        private IMarket MarketTitabetParse(XPathNodeIterator iteratorMarket)
        {

            IMarket _market = new Market();
            XPathNavigator _marketNameNavigator = iteratorMarket.Current.Clone();
            _market.ID = int.Parse(_marketNameNavigator.GetAttribute("tid", ""));
            _market.Name = _marketNameNavigator.GetAttribute("name", "");

            if (_marketNameNavigator.HasChildren)
            {
                IOutcome _outcome = OutcomeTitabetParse(iteratorMarket);
                if(_market.Outcomes==null){
                    _market.Outcomes = new List<IOutcome>();
                }
                _market.Outcomes.Add(_outcome);
            }


            return _market;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iteratorChoice"></param>
        /// <returns></returns>
        private IOutcome OutcomeTitabetParse(XPathNodeIterator iteratorChoice)
        {
            IOutcome _outcome = new Outcome();
            XPathNavigator _choiceNameNavigator = iteratorChoice.Current.Clone();
            _outcome.Name = _choiceNameNavigator.GetAttribute("name", "");
            _outcome.Odds = float.Parse(_choiceNameNavigator.GetAttribute("odds", ""));
            _outcome.ID = int.Parse(_choiceNameNavigator.GetAttribute("id", ""));

            return _outcome;
        }
        #endregion
        #region Pinacle XML Parser
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iteratorSport"></param>
        /// <returns></returns>
        private ISport SportPinnacleParse(String urlPathLeague, String urlPathFeed, XPathNodeIterator iteratorSport)
        {
            ISport _sport = new Sport();
            XPathNavigator _sportNameNavigator = iteratorSport.Current.Clone();
            int feedContentSport = Convert.ToInt32(_sportNameNavigator.GetAttribute("feedContents", ""));
            if (feedContentSport > 0)
            {
                // ISport _sport = new Sport();
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
                    ILeague _league = LeaguePinnacleParse(urlPathFeed, _sport.ID, _sportNameNavigator, iteratorLeague);
                    if (_sport.Leagues==null)

                    {
                        _sport.Leagues = new List<ILeague>();
                    }
                    _sport.Leagues.Add(_league);
                }
            }
            return _sport;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iteratorSport"></param>
        /// <returns></returns>
        private ILeague LeaguePinnacleParse(String urlPathFeed, int sportID, XPathNavigator sportNameNavigator, XPathNodeIterator iteratorLeague)
        {
            ILeague _league = new League();
            XPathNavigator _eventNameNavigator = iteratorLeague.Current.Clone();
            int feedContentLeague = Convert.ToInt32(sportNameNavigator.GetAttribute("feedContents", ""));
            if (feedContentLeague > 0)
            {
                //ILeague _league = new League();
                _league.ID = Convert.ToInt32(_eventNameNavigator.GetAttribute("id", ""));
                _league.Name = _eventNameNavigator.Value;
                //Add the current League to the latest sport
                //  sports[sports.Count - 1].Leagues.Add(_league);

                //match - home team - awayteam  
                XmlTextReader readerMatch = new XmlTextReader(string.Format(urlPathFeed, sportID, _league.ID));
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
                        IMatch _match = MatchPinnacleParse(iteratorMatchEvent);
                        if (_league.Matches == null)
                        {
                            _league.Matches = new List<IMatch>();
                        }
                        _league.Matches.Add(_match);

                    }




                }
            }

            return _league;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iteratorSport"></param>
        /// <returns></returns>
        private IMatch MatchPinnacleParse(XPathNodeIterator iteratorMatchEvent)
        {


            XPathNavigator _matchEventNameNavigator = iteratorMatchEvent.Current.Clone();
            IMatch _match = new Match();
            _match.ID = Convert.ToInt32(_matchEventNameNavigator.SelectSingleNode("id").ToString()); ;
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
                    XPathNavigator _betNameNavigator = iteratorBet.Current.Clone();
                    IPeriod _period =  PeriodPinnacleParse(_matchEventNameNavigator, iteratorBet);
                    if (_match.Periods == null)
                    {
                        _match.Periods = new List<IPeriod>();
                    }
                    _match.Periods.Add(_period);
                }
            }

            return _match;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iteratorSport"></param>
        /// <returns></returns>
        private IPeriod PeriodPinnacleParse( XPathNavigator matchEventNameNavigator,XPathNodeIterator iteratorBet)
        {
          
            XPathNavigator _betNameNavigator = iteratorBet.Current.Clone();
            IPeriod _period = new Period();
            _period.Description = _betNameNavigator.SelectSingleNode("description").Value; ;

            // Add period to Match

            //handicap
            XPathExpression exprehandicap;
            exprehandicap = matchEventNameNavigator.Compile("spreads/spread");
            XPathNodeIterator iteratorHandicap = matchEventNameNavigator.Select(exprehandicap);
            //total
            XPathExpression expretotal;
            expretotal = matchEventNameNavigator.Compile("totals/total");
            XPathNodeIterator iteratorTotal = matchEventNameNavigator.Select(expretotal);
            //moneyline
            XPathExpression expremoneyline;
            expremoneyline = matchEventNameNavigator.Compile("moneyLine");
            XPathNodeIterator iteratorMoneyLine = matchEventNameNavigator.Select(expremoneyline);
            while (iteratorHandicap.MoveNext())
            {
                ISpread _spread = SpreadPinnacleParse(iteratorHandicap);
                if (_period.Spreads==null)
                {
                    _period.Spreads = new List<ISpread>();
                }
                _period.Spreads.Add(_spread);
            }

            while (iteratorTotal.MoveNext())
            {
                ITotal _total = TotalPinnacleParse(iteratorTotal);
                if (_period.Totals==null)
                {
                    _period.Totals = new List<ITotal>();
                }
                _period.Totals.Add(_total);

            }

            while (iteratorMoneyLine.MoveNext())
            {
                IMoneyLine _moneyline = MoneyLinePinnacleParse(iteratorMoneyLine);
                if (_period.MoneyLines==null)
                {
                    _period.MoneyLines = new List<IMoneyLine>();
                }
                _period.MoneyLines.Add(_moneyline);
            }


            return _period;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iteratorSport"></param>
        /// <returns></returns>
        private IMoneyLine MoneyLinePinnacleParse(XPathNodeIterator iteratorMoneyLine)
        {
            IMoneyLine _moneyline = new MoneyLine();
            XPathNavigator _choiceNameNavigator = iteratorMoneyLine.Current.Clone();
            _moneyline.AwayPrice = float.Parse(_choiceNameNavigator.SelectSingleNode("awayPrice").Value);
            _moneyline.HomePrice = float.Parse(_choiceNameNavigator.SelectSingleNode("homePrice").Value);
            _moneyline.DrawPrice = float.Parse(_choiceNameNavigator.SelectSingleNode("drawPrice").Value);
            return _moneyline;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iteratorSport"></param>
        /// <returns></returns>
        private ITotal TotalPinnacleParse(XPathNodeIterator iteratorTotal)
        {
            XPathNavigator _choiceNameNavigator = iteratorTotal.Current.Clone();
            ITotal _total = new Total();
            _total.Point = float.Parse(_choiceNameNavigator.SelectSingleNode("points").Value);
            _total.OverPrice = float.Parse(_choiceNameNavigator.SelectSingleNode("overPrice").Value);
            _total.UnderPrice = float.Parse(_choiceNameNavigator.SelectSingleNode("underPrice").Value);
            return _total;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iteratorSport"></param>
        /// <returns></returns>
        private ISpread SpreadPinnacleParse(XPathNodeIterator iteratorHandicap)
        {
            XPathNavigator _choiceNameNavigator = iteratorHandicap.Current.Clone();
            ISpread _spread = new Spread();
            _spread.AwaySpread = float.Parse(_choiceNameNavigator.SelectSingleNode("awaySpread").Value);
            _spread.AwayPrice = float.Parse(_choiceNameNavigator.SelectSingleNode("awayPrice").Value);
            _spread.HomeSpread = float.Parse(_choiceNameNavigator.SelectSingleNode("homeSpread").Value);
            _spread.HomePrice = float.Parse(_choiceNameNavigator.SelectSingleNode("homePrice").Value);
            return _spread;
        }
        #endregion
        #region Render UI
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String RenderJson(){
        return "not implemented";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String RenderXML()
        {
            return "not implementd";
        }
        #endregion
    }

}
 


