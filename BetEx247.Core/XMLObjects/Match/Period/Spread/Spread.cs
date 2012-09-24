using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XMLObjects.Match.Period.Spread.Interface;
namespace BetEx247.Core.XMLObjects.Match.Period.Spread
{
     [Serializable]
   public class Spread:ISpread
    {
        private float awaySpread;
        private float awayPrice;
        private float homeSpread;
        private float homePrice;
        /// <summary>
        /// Without paramater
        /// </summary>
        public Spread(){}
        /// <summary>
        /// Imit with all paramaters
        /// </summary>
        /// <param name="awaySpread"></param>
        /// <param name="awayPrice"></param>
        /// <param name="homeSpread"></param>
        /// <param name="homePrice"></param>
        Spread(float awaySpread,float awayPrice,float homeSpread,float homePrice) {
            AwaySpread = awaySpread;
            AwayPrice = awayPrice;
            HomeSpread = homeSpread;
            HomePrice = homePrice;
        }

        /// <summary>
        /// Set/Get awayspread
        /// </summary>
        public float AwaySpread
        {
            set { awaySpread = value; }
            get { return awaySpread; }
        }
        /// <summary>
        /// Set/Get awayprice
        /// </summary>
        public float AwayPrice
        {
            set { awayPrice = value; }
            get { return awayPrice; }
        }
        /// <summary>
        /// Set/Get homeSpread
        /// </summary>
        public float HomeSpread
        {
            set { homeSpread = value; }
            get { return homeSpread; }
        }
        /// <summary>
        /// Set/Get homePrice
        /// </summary>
        public float HomePrice
        {
            set { homePrice = value; }
            get { return homePrice; }
        }
    }
}
