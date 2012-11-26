using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;

namespace BetEx247.Data.DAL
{
    public partial interface ICustomerService : IBase<Member>
    {      
        #region Customers
        /// <summary>
        /// Sets a customer email
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="newEmail">New email</param>
        /// <returns>Customer</returns>
        Member SetEmail(long customerId, string newEmail);

        /// <summary>
        /// Marks customer as deleted
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        bool MarkCustomerAsDeleted(long customerId);

        /// <summary>
        /// Marks customer as active the first time throught active email
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        bool UpdateFirstActive(long customerId);

        /// <summary>
        /// Gets a customer by email
        /// </summary>
        /// <param name="email">Customer Email</param>
        /// <returns>A customer</returns>
        Member GetCustomerByEmail(string email);

        /// <summary>
        /// Gets a customer by username
        /// </summary>
        /// <param name="username">Customer username</param>
        /// <returns>A customer</returns>
        Member GetCustomerByUsername(string username);

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>A customer</returns>
        Member GetCustomerById(long customerId); 
       
        /// <summary>
        /// Authenticate a customer
        /// </summary>
        /// <param name="email">A customer email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        bool Authenticate(string email, string password);

        /// <summary>
        /// Logout customer
        /// </summary>
        void Logout();
        
        void ForgetPassword(string email);
        #endregion 
    
        #region Check Data
        /// <summary>
        /// Check email is exist in database
        /// </summary>
        /// <param name="email">email to check</param>
        /// <returns>true, false</returns>
        bool checkExistEmail(string email);

        /// <summary>
        /// Check nickname is exist in database
        /// </summary>
        /// <param name="email">nickname to check</param>
        /// <returns>true, false</returns>
        bool checkExistNickName(string nickName);
        #endregion

        #region History
        long InsertHistory(LoginHistory history);

        bool UpdateHistory(LoginHistory history);

        LoginHistory LastLogin(long memberID);
        #endregion 

        #region Account Info
        MyWallet GetAccountWallet(long memberId);

        /// <summary>
        /// Insert new user's wallet
        /// </summary>
        /// <param name="wallet">wallet info</param>
        /// <returns>wallet id</returns>
        long InsertWallet(MyWallet wallet);

        /// <summary>
        /// Update user's wallet
        /// </summary>
        /// <param name="wallet">wallet info</param>
        /// <returns>true:update succefully; false: otherwise</returns>
        bool UpdateWallet(MyWallet wallet);

        /// <summary>
        /// Amount of Net Winnings in all  markets which Bettor has betted 
        /// </summary>
        /// <param name="memberId">user id</param>
        /// <returns>money of net winnings</returns>
        decimal WinningHeld(long memberId);

        /// <summary>
        /// Amount of Commissions has paid for BetEx247
        /// </summary>
        /// <param name="memberId">user id</param>
        /// <returns>Amount of Commissions has paid for BetEx247</returns>
        decimal CommissionsHeld(long memberId);

        /// <summary>
        /// Amount of money in Funds
        /// </summary>
        /// <param name="memberId">user id</param>
        /// <returns>Amount of money in Funds</returns>
        decimal FundsHeld(long memberId);

        /// <summary>
        /// The discount (%) applied to the Maximum Market Rate for each Bettor depending on the number of BetEx247 Points they have when the market is settled.
        /// </summary>
        /// <param name="memberId">user id</param>
        /// <returns>The discount (%) applied to the Maximum Market Rate</returns>
        decimal LoyaltyDiscountRate(long memberId);

        /// <summary>
        /// Points earned by each Bettor in respect of their settled activity on the Exchange
        /// </summary>
        /// <param name="memberId">user id</param>
        /// <returns>Points earned by each Bettor</returns>
        decimal TotalPoints(long memberId);

        /// <summary>
        /// The cumulative amount of BetEx247 Points earned by Bettor, irrespective of the BetEx247 Decay Rate.
        /// </summary>
        /// <param name="memberId">user id</param>
        /// <returns>The cumulative amount of BetEx247 Points</returns>
        decimal LifetimePoints(long memberId);
        #endregion
    }
}
