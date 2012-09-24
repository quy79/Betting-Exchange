using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLBettingObjects.ElementsBase.Interface;
using XMLBettingObjects.Event;
namespace XMLBettingObjects.ElementsBase
{
    class BaseSport:IBaseInterface
    {
        private String name;
        private int id;
        private List<EventObject> events;
        public BaseSport()
        {
        }
        public BaseSport(int id, String name, List<EventObject> events)
        {
            this.id = id;
            this.name = name;
            this.events = events;
        }
        public int ID
        {
            set { id = value; }
            get { return id; }
        }
      
        public String Name
        {
            set { name = value; }
            get { return name; }
        }
        public List<EventObject> Events
        {
            set { events = value; }
            get { return events; }
        }
        public EventObject SearchEvent(int id)
        {
            if (Events == null)
            {
                return null;
            }
            foreach (EventObject ev in Events)
            {
                if (ev.ID == id)
                {
                    return ev;
                }
            }
            return null;
        }
        public EventObject SearchEvent(String  name)
        {
            if (Events == null)
            {
                return null;
            }
            foreach (EventObject ev in Events)
            {
                if (ev.Name.Equals(name))
                {
                    return ev;
                }
            }
            return null;
        }
       
    }
}
