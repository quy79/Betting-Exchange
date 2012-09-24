using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XMLObjects.Match.Period.MoneyLine.Interface;
namespace BetEx247.Core.XMLObjects.Match.Period.MoneyLine
{
    /// <summary>
    /// This class used for moneyline
    /// </summary>
    /// 
     [Serializable]
   public  class MoneyLine:IMoneyLine
    {
        private float awayPrice;
        private float homePrice;
        private float drawPrice;
       
        /// <summary>
        /// Without paramater
        /// </summary>
        public MoneyLine(){}

        /// <summary>
        /// Imit with all paramaters
        /// </summary>
        /// <param name="awayPrice"></param>
        /// <param name="homePrice"></param>
        /// <param name="drawPrice"></param>
        MoneyLine(float awayPrice,float homePrice,float drawPrice) {
            AwayPrice = awayPrice;
            HomePrice = homePrice;
            DrawPrice = drawPrice;
           
        }

        /// <summary>
        /// Set/Get AwayPrice
        /// </summary>
        public float AwayPrice
        {
            set { awayPrice = value; }
            get { return awayPrice; }
        }
        /// <summary>
        /// Set/Get homePrice
        /// </summary>
        public float HomePrice
        {
            set { homePrice = value; }
            get { return homePrice; }
        }
        /// <summary>
        /// Set/Get homeSpread
        /// </summary>
        public float DrawPrice
        {
            set { drawPrice = value; }
            get { return drawPrice; }
        }
       
    }
}
