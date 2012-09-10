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
        #region Credit cards
        /// <summary>
        /// Gets a credit card type
        /// </summary>
        /// <param name="creditCardTypeId">Credit card type identifier</param>
        /// <returns>Credit card type</returns>
        CreditCardType GetCreditCardTypeById(int creditCardTypeId);

        /// <summary>
        /// Marks a credit card type as deleted
        /// </summary>
        /// <param name="creditCardTypeId">Credit card type identifier</param>
        void MarkCreditCardTypeAsDeleted(int creditCardTypeId);

        /// <summary>
        /// Gets all credit card types
        /// </summary>
        /// <returns>Credit card type collection</returns>
        List<CreditCardType> GetAllCreditCardTypes();

        /// <summary>
        /// Inserts a credit card type
        /// </summary>
        /// <param name="creditCardType">Credit card type</param>
        void InsertCreditCardType(CreditCardType creditCardType);

        /// <summary>
        /// Updates the credit card type
        /// </summary>
        /// <param name="creditCardType">Credit card type</param>
        void UpdateCreditCardType(CreditCardType creditCardType);

        #endregion

        #region Payment methods

        /// <summary>
        /// Deletes a payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        void DeletePaymentMethod(int paymentMethodId);

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>Payment method</returns>
        PaymentMethod GetPaymentMethodById(int paymentMethodId);

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="systemKeyword">Payment method system keyword</param>
        /// <returns>Payment method</returns>
        PaymentMethod GetPaymentMethodBySystemKeyword(string systemKeyword);

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <returns>Payment method collection</returns>
        List<PaymentMethod> GetAllPaymentMethods();

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <param name="filterByCountryId">The country indentifier</param>
        /// <returns>Payment method collection</returns>
        List<PaymentMethod> GetAllPaymentMethods(int? filterByCountryId);

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <param name="filterByCountryId">The country indentifier</param>
        /// <param name="showHidden">A value indicating whether the not active payment methods should be load</param>
        /// <returns>Payment method collection</returns>
        List<PaymentMethod> GetAllPaymentMethods(int? filterByCountryId, bool showHidden);

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
        /// Gets additional handling fee
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>Additional handling fee</returns>
        decimal GetAdditionalHandlingFee(int paymentMethodId);

        /// <summary>
        /// Gets a value indicating whether capture is supported by payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A value indicating whether capture is supported</returns>
        bool CanCapture(int paymentMethodId);

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
        bool CanPartiallyRefund(int paymentMethodId);

        /// <summary>
        /// Gets a value indicating whether refund is supported by payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A value indicating whether refund is supported</returns>
        bool CanRefund(int paymentMethodId);

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
        bool CanVoid(int paymentMethodId);

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
        RecurringPaymentTypeEnum SupportRecurringPayments(int paymentMethodId);

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A payment method type</returns>
        PaymentMethodTypeEnum GetPaymentMethodType(int paymentMethodId);

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
