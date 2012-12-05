using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
namespace BetEx247.Plugin.DataManager.Settle
{
    class SoccerMatch4Settle:SoccerMatch
    {
        private List<ScoreInfo> scoreInfoList;
        private List<CardsInfo> cardInfoList;

        public List<ScoreInfo> ScoreInfoList
        {
            get { return scoreInfoList; }
            set { scoreInfoList = value; }
        }
        

        public List<CardsInfo> CardInfoList
        {
            get { return cardInfoList; }
            set { cardInfoList = value; }
        }	

    }
}
