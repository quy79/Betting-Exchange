using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.CustomerManagement;
using BetEx247.Core.Payment;

namespace BetEx247.Data.DAL
{
    public partial class BettingService:IBettingService
    {
        public Betting GetBettingById(long bettingId)
        {
            throw new NotImplementedException();
        }

        public void MarkBettingAsDeleted(long bettingId)
        {
            throw new NotImplementedException();
        }

        public List<Betting> SearchBettings(DateTime? startTime, DateTime? endTime, string memberEmail, BettingStatusEnum? os, PaymentStatusEnum? ps)
        {
            throw new NotImplementedException();
        }

        public List<Betting> LoadAllBettings()
        {
            throw new NotImplementedException();
        }

        public List<Betting> GetBettingsByMemberId(long memberId)
        {
            throw new NotImplementedException();
        }

        public Betting GetBettingByAuthorizationTransactionIdAndPaymentMethodId(string authorizationTransactionId, int paymentMethodId)
        {
            throw new NotImplementedException();
        }

        public void InsertBetting(Betting betting)
        {
            throw new NotImplementedException();
        }

        public void UpdateBetting(Betting betting)
        {
            throw new NotImplementedException();
        }

        public RecurringPayment GetRecurringPaymentById(int recurringPaymentId)
        {
            throw new NotImplementedException();
        }

        public void DeleteRecurringPayment(int recurringPaymentId)
        {
            throw new NotImplementedException();
        }

        public void InsertRecurringPayment(RecurringPayment recurringPayment)
        {
            throw new NotImplementedException();
        }

        public void UpdateRecurringPayment(RecurringPayment recurringPayment)
        {
            throw new NotImplementedException();
        }

        public List<RecurringPayment> SearchRecurringPayments(long memberId, long initialBettingId, BettingStatusEnum? initialBettingStatus)
        {
            throw new NotImplementedException();
        }

        public List<RecurringPayment> SearchRecurringPayments(bool showHidden, long memberId, long initialBettingId, BettingStatusEnum? initialBettingStatus)
        {
            throw new NotImplementedException();
        }

        public void DeleteRecurringPaymentHistory(int recurringPaymentHistoryId)
        {
            throw new NotImplementedException();
        }

        public RecurringPaymentHistory GetRecurringPaymentHistoryById(int recurringPaymentHistoryId)
        {
            throw new NotImplementedException();
        }

        public void InsertRecurringPaymentHistory(RecurringPaymentHistory recurringPaymentHistory)
        {
            throw new NotImplementedException();
        }

        public void UpdateRecurringPaymentHistory(RecurringPaymentHistory recurringPaymentHistory)
        {
            throw new NotImplementedException();
        }

        public List<RecurringPaymentHistory> SearchRecurringPaymentHistory(int recurringPaymentId, long BettingId)
        {
            throw new NotImplementedException();
        }

        public string GetReturnRequestStatusName(ReturnStatusEnum rs)
        {
            throw new NotImplementedException();
        }

        public bool IsReturnRequestAllowed(Betting betting)
        {
            throw new NotImplementedException();
        }

        public ReturnRequest GetReturnRequestById(int returnRequestId)
        {
            throw new NotImplementedException();
        }

        public void DeleteReturnRequest(int returnRequestId)
        {
            throw new NotImplementedException();
        }

        public void InsertReturnRequest(ReturnRequest returnRequest, bool notifyStoreOwner)
        {
            throw new NotImplementedException();
        }

        public void UpdateReturnRequest(ReturnRequest returnRequest)
        {
            throw new NotImplementedException();
        }

        public string PlaceBetting(PaymentInfo paymentInfo, Model.Member Member, out long BettingId)
        {
            throw new NotImplementedException();
        }

        public void ReBetting(long BettingId)
        {
            throw new NotImplementedException();
        }

        public void ProcessNextRecurringPayment(int recurringPaymentId)
        {
            throw new NotImplementedException();
        }

        public RecurringPayment CancelRecurringPayment(int recurringPaymentId)
        {
            throw new NotImplementedException();
        }

        public RecurringPayment CancelRecurringPayment(int recurringPaymentId, bool throwException)
        {
            throw new NotImplementedException();
        }

        public bool CanCancelRecurringPayment(Model.Member memberToValidate, RecurringPayment recurringPayment)
        {
            throw new NotImplementedException();
        }

        public bool CanCancelBetting(Betting betting)
        {
            throw new NotImplementedException();
        }

        public Betting CancelBetting(long bettingId, bool notifyMember)
        {
            throw new NotImplementedException();
        }

        public bool CanMarkBettingAsAuthorized(Betting betting)
        {
            throw new NotImplementedException();
        }

        public Betting MarkAsAuthorized(long bettingId)
        {
            throw new NotImplementedException();
        }

        public bool CanCapture(Betting betting)
        {
            throw new NotImplementedException();
        }

        public Betting Capture(long bettingId, ref string error)
        {
            throw new NotImplementedException();
        }

        public bool CanMarkBettingAsPaid(Betting betting)
        {
            throw new NotImplementedException();
        }

        public Betting MarkBettingAsPaid(long bettingId)
        {
            throw new NotImplementedException();
        }

        public bool CanRefund(Betting betting)
        {
            throw new NotImplementedException();
        }

        public Betting Refund(long bettingId, ref string error)
        {
            throw new NotImplementedException();
        }

        public bool CanRefundOffline(Betting betting)
        {
            throw new NotImplementedException();
        }

        public Betting RefundOffline(long bettingId)
        {
            throw new NotImplementedException();
        }

        public bool CanPartiallyRefund(Betting betting, decimal amountToRefund)
        {
            throw new NotImplementedException();
        }

        public Betting PartiallyRefund(long bettingId, decimal amountToRefund, ref string error)
        {
            throw new NotImplementedException();
        }

        public bool CanPartiallyRefundOffline(Betting betting, decimal amountToRefund)
        {
            throw new NotImplementedException();
        }

        public Betting PartiallyRefundOffline(long bettingId, decimal amountToRefund)
        {
            throw new NotImplementedException();
        }

        public bool CanVoid(Betting betting)
        {
            throw new NotImplementedException();
        }

        public Betting Void(long bettingId, ref string error)
        {
            throw new NotImplementedException();
        }

        public bool CanVoidOffline(Betting betting)
        {
            throw new NotImplementedException();
        }

        public Betting VoidOffline(long bettingId)
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
