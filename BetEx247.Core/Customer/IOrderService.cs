using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Payment;

namespace BetEx247.Core.CustomerManagement
{
    /// <summary>
    /// Order service
    /// </summary>
    public partial interface IOrderService
    {
        #region Orders

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderId">The order identifier</param>
        /// <returns>Order</returns>
        Order GetOrderById(int orderId);

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderGuid">The order identifier</param>
        /// <returns>Order</returns>
        Order GetOrderByGuid(Guid orderGuid);

        /// <summary>
        /// Marks an order as deleted
        /// </summary>
        /// <param name="orderId">The order identifier</param>
        void MarkOrderAsDeleted(int orderId);

        /// <summary>
        /// Search orders
        /// </summary>
        /// <param name="startTime">Order start time; null to load all orders</param>
        /// <param name="endTime">Order end time; null to load all orders</param>
        /// <param name="customerEmail">Customer email</param>
        /// <param name="os">Order status; null to load all orders</param>
        /// <param name="ps">Order payment status; null to load all orders</param>
        /// <param name="ss">Order shippment status; null to load all orders</param>
        /// <returns>Order collection</returns>
        List<Order> SearchOrders(DateTime? startTime, DateTime? endTime,
            string customerEmail, OrderStatusEnum? os, PaymentStatusEnum? ps);

        /// <summary>
        /// Search orders
        /// </summary>
        /// <param name="startTime">Order start time; null to load all orders</param>
        /// <param name="endTime">Order end time; null to load all orders</param>
        /// <param name="customerEmail">Customer email</param>
        /// <param name="os">Order status; null to load all orders</param>
        /// <param name="ps">Order payment status; null to load all orders</param>
        /// <param name="ss">Order shippment status; null to load all orders</param>
        /// <param name="orderGuid">Search by order GUID (Global unique identifier) or part of GUID. Leave empty to load all orders.</param>
        /// <returns>Order collection</returns>
        List<Order> SearchOrders(DateTime? startTime, DateTime? endTime,
            string customerEmail, OrderStatusEnum? os, PaymentStatusEnum? ps,
            string orderGuid);

        /// <summary>
        /// Load all orders
        /// </summary>
        /// <returns>Order collection</returns>
        List<Order> LoadAllOrders();

        /// <summary>
        /// Gets all orders by customer identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Order collection</returns>
        List<Order> GetOrdersByCustomerId(int customerId);

        /// <summary>
        /// Gets an order by authorization transaction identifier
        /// </summary>
        /// <param name="authorizationTransactionId">Authorization transaction identifier</param>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>Order</returns>
        Order GetOrderByAuthorizationTransactionIdAndPaymentMethodId(string authorizationTransactionId,
            int paymentMethodId);

        /// <summary>
        /// Gets all orders by affiliate identifier
        /// </summary>
        /// <param name="affiliateId">Affiliate identifier</param>
        /// <returns>Order collection</returns>
        List<Order> GetOrdersByAffiliateId(int affiliateId);

        /// <summary>
        /// Inserts an order
        /// </summary>
        /// <param name="order">Order</param>
        void InsertOrder(Order order);

        /// <summary>
        /// Updates the order
        /// </summary>
        /// <param name="order">The order</param>
        void UpdateOrder(Order order);

        #endregion                     


        #region Recurring payments

        /// <summary>
        /// Gets a recurring payment
        /// </summary>
        /// <param name="recurringPaymentId">The recurring payment identifier</param>
        /// <returns>Recurring payment</returns>
        RecurringPayment GetRecurringPaymentById(int recurringPaymentId);

        /// <summary>
        /// Deletes a recurring payment
        /// </summary>
        /// <param name="recurringPaymentId">Recurring payment identifier</param>
        void DeleteRecurringPayment(int recurringPaymentId);

        /// <summary>
        /// Inserts a recurring payment
        /// </summary>
        /// <param name="recurringPayment">Recurring payment</param>
        void InsertRecurringPayment(RecurringPayment recurringPayment);

        /// <summary>
        /// Updates the recurring payment
        /// </summary>
        /// <param name="recurringPayment">Recurring payment</param>
        void UpdateRecurringPayment(RecurringPayment recurringPayment);

        /// <summary>
        /// Search recurring payments
        /// </summary>
        /// <param name="customerId">The customer identifier; 0 to load all records</param>
        /// <param name="initialOrderId">The initial order identifier; 0 to load all records</param>
        /// <param name="initialOrderStatus">Initial order status identifier; null to load all records</param>
        /// <returns>Recurring payment collection</returns>
        List<RecurringPayment> SearchRecurringPayments(int customerId,
            int initialOrderId, OrderStatusEnum? initialOrderStatus);

