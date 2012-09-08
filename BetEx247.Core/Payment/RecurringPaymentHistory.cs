using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.CustomerManagement;
using BetEx247.Core.Infrastructure;

namespace BetEx247.Core.Payment
{
    /// <summary>
    /// Represents a recurring payment history
    /// </summary>
    public partial class RecurringPaymentHistory : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the recurring payment history identifier
        /// </summary>
        public int RecurringPaymentHistoryId { get; set; }

        /// <summary>
        /// Gets or sets the recurring payment identifier
        /// </summary>
        public int RecurringPaymentId { get; set; }

        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOn { get; set; }

        #endregion

        #region Custom Properties
        /// <summary>
        /// Gets the initial order
        /// </summary>
        public Order Order
        {
            get
            {
                return IoC.Resolve<IOrderService>().GetOrderById(this.OrderId);
            }
        }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// Gets the recurring payment
        /// </summary>
        public virtual RecurringPayment NpRecurringPayment { get; set; }

        #endregion
    }
}
