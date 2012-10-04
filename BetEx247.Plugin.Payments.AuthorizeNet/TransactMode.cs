using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Plugin.Payments.AuthorizeNet
{
    /// <summary>
    /// Represents an Authorize.NET payment processor transaction mode
    /// </summary>
    public enum TransactMode : byte
    {
        /// <summary>
        /// Authorize transaction mode
        /// </summary>
        Authorize = 1,
        /// <summary>
        /// Authorize and capture transaction mode
        /// </summary>
        AuthorizeAndCapture = 2
    }
}
