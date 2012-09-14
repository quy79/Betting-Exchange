using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Payment;
using BetEx247.Data.Model;

namespace BetEx247.Data.DAL
{
    /// <summary>
    /// Provides an interface for creating payment gateways
    /// </summary>
    public partial interface IPaymentMethod
    {
        #region Methods
        /// <summary>
        /// Process payment
        /// </summary>
        /// <param name="transactionPayment">Payment info required for an betting processing</param>
        /// <param name="bettingGuid">Unique betting identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        void ProcessPayment(TransactionPayment transactionPayment,
            Guid bettingGuid, ref ProcessPaymentResult processPaymentResult);

        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="betting">betting</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        string PostProcessPayment(TransactionPayment transactionPayment);

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="processPaymentResult">Process payment result</param>
        void Capture(TransactionPayment transactionPayment, ref ProcessPaymentResult processPaymentResult);

        /// <summary>
        /// Refunds payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>
        void Refund(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult);

        /// <summary>
        /// Voids paymen
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>
        void Void(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult);

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="transactionPayment">Payment info required for an payment processing</param>
        /// <param name="member">member</param>
        /// <param name="bettingGuid">Unique betting identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        void ProcessRecurringPayment(TransactionPayment transactionPayment,
            Guid bettingGuid, ref ProcessPaymentResult processPaymentResult);

        /// <summary>
        /// Cancels recurring payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>
        void CancelRecurringPayment(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult);

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        bool CanCapture { get; }

        /// <summary>
        /// Gets a value indicating whether partial refund is supported
        /// </summary>
        bool CanPartiallyRefund { get; }

        /// <summary>
        /// Gets a value indicating whether refund is supported
        /// </summary>
        bool CanRefund { get; }

        /// <summary>
        /// Gets a value indicating whether void is supported
        /// </summary>
        bool CanVoid { get; }

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        /// <returns>A recurring payment type of payment method</returns>
        RecurringPaymentTypeEnum SupportRecurringPayments { get; }

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        /// <returns>A payment method type</returns>
        PaymentMethodTypeEnum PaymentMethodType { get; }

        #endregion
    }
}
