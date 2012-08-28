using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.CustomerManagement;
using BetEx247.Core.Common.Utils;
using BetEx247.Core.Infrastructure;

namespace BetEx247.Core.Payment
{
    /// <summary>
    /// Payment service
    /// </summary>
    public partial class PaymentService //: IPaymentService
    {
        //#region Constants
        //private const string CREDITCARDS_ALL_KEY = "creditcard.all";
        //private const string CREDITCARDS_BY_ID_KEY = "creditcard.id-{0}";
        //private const string CREDITCARDS_PATTERN_KEY = "creditcard.";

        //private const string PAYMENTMETHODS_BY_ID_KEY = "paymentmethod.id-{0}";
        //private const string PAYMENTMETHODS_PATTERN_KEY = "paymentmethod.";

        //#endregion

        //#region Fields

        ///// <summary>
        ///// Object context
        ///// </summary>
        //private readonly NopObjectContext _context;    
       
        //#endregion

        //#region Ctor

        ///// <summary>
        ///// Ctor
        ///// </summary>
        ///// <param name="context">Object context</param>
        //public PaymentService(NopObjectContext context)
        //{
        //    this._context = context;
        //}

        //#endregion

        //#region Methods

        //#region Credit cards
        ///// <summary>
        ///// Gets a credit card type
        ///// </summary>
        ///// <param name="creditCardTypeId">Credit card type identifier</param>
        ///// <returns>Credit card type</returns>
        //public CreditCardType GetCreditCardTypeById(int creditCardTypeId)
        //{
        //    if (creditCardTypeId == 0)
        //        return null;

        //    string key = string.Format(CREDITCARDS_BY_ID_KEY, creditCardTypeId);          


        //    var query = from cct in _context.CreditCardTypes
        //                where cct.CreditCardTypeId == creditCardTypeId
        //                select cct;
        //    var creditCardType = query.SingleOrDefault();            
        //    return creditCardType;
        //}

        ///// <summary>
        ///// Marks a credit card type as deleted
        ///// </summary>
        ///// <param name="creditCardTypeId">Credit card type identifier</param>
        //public void MarkCreditCardTypeAsDeleted(int creditCardTypeId)
        //{
        //    var creditCardType = GetCreditCardTypeById(creditCardTypeId);
        //    if (creditCardType != null)
        //    {
        //        creditCardType.Deleted = true;
        //        UpdateCreditCardType(creditCardType);
        //    }               
        //}

        ///// <summary>
        ///// Gets all credit card types
        ///// </summary>
        ///// <returns>Credit card type collection</returns>
        //public List<CreditCardType> GetAllCreditCardTypes()
        //{
        //    string key = string.Format(CREDITCARDS_ALL_KEY);
          
        //    var query = from cct in _context.CreditCardTypes
        //                orderby cct.DisplayOrder
        //                where !cct.Deleted
        //                select cct;
        //    var creditCardTypeCollection = query.ToList();    
        //    return creditCardTypeCollection;
        //}

        ///// <summary>
        ///// Inserts a credit card type
        ///// </summary>
        ///// <param name="creditCardType">Credit card type</param>
        //public void InsertCreditCardType(CreditCardType creditCardType)
        //{
        //    if (creditCardType == null)
        //        throw new ArgumentNullException("creditCardType");

        //    creditCardType.Name = CommonHelper.EnsureNotNull(creditCardType.Name);
        //    creditCardType.Name = CommonHelper.EnsureMaximumLength(creditCardType.Name, 100);
        //    creditCardType.SystemKeyword = CommonHelper.EnsureNotNull(creditCardType.SystemKeyword);
        //    creditCardType.SystemKeyword = CommonHelper.EnsureMaximumLength(creditCardType.SystemKeyword, 100);



        //    _context.CreditCardTypes.AddObject(creditCardType);
        //    _context.SaveChanges();
        //}

        ///// <summary>
        ///// Updates the credit card type
        ///// </summary>
        ///// <param name="creditCardType">Credit card type</param>
        //public void UpdateCreditCardType(CreditCardType creditCardType)
        //{
        //    if (creditCardType == null)
        //        throw new ArgumentNullException("creditCardType");

        //    creditCardType.Name = CommonHelper.EnsureNotNull(creditCardType.Name);
        //    creditCardType.Name = CommonHelper.EnsureMaximumLength(creditCardType.Name, 100);
        //    creditCardType.SystemKeyword = CommonHelper.EnsureNotNull(creditCardType.SystemKeyword);
        //    creditCardType.SystemKeyword = CommonHelper.EnsureMaximumLength(creditCardType.SystemKeyword, 100);


