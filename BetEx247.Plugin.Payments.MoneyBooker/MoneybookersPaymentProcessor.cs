using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Payment;
using BetEx247.Core.CustomerManagement;
using BetEx247.Core.Common.Utils;
using BetEx247.Core.Infrastructure;
using System.Globalization;

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
            payToEmail = "chantinh2204@gmail.com";// IoC.Resolve<ISettingManager>().GetSettingValue("PaymentMethod.Moneybookers.PayToEmail");
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
        /// <param name="paymentInfo">Payment info required for an order processing</param>
        /// <param name="customer">Customer</param>
        /// <param name="orderGuid">Unique order identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void ProcessPayment(PaymentInfo paymentInfo, Customer customer, Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            processPaymentResult.PaymentStatus = PaymentStatusEnum.Pending;
        }

        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public string PostProcessPayment(Order order)
        {
            RemotePost remotePostHelper = new RemotePost();
            remotePostHelper.FormName = "MoneybookersForm";
            remotePostHelper.Url = GetMoneybookersUrl();

            remotePostHelper.Add("pay_to_email", payToEmail);
            remotePostHelper.Add("recipient_description", "Test");// IoC.Resolve<ISettingManager>().StoreName);
            remotePostHelper.Add("transaction_id", order.OrderId.ToString());
            remotePostHelper.Add("cancel_url", CommonHelper.GetStoreLocation(false));
            remotePostHelper.Add("status_url", CommonHelper.GetStoreLocation(false) + "MoneybookersReturn.aspx");
            //supported moneybookers languages (EN, DE, ES, FR, IT, PL, GR, RO, RU, TR, CN, CZ or NL)
            remotePostHelper.Add("language", "EN");
            remotePostHelper.Add("amount", order.OrderTotal.ToString(new CultureInfo("en-US", false).NumberFormat));
            remotePostHelper.Add("currency", "USD");//IoC.Resolve<ICurrencyService>().PrimaryStoreCurrency.CurrencyCode);
            remotePostHelper.Add("detail1_description", "Order ID:");
            remotePostHelper.Add("detail1_text", order.OrderId.ToString());

            remotePostHelper.Add("firstname", order.BillingFirstName);
            remotePostHelper.Add("lastname", order.BillingLastName);
            remotePostHelper.Add("address", order.BillingAddress1);
            remotePostHelper.Add("phone_number", order.BillingPhoneNumber);
            remotePostHelper.Add("postal_code", order.BillingZipPostalCode);
            remotePostHelper.Add("city", order.BillingCity);
            //StateProvince billingStateProvince = IoC.Resolve<IStateProvinceService>().GetStateProvinceById(order.BillingStateProvinceId);
            //if (billingStateProvince != null)
            //    remotePostHelper.Add("state", billingStateProvince.Abbreviation);
            //else
                remotePostHelper.Add("state", order.BillingStateProvince);
            //Country billingCountry = IoC.Resolve<ICountryService>().GetCountryById(order.BillingCountryId);
            //if (billingCountry != null)
            //    remotePostHelper.Add("country", billingCountry.ThreeLetterIsoCode);
            //else
                remotePostHelper.Add("country", order.BillingCountry);
            remotePostHelper.Post();
            return string.Empty;
        }           

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void Capture(Order order, ref ProcessPaymentResult processPaymentResult)
        {
            throw new Exception("Capture method not supported");
        }

        /// <summary>
        /// Refunds payment
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>        
        public void Refund(Order order, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new Exception("Refund method not supported");
        }

        /// <summary>
        /// Voids payment
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>        
        public void Void(Order order, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new Exception("Void method not supported");
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="paymentInfo">Payment info required for an order processing</param>
        /// <param name="customer">Customer</param>
        /// <param name="orderGuid">Unique order identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void ProcessRecurringPayment(PaymentInfo paymentInfo, Customer customer, Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            throw new Exception("Recurring payments not supported");
        }

        /// <summary>
        /// Cancels recurring payment
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>        
        public void CancelRecurringPayment(Order order, ref CancelPaymentResult cancelPaymentResult)
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