        /// <summary>
        /// Search recurring payments
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="customerId">The customer identifier; 0 to load all records</param>
        /// <param name="initialOrderId">The initial order identifier; 0 to load all records</param>
        /// <param name="initialOrderStatus">Initial order status identifier; null to load all records</param>
        /// <returns>Recurring payment collection</returns>
        List<RecurringPayment> SearchRecurringPayments(bool showHidden,
            int customerId, int initialOrderId, OrderStatusEnum? initialOrderStatus);

        /// <summary>
        /// Deletes a recurring payment history
        /// </summary>
        /// <param name="recurringPaymentHistoryId">Recurring payment history identifier</param>
        void DeleteRecurringPaymentHistory(int recurringPaymentHistoryId);

        /// <summary>
        /// Gets a recurring payment history
        /// </summary>
        /// <param name="recurringPaymentHistoryId">The recurring payment history identifier</param>
        /// <returns>Recurring payment history</returns>
        RecurringPaymentHistory GetRecurringPaymentHistoryById(int recurringPaymentHistoryId);

        /// <summary>
        /// Inserts a recurring payment history
        /// </summary>
        /// <param name="recurringPaymentHistory">Recurring payment history</param>
        void InsertRecurringPaymentHistory(RecurringPaymentHistory recurringPaymentHistory);

        /// <summary>
        /// Updates the recurring payment history
        /// </summary>
        /// <param name="recurringPaymentHistory">Recurring payment history</param>
        void UpdateRecurringPaymentHistory(RecurringPaymentHistory recurringPaymentHistory);

        /// <summary>
        /// Search recurring payment history
        /// </summary>
        /// <param name="recurringPaymentId">The recurring payment identifier; 0 to load all records</param>
        /// <param name="orderId">The order identifier; 0 to load all records</param>
        /// <returns>Recurring payment history collection</returns>
        List<RecurringPaymentHistory> SearchRecurringPaymentHistory(int recurringPaymentId,
            int orderId);

        #endregion    

        #region Return requests (RMA)

        /// <summary>
        /// Gets a return request status name
        /// </summary>
        /// <param name="rs">Return status</param>
        /// <returns>Return request status name</returns>
        string GetReturnRequestStatusName(ReturnStatusEnum rs);

        /// <summary>
        /// Check whether return request is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        bool IsReturnRequestAllowed(Order order);

        /// <summary>
        /// Gets a return request
        /// </summary>
        /// <param name="returnRequestId">Return request identifier</param>
        /// <returns>Return request</returns>
        ReturnRequest GetReturnRequestById(int returnRequestId);

        /// <summary>
        /// Deletes a return request
        /// </summary>
        /// <param name="returnRequestId">Return request identifier</param>
        void DeleteReturnRequest(int returnRequestId);

        /// <summary>
        /// Search return requests
        /// </summary>
        /// <param name="customerId">Customer identifier; null to load all entries</param>
        /// <param name="orderProductVariantId">Order product variant identifier; null to load all entries</param>
        /// <param name="rs">Return status; null to load all entries</param>
        /// <returns>Return requests</returns>
        List<ReturnRequest> SearchReturnRequests(int customerId,
            int orderProductVariantId, ReturnStatusEnum? rs);

        /// <summary>
        /// Inserts a return request
        /// </summary>
        /// <param name="returnRequest">Return request</param>
        /// <param name="notifyStoreOwner">A value indicating whether to notify about new return request</param>
        void InsertReturnRequest(ReturnRequest returnRequest, bool notifyStoreOwner);

        /// <summary>
        /// Updates the return request
        /// </summary>
        /// <param name="returnRequest">Return request</param>
        void UpdateReturnRequest(ReturnRequest returnRequest);

        #endregion

