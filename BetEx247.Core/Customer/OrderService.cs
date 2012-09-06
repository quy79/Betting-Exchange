using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.CustomerManagement
{
    /// <summary>
    /// Order service
    /// </summary>
    public partial class OrderService : IOrderService
    {

        public Order GetOrderById(int orderId)
        {
            throw new NotImplementedException();
        }

        public Order GetOrderByGuid(Guid orderGuid)
        {
            throw new NotImplementedException();
        }

        public void MarkOrderAsDeleted(int orderId)
        {
            throw new NotImplementedException();
        }

        public List<Order> SearchOrders(DateTime? startTime, DateTime? endTime, string customerEmail, OrderStatusEnum? os, Payment.PaymentStatusEnum? ps)
        {
            throw new NotImplementedException();
        }

        public List<Order> SearchOrders(DateTime? startTime, DateTime? endTime, string customerEmail, OrderStatusEnum? os, Payment.PaymentStatusEnum? ps, string orderGuid)
        {
            throw new NotImplementedException();
        }

        public List<Order> LoadAllOrders()
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrdersByCustomerId(int customerId)
        {
            throw new NotImplementedException();
        }

        public Order GetOrderByAuthorizationTransactionIdAndPaymentMethodId(string authorizationTransactionId, int paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrdersByAffiliateId(int affiliateId)
        {
            throw new NotImplementedException();
        }

        public void InsertOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public Payment.RecurringPayment GetRecurringPaymentById(int recurringPaymentId)
        {
            throw new NotImplementedException();
        }

        public void DeleteRecurringPayment(int recurringPaymentId)
        {
            throw new NotImplementedException();
        }

        public void InsertRecurringPayment(Payment.RecurringPayment recurringPayment)
        {
            throw new NotImplementedException();
        }

        public void UpdateRecurringPayment(Payment.RecurringPayment recurringPayment)
        {
            throw new NotImplementedException();
        }

        public List<Payment.RecurringPayment> SearchRecurringPayments(int customerId, int initialOrderId, OrderStatusEnum? initialOrderStatus)
        {
            throw new NotImplementedException();
        }

        public List<Payment.RecurringPayment> SearchRecurringPayments(bool showHidden, int customerId, int initialOrderId, OrderStatusEnum? initialOrderStatus)
        {
            throw new NotImplementedException();
        }

        public void DeleteRecurringPaymentHistory(int recurringPaymentHistoryId)
        {
            throw new NotImplementedException();
        }

        public Payment.RecurringPaymentHistory GetRecurringPaymentHistoryById(int recurringPaymentHistoryId)
        {
            throw new NotImplementedException();
        }

        public void InsertRecurringPaymentHistory(Payment.RecurringPaymentHistory recurringPaymentHistory)
        {
            throw new NotImplementedException();
        }

        public void UpdateRecurringPaymentHistory(Payment.RecurringPaymentHistory recurringPaymentHistory)
        {
            throw new NotImplementedException();
        }

        public List<Payment.RecurringPaymentHistory> SearchRecurringPaymentHistory(int recurringPaymentId, int orderId)
        {
            throw new NotImplementedException();
        }

        public string GetReturnRequestStatusName(Payment.ReturnStatusEnum rs)
        {
            throw new NotImplementedException();
        }

        public bool IsReturnRequestAllowed(Order order)
        {
            throw new NotImplementedException();
        }

        public Payment.ReturnRequest GetReturnRequestById(int returnRequestId)
        {
            throw new NotImplementedException();
        }

        public void DeleteReturnRequest(int returnRequestId)
        {
            throw new NotImplementedException();
        }

        public List<Payment.ReturnRequest> SearchReturnRequests(int customerId, int orderProductVariantId, Payment.ReturnStatusEnum? rs)
        {
            throw new NotImplementedException();
        }

        public void InsertReturnRequest(Payment.ReturnRequest returnRequest, bool notifyStoreOwner)
        {
            throw new NotImplementedException();
        }

        public void UpdateReturnRequest(Payment.ReturnRequest returnRequest)
        {
            throw new NotImplementedException();
        }

        public string PlaceOrder(Payment.PaymentInfo paymentInfo, Customer customer, out int orderId)
        {
            throw new NotImplementedException();
        }

        public string PlaceOrder(Payment.PaymentInfo paymentInfo, Customer customer, Guid orderGuid, out int orderId)
        {
            throw new NotImplementedException();
        }

        public void ReOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        public void ProcessNextRecurringPayment(int recurringPaymentId)
        {
            throw new NotImplementedException();
        }

        public Payment.RecurringPayment CancelRecurringPayment(int recurringPaymentId)
        {
            throw new NotImplementedException();
        }

        public Payment.RecurringPayment CancelRecurringPayment(int recurringPaymentId, bool throwException)
        {
            throw new NotImplementedException();
        }

        public bool CanCancelRecurringPayment(Customer customerToValidate, Payment.RecurringPayment recurringPayment)
        {
            throw new NotImplementedException();
        }

        public bool CanCancelOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public Order CancelOrder(int orderId, bool notifyCustomer)
        {
            throw new NotImplementedException();
        }

        public bool CanMarkOrderAsAuthorized(Order order)
        {
            throw new NotImplementedException();
        }

        public Order MarkAsAuthorized(int orderId)
        {
            throw new NotImplementedException();
        }

        public bool CanCapture(Order order)
        {
            throw new NotImplementedException();
        }

        public Order Capture(int orderId, ref string error)
        {
            throw new NotImplementedException();
        }

        public bool CanMarkOrderAsPaid(Order order)
        {
            throw new NotImplementedException();
        }

        public Order MarkOrderAsPaid(int orderId)
        {
            throw new NotImplementedException();
        }

        public bool CanRefund(Order order)
        {
            throw new NotImplementedException();
        }

        public Order Refund(int orderId, ref string error)
        {
            throw new NotImplementedException();
        }

        public bool CanRefundOffline(Order order)
        {
            throw new NotImplementedException();
        }

        public Order RefundOffline(int orderId)
        {
            throw new NotImplementedException();
        }

        public bool CanPartiallyRefund(Order order, decimal amountToRefund)
        {
            throw new NotImplementedException();
        }

        public Order PartiallyRefund(int orderId, decimal amountToRefund, ref string error)
        {
            throw new NotImplementedException();
        }

        public bool CanPartiallyRefundOffline(Order order, decimal amountToRefund)
        {
            throw new NotImplementedException();
        }

        public Order PartiallyRefundOffline(int orderId, decimal amountToRefund)
        {
            throw new NotImplementedException();
        }

        public bool CanVoid(Order order)
        {
            throw new NotImplementedException();
        }

        public Order Void(int orderId, ref string error)
        {
            throw new NotImplementedException();
        }

        public bool CanVoidOffline(Order order)
        {
            throw new NotImplementedException();
        }

        public Order VoidOffline(int orderId)
        {
            throw new NotImplementedException();
        }

        public decimal ConvertRewardPointsToAmount(int rewardPoints)
        {
            throw new NotImplementedException();
        }

        public int ConvertAmountToRewardPoints(decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
