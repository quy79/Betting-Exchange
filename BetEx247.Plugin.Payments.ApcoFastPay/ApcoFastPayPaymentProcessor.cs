using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.DAL;
using BetEx247.Core;
using BetEx247.Core.Payment;
using BetEx247.Core.Common.Utils;
using System.Globalization;
using BetEx247.Plugin.Payments.ApcoFastPay.biz.apsp.api;
using System.Net;
using System.Collections.Specialized;

namespace BetEx247.Plugin.Payments.ApcoFastPay
{
    public class ApcoFastPayPaymentProcessor : IPaymentMethod
    {
        #region Fields
        private string loginID;
        private string loginPass;
        private string payToEmail = string.Empty;
        private Service webService = null;
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the MoneybookersPaymentProcessor class
        /// </summary>
        public ApcoFastPayPaymentProcessor()
        {
            loginPass = Constant.Payment.APCOPAYMENTTRANSACTION;
            loginID = Constant.Payment.APCOPAYMENTLOGINID;      
            payToEmail = Constant.Payment.APCOPAYMENTEMAIL;

            webService = new Service();
            webService.Url = GetApcPaymentUrl();
        }
        #endregion

        #region Methods       

        /// <summary>
        /// Gets Moneybookers URL
        /// </summary>
        /// <returns></returns>
        private string GetApcPaymentUrl()
        {
            return "https://www.apsp.biz:9085/service.asmx";
        }

