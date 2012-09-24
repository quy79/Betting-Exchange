using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLBettingObjects.Match;
namespace XMLBettingObjects.Event.Interface
{
    interface IEventInterface
    {
         int ID { get; set; }
         String Name { get; set; }
         List<MatchObject> Matches { get; set; }
         MatchObject SearchMatch(int id);
         MatchObject SearchMatch(String name);
         MatchObject SearchMatch(DateTime startDate);
         MatchObject SearchMatchByLiveID(int liveID);
         MatchObject SearchMatchByStreaming(int streaming); 
    }
}