        //    if (!_context.IsAttached(creditCardType))
        //        _context.CreditCardTypes.Attach(creditCardType);

        //    _context.SaveChanges();   
        //}
        //#endregion

        //#region Payment methods

        ///// <summary>
        ///// Deletes a payment method
        ///// </summary>
        ///// <param name="paymentMethodId">Payment method identifier</param>
        //public void DeletePaymentMethod(int paymentMethodId)
        //{
        //    var paymentMethod = GetPaymentMethodById(paymentMethodId);
        //    if (paymentMethod == null)
        //        return;


        //    if (!_context.IsAttached(paymentMethod))
        //        _context.PaymentMethods.Attach(paymentMethod);
        //    _context.DeleteObject(paymentMethod);
        //    _context.SaveChanges();
        //}

        ///// <summary>
        ///// Gets a payment method
        ///// </summary>
        ///// <param name="paymentMethodId">Payment method identifier</param>
        ///// <returns>Payment method</returns>
        //public PaymentMethod GetPaymentMethodById(int paymentMethodId)
        //{
        //    if (paymentMethodId == 0)
        //        return null;

        //    string key = string.Format(PAYMENTMETHODS_BY_ID_KEY, paymentMethodId);
          
        //    var query = from pm in _context.PaymentMethods
        //                where pm.PaymentMethodId == paymentMethodId
        //                select pm;
        //    var paymentMethod = query.SingleOrDefault();
        //    return paymentMethod;
        //}

        ///// <summary>
        ///// Gets a payment method
        ///// </summary>
        ///// <param name="systemKeyword">Payment method system keyword</param>
        ///// <returns>Payment method</returns>
        //public PaymentMethod GetPaymentMethodBySystemKeyword(string systemKeyword)
        //{

        //    var query = from pm in _context.PaymentMethods
        //                where pm.SystemKeyword == systemKeyword
        //                select pm;
        //    var paymentMethod = query.FirstOrDefault();

        //    return paymentMethod;
        //}

        ///// <summary>
        ///// Gets all payment methods
        ///// </summary>
        ///// <returns>Payment method collection</returns>
        //public List<PaymentMethod> GetAllPaymentMethods()
        //{
        //    return GetAllPaymentMethods(null);
        //}

        ///// <summary>
        ///// Gets all payment methods
        ///// </summary>
        ///// <param name="filterByCountryId">The country indentifier</param>
        ///// <returns>Payment method collection</returns>
        //public List<PaymentMethod> GetAllPaymentMethods(int? filterByCountryId)
        //{
        //    bool showHidden = false;// NopContext.Current.IsAdmin;

        //    return GetAllPaymentMethods(filterByCountryId, showHidden);
        //}

        ///// <summary>
        ///// Gets all payment methods
        ///// </summary>
        ///// <param name="filterByCountryId">The country indentifier</param>
        ///// <param name="showHidden">A value indicating whether the not active payment methods should be load</param>
        ///// <returns>Payment method collection</returns>
        //public List<PaymentMethod> GetAllPaymentMethods(int? filterByCountryId, bool showHidden)
        //{
        //    if (filterByCountryId.HasValue && filterByCountryId.Value > 0)
        //    {
        //        var query1 = from pm in _context.PaymentMethods
        //                     where
        //                     pm.NpRestrictedCountries.Select(c => c.CountryId).Contains(filterByCountryId.Value)
        //                     select pm.PaymentMethodId;

        //        var query2 = from pm in _context.PaymentMethods
        //                     where (!query1.Contains(pm.PaymentMethodId)) &&
        //                     (showHidden || pm.IsActive)
        //                     orderby pm.DisplayOrder
        //                     select pm;

        //        var paymentMethods = query2.ToList();
        //        return paymentMethods;
        //    }
        //    else
        //    {
        //        var query = from pm in _context.PaymentMethods
        //                    where (showHidden || pm.IsActive)
        //                    orderby pm.DisplayOrder
        //                    select pm;

        //        var paymentMethods = query.ToList();
        //        return paymentMethods;
        //    }
        //}

        ///// <summary>
        ///// Inserts a payment method
        ///// </summary>
        ///// <param name="paymentMethod">Payment method</param>
        //public void InsertPaymentMethod(PaymentMethod paymentMethod)
        //{
        //    if (paymentMethod == null)
        //        throw new ArgumentNullException("paymentMethod");

