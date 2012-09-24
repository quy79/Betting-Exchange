using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XMLObjects.Market.Outcome.Interface;
namespace BetEx247.Core.XMLObjects.Market.Outcome
{
     [Serializable]
    public  class Outcome:IOutcome
    {
        private int id = 0;
        private string name = string.Empty;
        private float odds;
        public Outcome()
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
         public float Odds
        {
            get { return odds; }
            set { odds = value; }
        }

    }
}
