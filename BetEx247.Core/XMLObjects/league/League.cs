using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core;
using BetEx247.Core.XMLObjects.Match.Interface;
using BetEx247.Core.XMLObjects.League.Interface;
using BetEx247.Core.XMLObjects.Market.Interface;
namespace BetEx247.Core.XMLObjects.League
{
     [Serializable]
   public class League:ILeague
    {
         private int id = 0;
        private string name = string.Empty;
        private List<IMatch> matches ;
       // private List<IMarket> markets;
        private Constant.SourceFeedType  sourceFeedType;
        public League()
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
         public List<IMatch>  Matches
        {
             
            get { return matches; }
            set { matches = value; }
        }
        // public List<IMarket>  Markets
        //{
             
        //    get { return markets; }
        //    set { markets = value; }
        //}
         public Constant.SourceFeedType SourceFeedType
         {
             get { return sourceFeedType; }
             set { sourceFeedType = value; }
         }
    }
}
