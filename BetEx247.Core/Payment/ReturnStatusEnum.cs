using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.Payment
{
    /// <summary>
    /// Represents a return status
    /// </summary>
    public enum ReturnStatusEnum : int
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 0,
        /// <summary>
        /// Received
        /// </summary>
        Received = 10,
        /// <summary>
        /// Return authorized
        /// </summary>
        ReturnAuthorized = 20,
        /// <summary>
        /// Item(s) repaired
        /// </summary>
        ItemsRepaired = 30,
        /// <summary>
        /// Item(s) refunded
        /// </summary>
        ItemsRefunded = 40,
        /// <summary>
        /// Request rejected
        /// </summary>
        RequestRejected = 50,
        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 60,
    }
}
