using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLBettingObjects.ElementsBase;
using XMLBettingObjects.Event;
namespace XMLBettingObjects.ElementsBase
{
    class Baseball:BaseSport
    {
          public Baseball()
        {
        }
          public Baseball(int id, String name, List<EventObject> events)
             : base(id, name, events)
        {
           
        }
    }
}
