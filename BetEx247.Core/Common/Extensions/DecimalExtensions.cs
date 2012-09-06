using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.Common.Extensions
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// Because of ToString("#,##0.##") show incorrect in case .00 , it returns an empty string.
        /// So we use this function to display fees with 2 decimal places
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string ToFeeFormat(this decimal me)
        {
            return me.ToString("N");  
        }
    }
}
