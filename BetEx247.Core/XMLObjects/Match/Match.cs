using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XMLObjects.Match.Period.Interface;
using BetEx247.Core.XMLObjects.Match.Interface;
using BetEx247.Core.XMLObjects.Market.Interface;
namespace BetEx247.Core.XMLObjects.Match
{
     [Serializable]
   public  class Match:IMatch
    {
         private int id = 0;
        private string name = string.Empty;
        private List<IPeriod> periods ;
        private List<IMarket> markets;



        private DateTime startDateTime;
        private bool liveID;
        private String status;
        private String homeTeam;
        private String awayTeam;
     

        public Match()
        {
       
        }
         public int ID
        {
            get { return id; }
            set { id = value; }
        }
         public String Name
        {
            get { return name; }
            set { name = value; }
        }
         public List<IPeriod> Periods
        {
            get { return periods; }
            set { periods = value; }
        }
         public List<IMarket> Markets
         {
             get { return markets; }
             set { markets = value; }
         }
         public DateTime StartDateTime
         {
             get { return startDateTime; }
             set { startDateTime = value; }
         }
         public bool LiveID
         {
             get { return liveID; }
             set { liveID = value; }
         }
         public String Status
         {
             get { return status; }
             set { status = value; }
         }
         public String HomeTeam
         {
             get { return homeTeam; }
             set { homeTeam = value; }
         }
         public String AwayTeam
         {
             get { return awayTeam; }
             set { awayTeam = value; }
         }
    }
}
