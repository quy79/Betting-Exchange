using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using BetEx247.Core.Common.Utils;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using BetEx247.Plugin.DataManager.XMLObjects.Sport;
using BetEx247.Plugin.DataManager.XMLObjects.SoccerMatch;
using BetEx247.Plugin.DataManager.XMLObjects.SoccerCountry;
using BetEx247.Plugin.DataManager.XMLObjects.SoccerLeague;
using BetEx247.Core;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports;


namespace BetEx247.Plugin.DataManager
{

    /// <summary>
    /// This classs is a container to store all data object of sports and their details.
    /// </summary>
    /// 
    [Serializable()]
    public class XMLParserManager : IGeneric
    {
        private DateTime fdTime;
        public InitMasterTables masterTableManager = new InitMasterTables();
        public bool updatedMastertable = false;
       // private List<SoccerCountry> soccerCountries;
        private List<Bet247xSport> sports;
       // private Constant.SourceFeedType sourceFeed;
        /// <summary>
        /// Init without paramater
        /// </summary>
        public XMLParserManager() { }
        /// <summary>
        /// Init with only Time of XML feed
        /// </summary>
        /// <param name="fdTime"></param>
        XMLParserManager(DateTime fdTime)
        {
            FDTime = fdTime;
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
        public List<Bet247xSport> Sports
        {
            set { sports = value; }
            get { return sports; }
        }
        public void InitMasterTable()
        {
            // Read file update.xml to see if need to update
           String strPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);

          // XmlTextReader textReader = new XmlTextReader("E:\\beauty\\Dropbox\\Betting\\code\\sport.xml");
           XDocument doc = XDocument.Load(Constant.SourceXML.MASTERXMLSOURCE + "\\update.xml");

           foreach (XElement element in doc.Root.Nodes())
           {
               if ("xml".Equals(element.Attribute("name").Value) && !"No".Equals(element.Value))
               {
                   element.SetValue("No");
                   doc.Save(Constant.SourceXML.MASTERXMLSOURCE + "\\update.xml");
                  
               }
               else
               {
               }
               masterTableManager.InitBetStatusTable();
               masterTableManager.InitMatchStatusTable();
               masterTableManager.InitSportTable();
               masterTableManager.InitSoccerCountryTable();
               updatedMastertable = true;
               
           }
          
          
        }

       
       
   

        #region Serialier
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public XMLParserManager(SerializationInfo info, StreamingContext ctxt)
        {

            this.Sports = (List<Bet247xSport>)info.GetValue("Sports", typeof(List<Bet247xSport>));
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
        public void SerializeObject(string filename, List<Bet247xSport> objectToSerialize)
        {
            if (objectToSerialize != null)
            {
                Stream stream = File.Open(filename, FileMode.Create);
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, objectToSerialize);
                stream.Close();
            }
        }