        /// <summary>
        /// Process payment
        /// </summary>
        /// <param name="transactionPayment">transactionPayment required for an betting processing</param>
        /// <param name="bettingGuid">Unique betting identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void ProcessPayment(TransactionPayment transactionPayment,Guid bettingGuid, ref ProcessPaymentResult processPaymentResult)
        {     
            #region tempcode
            //WebClient webClient = new WebClient();
            //NameValueCollection form = new NameValueCollection();
            //form.Add("MerchID", loginID);
            //form.Add("Pass", loginPass);            
            //form.Add("TrType", "FALSE");      
            //form.Add("CardNum", transactionPayment.PaymentMenthod.CreditCardNumber);
            //form.Add("CVV2", transactionPayment.PaymentMenthod.CardCvv2);
            //form.Add("ExpDay", "31");
            //form.Add("ExpMonth", transactionPayment.PaymentMenthod.CardExpirationMonth);
            //form.Add("ExpYear", transactionPayment.PaymentMenthod.CardExpirationYear);
            //form.Add("CardHName", transactionPayment.PaymentMenthod.NameOnCard);
            //form.Add("Amount", transactionPayment.TransactionPaymentTotal.ToString("0.00", CultureInfo.InvariantCulture));
            //form.Add("CurrencyCode", Constant.Payment.CURRENCYCODE);
            //form.Add("Addr", transactionPayment.Customer.Address);
            //form.Add("PostCode",transactionPayment.Customer.PostalCode);
            //form.Add("TransID", transactionPayment.TransactionPaymentId.ToString());
            //form.Add("UserIP",CommonHelper.GetRequestIP());
            //form.Add("UDF1", "Test 1");
            //form.Add("UDF2", "Test 1");
            //form.Add("UDF3", "Test 1");

            //string reply = null;
            //Byte[] responseData = webClient.UploadValues(GetApcPaymentUrl(), form);
            //reply = Encoding.ASCII.GetString(responseData);
            #endregion

            int transactionMode = (int)TransactMode.Authorization;
            string reply = webService.DoTransaction(loginID, loginPass, transactionMode.ToString(),
                transactionPayment.PaymentMenthod.CreditCardNumber, transactionPayment.PaymentMenthod.CardCvv2, "", transactionPayment.PaymentMenthod.CardExpirationMonth, transactionPayment.PaymentMenthod.CardExpirationYear,
                transactionPayment.PaymentMenthod.NameOnCard, transactionPayment.TransactionPaymentTotal.ToString("0.00", CultureInfo.InvariantCulture),
                Constant.Payment.CURRENCYCODENUMBER, transactionPayment.Customer.Address, transactionPayment.Customer.PostalCode, transactionPayment.TransactionPaymentId.ToString(),
                CommonHelper.GetRequestIP(), "Authorization 1", "Authorization 2", "Authorization 3");

            if (!String.IsNullOrEmpty(reply))
            {
                reply=reply.Replace("||","|");
                string[] responseFields = reply.Split('|');
                switch (responseFields[0])
                {
                    case "APPROVED":
                        processPaymentResult.AuthorizationTransactionCode = string.Format("{0}", responseFields[1]);
                        processPaymentResult.AuthorizationTransactionResult = string.Format("Approved ({0}: {1})", responseFields[2], responseFields[11]);
                        processPaymentResult.AVSResult = responseFields[0];
                        //responseFields[38];
                        if (transactionMode == (int)TransactMode.Authorization)
                        {
                            processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
                        }
                        else
                        {
                            processPaymentResult.PaymentStatus = PaymentStatusEnum.Authorized;
                        }
                        break;
                    default:
                        processPaymentResult.Error = string.Format("Declined {0}", responseFields[0] + " " + transactionPayment.PaymentMenthod.CreditCardNumber);
                        processPaymentResult.FullError = string.Format("Declined {0}", responseFields[0] + " " + transactionPayment.PaymentMenthod.CreditCardNumber);
                        break;                    
                }
            }
            else
            {
                processPaymentResult.Error = "Unknown error";
                processPaymentResult.FullError = "Unknown error";
            }
        }

        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="betting">betting</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public string PostProcessPayment(TransactionPayment transactionPayment)
        {
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
        /// <param name="bettingGuid">Unique betting identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void ProcessRecurringPayment(TransactionPayment transactionPayment, Guid bettingGuid, ref ProcessPaymentResult processPaymentResult)
        {
            int transactionMode = (int)TransactMode.OriginalCredit;
            string reply = webService.DoTransaction(loginID, loginPass, transactionMode.ToString(),
                transactionPayment.PaymentMenthod.CreditCardNumber, transactionPayment.PaymentMenthod.CardCvv2, "", transactionPayment.PaymentMenthod.CardExpirationMonth, transactionPayment.PaymentMenthod.CardExpirationYear,
                transactionPayment.PaymentMenthod.NameOnCard, transactionPayment.TransactionPaymentTotal.ToString("0.00", CultureInfo.InvariantCulture),
                Constant.Payment.CURRENCYCODENUMBER, transactionPayment.Customer.Address, transactionPayment.Customer.PostalCode,transactionPayment.TransactionIDRespone.ToString(), //transactionPayment.TransactionPaymentId.ToString(),
                CommonHelper.GetRequestIP(), "Authorization 1", "Authorization 2", "Authorization 3");

            if (!String.IsNullOrEmpty(reply))
            {
                reply = reply.Replace("||", "|");
                string[] responseFields = reply.Split('|');
                switch (responseFields[0])
                {
                    case "CAPTURED":
                        processPaymentResult.AuthorizationTransactionCode = string.Format("{0}", responseFields[1]);
                        processPaymentResult.AuthorizationTransactionResult = string.Format("Approved ({0}: {1})", responseFields[2], responseFields[11]);
                        processPaymentResult.AVSResult = responseFields[0];
                        //responseFields[38];
                        if (transactionMode == (int)TransactMode.Authorization)
                        {
                            processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
                        }
                        else
                        {
                            processPaymentResult.PaymentStatus = PaymentStatusEnum.Authorized;
                        }
                        break;
                    default:
                        processPaymentResult.Error = string.Format("Declined {0}", responseFields[0]);
                        processPaymentResult.FullError = string.Format("Declined {0}", responseFields[0]);
                        break;
                }
            }
            else
            {
                processPaymentResult.Error = "Unknown error";
                processPaymentResult.FullError = "Unknown error";
            }
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
