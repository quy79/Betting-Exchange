using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Core.Infrastructure;

namespace BetEx247.Data.DAL
{
    public partial class TransactionPayment
    {
        private Member _customer;
        private PaymentMethod _paymentMenthod;

        #region Properties
        /// <summary>
        /// Gets or sets the TransactionPayment identifier
        /// </summary>
        public long TransactionPaymentId { get; set; }

        /// <summary>
        /// Gets or sets the TransactionPayment Type identifier
        /// 1 : Deposit
        /// 2 : Withdraw
        /// </summary>
        public Int16 TransactionPaymentType { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public long MemberId { get; set; }  
      
        /// <summary>
        /// Gets or sets the Member IP
        /// </summary>
        public string MemberIP { get; set; }
        
        /// <summary>
        /// Gets or sets the TransactionPayment total
        /// </summary>
        public decimal TransactionPaymentTotal { get; set; } 
        
        /// <summary>
        /// Gets or sets an TransactionPayment status identifer
        /// </summary>
        public int TransactionPaymentStatusId { get; set; }

        /// <summary>
        /// Gets or sets the payment method identifier
        /// </summary>
        public long PaymentMethodId { get; set; }

        /// <summary>
        /// Gets or sets the member email
        /// </summary>
        public string MemberEmail { get; set; }

        /// <summary>
        /// Gets or sets a value idicating whether it's a recurring payment (initial payment was already processed)
        /// </summary>
        public bool IsRecurringPayment { get; set; }

        /// <summary>
        /// Gets or sets the cycle length
        /// </summary>
        public int RecurringCycleLength { get; set; }

        /// <summary>
        /// Gets or sets the cycle period
        /// </summary>
        public int RecurringCyclePeriod { get; set; }

        /// <summary>
        /// Gets or sets the total cycles
        /// </summary>
        public int RecurringTotalCycles { get; set; }
        #endregion

        /// <summary>
        /// Gets the Member
        /// </summary>                    
        public Member Customer
        {
            get
            {
                if (_customer == null)
                    _customer = IoC.Resolve<ICustomerService>().GetCustomerById(this.MemberId);
                return _customer;
            }
        }

        public PaymentMethod PaymentMenthod
        {
            get
            {
                if (_paymentMenthod == null)
                    _paymentMenthod = IoC.Resolve<IPaymentService>().GetPaymentMethodById(this.PaymentMethodId);
                return _paymentMenthod;
            }
        }
    }
}
