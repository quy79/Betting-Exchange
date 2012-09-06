using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BetEx247.Core
{
    public static class Constant
    {
        public static class SourceXML
        {
            public const string BETCLICK = "betclick";
            public const string PINNACLESPORTS = "pinnaclesports";
            public const string TITANBET = "titanbet";

            public static string BETCLICKURL = ConfigurationManager.AppSettings["BETCLICKURL"];//"D:/Project/PN Technologies/BetEx247/BetEx247.Web/App_Data/odds_en.xml";
            public static string PINNACLESPORTSURL = ConfigurationManager.AppSettings["PINNACLESPORTSURL"].ToString();
            public static string PINNACLELEAGUEURL = ConfigurationManager.AppSettings["PINNACLELEAGUEURL"].ToString();
            public static string PINNACLEFEEDURL = ConfigurationManager.AppSettings["PINNACLEFEEDURL"].ToString();
            public static string TITABETURL = "D:/Project/PN Technologies/BetEx247/BetEx247.Web/App_Data/feed.xml";// ConfigurationManager.AppSettings["TITABETURL"].ToString();//
        }

        public static class TitanBetOddTypeID
        {
            public static string CRICKET = ConfigurationManager.AppSettings["TITANBETMAINSPORT_CRICKET"];
            public static string HANDBALL = ConfigurationManager.AppSettings["TITANBETMAINSPORT_HANDBALL"];
            public static string HORSERACING = ConfigurationManager.AppSettings["TITANBETMAINSPORT_HORSERACING"];
            public static string MOTORSPORTS = ConfigurationManager.AppSettings["TITANBETMAINSPORT_MOTORSPORTS"];
            public static string BOXING = ConfigurationManager.AppSettings["TITANBETMAINSPORT_BOXING"];
            public static string GOLF = ConfigurationManager.AppSettings["TITANBETMAINSPORT_GOLF"];
            public static string TENNIS = ConfigurationManager.AppSettings["TITANBETMAINSPORT_TENNIS"];
            public static string FOOTBALL = ConfigurationManager.AppSettings["TITANBETMAINSPORT_FOOTBALL"];
        }

        public static class TitanBetOddType
        {
            public enum Cricket
            {
                _2WayMatchBetting = 120,
                TotalRunOuts = 272
            }

            public enum Handball
            {
                _3Way = 1
            }

            public enum HorseRacing
            {
                RaceWinner = 396
            }

            public enum MotorSports
            {
                Outright = 67
            }

            public enum Boxing
            {
                _2WayMatchBetting = 67,
                Total_Rounds = 107
            }

            public enum Golf
            {
                OutrightWinner = 148
            }

            public enum Tennis
            {
                MatchWinner = 120,
                Set1Winner = 121,
                TotalGames = 268,
                GameHandicap = 255
            }

            public enum Football
            {
                MatchResult = 1,
                DrawNoBet = 120,
                TotalGoals_O_U = 2,
                Additional_O_U = 451,
                AsianHandicap = 277,
                CorrectScore = 28,
                _1stHalfResult = 5,
                _1stHalfGoals_O_U = 6,
                _1stHalfAsianHandicap = 278,
                _1stHalfCorrectScore = 78,
                WinningMargin = 155,
                Odd_Even_GoalsTotal = 269,
                _1stHalfGoals_O_E = 439
            }
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
