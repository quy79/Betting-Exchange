using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.Payment;
using BetEx247.Core.Infrastructure;
using BetEx247.Core;
using BetEx247.Data.Model;

namespace BetEx247.Data.DAL
{
    /// <summary>
    /// Represents a recurring payment
    /// </summary>
    public partial class RecurringPayment : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the recurring payment identifier
        /// </summary>
        public int RecurringPaymentId { get; set; }

        /// <summary>
        /// Gets or sets the initial transactionPayment identifier
        /// </summary>
        public int InitialTransactionPaymentId { get; set; }

        /// <summary>
        /// Gets or sets the cycle length
        /// </summary>
        public int CycleLength { get; set; }

        /// <summary>
        /// Gets or sets the cycle period
        /// </summary>
        public int CyclePeriod { get; set; }

        /// <summary>
        /// Gets or sets the total cycles
        /// </summary>
        public int TotalCycles { get; set; }

        /// <summary>
        /// Gets or sets the start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the payment is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the date and time of payment creation
        /// </summary>
        public DateTime CreatedOn { get; set; }

        #endregion

        #region Custom Properties
        /// <summary>
        /// Gets the initial transactionPayment
        /// </summary>
        public TransactionPayment InitialTransactionPayment
        {
            get
            {
                return IoC.Resolve<ITransactionPaymentService>().GetTransactionPaymentById(this.InitialTransactionPaymentId);
            }
        }

        /// <summary>
        /// Gets the initial Member
        /// </summary>
        public Member Member
        {
            get
            {
                Member member = null;
                TransactionPayment initialTransactionPayment = this.InitialTransactionPayment;
                if (initialTransactionPayment != null)
                {
                    member = initialTransactionPayment.Customer;
                }
                return member;
            }
        }

        /// <summary>
        /// Gets the recurring payment history
        /// </summary>
        public List<RecurringPaymentHistory> RecurringPaymentHistory
        {
            get
            {
                return IoC.Resolve<ITransactionPaymentService>().SearchRecurringPaymentHistory(this.RecurringPaymentId, 0);
            }
        }

        /// <summary>
        /// Gets the next payment date
        /// </summary>
        public DateTime? NextPaymentDate
        {
            get
            {
                //result
                DateTime? result = null;

                if (!this.IsActive)
                    return result;

                var historyCollection = this.RecurringPaymentHistory;
                if (historyCollection.Count >= this.TotalCycles)
                {
                    return result;
                }

                //set another value to change calculation method
                bool useLatestPayment = false;
                if (useLatestPayment)
                {
                    //get latest payment
                    RecurringPaymentHistory latestPayment = null;
                    foreach (var historyRecord in historyCollection)
                    {
                        if (latestPayment != null)
                        {
                            if (historyRecord.CreatedOn >= latestPayment.CreatedOn)
                            {
                                latestPayment = historyRecord;
                            }
                        }
                        else
                        {
                            latestPayment = historyRecord;
                        }
                    }


                    //calculate next payment date
                    if (latestPayment != null)
                    {
                        switch (this.CyclePeriod)
                        {
                            case (int)RecurringProductCyclePeriodEnum.Days:
                                result = latestPayment.CreatedOn.AddDays((double)this.CycleLength);
                                break;
                            case (int)RecurringProductCyclePeriodEnum.Weeks:
                                result = latestPayment.CreatedOn.AddDays((double)(7 * this.CycleLength));
                                break;
                            case (int)RecurringProductCyclePeriodEnum.Months:
                                result = latestPayment.CreatedOn.AddMonths(this.CycleLength);
                                break;
                            case (int)RecurringProductCyclePeriodEnum.Years:
                                result = latestPayment.CreatedOn.AddYears(this.CycleLength);
                                break;
                            default:
                                throw new Exception("Not supported cycle period");
                        }
                    }
                    else
                    {
                        if (this.TotalCycles > 0)
                            result = this.StartDate;
                    }
                }
                else
                {
                    if (historyCollection.Count > 0)
                    {
                        switch (this.CyclePeriod)
                        {
                            case (int)RecurringProductCyclePeriodEnum.Days:
                                result = this.StartDate.AddDays((double)this.CycleLength * historyCollection.Count);
                                break;
                            case (int)RecurringProductCyclePeriodEnum.Weeks:
                                result = this.StartDate.AddDays((double)(7 * this.CycleLength) * historyCollection.Count);
                                break;
                            case (int)RecurringProductCyclePeriodEnum.Months:
                                result = this.StartDate.AddMonths(this.CycleLength * historyCollection.Count);
                                break;
                            case (int)RecurringProductCyclePeriodEnum.Years:
                                result = this.StartDate.AddYears(this.CycleLength * historyCollection.Count);
                                break;
                            default:
                                throw new Exception("Not supported cycle period");
                        }
                    }
                    else
                    {
                        if (this.TotalCycles > 0)
                            result = this.StartDate;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the cycles remaining
        /// </summary>
        public int CyclesRemaining
        {
            get
            {
                //result
                var historyCollection = this.RecurringPaymentHistory;
                int result = this.TotalCycles - historyCollection.Count;
                if (result < 0)
                    result = 0;

                return result;
            }
        }

        /// <summary>
        /// Gets a recurring payment type
        /// </summary>
        public RecurringPaymentTypeEnum RecurringPaymentType
        {
            get
            {
                TransactionPayment transactionPayment = this.InitialTransactionPayment;
                if (transactionPayment == null)
                    return RecurringPaymentTypeEnum.NotSupported;

                return IoC.Resolve<IPaymentService>().SupportRecurringPayments(transactionPayment.PaymentMethodId);
            }
        }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// Gets the recurring payment history
        /// </summary>
        public virtual ICollection<RecurringPaymentHistory> NpRecurringPaymentHistory { get; set; }

        #endregion
    }
}
