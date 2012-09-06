using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Common.Utils;
using BetEx247.Core;
using System.Xml;
using System.Xml.XPath;

namespace BetEx247.Plugin.DownloadFeed
{
    public class PinnaclesportsFeed : IDownloadFeed
    {
        public void DownloadXML()
        {
            try
            {
                string downloadTime = DateTime.Now.Ticks.ToString();
                string urlPathSport = Constant.SourceXML.PINNACLESPORTSURL;
                string urlPathLeague = Constant.SourceXML.PINNACLELEAGUEURL;
                string urlPathFeed = Constant.SourceXML.PINNACLEFEEDURL;
                //download sport xml
                CommonHelper.DownloadXML(urlPathSport, Constant.SourceXML.PINNACLESPORTS, 1,downloadTime);
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
                    int _leagueId = 0;

                    while (iteratorSport.MoveNext())
                    {
                        XPathNavigator _sportNameNavigator = iteratorSport.Current.Clone();
                        _sportId = Convert.ToInt32(_sportNameNavigator.GetAttribute("id", ""));
                        //download league
                        CommonHelper.DownloadXML(string.Format(urlPathLeague, _sportId), Constant.SourceXML.PINNACLESPORTS, 2,downloadTime);
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
                            _leagueId = Convert.ToInt32(_eventNameNavigator.GetAttribute("id", ""));
                            //download feed
                            CommonHelper.DownloadXML(string.Format(urlPathFeed, _sportId, _leagueId), Constant.SourceXML.PINNACLESPORTS, 3,downloadTime);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw new Exception(ex.Message);
                }
            }
            catch { }
        }
    }
}
