using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Specialized;
using System.Net;
using BetEx247.Plugin.Payments.AuthorizeNet.net.authorize.api; 
using BetEx247.Core.Payment;
using BetEx247.Core.Infrastructure;
using BetEx247.Core.CustomerManagement;
using BetEx247.Core;
using BetEx247.Data.DAL;
using BetEx247.Data.Model;

namespace BetEx247.Plugin.Payments.AuthorizeNet
{
    /// <summary>
    /// Authorize.Net payment processor
    /// </summary>
    public partial class AuthorizeNetPaymentProcessor : IPaymentMethod
    {
        #region Fields

        private bool useSandBox = true;
        private string loginID;
        private string transactionKey;
        private Service webService = null;

        #endregion

        #region Utilities

        /// <summary>
        /// Initializes the Authorize.NET payment processor
        /// </summary>
        private void InitSettings()
        {
            useSandBox = Constant.Payment.AUTHORIZESANBOX;
            transactionKey = Constant.Payment.AUTHORIZENETTRANSACTION;
            loginID = Constant.Payment.AUTHORIZENETLOGINID;

            if (string.IsNullOrEmpty(transactionKey))
                throw new Exception("Authorize.NET API transaction key is not set");

            if (string.IsNullOrEmpty(loginID))
                throw new Exception("Authorize.NET API login ID is not set");

            webService = new AuthorizeNet.net.authorize.api.Service();
            if (useSandBox)
            {
                webService.Url = "https://apitest.authorize.net/soap/v1/Service.asmx";
            }
            else
            {
                webService.Url = "https://api.authorize.net/soap/v1/Service.asmx";
            }
        }

        /// <summary>
        /// Gets Authorize.NET URL
        /// </summary>
        /// <returns></returns>
        private string GetAuthorizeNETUrl()
        {
            return useSandBox ? "https://test.authorize.net/gateway/transact.dll" :
                "https://secure.authorize.net/gateway/transact.dll";
        }

        // Populate merchant authentication (ARB Support)
        private MerchantAuthenticationType PopulateMerchantAuthentication()
        {
            MerchantAuthenticationType authentication = new MerchantAuthenticationType();
            authentication.name = loginID;
            authentication.transactionKey = transactionKey;
            return authentication;
        }
        /// <summary>
        ///  Get errors (ARB Support)
        /// </summary>
        /// <param name="response"></param>
        private static string GetErrors(ANetApiResponseType response)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("The API request failed with the following errors:");
            for (int i = 0; i < response.messages.Length; i++)
            {
                sb.AppendLine("[" + response.messages[i].code + "] " + response.messages[i].text);
            }
            return sb.ToString();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets current transaction mode
        /// </summary>
        /// <returns>Current transaction mode</returns>
        public static TransactMode GetCurrentTransactionMode()
        {
            TransactMode transactionModeEnum = TransactMode.Authorize;
            string transactionMode = "AuthorizeAndCapture";// IoC.Resolve<ISettingManager>().GetSettingValue("PaymentMethod.AuthorizeNET.TransactionMode");
            if (!String.IsNullOrEmpty(transactionMode))
                transactionModeEnum = (TransactMode)Enum.Parse(typeof(TransactMode), transactionMode);
            return transactionModeEnum;
        }

        /// <summary>
        /// Process payment
        /// </summary>
        /// <param name="paymentInfo">Payment info required for an betting processing</param>
        /// <param name="Member">Member</param>
        /// <param name="orderGuid">Unique order identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void ProcessPayment(TransactionPayment betting, Member member, Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            InitSettings();
            TransactMode transactionMode = GetCurrentTransactionMode();

            WebClient webClient = new WebClient();
            NameValueCollection form = new NameValueCollection();
            form.Add("x_login", loginID);
            form.Add("x_tran_key", transactionKey);
            if (useSandBox)
                form.Add("x_test_request", "TRUE");
            else
                form.Add("x_test_request", "FALSE");

