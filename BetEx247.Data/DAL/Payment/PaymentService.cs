using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Payment;
using BetEx247.Data.Model;
using BetEx247.Core.Common.Utils;
using BetEx247.Core.Infrastructure;

namespace BetEx247.Data.DAL
{
    /// <summary>
    /// Payment service
    /// </summary>
    public partial class PaymentService : IPaymentService
    {
        #region Constants
        private const string PAYMENTMETHODS_BY_ID_KEY = "paymentmethod.id-{0}";
        private const string PAYMENTMETHODS_PATTERN_KEY = "paymentmethod.";
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        #endregion

        #region Menthod
        /// <summary>
        /// Deletes a payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        public void DeletePaymentMethod(long paymentMethodId)
        {
            var paymentMethod = GetPaymentMethodById(paymentMethodId);
            if (paymentMethod == null)
                return;
            _context.DeleteObject(paymentMethod);
            _context.SaveChanges();
        }

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>Payment method</returns>
        public PaymentMethod GetPaymentMethodById(long paymentMethodId)
        {
            if (paymentMethodId == 0)
                return null;          
            var query = from pm in _context.PaymentMethods
                        where pm.ID == paymentMethodId
                        select pm;
            var paymentMethod = query.SingleOrDefault();
            return paymentMethod;
        }

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <returns>Payment method collection</returns>
        public List<PaymentMethod> GetAllPaymentMethods()
        {
            var query = from pm in _context.PaymentMethods
                        select pm;
            var paymentMethod = query.ToList();
            return paymentMethod;
        }

        /// <summary>
        /// Inserts a payment method
        /// </summary>
        /// <param name="paymentMethod">Payment method</param>
        public void InsertPaymentMethod(PaymentMethod paymentMethod)
        {
            _context.PaymentMethods.AddObject(paymentMethod);
            _context.SaveChanges();
        }

        public void UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
                throw new ArgumentNullException("paymentMethod");
            
            paymentMethod.Name = CommonHelper.EnsureNotNull(paymentMethod.Name);
            paymentMethod.Name = CommonHelper.EnsureMaximumLength(paymentMethod.Name, 100);
            paymentMethod.Description = CommonHelper.EnsureNotNull(paymentMethod.Description);
            paymentMethod.Description = CommonHelper.EnsureMaximumLength(paymentMethod.Description, 4000);
            paymentMethod.CreditCardNumber = CommonHelper.EnsureNotNull(paymentMethod.CreditCardNumber);
            paymentMethod.CreditCardNumber = CommonHelper.EnsureMaximumLength(paymentMethod.CreditCardNumber, 30);
            paymentMethod.NameOnCard = CommonHelper.EnsureNotNull(paymentMethod.NameOnCard);
            paymentMethod.NameOnCard = CommonHelper.EnsureMaximumLength(paymentMethod.NameOnCard, 100);
            paymentMethod.ValidFrom = paymentMethod.ValidFrom;
            paymentMethod.ValidTo = paymentMethod.ValidTo;
            paymentMethod.CardExpirationMonth = paymentMethod.CardExpirationMonth;
            paymentMethod.CardExpirationYear = paymentMethod.CardExpirationYear;
            paymentMethod.Address = paymentMethod.Address;
            paymentMethod.Country = paymentMethod.Country;
            paymentMethod.Zipcode = paymentMethod.Zipcode;
            paymentMethod.Email = paymentMethod.Email;
            paymentMethod.Bank = paymentMethod.Bank;
            paymentMethod.Branch = paymentMethod.Branch;
            paymentMethod.BranchCode = paymentMethod.BranchCode;
            paymentMethod.SwiftCode = paymentMethod.SwiftCode;
            paymentMethod.IBAN = paymentMethod.IBAN;
            paymentMethod.Verified = paymentMethod.Verified;

            _context.SaveChanges();
        }
        #endregion
        #region Workflow

