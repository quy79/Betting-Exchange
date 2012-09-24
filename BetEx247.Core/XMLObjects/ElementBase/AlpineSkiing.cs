using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLBettingObjects.ElementsBase.Interface;
using XMLBettingObjects.Event;
namespace XMLBettingObjects.ElementsBase
{
    class AlpineSkiing:BaseSport
    {
       
        public AlpineSkiing()
        {
        }
        public AlpineSkiing(int id, String name, List<EventObject> events):base( id,  name,  events)
        {
           
        }
    }
}
