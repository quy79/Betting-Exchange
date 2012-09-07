using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Core.Payment;
using BetEx247.Core.CustomerManagement;

namespace BetEx247.Data.DAL
{
    public partial interface IBettingService
    {
        #region Bettings

        /// <summary>
        /// Gets an Betting
        /// </summary>
        /// <param name="BettingId">The Betting identifier</param>
        /// <returns>Betting</returns>
        Betting GetBettingById(long bettingId);   
        
        /// <summary>
        /// Marks an Betting as deleted
        /// </summary>
        /// <param name="BettingId">The Betting identifier</param>
        void MarkBettingAsDeleted(long bettingId);

        /// <summary>
        /// Search Bettings
        /// </summary>
        /// <param name="startTime">Betting start time; null to load all Bettings</param>
        /// <param name="endTime">Betting end time; null to load all Bettings</param>
        /// <param name="MemberEmail">Member email</param>
        /// <param name="os">Betting status; null to load all Bettings</param>
        /// <param name="ps">Betting payment status; null to load all Bettings</param>
        /// <param name="ss">Betting shippment status; null to load all Bettings</param>
        /// <returns>Betting collection</returns>
        List<Betting> SearchBettings(DateTime? startTime, DateTime? endTime,
            string memberEmail, BettingStatusEnum? os, PaymentStatusEnum? ps);    
        
        /// <summary>
        /// Load all Bettings
        /// </summary>
        /// <returns>Betting collection</returns>
        List<Betting> LoadAllBettings();

        /// <summary>
        /// Gets all Bettings by Member identifier
        /// </summary>
        /// <param name="MemberId">Member identifier</param>
        /// <returns>Betting collection</returns>
        List<Betting> GetBettingsByMemberId(long memberId);

        /// <summary>
        /// Gets an Betting by authorization transaction identifier
        /// </summary>
        /// <param name="authorizationTransactionId">Authorization transaction identifier</param>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>Betting</returns>
        Betting GetBettingByAuthorizationTransactionIdAndPaymentMethodId(string authorizationTransactionId,
            int paymentMethodId);           
       
        /// <summary>
        /// Inserts an Betting
        /// </summary>
        /// <param name="Betting">Betting</param>
        void InsertBetting(Betting betting);

        /// <summary>
        /// Updates the Betting
        /// </summary>
        /// <param name="Betting">The Betting</param>
        void UpdateBetting(Betting betting);

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
        /// <param name="initialBettingId">The initial Betting identifier; 0 to load all records</param>
        /// <param name="initialBettingStatus">Initial Betting status identifier; null to load all records</param>
        /// <returns>Recurring payment collection</returns>
        List<RecurringPayment> SearchRecurringPayments(long memberId,
            long initialBettingId, BettingStatusEnum? initialBettingStatus);

        /// <summary>
        /// Search recurring payments
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="MemberId">The Member identifier; 0 to load all records</param>
        /// <param name="initialBettingId">The initial Betting identifier; 0 to load all records</param>
        /// <param name="initialBettingStatus">Initial Betting status identifier; null to load all records</param>
        /// <returns>Recurring payment collection</returns>
        List<RecurringPayment> SearchRecurringPayments(bool showHidden,
            long memberId, long initialBettingId, BettingStatusEnum? initialBettingStatus);

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
        /// <param name="BettingId">The Betting identifier; 0 to load all records</param>
        /// <returns>Recurring payment history collection</returns>
        List<RecurringPaymentHistory> SearchRecurringPaymentHistory(int recurringPaymentId,
            long BettingId);

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
        /// <param name="Betting">Betting</param>
        /// <returns>Result</returns>
        bool IsReturnRequestAllowed(Betting betting);

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
        /// Places an Betting
        /// </summary>
        /// <param name="paymentInfo">Payment info</param>
        /// <param name="Member">Member</param>
        /// <param name="BettingId">Betting identifier</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        string PlaceBetting(PaymentInfo paymentInfo, Member Member,
            out long BettingId); 
      
        /// <summary>
        /// Place Betting items in current user shopping cart.
        /// </summary>
        /// <param name="BettingId">The Betting identifier</param>
        void ReBetting(long BettingId);

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
        /// <param name="Betting">Betting</param>
        /// <returns>A value indicating whether cancel is allowed</returns>
        bool CanCancelBetting(Betting betting);

        /// <summary>
        /// Cancels Betting
        /// </summary>
        /// <param name="BettingId">Betting identifier</param>
        /// <param name="notifyMember">True to notify Member</param>
        /// <returns>Cancelled Betting</returns>
        Betting CancelBetting(long bettingId, bool notifyMember);

