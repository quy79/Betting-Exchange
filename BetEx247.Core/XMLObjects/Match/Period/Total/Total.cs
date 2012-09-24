using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XMLObjects.Match.Period.Total.Interface;
namespace BetEx247.Core.XMLObjects.Match.Period.Total
{
    /// <summary>
    /// Implement ITotal 
    /// </summary>
    /// 
     [Serializable]
   public class Total :ITotal
    {
        private float point;
        private float overPrice;
        private float underPrice;
        /// <summary>
        /// Init without any parammater
        /// </summary>
        public Total() { }
        /// <summary>
        /// Init and pass poit paramater
        /// </summary>
        /// <param name="point"></param>
        Total(float point) {
            Point = point;
        }
        /// <summary>
        /// Init with point and overPrice
        /// </summary>
        /// <param name="point"></param>
        /// <param name="overPrice"></param>
        Total(float point, float overPrice) {
            Point = point;
            OverPrice = overPrice;
        }
        /// <summary>
        /// Init with All paramaters
        /// </summary>
        /// <param name="point"></param>
        /// <param name="overPrice"></param>
        /// <param name="underPrice"></param>
        Total(float point, float overPrice, float underPrice) {
            Point = point;
            OverPrice = overPrice;
            UnderPrice = underPrice;
        }
        /// <summary>
        /// Set/Get Point
        /// </summary>
        public float Point
        {
            set { point = value; }
            get { return point; }
        }
        /// <summary>
        /// Set/Get OverPrice
        /// </summary>
        public float OverPrice
        {
            set { overPrice = value; }
            get { return overPrice; }
        }
        /// <summary>
        /// Set/Get Underprice
        /// </summary>
        public float UnderPrice
        {
            set { underPrice = value; }
            get { return underPrice; }
        }
    }
}