        /// <summary>
        /// Process payment
        /// </summary>
        /// <param name="paymentInfo">Payment info required for an order processing</param>      
        /// <param name="orderGuid">Unique order identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void ProcessPayment(TransactionPayment paymentInfo,
            Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            if (paymentInfo.TransactionPaymentTotal == decimal.Zero)
            {
                processPaymentResult.Error = string.Empty;
                processPaymentResult.FullError = string.Empty;
                processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
            }
            else
            {
                var paymentMethod = GetPaymentMethodById(paymentInfo.PaymentMethodId);
                if (paymentMethod == null)
                    throw new Exception("Payment method couldn't be loaded");
                var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
                iPaymentMethod.ProcessPayment(paymentInfo, orderGuid, ref processPaymentResult);
            }
        }

        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public string PostProcessPayment(TransactionPayment transactionPayment)
        {
            var paymentMethod = GetPaymentMethodById(transactionPayment.PaymentMethodId);
            if (paymentMethod == null)
                throw new Exception("Payment method couldn't be loaded");
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.PostProcessPayment(transactionPayment);
        }

        /// <summary>
        /// Gets a value indicating whether capture is supported by payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A value indicating whether capture is supported</returns>
        public bool CanCapture(long paymentMethodId)
        {
            var paymentMethod = GetPaymentMethodById(paymentMethodId);
            if (paymentMethod == null)
                return false;
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.CanCapture;
        }

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="TransactionPayment">TransactionPayment</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void Capture(TransactionPayment transactionPayment, ref ProcessPaymentResult processPaymentResult)
        {
            var paymentMethod = GetPaymentMethodById(transactionPayment.PaymentMethodId);
            if (paymentMethod == null)
                throw new Exception("Payment method couldn't be loaded");
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            iPaymentMethod.Capture(transactionPayment, ref processPaymentResult);
        }

        /// <summary>
        /// Gets a value indicating whether partial refund is supported by payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A value indicating whether partial refund is supported</returns>
        public bool CanPartiallyRefund(long paymentMethodId)
        {
            var paymentMethod = GetPaymentMethodById(paymentMethodId);
            if (paymentMethod == null)
                return false;
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.CanPartiallyRefund;
        }

        /// <summary>
        /// Gets a value indicating whether refund is supported by payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A value indicating whether refund is supported</returns>
        public bool CanRefund(long paymentMethodId)
        {
            var paymentMethod = GetPaymentMethodById(paymentMethodId);
            if (paymentMethod == null)
                return false;
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.CanRefund;
        }

        /// <summary>
        /// Refunds payment
        /// </summary>
        /// <param name="TransactionPayment">TransactionPayment</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>
        public void Refund(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult)
        {
            var paymentMethod = GetPaymentMethodById(transactionPayment.PaymentMethodId);
            if (paymentMethod == null)
                throw new Exception("Payment method couldn't be loaded");
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            iPaymentMethod.Refund(transactionPayment, ref cancelPaymentResult);
        }

        /// <summary>
        /// Gets a value indicating whether void is supported by payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A value indicating whether void is supported</returns>
        public bool CanVoid(long paymentMethodId)
        {
            var paymentMethod = GetPaymentMethodById(paymentMethodId);
            if (paymentMethod == null)
                return false;
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.CanVoid;
        }

        /// <summary>
        /// Voids payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>
        public void Void(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult)
        {
            var paymentMethod = GetPaymentMethodById(transactionPayment.PaymentMethodId);
            if (paymentMethod == null)
                throw new Exception("Payment method couldn't be loaded");
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            iPaymentMethod.Void(transactionPayment, ref cancelPaymentResult);
        }

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A recurring payment type of payment method</returns>
        public RecurringPaymentTypeEnum SupportRecurringPayments(long paymentMethodId)
        {
            var paymentMethod = GetPaymentMethodById(paymentMethodId);
            if (paymentMethod == null)
                return RecurringPaymentTypeEnum.NotSupported;
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.SupportRecurringPayments;
        }

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>A payment method type</returns>
        public PaymentMethodTypeEnum GetPaymentMethodType(long paymentMethodId)
        {
            var paymentMethod = GetPaymentMethodById(paymentMethodId);
            if (paymentMethod == null)
                return PaymentMethodTypeEnum.Unknown;
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.PaymentMethodType;
        }

        /// <summary>
        /// Process recurring payments
        /// </summary>
        /// <param name="transactionPayment">transactionPayment required for an order processing</param>
        /// <param name="customer">Customer</param>
        /// <param name="orderGuid">Unique order identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void ProcessRecurringPayment(TransactionPayment transactionPayment, Guid transactionPaymentGuid, ref ProcessPaymentResult processPaymentResult)
        {
            if (transactionPayment.TransactionPaymentTotal == decimal.Zero)
            {
                processPaymentResult.Error = string.Empty;
                processPaymentResult.FullError = string.Empty;
                processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
            }
            else
            {
                var paymentMethod = GetPaymentMethodById(transactionPayment.PaymentMethodId);
                if (paymentMethod == null)
                    throw new Exception("Payment method couldn't be loaded");
                var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
                iPaymentMethod.ProcessRecurringPayment(transactionPayment, transactionPaymentGuid, ref processPaymentResult);
            }
        }

        /// <summary>
        /// Cancels recurring payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>
        public void CancelRecurringPayment(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult)
        {
            if (transactionPayment.TransactionPaymentTotal == decimal.Zero)
                return;

            var paymentMethod = GetPaymentMethodById(transactionPayment.PaymentMethodId);
            if (paymentMethod == null)
                throw new Exception("Payment method couldn't be loaded");
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            iPaymentMethod.CancelRecurringPayment(transactionPayment, ref cancelPaymentResult);
        }

        /// <summary>
        /// Gets masked credit card number
        /// </summary>
        /// <param name="creditCardNumber">Credit card number</param>
        /// <returns>Masked credit card number</returns>
        public string GetMaskedCreditCardNumber(string creditCardNumber)
        {
            if (String.IsNullOrEmpty(creditCardNumber))
                return string.Empty;

            if (creditCardNumber.Length <= 4)
                return creditCardNumber;

            string last4 = creditCardNumber.Substring(creditCardNumber.Length - 4, 4);
            string maskedChars = string.Empty;
            for (int i = 0; i < creditCardNumber.Length - 4; i++)
            {
                maskedChars += "*";
            }
            return maskedChars + last4;
        }
        #endregion
    }
}
