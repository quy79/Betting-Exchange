using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.XML
{
    public partial class Sport
    {
        private int _sportid = 0;
        private string _sportname = string.Empty;

        public int sportId
        {
            get { return _sportid; }
            set { _sportid = value; }
        }

        public string sportName
        {
            get { return _sportname; }
            set { _sportname = value; }
        }
    }

    public partial class Event
    {
        private int _eventid = 0;
        private int _sportid = 0;
        private string _eventname = string.Empty;

        public int eventId
        {
            get { return _eventid; }
            set { _eventid = value; }
        }
        public int sportId
        {
            get { return _sportid; }
            set { _sportid = value; }
        }
        public string eventName
        {
            get { return _eventname; }
            set { _eventname = value; }
        }
    }

    public partial class Match
    {
        private long _matchId = 0;
        private int _eventId = 0;
        //private string _nameMatch = string.Empty;
        private string _homeTeam = string.Empty;
        private string _awayTeam = string.Empty;
        private DateTime _startTime = new DateTime();

        public long matchId
        {
            get { return _matchId; }
            set { _matchId = value; }
        }
        public int eventId
        {
            get { return _eventId; }
            set { _eventId = value; }
        }
        //public string nameMatch
        //{
        //    get { return _nameMatch; }
        //    set { _nameMatch = value; }
        //}
        public string homeTeam
        {
            get { return _homeTeam; }
            set { _homeTeam = value; }
        }
        public string awayTeam { get { return _awayTeam; } set { _awayTeam = value; } }
        public DateTime startTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
    }

    public partial class Bet
    {
        private long _betId = 0;
        private long _matchId = 0;
        private string _betCode = string.Empty;
        private string _betName = string.Empty;

        public long betId
        {
            get { return _betId; }
            set { _betId = value; }
        }
        public long matchId
        {
            get { return _matchId; }
            set { _matchId = value; }
        }
        public string betCode
        {
            get { return _betCode; }
            set { _betCode = value; }
        }
        public string betName
        {
            get { return _betName; }
            set { _betName = value; }
        }
    }

    public partial class Choice
    {
        private long _choiceId = 0;
        private long _betId = 0;
        private string _choiceName = string.Empty;
        private string _odd = string.Empty;
        private string _american_odd = string.Empty;
        private string _fra_odd = string.Empty;

        public long choiceId
        {
            get { return _choiceId; }
            set { _choiceId = value; }
        }
        public long betId
        {
            get { return _betId; }
            set { _betId = value; }
        }
        public string choiceName
        {
            get { return _choiceName; }
            set { _choiceName = value; }
        }
        public string odd
        {
            get { return _odd; }
            set { _odd = value; }
        }
        public string american_odd 
        {
            get { return _american_odd;}
            set { _american_odd = value; } 
        }
        public string fra_odd 
        {
            get { return _fra_odd; }
            set { _fra_odd=value;} 
        }        
    }
}