        //    paymentMethod.Name = CommonHelper.EnsureNotNull(paymentMethod.Name);
        //    paymentMethod.Name = CommonHelper.EnsureMaximumLength(paymentMethod.Name, 100);
        //    paymentMethod.VisibleName = CommonHelper.EnsureNotNull(paymentMethod.VisibleName);
        //    paymentMethod.VisibleName = CommonHelper.EnsureMaximumLength(paymentMethod.VisibleName, 100);
        //    paymentMethod.Description = CommonHelper.EnsureNotNull(paymentMethod.Description);
        //    paymentMethod.Description = CommonHelper.EnsureMaximumLength(paymentMethod.Description, 4000);
        //    paymentMethod.ConfigureTemplatePath = CommonHelper.EnsureNotNull(paymentMethod.ConfigureTemplatePath);
        //    paymentMethod.ConfigureTemplatePath = CommonHelper.EnsureMaximumLength(paymentMethod.ConfigureTemplatePath, 500);
        //    paymentMethod.UserTemplatePath = CommonHelper.EnsureNotNull(paymentMethod.UserTemplatePath);
        //    paymentMethod.UserTemplatePath = CommonHelper.EnsureMaximumLength(paymentMethod.UserTemplatePath, 500);
        //    paymentMethod.ClassName = CommonHelper.EnsureNotNull(paymentMethod.ClassName);
        //    paymentMethod.ClassName = CommonHelper.EnsureMaximumLength(paymentMethod.ClassName, 500);
        //    paymentMethod.SystemKeyword = CommonHelper.EnsureNotNull(paymentMethod.SystemKeyword);
        //    paymentMethod.SystemKeyword = CommonHelper.EnsureMaximumLength(paymentMethod.SystemKeyword, 500);     

        //    _context.PaymentMethods.AddObject(paymentMethod);
        //    _context.SaveChanges();
        //}

        ///// <summary>
        ///// Updates the payment method
        ///// </summary>
        ///// <param name="paymentMethod">Payment method</param>
        //public void UpdatePaymentMethod(PaymentMethod paymentMethod)
        //{
        //    if (paymentMethod == null)
        //        throw new ArgumentNullException("paymentMethod");

        //    paymentMethod.Name = CommonHelper.EnsureNotNull(paymentMethod.Name);
        //    paymentMethod.Name = CommonHelper.EnsureMaximumLength(paymentMethod.Name, 100);
        //    paymentMethod.VisibleName = CommonHelper.EnsureNotNull(paymentMethod.VisibleName);
        //    paymentMethod.VisibleName = CommonHelper.EnsureMaximumLength(paymentMethod.VisibleName, 100);
        //    paymentMethod.Description = CommonHelper.EnsureNotNull(paymentMethod.Description);
        //    paymentMethod.Description = CommonHelper.EnsureMaximumLength(paymentMethod.Description, 4000);
        //    paymentMethod.ConfigureTemplatePath = CommonHelper.EnsureNotNull(paymentMethod.ConfigureTemplatePath);
        //    paymentMethod.ConfigureTemplatePath = CommonHelper.EnsureMaximumLength(paymentMethod.ConfigureTemplatePath, 500);
        //    paymentMethod.UserTemplatePath = CommonHelper.EnsureNotNull(paymentMethod.UserTemplatePath);
        //    paymentMethod.UserTemplatePath = CommonHelper.EnsureMaximumLength(paymentMethod.UserTemplatePath, 500);
        //    paymentMethod.ClassName = CommonHelper.EnsureNotNull(paymentMethod.ClassName);
        //    paymentMethod.ClassName = CommonHelper.EnsureMaximumLength(paymentMethod.ClassName, 500);
        //    paymentMethod.SystemKeyword = CommonHelper.EnsureNotNull(paymentMethod.SystemKeyword);
        //    paymentMethod.SystemKeyword = CommonHelper.EnsureMaximumLength(paymentMethod.SystemKeyword, 500);


        //    if (!_context.IsAttached(paymentMethod))
        //        _context.PaymentMethods.Attach(paymentMethod);

        //    _context.SaveChanges();
        //}          
        //#endregion

        //#region Workflow

        ///// <summary>
        ///// Process payment
        ///// </summary>
        ///// <param name="paymentInfo">Payment info required for an order processing</param>
        ///// <param name="customer">Customer</param>
        ///// <param name="orderGuid">Unique order identifier</param>
        ///// <param name="processPaymentResult">Process payment result</param>
        //public void ProcessPayment(PaymentInfo paymentInfo, Customer customer,
        //    Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
        //{
        //    if (paymentInfo.OrderTotal == decimal.Zero)
        //    {
        //        processPaymentResult.Error = string.Empty;
        //        processPaymentResult.FullError = string.Empty;
        //        processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
        //    }
        //    else
        //    {
        //        var paymentMethod = GetPaymentMethodById(paymentInfo.PaymentMethodId);
        //        if (paymentMethod == null)
        //            throw new Exception("Payment method couldn't be loaded");
        //        var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
        //        iPaymentMethod.ProcessPayment(paymentInfo, customer, orderGuid, ref processPaymentResult);
        //    }
        //}

