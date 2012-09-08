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
        void MarkCustomerAsDeleted(long customerId);

        /// <summary>
        /// Gets a customer by email
        /// </summary>
        /// <param name="email">Customer Email</param>
        /// <returns>A customer</returns>
        Member GetCustomerByEmail(string email);

        /// <summary>
        /// Gets a customer by email
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
        /// Login a customer
        /// </summary>
        /// <param name="email">A customer email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        bool Login(string email, string password);

        /// <summary>
        /// Logout customer
        /// </summary>
        void Logout();

        #endregion     
    }
}
