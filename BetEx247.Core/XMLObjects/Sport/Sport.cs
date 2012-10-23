using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BetEx247.Core.XMLObjects.Sport.Interface;
using BetEx247.Core.XMLObjects.League.Interface;

namespace BetEx247.Core.XMLObjects.Sport
{
    [Serializable()]
    public class Sport : ISport
    {
        private int id = 0;
        private string name = string.Empty;
        private List<ILeague> leagues;
        Constant.SourceFeedType sportFeedType;
        public Sport()
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
        public List<ILeague> Leagues
        {
            get { return leagues; }
            set { leagues = value; }
        }
        public Constant.SourceFeedType SportFeedType
        {
            get { return sportFeedType; }
            set { sportFeedType = value; }
        }
    }
}