        #region Etc          
        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="paymentInfo">Payment info</param>
        /// <param name="customer">Customer</param>
        /// <param name="orderId">Order identifier</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        string PlaceOrder(PaymentInfo paymentInfo, Customer customer,
            out int orderId);

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="paymentInfo">Payment info</param>
        /// <param name="customer">Customer</param>
        /// <param name="orderGuid">Order GUID to use</param>
        /// <param name="orderId">Order identifier</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        string PlaceOrder(PaymentInfo paymentInfo, Customer customer,
            Guid orderGuid, out int orderId);

        /// <summary>
        /// Place order items in current user shopping cart.
        /// </summary>
        /// <param name="orderId">The order identifier</param>
        void ReOrder(int orderId);

        /// <summary>
        /// Process next recurring psayment
        /// </summary>
        /// <param name="recurringPaymentId">Recurring payment identifier</param>
        void ProcessNextRecurringPayment(int recurringPaymentId);

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="recurringPaymentId">Recurring payment identifier</param>
        RecurringPayment CancelRecurringPayment(int recurringPaymentId);

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="recurringPaymentId">Recurring payment identifier</param>
        /// <param name="throwException">A value indicating whether to throw the exception after an error has occupied.</param>
        RecurringPayment CancelRecurringPayment(int recurringPaymentId,
            bool throwException);

        /// <summary>
        /// Gets a value indicating whether a customer can cancel recurring payment
        /// </summary>
        /// <param name="customerToValidate">Customer</param>
        /// <param name="recurringPayment">Recurring Payment</param>
        /// <returns>value indicating whether a customer can cancel recurring payment</returns>
        bool CanCancelRecurringPayment(Customer customerToValidate, RecurringPayment recurringPayment);
    
        /// <summary>
        /// Gets a value indicating whether cancel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether cancel is allowed</returns>
        bool CanCancelOrder(Order order);

        /// <summary>
        /// Cancels order
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <param name="notifyCustomer">True to notify customer</param>
        /// <returns>Cancelled order</returns>
        Order CancelOrder(int orderId, bool notifyCustomer);

        /// <summary>
        /// Gets a value indicating whether order can be marked as authorized
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether order can be marked as authorized</returns>
        bool CanMarkOrderAsAuthorized(Order order);

        /// <summary>
        /// Marks order as authorized
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <returns>Authorized order</returns>
        Order MarkAsAuthorized(int orderId);

        /// <summary>
        /// Gets a value indicating whether capture from admin panel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether capture from admin panel is allowed</returns>
        bool CanCapture(Order order);

        /// <summary>
        /// Captures order (from admin panel)
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <param name="error">Error</param>
        /// <returns>Captured order</returns>
        Order Capture(int orderId, ref string error);

        /// <summary>
        /// Gets a value indicating whether order can be marked as paid
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether order can be marked as paid</returns>
        bool CanMarkOrderAsPaid(Order order);

        /// <summary>
        /// Marks order as paid
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <returns>Updated order</returns>
        Order MarkOrderAsPaid(int orderId);

        /// <summary>
        /// Gets a value indicating whether refund from admin panel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether refund from admin panel is allowed</returns>
        bool CanRefund(Order order);

        /// <summary>
        /// Refunds an order (from admin panel)
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <param name="error">Error</param>
        /// <returns>Refunded order</returns>
        Order Refund(int orderId, ref string error);

        /// <summary>
        /// Gets a value indicating whether order can be marked as refunded
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether order can be marked as refunded</returns>
        bool CanRefundOffline(Order order);

        /// <summary>
        /// Refunds an order (offline)
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <returns>Updated order</returns>
        Order RefundOffline(int orderId);

        /// <summary>
        /// Gets a value indicating whether partial refund from admin panel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <returns>A value indicating whether refund from admin panel is allowed</returns>
        bool CanPartiallyRefund(Order order, decimal amountToRefund);

        /// <summary>
        /// Partially refunds an order (from admin panel)
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <param name="error">Error</param>
        /// <returns>Refunded order</returns>
        Order PartiallyRefund(int orderId, decimal amountToRefund, ref string error);

        /// <summary>
        /// Gets a value indicating whether order can be marked as partially refunded
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <returns>A value indicating whether order can be marked as partially refunded</returns>
        bool CanPartiallyRefundOffline(Order order, decimal amountToRefund);

        /// <summary>
        /// Partially refunds an order (offline)
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <returns>Updated order</returns>
        Order PartiallyRefundOffline(int orderId, decimal amountToRefund);

        /// <summary>
        /// Gets a value indicating whether void from admin panel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether void from admin panel is allowed</returns>
        bool CanVoid(Order order);

        /// <summary>
        /// Voids order (from admin panel)
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <param name="error">Error</param>
        /// <returns>Voided order</returns>
        Order Void(int orderId, ref string error);

        /// <summary>
        /// Gets a value indicating whether order can be marked as voided
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether order can be marked as voided</returns>
        bool CanVoidOffline(Order order);

        /// <summary>
        /// Voids order (offline)
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <returns>Updated order</returns>
        Order VoidOffline(int orderId);

        /// <summary>
        /// Converts reward points to amount primary store currency
        /// </summary>
        /// <param name="rewardPoints">Reward points</param>
        /// <returns>Converted value</returns>
        decimal ConvertRewardPointsToAmount(int rewardPoints);

        /// <summary>
        /// Converts an amount in primary store currency to reward points
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <returns>Converted value</returns>
        int ConvertAmountToRewardPoints(decimal amount);  
        #endregion                    
    }
}
