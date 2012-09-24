using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLBettingObjects.ElementsBase.Interface;
using XMLBettingObjects.Event;
namespace XMLBettingObjects.ElementsBase
{
    class Generic:BaseSport
    {
       
        public Generic()
        {
        }
        public Generic(int id, String name, List<EventObject> events)
            : base(id, name, events)
        {
           
        }
    }
}
