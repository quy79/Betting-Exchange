using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Common.Utils;
using BetEx247.Core;

namespace BetEx247.Plugin.DownloadFeed
{
    public class GoalServeFeed
    {
        public void DownloadXML(String url,String country)
        {
            try
            {
                string downloadTime = country;// +"_" + DateTime.Now.Ticks.ToString();
                CommonHelper.DownloadXML(url, Constant.SourceXML.GOALSERVE, null, downloadTime);
            }
            catch (Exception e)
            {
            }
        }

        public void DownloadOtherSportXML(String url, String country, int sportID)
        {
            try
            {
                string downloadTime = country+"_sport_"+sportID;// +"_" + DateTime.Now.Ticks.ToString();
                CommonHelper.DownloadXML(url, Constant.SourceXML.PINNACLESPORTS, null, downloadTime);
            }
            catch (Exception e)
            {
            }
        }
    }
}
