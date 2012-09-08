using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.CustomerManagement;
using BetEx247.Core.Infrastructure;

namespace BetEx247.Core.Payment
{
    /// <summary>
    /// Represents a return request
    /// </summary>
    public partial class ReturnRequest : BaseEntity
    {
        #region Fields
        private Customer _customer;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the return request identifier
        /// </summary>
        public int ReturnRequestId { get; set; }   
     
        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the reason to return
        /// </summary>
        public string ReasonForReturn { get; set; }

        /// <summary>
        /// Gets or sets the requested action
        /// </summary>
        public string RequestedAction { get; set; }

        /// <summary>
        /// Gets or sets the customer comments
        /// </summary>
        public string CustomerComments { get; set; }

        /// <summary>
        /// Gets or sets the staff notes
        /// </summary>
        public string StaffNotes { get; set; }

        /// <summary>
        /// Gets or sets the return status identifier
        /// </summary>
        public int ReturnStatusId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity update
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        #endregion

        #region Custom Properties

        /// <summary>
        /// Gets or sets the return status
        /// </summary>
        public ReturnStatusEnum ReturnStatus
        {
            get
            {
                return (ReturnStatusEnum)this.ReturnStatusId;
            }
            set
            {
                this.ReturnStatusId = (int)value;
            }
        }              
       
        /// <summary>
        /// Gets the customer
        /// </summary>
        public Customer Customer
        {
            get
            {
                if (_customer == null)
                    _customer = IoC.Resolve<ICustomerService>().GetCustomerById(this.CustomerId);
                return _customer;
            }
        }

        #endregion
    }
}