            form.Add("x_delim_data", "TRUE");
            form.Add("x_delim_char", "|");
            form.Add("x_encap_char", "");
            form.Add("x_version", APIVersion);
            form.Add("x_relay_response", "FALSE");
            form.Add("x_method", "CC");
            form.Add("x_currency_code", Constant.Payment.CURRENCYCODE);
            if (transactionMode == TransactMode.Authorize)
                form.Add("x_type", "AUTH_ONLY");
            else if (transactionMode == TransactMode.AuthorizeAndCapture)
                form.Add("x_type", "AUTH_CAPTURE");
            else
                throw new Exception("Not supported transaction mode");

            form.Add("x_amount",betting.TransactionPaymentTotal.ToString("0.00", CultureInfo.InvariantCulture));
            form.Add("x_card_num", betting.PaymentMenthod.CreditCardNumber);
            form.Add("x_exp_date", betting.PaymentMenthod.CardExpirationMonth +"/"+ betting.PaymentMenthod.CardExpirationYear);
            form.Add("x_card_code", betting.PaymentMenthod.CardCvv2);
            form.Add("x_first_name", betting.Customer.FirstName);
            form.Add("x_last_name", betting.Customer.LastName);
            form.Add("x_address", betting.Customer.Address);
            form.Add("x_city", betting.Customer.City);
            //if (paymentInfo.BillingAddress.StateProvince != null)
            //    form.Add("x_state", paymentInfo.BillingAddress.StateProvince.Abbreviation);
            form.Add("x_zip", betting.Customer.PostalCode);
            //if (paymentInfo.BillingAddress.Country != null)
            //    form.Add("x_country", paymentInfo.BillingAddress.Country.TwoLetterIsoCode);
            //20 chars maximum
            form.Add("x_invoice_num", orderGuid.ToString().Substring(0, 20));
            //form.Add("x_customer_ip",Request.Current.UserHostAddress);

            string reply = null;
            Byte[] responseData = webClient.UploadValues(GetAuthorizeNETUrl(), form);
            reply = Encoding.ASCII.GetString(responseData);

            if (!String.IsNullOrEmpty(reply))
            {
                string[] responseFields = reply.Split('|');
                switch (responseFields[0])
                {
                    case "1":
                        processPaymentResult.AuthorizationTransactionCode = string.Format("{0},{1}", responseFields[6], responseFields[4]);
                        processPaymentResult.AuthorizationTransactionResult = string.Format("Approved ({0}: {1})", responseFields[2], responseFields[3]);
                        processPaymentResult.AVSResult = responseFields[5];
                        //responseFields[38];
                        if (transactionMode == TransactMode.Authorize)
                        {
                            processPaymentResult.PaymentStatus = PaymentStatusEnum.Authorized;
                        }
                        else
                        {
                            processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
                        }
                        break;
                    case "2":
                        processPaymentResult.Error = string.Format("Declined ({0}: {1})", responseFields[2], responseFields[3]);
                        processPaymentResult.FullError = string.Format("Declined ({0}: {1})", responseFields[2], responseFields[3]);
                        break;
                    case "3":
                        processPaymentResult.Error = string.Format("Error: {0}", reply);
                        processPaymentResult.FullError = string.Format("Error: {0}", reply);
                        break;

                }
            }
            else
            {
                processPaymentResult.Error = "Authorize.NET unknown error";
                processPaymentResult.FullError = "Authorize.NET unknown error";
            }
        }

        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="betting">betting</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public string PostProcessPayment(TransactionPayment transaction)
        {
            return string.Empty;
        }
        
        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="betting">betting</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void Capture(TransactionPayment transactionPayment, ref ProcessPaymentResult processPaymentResult)
        {
            InitSettings();

            WebClient webClient = new WebClient();
            NameValueCollection form = new NameValueCollection();
            form.Add("x_login", loginID);
            form.Add("x_tran_key", transactionKey);
            if (useSandBox)
                form.Add("x_test_request", "TRUE");
            else
                form.Add("x_test_request", "FALSE");

            form.Add("x_delim_data", "TRUE");
            form.Add("x_delim_char", "|");
            form.Add("x_encap_char", "");
            form.Add("x_version", APIVersion);
            form.Add("x_relay_response", "FALSE");
            form.Add("x_method", "CC");
            form.Add("x_currency_code", Constant.Payment.CURRENCYCODE);
            form.Add("x_type", "PRIOR_AUTH_CAPTURE");

            form.Add("x_amount", transactionPayment.TransactionPaymentTotal.ToString("0.00", CultureInfo.InvariantCulture));
            string[] codes = processPaymentResult.AuthorizationTransactionCode.Split(',');
            //x_trans_id. When x_test_request (sandbox) is set to a positive response, 
            //or when Test mode is enabled on the payment gateway, this value will be "0".
            form.Add("x_trans_id", codes[0]);

            string reply = null;
            Byte[] responseData = webClient.UploadValues(GetAuthorizeNETUrl(), form);
            reply = Encoding.ASCII.GetString(responseData);

            if (!String.IsNullOrEmpty(reply))
            {
                string[] responseFields = reply.Split('|');
                switch (responseFields[0])
                {
                    case "1":
                        processPaymentResult.AuthorizationTransactionCode = string.Format("{0},{1}", responseFields[6], responseFields[4]);
                        processPaymentResult.AuthorizationTransactionResult = string.Format("Approved ({0}: {1})", responseFields[2], responseFields[3]);
                        processPaymentResult.AVSResult = responseFields[5];
                        //responseFields[38];
                        processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
                        break;
                    case "2":
                        processPaymentResult.Error = string.Format("Declined ({0}: {1})", responseFields[2], responseFields[3]);
                        processPaymentResult.FullError = string.Format("Declined ({0}: {1})", responseFields[2], responseFields[3]);
                        break;
                    case "3":
                        processPaymentResult.Error = string.Format("Error: {0}", reply);
                        processPaymentResult.FullError = string.Format("Error: {0}", reply);
                        break;
                }
            }
            else
            {
                processPaymentResult.Error = "Authorize.NET unknown error";
                processPaymentResult.FullError = "Authorize.NET unknown error";
            }
        }

