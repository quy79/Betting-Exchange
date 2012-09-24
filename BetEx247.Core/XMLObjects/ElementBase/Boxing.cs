using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLBettingObjects.ElementsBase;
using XMLBettingObjects.Event;
namespace XMLBettingObjects.ElementsBase
{
    class Boxing:BaseSport
    {
         public Boxing()
        {
        }
         public Boxing(int id, String name, List<EventObject> events)
             : base(id, name, events)
        {
           
        }
    }
}