        ///// <summary>
        ///// Post process payment (payment gateways that require redirecting)
        ///// </summary>
        ///// <param name="order">Order</param>
        ///// <returns>The error status, or String.Empty if no errors</returns>
        //public string PostProcessPayment(Order order)
        //{
        //    //already paid or order.OrderTotal == decimal.Zero
        //    if (order.OrderTotal == decimal.Zero)
        //        return string.Empty;

        //    var paymentMethod = GetPaymentMethodById(order.PaymentMethodId);
        //    if (paymentMethod == null)
        //        throw new Exception("Payment method couldn't be loaded");
        //    var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
        //    return iPaymentMethod.PostProcessPayment(order);
        //}

        ///// <summary>
        ///// Gets additional handling fee
        ///// </summary>
        ///// <param name="paymentMethodId">Payment method identifier</param>
        ///// <returns>Additional handling fee</returns>
        //public decimal GetAdditionalHandlingFee(int paymentMethodId)
        //{
        //    var paymentMethod = GetPaymentMethodById(paymentMethodId);
        //    if (paymentMethod == null)
        //        return decimal.Zero;
        //    var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;

        //    decimal result = iPaymentMethod.GetAdditionalHandlingFee();
        //    if (result < decimal.Zero)
        //        result = decimal.Zero;
        //    result = Math.Round(result, 2);
        //    return result;
        //}

        ///// <summary>
        ///// Gets a value indicating whether capture is supported by payment method
        ///// </summary>
        ///// <param name="paymentMethodId">Payment method identifier</param>
        ///// <returns>A value indicating whether capture is supported</returns>
        //public bool CanCapture(int paymentMethodId)
        //{
        //    var paymentMethod = GetPaymentMethodById(paymentMethodId);
        //    if (paymentMethod == null)
        //        return false;
        //    var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
        //    return iPaymentMethod.CanCapture;
        //}

        ///// <summary>
        ///// Captures payment
        ///// </summary>
        ///// <param name="order">Order</param>
        ///// <param name="processPaymentResult">Process payment result</param>
        //public void Capture(Order order, ref ProcessPaymentResult processPaymentResult)
        //{
        //    var paymentMethod = GetPaymentMethodById(order.PaymentMethodId);
        //    if (paymentMethod == null)
        //        throw new Exception("Payment method couldn't be loaded");
        //    var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
        //    iPaymentMethod.Capture(order, ref processPaymentResult);
        //}

        ///// <summary>
        ///// Gets a value indicating whether partial refund is supported by payment method
        ///// </summary>
        ///// <param name="paymentMethodId">Payment method identifier</param>
        ///// <returns>A value indicating whether partial refund is supported</returns>
        //public bool CanPartiallyRefund(int paymentMethodId)
        //{
        //    var paymentMethod = GetPaymentMethodById(paymentMethodId);
        //    if (paymentMethod == null)
        //        return false;
        //    var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
        //    return iPaymentMethod.CanPartiallyRefund;
        //}

        ///// <summary>
        ///// Gets a value indicating whether refund is supported by payment method
        ///// </summary>
        ///// <param name="paymentMethodId">Payment method identifier</param>
        ///// <returns>A value indicating whether refund is supported</returns>
        //public bool CanRefund(int paymentMethodId)
        //{
        //    var paymentMethod = GetPaymentMethodById(paymentMethodId);
        //    if (paymentMethod == null)
        //        return false;
        //    var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
        //    return iPaymentMethod.CanRefund;
        //}

        ///// <summary>
        ///// Refunds payment
        ///// </summary>
        ///// <param name="order">Order</param>
        ///// <param name="cancelPaymentResult">Cancel payment result</param>
        //public void Refund(Order order, ref CancelPaymentResult cancelPaymentResult)
        //{
        //    var paymentMethod = GetPaymentMethodById(order.PaymentMethodId);
        //    if (paymentMethod == null)
        //        throw new Exception("Payment method couldn't be loaded");
        //    var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
        //    iPaymentMethod.Refund(order, ref cancelPaymentResult);
        //}

