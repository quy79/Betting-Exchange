using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLBettingObjects.ElementsBase;
using XMLBettingObjects.Event;
namespace XMLBettingObjects.ElementsBase
{
    class Formula1:BaseSport
    {
         public Formula1()
        {
        }
         public Formula1(int id, String name, List<EventObject> events)
             : base(id, name, events)
        {
           
        }
    }
}
