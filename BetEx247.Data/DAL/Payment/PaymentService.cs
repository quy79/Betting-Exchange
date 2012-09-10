using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Payment;
using BetEx247.Data.Model;

namespace BetEx247.Data.DAL
{
    /// <summary>
    /// Payment service
    /// </summary>
    public partial class PaymentService : IPaymentService
    {

        public CreditCardType GetCreditCardTypeById(int creditCardTypeId)
        {
            throw new NotImplementedException();
        }

        public void MarkCreditCardTypeAsDeleted(int creditCardTypeId)
        {
            throw new NotImplementedException();
        }

        public List<CreditCardType> GetAllCreditCardTypes()
        {
            throw new NotImplementedException();
        }

        public void InsertCreditCardType(CreditCardType creditCardType)
        {
            throw new NotImplementedException();
        }

        public void UpdateCreditCardType(CreditCardType creditCardType)
        {
            throw new NotImplementedException();
        }

        public void DeletePaymentMethod(int paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public PaymentMethod GetPaymentMethodById(int paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public PaymentMethod GetPaymentMethodBySystemKeyword(string systemKeyword)
        {
            throw new NotImplementedException();
        }

        public List<PaymentMethod> GetAllPaymentMethods()
        {
            throw new NotImplementedException();
        }

        public List<PaymentMethod> GetAllPaymentMethods(int? filterByCountryId)
        {
            throw new NotImplementedException();
        }

        public List<PaymentMethod> GetAllPaymentMethods(int? filterByCountryId, bool showHidden)
        {
            throw new NotImplementedException();
        }

        public void InsertPaymentMethod(PaymentMethod paymentMethod)
        {
            throw new NotImplementedException();
        }

        public void UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            throw new NotImplementedException();
        }

        public void ProcessPayment(TransactionPayment transactionPayment, Member member, Guid transactionPaymentGuid, ref ProcessPaymentResult processPaymentResult)
        {
            throw new NotImplementedException();
        }

        public string PostProcessPayment(TransactionPayment transactionPayment)
        {
            throw new NotImplementedException();
        }

        public decimal GetAdditionalHandlingFee(int paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public bool CanCapture(int paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public void Capture(TransactionPayment transactionPayment, ref ProcessPaymentResult processPaymentResult)
        {
            throw new NotImplementedException();
        }

        public bool CanPartiallyRefund(int paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public bool CanRefund(int paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public void Refund(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new NotImplementedException();
        }

        public bool CanVoid(int paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public void Void(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new NotImplementedException();
        }

        public RecurringPaymentTypeEnum SupportRecurringPayments(int paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public PaymentMethodTypeEnum GetPaymentMethodType(int paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public void ProcessRecurringPayment(TransactionPayment transactionPayment, Member member, Guid transactionPaymentGuid, ref ProcessPaymentResult processPaymentResult)
        {
            throw new NotImplementedException();
        }

        public void CancelRecurringPayment(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new NotImplementedException();
        }

        public string GetMaskedCreditCardNumber(string creditCardNumber)
        {
            throw new NotImplementedException();
        }
    }
}
