using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.Payment
{
    /// <summary>
    /// Represents a recurring payment type
    /// </summary>
    public enum RecurringPaymentTypeEnum : int
    {
        /// <summary>
        /// Not supported
        /// </summary>
        NotSupported = 0,
        /// <summary>
        /// Manual
        /// </summary>
        Manual = 10,
        /// <summary>
        /// Automatic (payment is processed on payment gateway site)
        /// </summary>
        Automatic = 20
    }
}
