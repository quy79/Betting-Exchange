using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Core.Infrastructure;

namespace BetEx247.Data.DAL
{
    public partial class Betting
    {
        private Member _customer;

        #region Properties
        /// <summary>
        /// Gets or sets the Betting identifier
        /// </summary>
        public long BettingId { get; set; } 
        
        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public long MemberId { get; set; }  
      
        /// <summary>
        /// Gets or sets the Member IP
        /// </summary>
        public string MemberIP { get; set; }
        
        /// <summary>
        /// Gets or sets the Betting total
        /// </summary>
        public decimal BettingTotal { get; set; }

        /// <summary>
        /// Gets or sets the refunded amount
        /// </summary>
        public decimal RefundedAmount { get; set; }

        /// <summary>
        /// Gets or sets the Betting discount (applied to Betting total)
        /// </summary>
        public decimal BettingDiscount { get; set; }  
       
        /// <summary>
        /// Gets or sets the Betting total (Member currency)
        /// </summary>
        public decimal BettingTotalInMemberCurrency { get; set; }

        /// <summary>
        /// Gets or sets the Betting discount (Member currency) (applied to Betting total)
        /// </summary>
        public decimal BettingDiscountInMemberCurrency { get; set; } 
        
        /// <summary>
        /// Gets or sets the Member currency code
        /// </summary>
        public string MemberCurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets an Betting status identifer
        /// </summary>
        public int BettingStatusId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether storing of credit card number is allowed
        /// </summary>
        public bool AllowStoringCreditCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the card type
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        /// Gets or sets the card name
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// Gets or sets the card number
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the masked credit card number
        /// </summary>
        public string MaskedCreditCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the card CVV2
        /// </summary>
        public string CardCvv2 { get; set; }

        /// <summary>
        /// Gets or sets the card expiration month
        /// </summary>
        public string CardExpirationMonth { get; set; }

        /// <summary>
        /// Gets or sets the card expiration year
        /// </summary>
        public string CardExpirationYear { get; set; }

        /// <summary>
        /// Gets or sets the payment method identifier
        /// </summary>
        public int PaymentMethodId { get; set; }

        /// <summary>
        /// Gets or sets the payment method name
        /// </summary>
        public string PaymentMethodName { get; set; }

        /// <summary>
        /// Gets or sets the authorization transaction identifier
        /// </summary>
        public string AuthorizationTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the authorization transaction code
        /// </summary>
        public string AuthorizationTransactionCode { get; set; }

        /// <summary>
        /// Gets or sets the authorization transaction result
        /// </summary>
        public string AuthorizationTransactionResult { get; set; }

        /// <summary>
        /// Gets or sets the capture transaction identifier
        /// </summary>
        public string CaptureTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the capture transaction result
        /// </summary>
        public string CaptureTransactionResult { get; set; }

        /// <summary>
        /// Gets or sets the subscription transaction identifier
        /// </summary>
        public string SubscriptionTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the payment status identifier
        /// </summary>
        public int PaymentStatusId { get; set; }

        /// <summary>
        /// Gets or sets the paid date and time
        /// </summary>
        public DateTime? PaidDate { get; set; }   
       
        /// <summary>
        /// Gets or sets the tracking number of current Betting
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }   
     
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
    }
}
