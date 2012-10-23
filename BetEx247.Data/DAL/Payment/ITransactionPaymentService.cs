using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Core.Payment;

namespace BetEx247.Data.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface ITransactionPaymentService
    {
        #region TransactionPayments

        /// <summary>
        /// Gets an TransactionPayment
        /// </summary>
        /// <param name="TransactionPaymentId">The TransactionPayment identifier</param>
        /// <returns>TransactionPayment</returns>
        TransactionPayment GetTransactionPaymentById(long transactionPaymentId);

        /// <summary>
        /// Gets last TransactionPayment by userID
        /// </summary>
        /// <param name="UserId">The UserId identifier</param>
        /// <returns>TransactionPayment</returns>
        TransactionPayment GetTransactionPaymentByUserId(long UserId);

        /// <summary>
        /// Gets List Transaction by userID
        /// </summary>
        /// <param name="UserId">The UserId identifier</param>
        /// <returns>List Transaction</returns>
        List<Transaction> GetTransactionByUserId(long UserId);
        
        /// <summary>
        /// Search TransactionPayments
        /// </summary>
        /// <param name="startTime">TransactionPayment start time; null to load all TransactionPayments</param>
        /// <param name="endTime">TransactionPayment end time; null to load all TransactionPayments</param>
        /// <param name="MemberEmail">Member email</param>
        /// <param name="os">TransactionPayment status; null to load all TransactionPayments</param>
        /// <param name="ps">TransactionPayment payment status; null to load all TransactionPayments</param>
        /// <param name="ss">TransactionPayment shippment status; null to load all TransactionPayments</param>
        /// <returns>TransactionPayment collection</returns>
        List<TransactionPayment> SearchTransactionPayments(DateTime? startTime, DateTime? endTime,
            string memberEmail, TransactionStatusEnum? os, PaymentStatusEnum? ps);    
        
        /// <summary>
        /// Load all TransactionPayments
        /// </summary>
        /// <returns>TransactionPayment collection</returns>
        List<TransactionPayment> LoadAllTransactionPayments();

        /// <summary>
        /// Gets all TransactionPayments by Member identifier
        /// </summary>
        /// <param name="MemberId">Member identifier</param>
        /// <returns>TransactionPayment collection</returns>
        List<TransactionPayment> GetTransactionPaymentsByMemberId(long memberId);      
       
        /// <summary>
        /// Inserts an TransactionPayment
        /// </summary>
        /// <param name="TransactionPayment">TransactionPayment</param>
        void InsertTransactionPayment(TransactionPayment transactionPayment);

        /// <summary>
        /// Updates the TransactionPayment
        /// </summary>
        /// <param name="TransactionPayment">The TransactionPayment</param>
        void UpdateTransactionPayment(TransactionPayment transactionPayment);

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
        /// <param name="MemberId">The Member identifier; 0 to load all records</param>
        /// <param name="initialTransactionPaymentId">The initial TransactionPayment identifier; 0 to load all records</param>
        /// <param name="initialTransactionPaymentStatus">Initial TransactionPayment status identifier; null to load all records</param>
        /// <returns>Recurring payment collection</returns>
        List<RecurringPayment> SearchRecurringPayments(long memberId,
            long initialTransactionPaymentId, TransactionStatusEnum? initialTransactionPaymentStatus);

        /// <summary>
        /// Search recurring payments
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="MemberId">The Member identifier; 0 to load all records</param>
        /// <param name="initialTransactionPaymentId">The initial TransactionPayment identifier; 0 to load all records</param>
        /// <param name="initialTransactionPaymentStatus">Initial TransactionPayment status identifier; null to load all records</param>
        /// <returns>Recurring payment collection</returns>
        List<RecurringPayment> SearchRecurringPayments(bool showHidden,
            long memberId, long initialTransactionPaymentId, TransactionStatusEnum? initialTransactionPaymentStatus);

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
        /// <param name="TransactionPaymentId">The TransactionPayment identifier; 0 to load all records</param>
        /// <returns>Recurring payment history collection</returns>
        List<RecurringPaymentHistory> SearchRecurringPaymentHistory(int recurringPaymentId,
            long TransactionPaymentId);

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
        /// <param name="TransactionPayment">TransactionPayment</param>
        /// <returns>Result</returns>
        bool IsReturnRequestAllowed(TransactionPayment transactionPayment);

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
        /// Places an TransactionPayment
        /// </summary>
        /// <param name="paymentInfo">Payment info</param>
        /// <param name="Member">Member</param>
        /// <param name="TransactionPaymentId">TransactionPayment identifier</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        string PlaceTransactionPayment(TransactionPayment transactionPayment, out long TransactionPaymentId);

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="transactionPayment">transactionPayment</param>
        /// <param name="member">Member</param>
        /// <param name="transactionPaymentGuid">transactionPayment GUID to use</param>
        /// <param name="transactionPaymentId">transactionPayment identifier</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        string PlaceTransactionPayment(TransactionPayment transactionPayment,
            Guid transactionPaymentGuid, out long TransactionPaymentId); 
        
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
        /// Gets a value indicating whether a Member can cancel recurring payment
        /// </summary>
        /// <param name="MemberToValidate">Member</param>
        /// <param name="recurringPayment">Recurring Payment</param>
        /// <returns>value indicating whether a Member can cancel recurring payment</returns>
        bool CanCancelRecurringPayment(Member memberToValidate, RecurringPayment recurringPayment);

        /// <summary>
        /// Gets a value indicating whether cancel is allowed
        /// </summary>
        /// <param name="TransactionPayment">TransactionPayment</param>
        /// <returns>A value indicating whether cancel is allowed</returns>
        bool CanCancelTransactionPayment(TransactionPayment transactionPayment);

        /// <summary>
        /// Cancels TransactionPayment
        /// </summary>
        /// <param name="TransactionPaymentId">TransactionPayment identifier</param>
        /// <param name="notifyMember">True to notify Member</param>
        /// <returns>Cancelled TransactionPayment</returns>
        TransactionPayment CancelTransactionPayment(long transactionPaymentId, bool notifyMember);

        /// <summary>
        /// Gets a value indicating whether TransactionPayment can be marked as authorized
        /// </summary>
        /// <param name="TransactionPayment">TransactionPayment</param>
        /// <returns>A value indicating whether TransactionPayment can be marked as authorized</returns>
        bool CanMarkTransactionPaymentAsAuthorized(TransactionPayment transactionPayment);

        /// <summary>
        /// Marks TransactionPayment as authorized
        /// </summary>
        /// <param name="TransactionPaymentId">TransactionPayment identifier</param>
        /// <returns>Authorized TransactionPayment</returns>
        TransactionPayment MarkAsAuthorized(long transactionPaymentId);

        /// <summary>
        /// Gets a value indicating whether capture from admin panel is allowed
        /// </summary>
        /// <param name="TransactionPayment">TransactionPayment</param>
        /// <returns>A value indicating whether capture from admin panel is allowed</returns>
        bool CanCapture(TransactionPayment transactionPayment);

        /// <summary>
        /// Captures TransactionPayment (from admin panel)
        /// </summary>
        /// <param name="TransactionPaymentId">TransactionPayment identifier</param>
        /// <param name="error">Error</param>
        /// <returns>Captured TransactionPayment</returns>
        TransactionPayment Capture(long transactionPaymentId, ref string error);

        /// <summary>
        /// Gets a value indicating whether TransactionPayment can be marked as paid
        /// </summary>
        /// <param name="TransactionPayment">TransactionPayment</param>
        /// <returns>A value indicating whether TransactionPayment can be marked as paid</returns>
        bool CanMarkTransactionPaymentAsPaid(TransactionPayment transactionPayment);

        /// <summary>
        /// Marks TransactionPayment as paid
        /// </summary>
        /// <param name="TransactionPaymentId">TransactionPayment identifier</param>
        /// <returns>Updated TransactionPayment</returns>
        TransactionPayment MarkTransactionPaymentAsPaid(long transactionPaymentId);

        /// <summary>
        /// Gets a value indicating whether refund from admin panel is allowed
        /// </summary>
        /// <param name="TransactionPayment">TransactionPayment</param>
        /// <returns>A value indicating whether refund from admin panel is allowed</returns>
        bool CanRefund(TransactionPayment transactionPayment);

        /// <summary>
        /// Refunds an TransactionPayment (from admin panel)
        /// </summary>
        /// <param name="TransactionPaymentId">TransactionPayment identifier</param>
        /// <param name="error">Error</param>
        /// <returns>Refunded TransactionPayment</returns>
        TransactionPayment Refund(long transactionPaymentId, ref string error);

        /// <summary>
        /// Gets a value indicating whether TransactionPayment can be marked as refunded
        /// </summary>
        /// <param name="TransactionPayment">TransactionPayment</param>
        /// <returns>A value indicating whether TransactionPayment can be marked as refunded</returns>
        bool CanRefundOffline(TransactionPayment transactionPayment);

        /// <summary>
        /// Refunds an TransactionPayment (offline)
        /// </summary>
        /// <param name="TransactionPaymentId">TransactionPayment identifier</param>
        /// <returns>Updated TransactionPayment</returns>
        TransactionPayment RefundOffline(long transactionPaymentId);

        /// <summary>
        /// Gets a value indicating whether partial refund from admin panel is allowed
        /// </summary>
        /// <param name="TransactionPayment">TransactionPayment</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <returns>A value indicating whether refund from admin panel is allowed</returns>
        bool CanPartiallyRefund(TransactionPayment transactionPayment, decimal amountToRefund);

        /// <summary>
        /// Partially refunds an TransactionPayment (from admin panel)
        /// </summary>
        /// <param name="TransactionPaymentId">TransactionPayment identifier</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <param name="error">Error</param>
        /// <returns>Refunded TransactionPayment</returns>
        TransactionPayment PartiallyRefund(long transactionPaymentId, decimal amountToRefund, ref string error);

        /// <summary>
        /// Gets a value indicating whether TransactionPayment can be marked as partially refunded
        /// </summary>
        /// <param name="TransactionPayment">TransactionPayment</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <returns>A value indicating whether TransactionPayment can be marked as partially refunded</returns>
        bool CanPartiallyRefundOffline(TransactionPayment transactionPayment, decimal amountToRefund);

        /// <summary>
        /// Partially refunds an TransactionPayment (offline)
        /// </summary>
        /// <param name="TransactionPaymentId">TransactionPayment identifier</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <returns>Updated TransactionPayment</returns>
        TransactionPayment PartiallyRefundOffline(long transactionPaymentId, decimal amountToRefund);

        /// <summary>
        /// Gets a value indicating whether void from admin panel is allowed
        /// </summary>
        /// <param name="TransactionPayment">TransactionPayment</param>
        /// <returns>A value indicating whether void from admin panel is allowed</returns>
        bool CanVoid(TransactionPayment transactionPayment);

        /// <summary>
        /// Voids TransactionPayment (from admin panel)
        /// </summary>
        /// <param name="TransactionPaymentId">TransactionPayment identifier</param>
        /// <param name="error">Error</param>
        /// <returns>Voided TransactionPayment</returns>
        TransactionPayment Void(long transactionPaymentId, ref string error);

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

        #region MYWallter function
        /// <summary>
        /// Gets remain balance user
        /// </summary>
        /// <param name="memberId">The member identifier</param>
        /// <returns>money avalaible remain</returns>
        decimal GetBalancebyMemberId(long memberId);

        /// <summary>
        /// Check can withdraw by memberid
        /// </summary>
        /// <param name="memberId">member identifier</param>
        /// <param name="output">money want withdraw</param>
        /// <returns>true:can withdraw; false : otherwise</returns>
        bool CanWithdraw(long memberId,decimal output);

        /// <summary>
        /// Update ballance for User's Wallet
        /// </summary>
        /// <param name="memberId">member identifier</param>
        /// <param name="input">money user deposit</param>
        /// <returns>true : update successfully ; false : otherwise</returns>
        bool UpdateMyWalletDeposit(long memberId, decimal input);

        /// <summary>
        /// Update ballance for User's Wallet
        /// </summary>
        /// <param name="memberId">member identifier</param>
        /// <param name="output">money user withdraw</param>
        /// <returns>true : update successfully ; false : otherwise</returns>
        bool UpdateMyWalletWithdraw(long memberId, decimal output);
        #endregion
    }
}