        public List<Bet247xSport> DeSerializeObject(string filename)
        {
            List<Bet247xSport> objectToSerialize;
            try
            {
                Stream stream = File.Open(filename, FileMode.Open);
                BinaryFormatter bFormatter = new BinaryFormatter();
                objectToSerialize = (List<Bet247xSport>)bFormatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception e)
            {
                return null;
            }
            return objectToSerialize;
        }
        #endregion
        #region XML GoalServe Parser
        public void SoccerGoalServeParser(ref Bet247xSoccerCountry bet247xSoccerCountry)
        {
           // Bet247xSoccerCountry _bet247xSoccerCountry = new Bet247xSoccerCountry();
           // List<Bet247xSoccerLeague> _bet247xSoccerLeagues = new List<Bet247xSoccerLeague>();
            XDocument doc = null; ;
            string sFullPath = "";
            try {
                string folderPath = CommonHelper.CreateDirectory(Constant.PlaceFolder.GOALSERVE_FOLDER, "XML" /*"FeedData_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString()*/);
                sFullPath = string.Format("{0}/{1}/{2}.xml", CommonHelper.getLocalPath(), folderPath, bet247xSoccerCountry.Country);
                if (File.Exists(sFullPath))
                {
                    doc = XDocument.Load(sFullPath);
                }
                else
                {
                    return;
                }
               
            }
            catch(Exception ee){
                return;
            }
           // XDocument doc = XDocument.Load(bet247xSoccerCountry.Goalserve_OddsFeed);

            // Loop all catagories
            int indexLeague = 0;
            foreach (XElement element in doc.Root.Nodes())
            {
              //  Bet247xSoccerLeague _bet247xSoccerLeague = new Bet247xSoccerLeague();
                String _catogatry = element.Attribute("name").Value;
                SoccerLeagueService _soccerLeagueSrv = new SoccerLeagueService();
                long leagueCountryID = _soccerLeagueSrv.GoalServeSoccerLeague(_catogatry) == null ? 0 : _soccerLeagueSrv.GoalServeSoccerLeague(_catogatry).ID;
               // Search All Matches of the Catagory
               IEnumerable<XElement> _matchElements = element.XPathSelectElements("matches");
              foreach (XElement _matchElement in _matchElements)
               {
                   List<Bet247xSoccerMatch> _bet247xSoccerMatches = new List<Bet247xSoccerMatch>();
                  //String _localTeam = element.Attribute("localteam ").Value;
                   IEnumerable<XElement> _matchElementDetail = _matchElement.XPathSelectElements("match");
                   foreach (XElement _matchDetail in _matchElementDetail)
                   {
                       Bet247xSoccerMatch _bet247xSoccerMatch = new Bet247xSoccerMatch();
                      // IEnumerable<XElement> _localteamElements = _matchDetail.XPathSelectElements("localteam");
                       XElement _localteamElement = _matchDetail.XPathSelectElement("localteam");
                       _bet247xSoccerMatch.LeagueID = leagueCountryID;

                       String _startDate = _matchDetail.Attribute("formatted_date").Value;
                       String _startTime = _matchDetail.Attribute("time").Value;
                       int year, month,day, hours, minute;
                       year = int.Parse(_startDate.Split('.')[2]);
                       month = int.Parse(_startDate.Split('.')[1]);
                       day = int.Parse(_startDate.Split('.')[0]);
                       hours = int.Parse(_startTime.Split(':')[0]);
                       minute = int.Parse(_startTime.Split(':')[1]);

                       DateTime _marketCloseTime = new DateTime(year, month, day, hours, minute, 0); //formatted_date="27.10.2012" time="17:00"
                       _marketCloseTime.AddMinutes(-5);
                       _bet247xSoccerMatch.HomeTeam = _localteamElement.Attribute("name").Value;
                     
                       //IEnumerable<XElement> _visitorteamElements = _matchDetail.XPathSelectElements("visitorteam");
                       XElement  _visitorteamElement = _matchDetail.XPathSelectElement("visitorteam");
                       _bet247xSoccerMatch.AwayTeam = _visitorteamElement.Attribute("name").Value;
                       _bet247xSoccerMatch.StartDate = new DateTime(year, month, day, hours, minute, 0);
                       _bet247xSoccerMatch.StartTime = new DateTime(year, month, day, hours, minute, 0);
                     
                       // Check and save or update to database
                       SoccerMatchService _soccerMatchSvr = new SoccerMatchService();
                       SoccerLeague _tempSoccerLeague = (SoccerLeague)bet247xSoccerCountry.Bet247xSoccerLeagues[indexLeague];
                       SoccerMatch _tempSoccerMatch = _soccerMatchSvr.SoccerMatch(_tempSoccerLeague.ID, _bet247xSoccerMatch.HomeTeam, _bet247xSoccerMatch.AwayTeam, _bet247xSoccerMatch.StartDate, _bet247xSoccerMatch.StartTime);
                       _bet247xSoccerMatch.MatchStatusID = 11;// Not stated
                       _soccerMatchSvr.Update(_bet247xSoccerMatch.getMatch());
                       _bet247xSoccerMatch.ID = _soccerMatchSvr.SoccerMatch(_bet247xSoccerMatch.getMatch());
                       IEnumerable<XElement> _eventElements = _matchDetail.XPathSelectElements("events");
                       IEnumerable<XElement> _oddsElements = _matchDetail.XPathSelectElements("odds");

                       // Loop to find Odds
                       foreach (XElement _oddElement in _oddsElements)
                       {
                           IEnumerable<XElement> _oddElementDetail = _oddElement.XPathSelectElements("type");
                           foreach (XElement _matchBookerDetail in _oddElementDetail)
                             {
                                 String oddsName = _matchBookerDetail.Attribute("value").Value;
                                 if (oddsName.Equals("3Way Result"))
                                 {
                                     
                                     IEnumerable<XElement> _bookmakerElements = _matchBookerDetail.XPathSelectElements("bookmaker");
                                     foreach (XElement _bookmakerElementsDetail in _bookmakerElements)
                                     {

                                         IEnumerable<XElement> _bookerOddElement = _bookmakerElementsDetail.XPathSelectElements("odd");
                                        
                                         foreach (XElement _bookerOddElementDetail in _bookerOddElement)
                                         {
                                             Soccer_MatchOdds _soccer_MatchOddsTable = new Soccer_MatchOdds();
                                             _soccer_MatchOddsTable.Period = 0;
                                             _soccer_MatchOddsTable.Entrants = 3;
                                             _soccer_MatchOddsTable.LastUpdated = DateTime.Now;
                                             _soccer_MatchOddsTable.MarketCloseTime = _marketCloseTime;//formatted_date="27.10.2012" time="17:00"
                                             _soccer_MatchOddsTable.MatchID = _bet247xSoccerMatch.ID;
                                             String oddName = _bookerOddElementDetail.Attribute("name").Value;
                                             if (oddName.Equals("1"))
                                             { //(HomeWin price
                                                 _soccer_MatchOddsTable.HomePrice = decimal.Parse(_bookerOddElementDetail.Attribute("value").Value);
                                             }
                                             else if (oddName.Equals("X")) //(HomeWin price
                                             {
                                                 _soccer_MatchOddsTable.DrawPrice = decimal.Parse(_bookerOddElementDetail.Attribute("value").Value);
                                             }
                                             else
                                             { //(HomeWin price
                                                 _soccer_MatchOddsTable.AwayPrice = decimal.Parse(_bookerOddElementDetail.Attribute("value").Value);
                                             }
                                             _bet247xSoccerMatch.Bet247xSoccerMatchOdds.Add(_soccer_MatchOddsTable);
                                             SoccerMatchOddsService _soccermatchOddsSvr = new SoccerMatchOddsService();
                                             _soccermatchOddsSvr.Insert(_soccer_MatchOddsTable);
                                         }
                                        
                                        
                                     }
                                 }
                                 else if (oddsName.Equals("Home/Away"))
                                 {
                                     IEnumerable<XElement> _bookmakerElements = _matchBookerDetail.XPathSelectElements("bookmaker");
                                     foreach (XElement _bookmakerElementsDetail in _bookmakerElements)
                                     {

                                         IEnumerable<XElement> _bookerOddElement = _bookmakerElementsDetail.XPathSelectElements("odd");
                                         Soccer_DrawNoBet _soccer_DrawNoBetTable = new Soccer_DrawNoBet();
                                         _soccer_DrawNoBetTable.Period = "0";
                                         _soccer_DrawNoBetTable.Entrants = 2;
                                         _soccer_DrawNoBetTable.LastUpdated = DateTime.Now;
                                         _soccer_DrawNoBetTable.MarketCloseTime = _marketCloseTime;//formatted_date="27.10.2012" time="17:00"
                                         _soccer_DrawNoBetTable.MatchID = _bet247xSoccerMatch.ID;
                                         foreach (XElement _bookerOddElementDetail in _bookerOddElement)
                                         {
                                             String oddName = _bookerOddElementDetail.Attribute("name").Value;
                                             if (oddName.Equals("1"))
                                             { //(HomeWin price
                                                 _soccer_DrawNoBetTable.HomePrice = decimal.Parse(_bookerOddElementDetail.Attribute("value").Value);
                                             }
                                            
                                             else
                                             { //(HomeWin price
                                                 _soccer_DrawNoBetTable.AwayPrice = decimal.Parse(_bookerOddElementDetail.Attribute("value").Value);
                                             }
                                             _bet247xSoccerMatch.Bet247xSoccerDrawNoBets.Add(_soccer_DrawNoBetTable);
                                         }

                                         SoccerDrawNoBetService _soccerDrawNoBetSvr = new SoccerDrawNoBetService();
                                         _soccerDrawNoBetSvr.Insert(_soccer_DrawNoBetTable);

                                     }
                                 }
                                 else if (oddsName.Equals("Over/Under"))
                                 {
                                     IEnumerable<XElement> _bookmakerElements = _matchBookerDetail.XPathSelectElements("bookmaker");
                                     foreach (XElement _bookmakerElementsDetail in _bookmakerElements)
                                     {

                                         IEnumerable<XElement> _bookerOddElement = _bookmakerElementsDetail.XPathSelectElements("total");
                                       
                                         foreach (XElement _bookerOddElementDetail in _bookerOddElement)
                                         {
                                             IEnumerable<XElement> _bookerToalOddElement = _bookerOddElementDetail.XPathSelectElements("odd");
                                             String OU = _bookerOddElementDetail.Attribute("name").Value;
                                             Soccer_TotalGoalsOU _soccer_TotalGoalsOU = new Soccer_TotalGoalsOU();
                                             _soccer_TotalGoalsOU.Period = 0;
                                             _soccer_TotalGoalsOU.Entrants = 2;
                                             _soccer_TotalGoalsOU.LastUpdated = DateTime.Now;
                                             _soccer_TotalGoalsOU.InPlay = false;
                                             _soccer_TotalGoalsOU.MarketCloseTime = _marketCloseTime;//formatted_date="27.10.2012" time="17:00"
                                             _soccer_TotalGoalsOU.MatchID = _bet247xSoccerMatch.ID;
                                             foreach (XElement _totalOddElementDetail in _bookerToalOddElement)
                                             {
                                                 String oddName = _bookerOddElementDetail.Attribute("name").Value;
                                                 if (oddName.Equals("Under"))
                                                 { //(HomeWin price
                                                     _soccer_TotalGoalsOU.UnderPrice = decimal.Parse(_totalOddElementDetail.Attribute("value").Value);
                                                 }
                                                 else if (oddName.Equals("Over")) //(HomeWin price
                                                 {
                                                     _soccer_TotalGoalsOU.OverPrice = decimal.Parse(_totalOddElementDetail.Attribute("value").Value);
                                                 }
                                                 else
                                                 { //(HomeWin price
                                                     //_soccer_TotalGoalsOU.AwayPrice = decimal.Parse(_bookerOddElementDetail.Attribute("value").Value);
                                                 }
                                                // _bet247xSoccerMatch.Bet247xSoccerMatchOdds.Add(_soccer_MatchOddsTable);
                                             }
                                             _bet247xSoccerMatch.Bet247xSoccerTotalGoalsOUs.Add(_soccer_TotalGoalsOU);
                                             SoccerTotalGoalsOUService _soccerTotalOUSvr = new SoccerTotalGoalsOUService();
                                             _soccerTotalOUSvr.Insert(_soccer_TotalGoalsOU);
                                         }


                                     }
                                 }
                                 else if (oddsName.Equals("Handicap"))
                                 {
                                     IEnumerable<XElement> _bookmakerElements = _matchBookerDetail.XPathSelectElements("bookmaker");
                                     foreach (XElement _bookmakerElementsDetail in _bookmakerElements)
                                     {

                                         IEnumerable<XElement> _bookerOddElement = _bookmakerElementsDetail.XPathSelectElements("handicap");
                                         
                                         foreach (XElement _bookerOddElementDetail in _bookerOddElement)
                                         {
                                             IEnumerable<XElement> _bookerToalOddElement = _bookerOddElementDetail.XPathSelectElements("odd");
                                             String OU = _bookerOddElementDetail.Attribute("name").Value;
                                             Soccer_AsianHandicap _soccer_AsianHandicap = new Soccer_AsianHandicap();
                                             _soccer_AsianHandicap.Period = 0;
                                             _soccer_AsianHandicap.Entrants = 2;
                                             _soccer_AsianHandicap.LastUpdated = DateTime.Now;
                                             // _soccer_AsianHandicap.InPlay = false;
                                             _soccer_AsianHandicap.MarketCloseTime = _marketCloseTime;//formatted_date="27.10.2012" time="17:00"
                                             _soccer_AsianHandicap.MatchID = _bet247xSoccerMatch.ID;
                                             _soccer_AsianHandicap.HomeHandicap = _bookerOddElementDetail.Attribute("name").Value;
                                             foreach (XElement _totalOddElementDetail in _bookerToalOddElement)
                                             {
                                                 String oddName = _totalOddElementDetail.Attribute("name").Value;
                                                 if (oddName.Equals("1"))
                                                 { //(HomeWin price
                                                     _soccer_AsianHandicap.HomePrice = decimal.Parse(_totalOddElementDetail.Attribute("value").Value);
                                                 }
                                                 else if (oddName.Equals("2")) //(HomeWin price
                                                 {
                                                     _soccer_AsianHandicap.AwayPrice = decimal.Parse(_totalOddElementDetail.Attribute("value").Value);
                                                 }
                                                 else
                                                 { //(HomeWin price
                                                     //_soccer_TotalGoalsOU.AwayPrice = decimal.Parse(_bookerOddElementDetail.Attribute("value").Value);
                                                 }
                                                 // _bet247xSoccerMatch.Bet247xSoccerMatchOdds.Add(_soccer_MatchOddsTable);
                                             }
                                             _bet247xSoccerMatch.Bet247xSoccerAsianHandicaps.Add(_soccer_AsianHandicap);

                                             SoccerAsianHandicapService _soccerHandicapSvr = new SoccerAsianHandicapService();
                                             _soccerHandicapSvr.Insert(_soccer_AsianHandicap);
                                         }


                                     }
                                 } else if (oddsName.Equals("3Way Result 1st Half"))
                                 {
                                      IEnumerable<XElement> _bookmakerElements = _matchBookerDetail.XPathSelectElements("bookmaker");
                                      foreach (XElement _bookmakerElementsDetail in _bookmakerElements)
                                      {

                                          IEnumerable<XElement> _bookerOddElement = _bookmakerElementsDetail.XPathSelectElements("odd");
                                        
                                          foreach (XElement _bookerOddElementDetail in _bookerOddElement)
                                          {
                                              String oddName = _bookerOddElementDetail.Attribute("name").Value;
                                              Soccer_MatchOdds _soccer_MatchOddsTable = new Soccer_MatchOdds();
                                              _soccer_MatchOddsTable.Period = 1;
                                              _soccer_MatchOddsTable.Entrants = 3;
                                              _soccer_MatchOddsTable.LastUpdated = DateTime.Now;
                                              _soccer_MatchOddsTable.MarketCloseTime = _marketCloseTime;//formatted_date="27.10.2012" time="17:00"
                                              _soccer_MatchOddsTable.MatchID = _bet247xSoccerMatch.ID;
                                              if (oddName.Equals("1"))
                                              { //(HomeWin price
                                                  _soccer_MatchOddsTable.HomePrice = decimal.Parse(_bookerOddElementDetail.Attribute("value").Value);
                                              }
                                              else if (oddName.Equals("X")) //(HomeWin price
                                              {
                                                  _soccer_MatchOddsTable.DrawPrice = decimal.Parse(_bookerOddElementDetail.Attribute("value").Value);
                                              }
                                              else
                                              { //(HomeWin price
                                                  _soccer_MatchOddsTable.AwayPrice = decimal.Parse(_bookerOddElementDetail.Attribute("value").Value);
                                              }
                                              _bet247xSoccerMatch.Bet247xSoccerMatchOdds.Add(_soccer_MatchOddsTable);
                                              SoccerMatchOddsService _soccerMatchOddSvr = new SoccerMatchOddsService();
                                              _soccerMatchOddSvr.Insert(_soccer_MatchOddsTable);
                                          }
                                      }
                                 }
                                 else if (oddsName.Equals("Over/Under 1st Half"))
                                 {
                                     IEnumerable<XElement> _bookmakerElements = _matchBookerDetail.XPathSelectElements("bookmaker");
                                     foreach (XElement _bookmakerElementsDetail in _bookmakerElements)
                                     {

                                         IEnumerable<XElement> _bookerOddElement = _bookmakerElementsDetail.XPathSelectElements("total");
                                        
                                         foreach (XElement _bookerOddElementDetail in _bookerOddElement)
                                         {
                                             IEnumerable<XElement> _bookerToalOddElement = _bookerOddElementDetail.XPathSelectElements("odd");
                                             String OU = _bookerOddElementDetail.Attribute("name").Value;
                                             Soccer_TotalGoalsOU _soccer_TotalGoalsOU = new Soccer_TotalGoalsOU();
                                             _soccer_TotalGoalsOU.Period = 1;
                                             _soccer_TotalGoalsOU.Entrants = 2;
                                             _soccer_TotalGoalsOU.LastUpdated = DateTime.Now;
                                             _soccer_TotalGoalsOU.InPlay = false;
                                             _soccer_TotalGoalsOU.MarketCloseTime = _marketCloseTime;//formatted_date="27.10.2012" time="17:00"
                                             _soccer_TotalGoalsOU.MatchID = _bet247xSoccerMatch.ID;
                                             foreach (XElement _totalOddElementDetail in _bookerToalOddElement)
                                             {
                                                 String oddName = _bookerOddElementDetail.Attribute("name").Value;
                                                 if (oddName.Equals("Under"))
                                                 { //(HomeWin price
                                                     _soccer_TotalGoalsOU.UnderPrice = decimal.Parse(_totalOddElementDetail.Attribute("value").Value);
                                                 }
                                                 else if (oddName.Equals("Over")) //(HomeWin price
                                                 {
                                                     _soccer_TotalGoalsOU.OverPrice = decimal.Parse(_totalOddElementDetail.Attribute("value").Value);
                                                 }
                                                 else
                                                 { //(HomeWin price
                                                     //_soccer_TotalGoalsOU.AwayPrice = decimal.Parse(_bookerOddElementDetail.Attribute("value").Value);
                                                 }
                                                 // _bet247xSoccerMatch.Bet247xSoccerMatchOdds.Add(_soccer_MatchOddsTable);
                                             }
                                             _bet247xSoccerMatch.Bet247xSoccerTotalGoalsOUs.Add(_soccer_TotalGoalsOU);
                                             SoccerTotalGoalsOUService _soccerTotalOUSvr = new SoccerTotalGoalsOUService();
                                             _soccerTotalOUSvr.Insert(_soccer_TotalGoalsOU);
                                         }


                                     }
                                     
                                 }
                               
                           }

                           // Find CorrectScore in Betclick then Add to the Math
                           Soccer_CorrectScoresParser((Bet247xSoccerLeague)bet247xSoccerCountry.Bet247xSoccerLeagues[indexLeague],_bet247xSoccerMatch);
                           _bet247xSoccerMatches.Add(_bet247xSoccerMatch);
                       }

                       bet247xSoccerCountry.Bet247xSoccerLeagues[indexLeague].Bet247xSoccerMatches.AddRange(_bet247xSoccerMatches);
                      // _bet247xSoccerLeague.Bet247xSoccerMatches.AddRange(_bet247xSoccerMatches);
                   }
                  
                  // _bet247xSoccerLeagues.Add(_bet247xSoccerLeague);
               }
               
              
            }
           // _bet247xSoccerCountry.Bet247xSoccerLeagues.AddRange(_bet247xSoccerLeagues);
           // return _bet247xSoccerCountry;

            // Remove the file

            if (System.IO.File.Exists(sFullPath))
            {
                // Use a try block to catch IOExceptions, to 
                // handle the case of the file already being 
                // opened by another process. 
                try
                {
                 //   System.IO.File.Delete(sFullPath);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

           
        }

        private List<Soccer_CorrectScores> Soccer_CorrectScoresParser(Bet247xSoccerLeague bet247xSoccerLeague, SoccerMatch _soccerMatch)
        {
            List<Soccer_CorrectScores> _soccer_CorrectScoreses = new List<Soccer_CorrectScores>();

            if(bet247xSoccerLeague.LeagueName_Betclick== null) return null;
            string sFullPath = "";
            XmlTextReader reader= null; ;
            try
            {
                string folderPath = CommonHelper.CreateDirectory(Constant.PlaceFolder.BETCLICK_FOLDER, "XML" /*"FeedData_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString()*/);
                 sFullPath = string.Format("{0}/{1}/{2}.xml", CommonHelper.getLocalPath(), folderPath, "betclick");
                reader = new XmlTextReader(sFullPath);
            }
            catch (Exception ee)
            {
                return null;
            }



           // XmlTextReader reader = new XmlTextReader(/*bet247xSoccerLeague.LeagueName_Betclick*/"E:\\Projects\\modiface\\Dropbox\\Betting\\xml\\odds_en.xml");
            // Skip non-significant whitespace  
            reader.WhitespaceHandling = WhitespaceHandling.Significant;
            XPathDocument doc = new XPathDocument(reader, XmlSpace.Preserve);
            XPathNavigator nav = doc.CreateNavigator();

            XPathExpression exprSport;
            exprSport = nav.Compile("/sports/sport");
            XPathNodeIterator iteratorSport = nav.Select(exprSport);
          
                while (iteratorSport.MoveNext())
                {

                    XPathNavigator _sportNameNavigator = iteratorSport.Current.Clone();
                    String _sportName = _sportNameNavigator.GetAttribute("name", "");
                    if (_sportName.Equals("Football"))
                    {
                        if (_sportNameNavigator.HasChildren)
                        {
                            XPathExpression exprevent;
                            exprevent = _sportNameNavigator.Compile("event");
                            XPathNodeIterator iteratorEvent = _sportNameNavigator.Select(exprevent);
                            while (iteratorEvent.MoveNext())
                            {
                               // _eventId++;
                                XPathNavigator _eventNameNavigator = iteratorEvent.Current.Clone();
                              //  Event _event = new Event();
                              //  _event.eventId = _eventId;
                                //_event.sportId = _sportId;
                               String eventName = _eventNameNavigator.GetAttribute("name", "");
                               // _lstEvent.Add(_event);
                               if (eventName.Equals(bet247xSoccerLeague.LeagueName_Betclick))
                               {
                                   if (_eventNameNavigator.HasChildren)
                                   {
                                       XPathExpression exprematch;
                                       exprematch = _eventNameNavigator.Compile("match");
                                       XPathNodeIterator iteratorMatch = _eventNameNavigator.Select(exprematch);
                                       while (iteratorMatch.MoveNext())
                                       {
                                           XPathNavigator _matchNameNavigator = iteratorMatch.Current.Clone();
                                           String _leagueName=_matchNameNavigator.GetAttribute("name", "");
                                           string[] league = _leagueName.Split('-');
                                           DateTime dt = new DateTime(_soccerMatch.StartDate.Year, _soccerMatch.StartDate.Month, _soccerMatch.StartDate.Day, _soccerMatch.StartTime.Hour, _soccerMatch.StartTime.Minute, 0);
                                           dt.AddHours(1);
                                           String _datetime = dt.ToString("yyyy-MM-ddTHH:mm") + ":00";
                                           if (league.Length > 1 && (_leagueName.IndexOf(_soccerMatch.HomeTeam) >= 0 && _leagueName.IndexOf(_soccerMatch.AwayTeam) >= 0) && _datetime.Equals(_matchNameNavigator.GetAttribute("start_date", "")))
                                           {
                                               if (_matchNameNavigator.HasChildren)
                                               {
                                                   XPathExpression exprebet;
                                                   exprebet = _matchNameNavigator.Compile("bets/bet");
                                                   XPathNodeIterator iteratorBet = _matchNameNavigator.Select(exprebet);
                                                   while (iteratorBet.MoveNext())
                                                   {
                                                       XPathNavigator _betNameNavigator = iteratorBet.Current.Clone();

                                                       //_bet.betName = _betNameNavigator.GetAttribute("name", "");
                                                       String _betCodeID = _betNameNavigator.GetAttribute("code", "");
                                                       Debug.WriteLine("_betCodeID=" + _betCodeID);
                                                       if (_betCodeID.Equals("Ftb_Csc"))
                                                       {
                                                           if (_betNameNavigator.HasChildren)
                                                           {
                                                               XPathExpression exprechoice;
                                                               exprechoice = _betNameNavigator.Compile("choice");
                                                               XPathNodeIterator iteratorChoice = _betNameNavigator.Select(exprechoice);
                                                               while (iteratorChoice.MoveNext())
                                                               {
                                                                   XPathNavigator _choiceNameNavigator = iteratorChoice.Current.Clone();
                                                                   // _choice.choiceName = _choiceNameNavigator.GetAttribute("name", "");
                                                                   // _choice.odd = _choiceNameNavigator.GetAttribute("odd", "");
                                                                   Debug.WriteLine("choiceName=" + _choiceNameNavigator.GetAttribute("name", ""));
                                                                   if (!"Any other score".Equals(_choiceNameNavigator.GetAttribute("name", "")))
                                                                   {
                                                                       Soccer_CorrectScores _correctScore = new Soccer_CorrectScores();
                                                                       _correctScore.CorrectScore = _choiceNameNavigator.GetAttribute("name", "");
                                                                       _correctScore.Price = decimal.Parse(_choiceNameNavigator.GetAttribute("odd", ""));
                                                                       dt.AddHours(-1);
                                                                       dt.AddMinutes(-5);
                                                                       _correctScore.MarketCloseTime = dt;
                                                                       _correctScore.MatchID = _soccerMatch.ID;
                                                                       _correctScore.LastUpdated = DateTime.Now;
                                                                       SoccerCorrectScoresService _soccerCorectSvr = new SoccerCorrectScoresService();
                                                                       _soccerCorectSvr.Insert(_correctScore);
                                                                       _soccer_CorrectScoreses.Add(_correctScore);
                                                                   }
                                                                  
                                                               }
                                                           }
                                                       }
                                                   }

                                           }
                                          
                                          // _match.startTime = Convert.ToDateTime(_matchNameNavigator.GetAttribute("start_date", ""));
                                          

                                          

                                                  
                                           }
                                       }
                                   }
                                } // End of eventName
                                
                            }
                        }
                    }// end of if (_sportName.Equals("Football"))
                    
                    
                }
                if (System.IO.File.Exists(sFullPath))
                {
                    // Use a try block to catch IOExceptions, to 
                    // handle the case of the file already being 
                    // opened by another process. 
                    try
                    {
                     //   System.IO.File.Delete(sFullPath);
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                        
                    }
                }
                return _soccer_CorrectScoreses;
        }
        #endregion
        #region Pinacle XML Parser
        private void PinacleXMLParser(ref Bet247xSport bet247xSport)
        {
            // List<ISport> _sports = new List<ISport>();
            string urlPathSport = string.Empty;
            string urlPathLeague = string.Empty;
            string urlPathFeed = string.Empty;

            urlPathSport = "http://api.pinnaclesports.com/v1/sports";//Constant.SourceXML.PINNACLESPORTSURL;
            if (urlPathSport.IndexOf("http://") > -1)
            {
                urlPathLeague = "http://api.pinnaclesports.com/v1/leagues?sportid={0}";// Constant.SourceXML.PINNACLELEAGUEURL;
                urlPathFeed = "http://api.pinnaclesports.com/v1/feed?sportid={0}&amp;leagueid={1}&amp;clientid=PN514368&amp;apikey=4235dc98-c16d-45f7-a74d-d68861e80a47&amp;islive=0&amp;currencycode=usd";//;Constant.SourceXML.PINNACLEFEEDURL;
            }
            else
            {
                string[] arrPlitUrl = urlPathSport.Split('_');
                string timetick = arrPlitUrl[2].Replace(".xml", "");
                urlPathLeague = arrPlitUrl[0] + "_league_{0}_" + timetick + ".xml";
                urlPathFeed = arrPlitUrl[0] + "_feed_{0}_{1}_" + timetick + ".xml";
            }

            try
            {
                //sport
                XmlTextReader readerSport = new XmlTextReader(urlPathSport);
                // Skip non-significant whitespace  
                readerSport.WhitespaceHandling = WhitespaceHandling.Significant;
                XPathDocument doc = new XPathDocument(readerSport, XmlSpace.Preserve);
                XPathNavigator nav = doc.CreateNavigator();

                XPathExpression exprSport;
                exprSport = nav.Compile("/rsp/sports/sport");
                XPathNodeIterator iteratorSport = nav.Select(exprSport);

                while (iteratorSport.MoveNext())
                {

                    XPathNavigator _sportNameNavigator = iteratorSport.Current.Clone();
                    int feedContentSport = Convert.ToInt32(_sportNameNavigator.GetAttribute("feedContents", ""));
                    int sportID = int.Parse(_sportNameNavigator.GetAttribute("id", ""));

                    if (feedContentSport > 0)
                    {
                        // _sport = 
                        SportPinnacleParse(ref bet247xSport, urlPathLeague, urlPathFeed, iteratorSport);
                        String _sPortName = _sportNameNavigator.Value;


                    }
                }
            }catch(Exception ee){}
        }

        private void SportPinnacleParse(ref Bet247xSport bet247xSport,String urlPathLeague, String urlPathFeed, XPathNodeIterator iteratorSport)
        {
            //ISport _sport = new Sport();
            XPathNavigator _sportNameNavigator = iteratorSport.Current.Clone();
            int feedContentSport = Convert.ToInt32(_sportNameNavigator.GetAttribute("feedContents", ""));
            int _sportID = Convert.ToInt32(_sportNameNavigator.GetAttribute("id", ""));
            if (feedContentSport > 0)
            {
                // ISport _sport = new Sport();
               // _sport.ID = Convert.ToInt32(_sportNameNavigator.GetAttribute("id", ""));
              //  _sport.Name = _sportNameNavigator.Value;

                //Add sport to List
                // sports.Add(_sport);
                //league- event
                XmlTextReader readerLeague = new XmlTextReader(string.Format(urlPathLeague, _sportID));
                readerLeague.WhitespaceHandling = WhitespaceHandling.Significant;
                XPathDocument docLeague = new XPathDocument(readerLeague, XmlSpace.Preserve);
                XPathNavigator navLeague = docLeague.CreateNavigator();

                XPathExpression exprLeague;
                exprLeague = navLeague.Compile("/rsp/leagues/league");
                XPathNodeIterator iteratorLeague = navLeague.Select(exprLeague);
                // Loop all Leagues in each sport
                while (iteratorLeague.MoveNext())
                {
                   // Bet247xSportLeague _league = LeaguePinnacleParse(urlPathFeed, _sportID, _sportNameNavigator, iteratorLeague);
                   // if (_sport.Leagues == null)
                    {
                       // _sport.Leagues = new List<ILeague>();
                    }
                    //check league not null
                  //  if (_league != null)
                       // _sport.Leagues.Add(_league);
                }//
            }
           // return _sport;
        }
      /*  private Bet247xSportLeague LeaguePinnacleParse(String urlPathFeed, int sportID, XPathNavigator sportNameNavigator, XPathNodeIterator iteratorLeague)
        {
            Bet247xSportLeague _league = new Bet247xSportLeague();
            XPathNavigator _eventNameNavigator = iteratorLeague.Current.Clone();
            int feedContentLeague = Convert.ToInt32(sportNameNavigator.GetAttribute("feedContents", ""));
            if (feedContentLeague > 0)
            {
                //ILeague _league = new League();
                _league.ID = Convert.ToInt32(_eventNameNavigator.GetAttribute("id", ""));
                _league.LeagueName = _eventNameNavigator.Value;
                //Add the current League to the latest sport
                //  sports[sports.Count - 1].Leagues.Add(_league);
                try
                {
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
                            Bet247xSportMatch _match = MatchPinnacleParse(iteratorMatchEvent);
                            //if (_league.Matches == null)
                            //{
                            //    _league.Matches = new List<IMatch>();
                            //}
                            ////check match not null
                            //if (_match != null)
                            //    _league.Matches.Add(_match);
                        }
                    }
                }catch (Exception ee){
                    return null;
                }
            }

            return _league;
        }
        private Bet247xSportMatch MatchPinnacleParse(XPathNodeIterator iteratorMatchEvent)
        {
            XPathNavigator _matchEventNameNavigator = iteratorMatchEvent.Current.Clone();
            Bet247xSportMatch _match = new Bet247xSportMatch();
            // Loop all in Periods
            if (_matchEventNameNavigator.HasChildren)
            {
                XPathExpression exprebet;
                exprebet = _matchEventNameNavigator.Compile("periods/period");
                XPathNodeIterator iteratorBet = _matchEventNameNavigator.Select(exprebet);
                // Add the match into current League
                _match.ID = Convert.ToInt32(_matchEventNameNavigator.SelectSingleNode("id").ToString()); ;
                //_match.eventId = _eventId;
                //_match.nameMatch = _matchNameNavigator.GetAttribute("name", "");
                _match.HomeTeam = _matchEventNameNavigator.SelectSingleNode("homeTeam").SelectSingleNode("name").Value;
                _match.AwayTeam = _matchEventNameNavigator.SelectSingleNode("awayTeam").SelectSingleNode("name").Value;
                _match.StartDateTime = Convert.ToDateTime(_matchEventNameNavigator.SelectSingleNode("startDateTime").Value);
                while (iteratorBet.MoveNext())
                {
                    XPathNavigator _betNameNavigator = iteratorBet.Current.Clone();
                    //IPeriod _period = PeriodPinnacleParse(_betNameNavigator, iteratorBet); // PeriodPinnacleParse(_matchEventNameNavigator, iteratorBet);
           
                    //handicap
                    XPathExpression exprehandicap;
                    exprehandicap = _betNameNavigator.Compile("spreads/spread");
                    XPathNodeIterator iteratorHandicap = _betNameNavigator.Select(exprehandicap);
                    //total
                    XPathExpression expretotal;
                    expretotal = _betNameNavigator.Compile("totals/total");
                    XPathNodeIterator iteratorTotal = _betNameNavigator.Select(expretotal);
                    //moneyline
                    XPathExpression expremoneyline;
                    expremoneyline = _betNameNavigator.Compile("moneyLine");
                    XPathNodeIterator iteratorMoneyLine = _betNameNavigator.Select(expremoneyline);
                    //while (iteratorHandicap.MoveNext())
                    //{
                    //    ISpread _spread = SpreadPinnacleParse(iteratorHandicap);
                    //    if (_period.Spreads == null)
                    //    {
                    //        _period.Spreads = new List<ISpread>();
                    //    }
                    //    _period.Spreads.Add(_spread);
                    //}

                    //while (iteratorTotal.MoveNext())
                    //{
                    //    ITotal _total = TotalPinnacleParse(iteratorTotal);
                    //    if (_period.Totals == null)
                    //    {
                    //        _period.Totals = new List<ITotal>();
                    //    }
                    //    _period.Totals.Add(_total);

                    //}

                    //while (iteratorMoneyLine.MoveNext())
                    //{
                    //    IMoneyLine _moneyline = MoneyLinePinnacleParse(iteratorMoneyLine);
                    //    if (_period.MoneyLines == null)
                    //    {
                    //        _period.MoneyLines = new List<IMoneyLine>();
                    //    }
                    //    _period.MoneyLines.Add(_moneyline);
                    //}

                    //if (_period.Totals != null && _period.MoneyLines != null && _period.Spreads != null)
                    //    _period.Description = _betNameNavigator.SelectSingleNode("description").Value;
                }
            }

           
               
           

            return _match;
        }*/

#endregion
        #region Render UI
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String RenderJson()
        {
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



