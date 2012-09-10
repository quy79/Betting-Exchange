using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Infrastructure;
using BetEx247.Core;

namespace BetEx247.Data.DAL
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
        /// Gets or sets the TransactionPayment identifier
        /// </summary>
        public int TransactionPaymentId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOn { get; set; }

        #endregion

        #region Custom Properties
        /// <summary>
        /// Gets the initial TransactionPayment
        /// </summary>
        public TransactionPayment PaymentTransaction
        {
            get
            {
                return IoC.Resolve<ITransactionPaymentService>().GetTransactionPaymentById(this.TransactionPaymentId);
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
