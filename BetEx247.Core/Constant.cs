using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core
{
    public static class Constant
    {
        public static class SourceXML
        {
            public const string BETCLICK = "betclick";
            public const string PINNACLESPORTS = "pinnaclesports";
            public const string TITANBET = "titanbet";

            public const string BETCLICKURL = "http://xml.betclick.com/odds_en.xml";//"D:/Project/PN Technologies/BetEx247/BetEx247.Web/App_Data/odds_en.xml";
            public const string PINNACLESPORTSURL = "http://api.pinnaclesports.com/v1/sports";
            public const string PINNACLELEAGUEURL = "http://api.pinnaclesports.com/v1/leagues?sportid={0}";
            public const string PINNACLEFEEDURL = "http://api.pinnaclesports.com/v1/feed?sportid={0}&leagueid={1}&clientid=PN514368&apikey=4235dc98-c16d-45f7-a74d-d68861e80a47&islive=0&currencycode=usd";
            public const string TITABETURL = "http://cachefeeds.titanbet.com/feed.xml";//"D:/Project/PN Technologies/BetEx247/BetEx247.Web/App_Data/feed.xml";
        }

        public static class PlaceFolder
        {
            public const string BETCLICK_FOLDER = "App_Data/XMLFEED/BETCLICK";
            public const string PINNACLESPORTS_FOLDER = "App_Data/XMLFEED/PINNACLE";
            public const string TITABET_FOLDER = "App_Data/XMLFEED/TITANBET";
        }

        public enum BetType
        {
            MONEY_LINE = 154,
            MATCH_ODD = 1,
            OUTRIGHT = 67,
            DRAW_NO_BET = 120,
            _1ST_HALF_RESULT = 5,
            TOTAL_GOAL = 2,
            _1ST_HALF_TOTAL_GOAL = 6,
            ASIAN_HANDICAP = 277,
            _1ST_HALF_ASIAN_HANDICAP = 278,
            CORRECT_SCORE = 28,
            _1ST_HALF_CORRECT_SCORE = 78,
            HT_FT = 39,
            ODD_EVEN = 269,
            _1ST_HALF_ODD_EVEN = 439,
            WINNING_MARGIN = 155,
            TOTAL_HOME_GOAL_OU = 433,
            TOTAL_AWAY_GOAL_OU = 434,
            HOME_GOAL_OE = 463,
            AWAY_GOAL_OE = 464,
            TEAM_TO_SCORE_LAST_GOAL = 37,
            GAME = 5000,
            OTHER = 5001,
        }

        public enum ChoiceType
        {
            MONEY_LINE = 1,
            HANDICAP = 2,
            TOTAL = 3,
            OTHER = 4,
        }

        public static class Payment
        {
            public const string AUTHORIZENETLOGINID = "97U9wFsx62";
            public const string AUTHORIZENETTRANSACTION = "359ccj9fM2FC5XLc";
            public const bool AUTHORIZESANBOX = false;

            public const string MONEYBOOKERPAYMENTEMAIL = "chantinh2204@gmail.com";

            public const string STORENAME = "BetEx247";
            public const string CURRENCYCODE = "USD";
        }
    }
}
