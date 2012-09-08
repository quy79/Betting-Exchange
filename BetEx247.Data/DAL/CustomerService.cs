using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;

namespace BetEx247.Data.DAL
{
    public partial class CustomerService :ICustomerService
    {
        public Member SetEmail(long customerId, string newEmail)
        {
            throw new NotImplementedException();
        }

        public void MarkCustomerAsDeleted(long customerId)
        {
            throw new NotImplementedException();
        }

        public Member GetCustomerByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Member GetCustomerByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Member GetCustomerById(long customerId)
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.Members.Where(w => w.MemberID == customerId).SingleOrDefault();
            }
        }

        public bool Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }         

        public IList<Member> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.Members.ToList();
            }
        }

        public void Insert(Member entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Member entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Member entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Member> Table
        {
            get { throw new NotImplementedException(); }
        }
    }
}
