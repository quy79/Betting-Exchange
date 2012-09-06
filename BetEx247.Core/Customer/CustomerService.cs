using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetEx247.Core.CustomerManagement
{
    /// <summary>
    /// Customer service
    /// </summary>
    public partial class CustomerService : ICustomerService
    {
        public void DeleteAddress(int addressId)
        {
            throw new NotImplementedException();
        }

        public Address GetAddressById(int addressId)
        {
            throw new NotImplementedException();
        }

        public List<Address> GetAddressesByCustomerId(int customerId, bool getBillingAddresses)
        {
            throw new NotImplementedException();
        }

        public void InsertAddress(Address address)
        {
            throw new NotImplementedException();
        }

        public void UpdateAddress(Address address)
        {
            throw new NotImplementedException();
        }

        public bool CanUseAddressAsBillingAddress(Address address)
        {
            throw new NotImplementedException();
        }

        public bool CanUseAddressAsShippingAddress(Address address)
        {
            throw new NotImplementedException();
        }

        public Customer SetEmail(int customerId, string newEmail)
        {
            throw new NotImplementedException();
        }

        public void MarkCustomerAsDeleted(int customerId)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomerByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomerByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomerById(int customerId)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomerByGuid(Guid customerGuid)
        {
            throw new NotImplementedException();
        }

        public bool Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
