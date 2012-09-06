using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.CustomerManagement;
using BetEx247.Core.Infrastructure;

namespace BetEx247.Core.Payment
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
        /// Gets or sets the initial order identifier
        /// </summary>
        public int InitialOrderId { get; set; }

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
        /// Gets the initial order
        /// </summary>
        public Order InitialOrder
        {
            get
            {
                return IoC.Resolve<IOrderService>().GetOrderById(this.InitialOrderId);
            }
        }

        /// <summary>
        /// Gets the initial customer
        /// </summary>
        public Customer Customer
        {
            get
            {
                Customer customer = null;
                Order initialOrder = this.InitialOrder;
                if (initialOrder != null)
                {
                    customer = initialOrder.Customer;
                }
                return customer;
            }
        }

        /// <summary>
        /// Gets the recurring payment history
        /// </summary>
        public List<RecurringPaymentHistory> RecurringPaymentHistory
        {
            get
            {
                return IoC.Resolve<IOrderService>().SearchRecurringPaymentHistory(this.RecurringPaymentId, 0);
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
                Order order = this.InitialOrder;
                if (order == null)
                    return RecurringPaymentTypeEnum.NotSupported;

                return IoC.Resolve<IPaymentService>().SupportRecurringPayments(order.PaymentMethodId);
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
