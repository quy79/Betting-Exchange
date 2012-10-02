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
            public static string TITABETURL = ConfigurationManager.AppSettings["TITABETURL"].ToString();//"D:/Project/PN Technologies/BetEx247/BetEx247.Web/App_Data/feed.xml";// 
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

        public enum SourceFeedType
        {
            BETCLICK = 1,
            PINNACLESPORTS = 2,
            TITANBET = 3
        }
        /// <summary>
        /// Selelct sport type
        /// </summary>
        public enum SportType
        {
            EMPTY = 0,
            BADMINTON = 1,
            BASEBALL = 3,
            BASKETBALL = 4,
            BOXING = 6,
            CRICKET = 8,
            CURLING = 9,
            DARTS = 10,
            DARTS_LEGS = 11,
            E_SPORTS = 12,
            FLOORBALL = 14,
            FOOTBALL = 15,
            FUTSAL = 16,
            GOLF = 17,
            HANDBALL = 18,
            HOCKEY = 19,
            HORSE_RACING = 20,
            MIXED_MARTIAL_ARTS = 22,
            OTHER_SPORTS = 23,
            POLITICS = 24,
            RUGBY_LEAGUE = 26,
            RUGBY_UNION = 27,
            SNOOKER = 28,
            SOCCER = 29,
            SOFTBALL = 30,
            SQUASH = 31,
            TABLE_TENNIS = 32,
            TENNIS = 33,
            VOLLEYBALL = 34,
            VOLLEYBALL_POINTS = 35,
            WATER_POLO = 36,
            AUSSIE_RULES = 39,
            MOTOR_SPORTS = 41


        }
        //public enum SportName
        //{
        //    [StringValue("Cordless Power Drill")]
        //    BADMINTON = Badminton,
        //    BASEBALL = "Baseball",
        //    BASKETBALL = "Basketball",
        //    BOXING = "Boxing",
        //    CRICKET = "Cricket",
        //    CURLING = "Curling",
        //    DARTS = "Darts",
        //    DARTS_LEGS = "Darts (Legs)",
        //    E_SPORTS = "E Sports ",
        //    FLOORBALL = "Floorball",
        //    FOOTBALL = "Football",
        //    FUTSAL = "Futsal",
        //    GOLF = "Golf",
        //    HANDBALL = "Handball",
        //    HOCKEY = "Hockey",
        //    HORSE_RACING = "Horse Racing",
        //    MIXED_MARTIAL_ARTS = "Mixed Martial Arts",
        //    OTHER_SPORTS = "Other Sports",
        //    POLITICS = "Politics",
        //    RUGBY_LEAGUE = "Rugby League",
        //    RUGBY_UNION = "Rugby Union",
        //    SNOOKER = "Snooker",
        //    SOCCER = "Soccer",
        //    SOFTBALL = "Softball",
        //    SQUASH = "Squash",
        //    TABLE_TENNIS = "Table Tennis ",
        //    TENNIS = "Tennis",
        //    VOLLEYBALL = "Volleyball",
        //    VOLLEYBALL_POINTS = "Volleyball (Points)",
        //    WATER_POLO = "Water Polo",
        //    AUSSIE_RULES = "Aussie Rules",
        //    MOTOR_SPORTS = "Motor Sports"
        //}

        public static class Status
        {
            public const string ACTIVE = "ACTIVE";
            public const string INACTIVE = "INACTIVE";
            public const string DELETED = "DELETED";
            public const string NEW = "NEW";
            public const byte ACTIVENUM = 1;
            public const byte INACTIVENUM = 2;
        }

        public static class SEOLinkPage
        {
            public static string MEMBER_LOGIN { get { return ""; } }
        }
    }
    
}