        /// <summary>
        /// Gets a value indicating whether Betting can be marked as authorized
        /// </summary>
        /// <param name="Betting">Betting</param>
        /// <returns>A value indicating whether Betting can be marked as authorized</returns>
        bool CanMarkBettingAsAuthorized(Betting betting);

        /// <summary>
        /// Marks Betting as authorized
        /// </summary>
        /// <param name="BettingId">Betting identifier</param>
        /// <returns>Authorized Betting</returns>
        Betting MarkAsAuthorized(long bettingId);

        /// <summary>
        /// Gets a value indicating whether capture from admin panel is allowed
        /// </summary>
        /// <param name="Betting">Betting</param>
        /// <returns>A value indicating whether capture from admin panel is allowed</returns>
        bool CanCapture(Betting betting);

        /// <summary>
        /// Captures Betting (from admin panel)
        /// </summary>
        /// <param name="BettingId">Betting identifier</param>
        /// <param name="error">Error</param>
        /// <returns>Captured Betting</returns>
        Betting Capture(long bettingId, ref string error);

        /// <summary>
        /// Gets a value indicating whether Betting can be marked as paid
        /// </summary>
        /// <param name="Betting">Betting</param>
        /// <returns>A value indicating whether Betting can be marked as paid</returns>
        bool CanMarkBettingAsPaid(Betting betting);

        /// <summary>
        /// Marks Betting as paid
        /// </summary>
        /// <param name="BettingId">Betting identifier</param>
        /// <returns>Updated Betting</returns>
        Betting MarkBettingAsPaid(long bettingId);

        /// <summary>
        /// Gets a value indicating whether refund from admin panel is allowed
        /// </summary>
        /// <param name="Betting">Betting</param>
        /// <returns>A value indicating whether refund from admin panel is allowed</returns>
        bool CanRefund(Betting betting);

        /// <summary>
        /// Refunds an Betting (from admin panel)
        /// </summary>
        /// <param name="BettingId">Betting identifier</param>
        /// <param name="error">Error</param>
        /// <returns>Refunded Betting</returns>
        Betting Refund(long bettingId, ref string error);

        /// <summary>
        /// Gets a value indicating whether Betting can be marked as refunded
        /// </summary>
        /// <param name="Betting">Betting</param>
        /// <returns>A value indicating whether Betting can be marked as refunded</returns>
        bool CanRefundOffline(Betting betting);

        /// <summary>
        /// Refunds an Betting (offline)
        /// </summary>
        /// <param name="BettingId">Betting identifier</param>
        /// <returns>Updated Betting</returns>
        Betting RefundOffline(long bettingId);

        /// <summary>
        /// Gets a value indicating whether partial refund from admin panel is allowed
        /// </summary>
        /// <param name="Betting">Betting</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <returns>A value indicating whether refund from admin panel is allowed</returns>
        bool CanPartiallyRefund(Betting betting, decimal amountToRefund);

        /// <summary>
        /// Partially refunds an Betting (from admin panel)
        /// </summary>
        /// <param name="BettingId">Betting identifier</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <param name="error">Error</param>
        /// <returns>Refunded Betting</returns>
        Betting PartiallyRefund(long bettingId, decimal amountToRefund, ref string error);

        /// <summary>
        /// Gets a value indicating whether Betting can be marked as partially refunded
        /// </summary>
        /// <param name="Betting">Betting</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <returns>A value indicating whether Betting can be marked as partially refunded</returns>
        bool CanPartiallyRefundOffline(Betting betting, decimal amountToRefund);

        /// <summary>
        /// Partially refunds an Betting (offline)
        /// </summary>
        /// <param name="BettingId">Betting identifier</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <returns>Updated Betting</returns>
        Betting PartiallyRefundOffline(long bettingId, decimal amountToRefund);

        /// <summary>
        /// Gets a value indicating whether void from admin panel is allowed
        /// </summary>
        /// <param name="Betting">Betting</param>
        /// <returns>A value indicating whether void from admin panel is allowed</returns>
        bool CanVoid(Betting betting);

        /// <summary>
        /// Voids Betting (from admin panel)
        /// </summary>
        /// <param name="BettingId">Betting identifier</param>
        /// <param name="error">Error</param>
        /// <returns>Voided Betting</returns>
        Betting Void(long bettingId, ref string error);

        /// <summary>
        /// Gets a value indicating whether Betting can be marked as voided
        /// </summary>
        /// <param name="Betting">Betting</param>
        /// <returns>A value indicating whether Betting can be marked as voided</returns>
        bool CanVoidOffline(Betting betting);

        /// <summary>
        /// Voids Betting (offline)
        /// </summary>
        /// <param name="BettingId">Betting identifier</param>
        /// <returns>Updated Betting</returns>
        Betting VoidOffline(long bettingId);

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
