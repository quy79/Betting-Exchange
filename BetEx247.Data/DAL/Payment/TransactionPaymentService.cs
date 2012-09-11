using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.CustomerManagement;
using BetEx247.Core.Payment;
using BetEx247.Data.Model;

namespace BetEx247.Data.DAL
{
    public partial class TransactionPaymentService : ITransactionPaymentService
    {
        /// <summary>
        /// Gets an TransactionPayment
        /// </summary>
        /// <param name="TransactionPaymentId">The TransactionPayment identifier</param>
        /// <returns>TransactionPayment</returns>
        public TransactionPayment GetTransactionPaymentById(long transactionPaymentId)
        {
            TransactionPayment transactionPayment = new TransactionPayment();
            Transaction transaction;
            using (var dba = new BetEXDataContainer())
            {
                transaction = dba.Transactions.Where(w => w.ID == transactionPaymentId).SingleOrDefault();
            }
            if (transaction != null)
            {
                transactionPayment.TransactionPaymentId = transactionPaymentId;
                transactionPayment.TransactionPaymentType = transaction.Type;
                transactionPayment.MemberId = transaction.MemberId;
                transactionPayment.MemberIP = transaction.MemberIP;
                transactionPayment.TransactionPaymentTotal = transaction.Amount;
                transactionPayment.TransactionPaymentStatusId = transaction.Status;
                transactionPayment.PaymentMethodId = transaction.PaymentMenthodID;
                transactionPayment.MemberEmail = transaction.MemberEmail;
            }
            return transactionPayment;
        }

        public List<TransactionPayment> SearchTransactionPayments(DateTime? startTime, DateTime? endTime, string memberEmail, TransactionStatusEnum? os, PaymentStatusEnum? ps)
        {
            throw new NotImplementedException();
        }

        public List<TransactionPayment> LoadAllTransactionPayments()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all TransactionPayments by Member identifier
        /// </summary>
        /// <param name="MemberId">Member identifier</param>
        /// <returns>TransactionPayment collection</returns>
        public List<TransactionPayment> GetTransactionPaymentsByMemberId(long memberId)
        {
            List<TransactionPayment> lstTransactionPayment = new List<TransactionPayment>();
            List<Transaction> lstTransaction;
            using (var dba = new BetEXDataContainer())
            {
                lstTransaction = dba.Transactions.Where(w => w.MemberId == memberId).ToList();
            }
            if (lstTransaction != null && lstTransaction.Count > 0)
            {
                foreach (var transaction in lstTransaction)
                {
                    TransactionPayment transactionPayment = new TransactionPayment();
                    transactionPayment.TransactionPaymentId = transaction.ID;
                    transactionPayment.TransactionPaymentType = transaction.Type;
                    transactionPayment.MemberId = transaction.MemberId;
                    transactionPayment.MemberIP = transaction.MemberIP;
                    transactionPayment.TransactionPaymentTotal = transaction.Amount;
                    transactionPayment.TransactionPaymentStatusId = transaction.Status;
                    transactionPayment.PaymentMethodId = transaction.PaymentMenthodID;
                    transactionPayment.MemberEmail = transaction.MemberEmail;
                    lstTransactionPayment.Add(transactionPayment);
                }
            }
            return lstTransactionPayment;
        }


        public void InsertTransactionPayment(TransactionPayment transactionPayment)
        {
            throw new NotImplementedException();
        }

        public void UpdateTransactionPayment(TransactionPayment transactionPayment)
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

        public List<RecurringPayment> SearchRecurringPayments(long memberId, long initialTransactionPaymentId, TransactionStatusEnum? initialTransactionPaymentStatus)
        {
            throw new NotImplementedException();
        }

        public List<RecurringPayment> SearchRecurringPayments(bool showHidden, long memberId, long initialTransactionPaymentId, TransactionStatusEnum? initialTransactionPaymentStatus)
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

        public List<RecurringPaymentHistory> SearchRecurringPaymentHistory(int recurringPaymentId, long TransactionPaymentId)
        {
            throw new NotImplementedException();
        }

        public string GetReturnRequestStatusName(ReturnStatusEnum rs)
        {
            throw new NotImplementedException();
        }

        public bool IsReturnRequestAllowed(TransactionPayment transactionPayment)
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

        public string PlaceTransactionPayment(TransactionPayment transactionPayment, Member Member, out long TransactionPaymentId)
        {
            throw new NotImplementedException();
        }

        public void ReTransactionPayment(long TransactionPaymentId)
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

        public bool CanCancelRecurringPayment(Member memberToValidate, RecurringPayment recurringPayment)
        {
            throw new NotImplementedException();
        }

        public bool CanCancelTransactionPayment(TransactionPayment transactionPayment)
        {
            throw new NotImplementedException();
        }

        public TransactionPayment CancelTransactionPayment(long transactionPaymentId, bool notifyMember)
        {
            throw new NotImplementedException();
        }

        public bool CanMarkTransactionPaymentAsAuthorized(TransactionPayment transactionPayment)
        {
            throw new NotImplementedException();
        }

        public TransactionPayment MarkAsAuthorized(long transactionPaymentId)
        {
            throw new NotImplementedException();
        }

        public bool CanCapture(TransactionPayment transactionPayment)
        {
            throw new NotImplementedException();
        }

        public TransactionPayment Capture(long transactionPaymentId, ref string error)
        {
            throw new NotImplementedException();
        }

        public bool CanMarkTransactionPaymentAsPaid(TransactionPayment transactionPayment)
        {
            throw new NotImplementedException();
        }

        public TransactionPayment MarkTransactionPaymentAsPaid(long transactionPaymentId)
        {
            throw new NotImplementedException();
        }

        public bool CanRefund(TransactionPayment transactionPayment)
        {
            throw new NotImplementedException();
        }

        public TransactionPayment Refund(long transactionPaymentId, ref string error)
        {
            throw new NotImplementedException();
        }

        public bool CanRefundOffline(TransactionPayment transactionPayment)
        {
            throw new NotImplementedException();
        }

        public TransactionPayment RefundOffline(long transactionPaymentId)
        {
            throw new NotImplementedException();
        }

        public bool CanPartiallyRefund(TransactionPayment transactionPayment, decimal amountToRefund)
        {
            throw new NotImplementedException();
        }

        public TransactionPayment PartiallyRefund(long transactionPaymentId, decimal amountToRefund, ref string error)
        {
            throw new NotImplementedException();
        }

        public bool CanPartiallyRefundOffline(TransactionPayment transactionPayment, decimal amountToRefund)
        {
            throw new NotImplementedException();
        }

        public TransactionPayment PartiallyRefundOffline(long transactionPaymentId, decimal amountToRefund)
        {
            throw new NotImplementedException();
        }

        public bool CanVoid(TransactionPayment transactionPayment)
        {
            throw new NotImplementedException();
        }

        public TransactionPayment Void(long transactionPaymentId, ref string error)
        {
            throw new NotImplementedException();
        }

        public bool CanVoidOffline(TransactionPayment transactionPayment)
        {
            throw new NotImplementedException();
        }

        public TransactionPayment VoidOffline(long transactionPaymentId)
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
