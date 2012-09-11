using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Payment;
using BetEx247.Data.Model;

namespace BetEx247.Data.DAL
{
    /// <summary>
    /// Payment service interface
    /// </summary>
    public partial interface IPaymentService
    {
        #region Payment methods

        /// <summary>
        /// Deletes a payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        void DeletePaymentMethod(long paymentMethodId);

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>Payment method</returns>
        PaymentMethod GetPaymentMethodById(long paymentMethodId);   
        
        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <returns>Payment method collection</returns>
        List<PaymentMethod> GetAllPaymentMethods();
        
        /// <summary>
        /// Inserts a payment method
        /// </summary>
        /// <param name="paymentMethod">Payment method</param>
        void InsertPaymentMethod(PaymentMethod paymentMethod);

        /// <summary>
        /// Updates the payment method
        /// </summary>
        /// <param name="paymentMethod">Payment method</param>
        void UpdatePaymentMethod(PaymentMethod paymentMethod);
        #endregion

        #region Workflow

        /// <summary>
        /// Process payment
        /// </summary>
        /// <param name="paymentInfo">Payment info required for an transactionPayment processing</param>
        /// <param name="member">member</param>
        /// <param name="transactionPaymentGuid">Unique transactionPayment identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        void ProcessPayment(TransactionPayment transactionPayment, Member member,
            Guid transactionPaymentGuid, ref ProcessPaymentResult processPaymentResult);

        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        string PostProcessPayment(TransactionPayment transactionPayment);

        /// <summary>
        /// Gets a value indicating whether capture is supported by payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A value indicating whether capture is supported</returns>
        bool CanCapture(long paymentMethodId);

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="processPaymentResult">Process payment result</param>
        void Capture(TransactionPayment transactionPayment, ref ProcessPaymentResult processPaymentResult);

        /// <summary>
        /// Gets a value indicating whether partial refund is supported by payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A value indicating whether partial refund is supported</returns>
        bool CanPartiallyRefund(long paymentMethodId);

        /// <summary>
        /// Gets a value indicating whether refund is supported by payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A value indicating whether refund is supported</returns>
        bool CanRefund(long paymentMethodId);

        /// <summary>
        /// Refunds payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>
        void Refund(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult);

        /// <summary>
        /// Gets a value indicating whether void is supported by payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A value indicating whether void is supported</returns>
        bool CanVoid(long paymentMethodId);

        /// <summary>
        /// Voids payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>
        void Void(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult);

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A recurring payment type of payment method</returns>
        RecurringPaymentTypeEnum SupportRecurringPayments(long paymentMethodId);

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A payment method type</returns>
        PaymentMethodTypeEnum GetPaymentMethodType(long paymentMethodId);

        /// <summary>
        /// Process recurring payments
        /// </summary>
        /// <param name="paymentInfo">Payment info required for an transactionPayment processing</param>
        /// <param name="member">member</param>
        /// <param name="transactionPaymentGuid">Unique transactionPayment identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        void ProcessRecurringPayment(TransactionPayment transactionPayment, Member member,
            Guid transactionPaymentGuid, ref ProcessPaymentResult processPaymentResult);

        /// <summary>
        /// Cancels recurring payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>
        void CancelRecurringPayment(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult);

        /// <summary>
        /// Gets masked credit card number
        /// </summary>
        /// <param name="creditCardNumber">Credit card number</param>
        /// <returns>Masked credit card number</returns>
        string GetMaskedCreditCardNumber(string creditCardNumber);

        #endregion
    }
}
