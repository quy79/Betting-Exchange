using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.Payment
{
    /// <summary>
    /// Represents a payment method type
    /// </summary>
    public enum PaymentMethodTypeEnum : int
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Standard
        /// </summary>
        Standard = 10,
        /// <summary>
        /// Button
        /// </summary>
        Button = 20,
    }
}
