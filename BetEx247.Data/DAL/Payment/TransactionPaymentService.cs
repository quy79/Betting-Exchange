using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Payment;
using BetEx247.Data.Model;
using BetEx247.Core.Common.Utils;
using BetEx247.Core.Common.Extensions;
using BetEx247.Core.Infrastructure;

namespace BetEx247.Data.DAL
{
    public partial class TransactionPaymentService : ITransactionPaymentService
    {
        #region Fields

        /// <summary>
        /// Object context
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        #endregion

        #region TransactionPayments
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
                transactionPayment.TransactionIDRespone = transaction.ResponeTranId!=null? transaction.ResponeTranId.Value:0;
                transactionPayment.MemberEmail = transaction.MemberEmail;
            }
            return transactionPayment;
        }

        /// <summary>
        /// Gets last TransactionPayment by userID
        /// </summary>
        /// <param name="TransactionPaymentId">The UserId identifier</param>
        /// <returns>TransactionPayment</returns>
        public TransactionPayment GetTransactionPaymentByUserId(long UserId)
        {
            TransactionPayment transactionPayment = new TransactionPayment();
            Transaction transaction=new Transaction();
            using (var dba = new BetEXDataContainer())
            {
                byte type = (byte)1;
                var listTransaction = dba.Transactions.Where(w => w.MemberId == UserId && w.Type == type).OrderByDescending(z=>z.Amount).ToList();
                if (listTransaction != null)
                {
                    transaction = listTransaction[0];
                }
            }
            if (transaction != null)
            {
                transactionPayment.TransactionPaymentId = transaction.ID;
                transactionPayment.TransactionPaymentType = transaction.Type;
                transactionPayment.MemberId = transaction.MemberId;
                transactionPayment.MemberIP = transaction.MemberIP;
                transactionPayment.TransactionIDRespone = transaction.ResponeTranId!=null?transaction.ResponeTranId.Value:0;
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
                    transactionPayment.TransactionIDRespone = transaction.ResponeTranId != null ? transaction.ResponeTranId.Value : 0;
                    lstTransactionPayment.Add(transactionPayment);
                }
            }
            return lstTransactionPayment;
        }

        /// <summary>
        /// Inserts an TransactionPayment
        /// </summary>
        /// <param name="TransactionPayment">TransactionPayment</param>
        public void InsertTransactionPayment(TransactionPayment transactionPayment)
        {
            if (transactionPayment == null)
                throw new ArgumentNullException("transactionPayment");

            Transaction transaction = new Transaction();
            transaction.Type = transactionPayment.TransactionPaymentType;
            transaction.Amount = transactionPayment.TransactionPaymentTotal;
            transaction.Status = transactionPayment.TransactionPaymentStatusId;
            transaction.MemberId = transactionPayment.MemberId;
            transaction.MemberIP = transactionPayment.MemberIP;
            transaction.MemberEmail = transactionPayment.MemberEmail;
            transaction.ResponeTranId = transactionPayment.TransactionIDRespone;
            transaction.PaymentMenthodID = transactionPayment.PaymentMethodId;
            transaction.AddedDate = DateTime.Now;
            transaction.ModifyDate = DateTime.Now;

            _context.AddToTransactions(transaction);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates the TransactionPayment
        /// </summary>
        /// <param name="TransactionPayment">The TransactionPayment</param>
        public void UpdateTransactionPayment(TransactionPayment transactionPayment)
        {
            if (transactionPayment == null)
                throw new ArgumentNullException("transactionPayment");

            Transaction transaction = new Transaction();
            transaction = _context.Transactions.Where(w => w.ID == transactionPayment.TransactionPaymentId).SingleOrDefault();
            if (transaction != null)
            {
                transaction.ID = transactionPayment.TransactionPaymentId;
                transaction.Type = transactionPayment.TransactionPaymentType;
                transaction.Amount = transactionPayment.TransactionPaymentTotal;
                transaction.Status = transactionPayment.TransactionPaymentStatusId;
                transaction.MemberId = transactionPayment.MemberId;
                transaction.MemberIP = transactionPayment.MemberIP;
                transaction.MemberEmail = transactionPayment.MemberEmail;
                transaction.PaymentMenthodID = transactionPayment.PaymentMethodId;
                transaction.ModifyDate = DateTime.Now;
                _context.SaveChanges();
            }
        }
        #endregion

        #region Recurring payments
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
        #endregion

        #region Return requests (RMA)
        public string GetReturnRequestStatusName(ReturnStatusEnum rs)
        {
            string name = string.Empty;
            switch (rs)
            {
                case ReturnStatusEnum.Pending:
                    name = "Pending";
                    break;
                case ReturnStatusEnum.Received:
                    name = "Received";
                    break;
                case ReturnStatusEnum.ReturnAuthorized:
                    name = "ReturnAuthorized";
                    break;
                case ReturnStatusEnum.ItemsRepaired:
                    name = "ItemsRepaired";
                    break;
                case ReturnStatusEnum.ItemsRefunded:
                    name = "ItemsRefunded";
                    break;
                case ReturnStatusEnum.RequestRejected:
                    name = "RequestRejected";
                    break;
                case ReturnStatusEnum.Cancelled:
                    name = "Cancelled";
                    break;
                default:
                    name = CommonHelper.ConvertEnum(rs.ToString());
                    break;
            }
            return name;
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
        #endregion

        #region Etc
        /// <summary>
        /// Places an TransactionPayment
        /// </summary>
        /// <param name="transactionPayment">Payment info</param>
        /// <param name="Member">Member</param>
        /// <param name="TransactionPaymentId">TransactionPayment identifier</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public string PlaceTransactionPayment(TransactionPayment transactionPayment, out long TransactionPaymentId)
        {
            var TransactionGuid = Guid.NewGuid();
            return PlaceTransactionPayment(transactionPayment, TransactionGuid, out TransactionPaymentId);
        }

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="transactionPayment">Payment info</param>
        /// <param name="customer">Customer</param>
        /// <param name="orderGuid">Order GUID to use</param>
        /// <param name="orderId">Order identifier</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public string PlaceTransactionPayment(TransactionPayment transactionPayment,
            Guid transactionPaymentGuid, out long TransactionPaymentId)
        {
            TransactionPaymentId = 0;
            string outMessage = string.Empty;
            var processPaymentResult = new ProcessPaymentResult();
            var customerService = IoC.Resolve<ICustomerService>();
            var paymentService = IoC.Resolve<IPaymentService>();

            try
            {
                if (!CommonHelper.IsValidEmail(transactionPayment.Customer.Email1))
                {
                    throw new Exception("Email is not valid");
                }

                if (transactionPayment == null)
                    throw new ArgumentNullException("transactionPayment");

                //skip payment workflow if order total equals zero                  
                PaymentMethod paymentMethod = null;
                string paymentMethodName = string.Empty;
                paymentMethod = paymentService.GetPaymentMethodById(transactionPayment.PaymentMethodId);
                if (paymentMethod == null)
                    throw new Exception("Payment method couldn't be loaded");
                paymentMethodName = paymentMethod.Name;

                //recurring or standard 
                bool isRecurring = false;
                if (transactionPayment.TransactionPaymentType == 2)
                    isRecurring = true;
                //process payment
                if (!transactionPayment.IsRecurringPayment)
                {
                    if (isRecurring)
                    {
                        paymentService.ProcessRecurringPayment(transactionPayment, transactionPaymentGuid, ref processPaymentResult);
                        #region tempcode
                        //recurring cart
                        //var recurringPaymentType = paymentService.SupportRecurringPayments(transactionPayment.PaymentMethodId);
                        //switch (recurringPaymentType)
                        //{
                        //    case RecurringPaymentTypeEnum.NotSupported:
                        //        throw new Exception("Recurring payments are not supported by selected payment method");
                        //    case RecurringPaymentTypeEnum.Manual:
                        //    case RecurringPaymentTypeEnum.Automatic:
                        //        paymentService.ProcessRecurringPayment(transactionPayment, transactionPaymentGuid, ref processPaymentResult);
                        //        break;
                        //    default:
                        //        throw new Exception("Not supported recurring payment type");
                        //}
                        #endregion
                    }
                    else
                    {
                        //standard cart
                        paymentService.ProcessPayment(transactionPayment, transactionPaymentGuid, ref processPaymentResult);
                    }
                }
                else
                {
                    if (isRecurring)
                    {
                        paymentService.ProcessRecurringPayment(transactionPayment, transactionPaymentGuid, ref processPaymentResult);
                        #region tempcode
                        //var recurringPaymentType = paymentService.SupportRecurringPayments(transactionPayment.PaymentMethodId);
                        //switch (recurringPaymentType)
                        //{
                        //    case RecurringPaymentTypeEnum.NotSupported:
                        //        throw new Exception("Recurring payments are not supported by selected payment method");
                        //    case RecurringPaymentTypeEnum.Manual:
                        //paymentService.ProcessRecurringPayment(transactionPayment, transactionPaymentGuid, ref processPaymentResult);
                        //        break;
                        //    case RecurringPaymentTypeEnum.Automatic:
                        //        //payment is processed on payment gateway site
                        //        break;
                        //    default:
                        //        throw new Exception("Not supported recurring payment type");
                        //}
                        #endregion
                    }
                    else
                    {
                        throw new Exception("No recurring products");
                    }
                }


                //process transaction
                if (String.IsNullOrEmpty(processPaymentResult.Error))
                {
                    transactionPayment.TransactionIDRespone = processPaymentResult.AuthorizationTransactionCode.ToInt64();
                    InsertTransactionPayment(transactionPayment);
                    #region tempcode
                    //recurring orders
                    //if (!transactionPayment.IsRecurringPayment)
                    //{
                    //    if (isRecurring)
                    //    {
                    //        //create recurring payment
                    //        var rp = new RecurringPayment()
                    //        {
                    //            InitialTransactionPaymentId = transactionPayment.TransactionPaymentId,
                    //            CycleLength = transactionPayment.RecurringCycleLength,
                    //            CyclePeriod = transactionPayment.RecurringCyclePeriod,
                    //            TotalCycles = transactionPayment.RecurringTotalCycles,
                    //            StartDate = DateTime.UtcNow,
                    //            IsActive = true,
                    //            Deleted = false,
                    //            CreatedOn = DateTime.UtcNow
                    //        };
                    //        InsertRecurringPayment(rp);
                    //        var recurringPaymentType = paymentService.SupportRecurringPayments(transactionPayment.PaymentMethodId);
                    //        switch (recurringPaymentType)
                    //        {
                    //            case RecurringPaymentTypeEnum.NotSupported:
                    //                //not supported                                              
                    //                break;
                    //            case RecurringPaymentTypeEnum.Manual:
                    //                //first payment
                    //                var rph = new RecurringPaymentHistory()
                    //                {
                    //                    RecurringPaymentId = rp.RecurringPaymentId,
                    //                    TransactionPaymentId = transactionPayment.TransactionPaymentId,
                    //                    CreatedOn = DateTime.UtcNow
                    //                };
                    //                InsertRecurringPaymentHistory(rph);
                    //                break;
                    //            case RecurringPaymentTypeEnum.Automatic:
                    //                //will be created later (process is automated)
                    //                break;
                    //            default:
                    //                break;
                    //        }
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception exc)
            {
                processPaymentResult.Error = exc.Message;
                processPaymentResult.FullError = exc.ToString();
            }
            if (!string.IsNullOrEmpty(processPaymentResult.Error))
                outMessage = processPaymentResult.Error;
            else
                outMessage = processPaymentResult.AuthorizationTransactionResult;
            return outMessage;
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

        public decimal ConvertRewardPointsToAmount(int rewardPoints)
        {
            throw new NotImplementedException();
        }

        public int ConvertAmountToRewardPoints(decimal amount)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