        /// <summary>
        /// Refunds payment
        /// </summary>
        /// <param name="betting">betting</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>        
        public void Refund(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new Exception("Refund method not supported");
        }

        /// <summary>
        /// Voids payment
        /// </summary>
        /// <param name="betting">betting</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>        
        public void Void(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new Exception("Void method not supported");
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="paymentInfo">Payment info required for an betting processing</param>
        /// <param name="member">member</param>
        /// <param name="bettingGuid">Unique order identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void ProcessRecurringPayment(TransactionPayment transactionPayment, Member member, Guid bettingGuid, ref ProcessPaymentResult processPaymentResult)
        {
            InitSettings();
            MerchantAuthenticationType authentication = PopulateMerchantAuthentication();
            if (!transactionPayment.IsRecurringPayment)
            {
                ARBSubscriptionType subscription = new ARBSubscriptionType();
                AuthorizeNet.net.authorize.api.CreditCardType creditCard = new AuthorizeNet.net.authorize.api.CreditCardType();

                subscription.name = bettingGuid.ToString();
                creditCard.cardNumber = transactionPayment.PaymentMenthod.CreditCardNumber;
                creditCard.expirationDate = transactionPayment.PaymentMenthod.CardExpirationYear + "-" + transactionPayment.PaymentMenthod.CardExpirationMonth; // required format for API is YYYY-MM
                creditCard.cardCode = transactionPayment.PaymentMenthod.CardCvv2;

                subscription.payment = new PaymentType();
                subscription.payment.Item = creditCard;      
                subscription.billTo = new NameAndAddressType();
                subscription.billTo.firstName = transactionPayment.Customer.FirstName;
                subscription.billTo.lastName = transactionPayment.Customer.LastName;
                subscription.billTo.address = transactionPayment.Customer.Address;
                subscription.billTo.city = transactionPayment.Customer.City;
                subscription.billTo.zip = transactionPayment.Customer.PostalCode;   
                subscription.customer = new CustomerType();
                subscription.customer.email = transactionPayment.Customer.Email1;
                subscription.customer.phoneNumber = transactionPayment.Customer.Telephone;   
                subscription.order = new OrderType();
                subscription.order.description =Constant.Payment.STORENAME+" "+ "Recurring payment";

                // Create a subscription that is leng of specified occurrences and interval is amount of days ad runs

                subscription.paymentSchedule = new PaymentScheduleType();
                DateTime dtNow = DateTime.UtcNow;
                subscription.paymentSchedule.startDate = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day);
                subscription.paymentSchedule.startDateSpecified = true;

                subscription.paymentSchedule.totalOccurrences = Convert.ToInt16(transactionPayment.RecurringTotalCycles);
                subscription.paymentSchedule.totalOccurrencesSpecified = true;

                subscription.amount = transactionPayment.TransactionPaymentTotal;
                subscription.amountSpecified = true;

                // Interval can't be updated once a subscription is created.
                subscription.paymentSchedule.interval = new PaymentScheduleTypeInterval();
                switch (transactionPayment.RecurringCyclePeriod)
                {
                    case (int)RecurringProductCyclePeriodEnum.Days:
                        subscription.paymentSchedule.interval.length = Convert.ToInt16(transactionPayment.RecurringCycleLength);
                        subscription.paymentSchedule.interval.unit = ARBSubscriptionUnitEnum.days;
                        break;
                    case (int)RecurringProductCyclePeriodEnum.Weeks:
                        subscription.paymentSchedule.interval.length = Convert.ToInt16(transactionPayment.RecurringCycleLength * 7);
                        subscription.paymentSchedule.interval.unit = ARBSubscriptionUnitEnum.days;
                        break;
                    case (int)RecurringProductCyclePeriodEnum.Months:
                        subscription.paymentSchedule.interval.length = Convert.ToInt16(transactionPayment.RecurringCycleLength);
                        subscription.paymentSchedule.interval.unit = ARBSubscriptionUnitEnum.months;
                        break;
                    case (int)RecurringProductCyclePeriodEnum.Years:
                        subscription.paymentSchedule.interval.length = Convert.ToInt16(transactionPayment.RecurringCycleLength * 12);
                        subscription.paymentSchedule.interval.unit = ARBSubscriptionUnitEnum.months;
                        break;
                    default:
                        throw new Exception("Not supported cycle period");
                }


                ARBCreateSubscriptionResponseType response = webService.ARBCreateSubscription(authentication, subscription);

                if (response.resultCode == MessageTypeEnum.Ok)
                {
                    processPaymentResult.SubscriptionTransactionId = response.subscriptionId.ToString();
                    processPaymentResult.AuthorizationTransactionCode = response.resultCode.ToString();
                    processPaymentResult.AuthorizationTransactionResult = string.Format("Approved ({0}: {1})", response.resultCode.ToString(), response.subscriptionId.ToString());
                }
                else
                {
                    processPaymentResult.Error = string.Format("Error processing recurring payment. {0}", GetErrors(response));
                    processPaymentResult.FullError = string.Format("Error processing recurring payment. {0}", GetErrors(response));
                }
            }
        }

        /// <summary>
        /// Cancels recurring payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>        
        public void CancelRecurringPayment(TransactionPayment transactionPayment, ref CancelPaymentResult cancelPaymentResult)
        {
            InitSettings();
            MerchantAuthenticationType authentication = PopulateMerchantAuthentication();
            long subscriptionID = 0;
            long.TryParse(cancelPaymentResult.SubscriptionTransactionId, out subscriptionID);
            ARBCancelSubscriptionResponseType response = webService.ARBCancelSubscription(authentication, subscriptionID);

            if (response.resultCode == MessageTypeEnum.Ok)
            {
                //ok
            }
            else
            {
                cancelPaymentResult.Error = "Error cancelling subscription, please contact customer support. " + GetErrors(response);
                cancelPaymentResult.FullError = "Error cancelling subscription, please contact customer support. " + GetErrors(response);
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets Authorize.NET API version
        /// </summary>
        public string APIVersion
        {
            get
            {
                return "3.1";
            }
        }

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        public bool CanCapture
        {
            get
            {
                return true;
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
                return RecurringPaymentTypeEnum.Manual;
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