        ///// <summary>
        ///// Gets a value indicating whether void is supported by payment method
        ///// </summary>
        ///// <param name="paymentMethodId">Payment method identifier</param>
        ///// <returns>A value indicating whether void is supported</returns>
        //public bool CanVoid(int paymentMethodId)
        //{
        //    var paymentMethod = GetPaymentMethodById(paymentMethodId);
        //    if (paymentMethod == null)
        //        return false;
        //    var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
        //    return iPaymentMethod.CanVoid;
        //}

        ///// <summary>
        ///// Voids payment
        ///// </summary>
        ///// <param name="order">Order</param>
        ///// <param name="cancelPaymentResult">Cancel payment result</param>
        //public void Void(Order order, ref CancelPaymentResult cancelPaymentResult)
        //{
        //    var paymentMethod = GetPaymentMethodById(order.PaymentMethodId);
        //    if (paymentMethod == null)
        //        throw new Exception("Payment method couldn't be loaded");
        //    var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
        //    iPaymentMethod.Void(order, ref cancelPaymentResult);
        //}

        ///// <summary>
        ///// Gets a recurring payment type of payment method
        ///// </summary>
        ///// <param name="paymentMethodId">Payment method identifier</param>
        ///// <returns>A recurring payment type of payment method</returns>
        //public RecurringPaymentTypeEnum SupportRecurringPayments(int paymentMethodId)
        //{
        //    var paymentMethod = GetPaymentMethodById(paymentMethodId);
        //    if (paymentMethod == null)
        //        return RecurringPaymentTypeEnum.NotSupported;
        //    var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
        //    return iPaymentMethod.SupportRecurringPayments;
        //}

        ///// <summary>
        ///// Gets a payment method type
        ///// </summary>
        ///// <param name="paymentMethodId">Payment method identifier</param>
        ///// <returns>A payment method type</returns>
        //public PaymentMethodTypeEnum GetPaymentMethodType(int paymentMethodId)
        //{
        //    var paymentMethod = GetPaymentMethodById(paymentMethodId);
        //    if (paymentMethod == null)
        //        return PaymentMethodTypeEnum.Unknown;
        //    var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
        //    return iPaymentMethod.PaymentMethodType;
        //}

        ///// <summary>
        ///// Process recurring payments
        ///// </summary>
        ///// <param name="paymentInfo">Payment info required for an order processing</param>
        ///// <param name="customer">Customer</param>
        ///// <param name="orderGuid">Unique order identifier</param>
        ///// <param name="processPaymentResult">Process payment result</param>
        //public void ProcessRecurringPayment(PaymentInfo paymentInfo, Customer customer,
        //    Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
        //{
        //    if (paymentInfo.OrderTotal == decimal.Zero)
        //    {
        //        processPaymentResult.Error = string.Empty;
        //        processPaymentResult.FullError = string.Empty;
        //        processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
        //    }
        //    else
        //    {
        //        var paymentMethod = GetPaymentMethodById(paymentInfo.PaymentMethodId);
        //        if (paymentMethod == null)
        //            throw new Exception("Payment method couldn't be loaded");
        //        var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
        //        iPaymentMethod.ProcessRecurringPayment(paymentInfo, customer, orderGuid, ref processPaymentResult);
        //    }
        //}

        ///// <summary>
        ///// Cancels recurring payment
        ///// </summary>
        ///// <param name="order">Order</param>
        ///// <param name="cancelPaymentResult">Cancel payment result</param>
        //public void CancelRecurringPayment(Order order, ref CancelPaymentResult cancelPaymentResult)
        //{
        //    if (order.OrderTotal == decimal.Zero)
        //        return;

        //    var paymentMethod = GetPaymentMethodById(order.PaymentMethodId);
        //    if (paymentMethod == null)
        //        throw new Exception("Payment method couldn't be loaded");
        //    var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
        //    iPaymentMethod.CancelRecurringPayment(order, ref cancelPaymentResult);
        //}

        ///// <summary>
        ///// Gets masked credit card number
        ///// </summary>
        ///// <param name="creditCardNumber">Credit card number</param>
        ///// <returns>Masked credit card number</returns>
        //public string GetMaskedCreditCardNumber(string creditCardNumber)
        //{
        //    if (String.IsNullOrEmpty(creditCardNumber))
        //        return string.Empty;

        //    if (creditCardNumber.Length <= 4)
        //        return creditCardNumber;

        //    string last4 = creditCardNumber.Substring(creditCardNumber.Length - 4, 4);
        //    string maskedChars = string.Empty;
        //    for (int i = 0; i < creditCardNumber.Length - 4; i++)
        //    {
        //        maskedChars += "*";
        //    }
        //    return maskedChars + last4;
        //}                    
        //#endregion               
        //#endregion                       
    }
}
