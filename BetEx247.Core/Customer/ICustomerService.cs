using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.CustomerManagement
{
    /// <summary>
    /// Customer service interface
    /// </summary>
    public partial interface ICustomerService
    {
        #region Addresses

        /// <summary>
        /// Deletes an address by address identifier 
        /// </summary>
        /// <param name="addressId">Address identifier</param>
        void DeleteAddress(int addressId);

        /// <summary>
        /// Gets an address by address identifier
        /// </summary>
        /// <param name="addressId">Address identifier</param>
        /// <returns>Address</returns>
        Address GetAddressById(int addressId);

        /// <summary>
        /// Gets a collection of addresses by customer identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="getBillingAddresses">Gets or sets a value indicating whether the addresses are billing or shipping</param>
        /// <returns>A collection of addresses</returns>
        List<Address> GetAddressesByCustomerId(int customerId, bool getBillingAddresses);

        /// <summary>
        /// Inserts an address
        /// </summary>
        /// <param name="address">Address</param>
        void InsertAddress(Address address);

        /// <summary>
        /// Updates the address
        /// </summary>
        /// <param name="address">Address</param>
        void UpdateAddress(Address address);

        /// <summary>
        /// Gets a value indicating whether address can be used as billing address
        /// </summary>
        /// <param name="address">Address to validate</param>
        /// <returns>Result</returns>
        bool CanUseAddressAsBillingAddress(Address address);

        /// <summary>
        /// Gets a value indicating whether address can be used as shipping address
        /// </summary>
        /// <param name="address">Address to validate</param>
        /// <returns>Result</returns>
        bool CanUseAddressAsShippingAddress(Address address);

        #endregion

        #region Customers          
        /// <summary>
        /// Sets a customer email
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="newEmail">New email</param>
        /// <returns>Customer</returns>
        Customer SetEmail(int customerId, string newEmail);     
    
        /// <summary>
        /// Marks customer as deleted
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        void MarkCustomerAsDeleted(int customerId);

        /// <summary>
        /// Gets a customer by email
        /// </summary>
        /// <param name="email">Customer Email</param>
        /// <returns>A customer</returns>
        Customer GetCustomerByEmail(string email);

        /// <summary>
        /// Gets a customer by email
        /// </summary>
        /// <param name="username">Customer username</param>
        /// <returns>A customer</returns>
        Customer GetCustomerByUsername(string username);

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>A customer</returns>
        Customer GetCustomerById(int customerId);

        /// <summary>
        /// Gets a customer by GUID
        /// </summary>
        /// <param name="customerGuid">Customer GUID</param>
        /// <returns>A customer</returns>
        Customer GetCustomerByGuid(Guid customerGuid);          

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
