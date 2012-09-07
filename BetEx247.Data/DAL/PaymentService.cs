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

        public BetEx247.Data.Model.PaymentMethod GetPaymentMethodById(int paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public BetEx247.Data.Model.PaymentMethod GetPaymentMethodBySystemKeyword(string systemKeyword)
        {
            throw new NotImplementedException();
        }

        public List<BetEx247.Data.Model.PaymentMethod> GetAllPaymentMethods()
        {
            throw new NotImplementedException();
        }

        public List<BetEx247.Data.Model.PaymentMethod> GetAllPaymentMethods(int? filterByCountryId)
        {
            throw new NotImplementedException();
        }

        public List<BetEx247.Data.Model.PaymentMethod> GetAllPaymentMethods(int? filterByCountryId, bool showHidden)
        {
            throw new NotImplementedException();
        }

        public void InsertPaymentMethod(BetEx247.Data.Model.PaymentMethod paymentMethod)
        {
            throw new NotImplementedException();
        }

        public void UpdatePaymentMethod(BetEx247.Data.Model.PaymentMethod paymentMethod)
        {
            throw new NotImplementedException();
        }

        public void ProcessPayment(PaymentInfo paymentInfo, Member member, Guid bettingGuid, ref ProcessPaymentResult processPaymentResult)
        {
            throw new NotImplementedException();
        }

        public string PostProcessPayment(Betting betting)
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

        public void Capture(Betting betting, ref ProcessPaymentResult processPaymentResult)
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

        public void Refund(Betting betting, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new NotImplementedException();
        }

        public bool CanVoid(int paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public void Void(Betting betting, ref CancelPaymentResult cancelPaymentResult)
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

        public void ProcessRecurringPayment(PaymentInfo paymentInfo, Member member, Guid bettingGuid, ref ProcessPaymentResult processPaymentResult)
        {
            throw new NotImplementedException();
        }

        public void CancelRecurringPayment(Betting betting, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new NotImplementedException();
        }

        public string GetMaskedCreditCardNumber(string creditCardNumber)
        {
            throw new NotImplementedException();
        }
    }
}
