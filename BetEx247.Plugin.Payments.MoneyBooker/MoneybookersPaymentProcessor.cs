using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Payment;
using BetEx247.Core.CustomerManagement;
using BetEx247.Core.Common.Utils;
using BetEx247.Core.Infrastructure;
using System.Globalization;
using BetEx247.Core;
using BetEx247.Data.DAL;
using BetEx247.Data.Model;

namespace BetEx247.Plugin.Payments.MoneyBooker
{
    public class MoneybookersPaymentProcessor : IPaymentMethod
    {
        #region Fields
        private string payToEmail = string.Empty;
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the MoneybookersPaymentProcessor class
        /// </summary>
        public MoneybookersPaymentProcessor()
        {
            payToEmail =Constant.Payment.MONEYBOOKERPAYMENTEMAIL;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets Moneybookers URL
        /// </summary>
        /// <returns></returns>
        private string GetMoneybookersUrl()
        {
            //return useSandBox ? "http://www.moneybookers.com/app/test_payment.pl" : "https://www.moneybookers.com/app/payment.pl";
            return "https://www.moneybookers.com/app/payment.pl";
        }

        /// <summary>
        /// Process payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment required for an betting processing</param>
        /// <param name="member">member</param>
        /// <param name="bettingGuid">Unique betting identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void ProcessPayment(TransactionPayment transactionPayment, Member member, Guid bettingGuid, ref ProcessPaymentResult processPaymentResult)
        {
            processPaymentResult.PaymentStatus = PaymentStatusEnum.Pending;
        }

        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="betting">betting</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public string PostProcessPayment(TransactionPayment transactionPayment)
        {
            RemotePost remotePostHelper = new RemotePost();
            remotePostHelper.FormName = "MoneybookersForm";
            remotePostHelper.Url = GetMoneybookersUrl();

            remotePostHelper.Add("pay_to_email", payToEmail);
            remotePostHelper.Add("recipient_description",Constant.Payment.STORENAME);
            remotePostHelper.Add("transaction_id", transactionPayment.TransactionPaymentId.ToString());
            remotePostHelper.Add("cancel_url", CommonHelper.GetStoreLocation(false));
            remotePostHelper.Add("status_url", CommonHelper.GetStoreLocation(false) + "MoneybookersReturn.aspx");
            //supported moneybookers languages (EN, DE, ES, FR, IT, PL, GR, RO, RU, TR, CN, CZ or NL)
            remotePostHelper.Add("language", "EN");
            remotePostHelper.Add("amount", transactionPayment.TransactionPaymentTotal.ToString(new CultureInfo("en-US", false).NumberFormat));
            remotePostHelper.Add("currency", Constant.Payment.CURRENCYCODE);
            remotePostHelper.Add("detail1_description", "TransactionPayment ID:");
            remotePostHelper.Add("detail1_text", transactionPayment.TransactionPaymentId.ToString());

            remotePostHelper.Add("firstname", transactionPayment.Customer.FirstName);
            remotePostHelper.Add("lastname", transactionPayment.Customer.Language);
            remotePostHelper.Add("address", transactionPayment.Customer.Address);
            remotePostHelper.Add("phone_number", transactionPayment.Customer.Telephone);
            remotePostHelper.Add("postal_code", transactionPayment.Customer.PostalCode);
            remotePostHelper.Add("city", transactionPayment.Customer.City);     
            //review later
            //Country billingCountry = IoC.Resolve<ICountryService>().GetCountryById(order.BillingCountryId);
            //if (billingCountry != null)
            //    remotePostHelper.Add("country", billingCountry.ThreeLetterIsoCode);
            //else
            remotePostHelper.Add("country", transactionPayment.Customer.Country.ToString());
            remotePostHelper.Post();
            return string.Empty;
        }           

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void Capture(TransactionPayment transactionPayment, ref ProcessPaymentResult processPaymentResult)
        {
            throw new Exception("Capture method not supported");
        }

        /// <summary>
        /// Refunds payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>        
        public void Refund(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new Exception("Refund method not supported");
        }

        /// <summary>
        /// Voids payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>        
        public void Void(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new Exception("Void method not supported");
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment required for an betting processing</param>
        /// <param name="member">member</param>
        /// <param name="bettingGuid">Unique betting identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void ProcessRecurringPayment(TransactionPayment transactionPayment, Member member, Guid bettingGuid, ref ProcessPaymentResult processPaymentResult)
        {
            throw new Exception("Recurring payments not supported");
        }

        /// <summary>
        /// Cancels recurring payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>        
        public void CancelRecurringPayment(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult)
        {
        }
        #endregion

        #region Properies

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        public bool CanCapture
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether partial refund is supported
        /// </summary>
        public bool CanPartiallyRefund
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether refund is supported
        /// </summary>
        public bool CanRefund
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether void is supported
        /// </summary>
        public bool CanVoid
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        /// <returns>A recurring payment type of payment method</returns>
        public RecurringPaymentTypeEnum SupportRecurringPayments
        {
            get
            {
                return RecurringPaymentTypeEnum.NotSupported;
            }
        }

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        /// <returns>A payment method type</returns>
        public PaymentMethodTypeEnum PaymentMethodType
        {
            get
            {
                return PaymentMethodTypeEnum.Standard;
            }
        }
        #endregion
    }
}
