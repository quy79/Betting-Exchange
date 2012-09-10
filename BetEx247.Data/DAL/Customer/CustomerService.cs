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
            using (var dba = new BetEXDataContainer())
            {
                return dba.Members.Where(w => w.Email1 == email || w.Email2 == email).SingleOrDefault();
            }
        }

        public Member GetCustomerByUsername(string username)
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.Members.Where(w => w.NickName == username).SingleOrDefault();
            }
        }

        public Member GetCustomerById(long customerId)
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.Members.Where(w => w.MemberID == customerId).SingleOrDefault();
            }
        }

        public bool Login(string nickname, string password)
        {
            using (var dba = new BetEXDataContainer())
            {
                var profile = dba.Members.Where(w => w.NickName == nickname && w.Password==password).SingleOrDefault();
                if (profile != null)
                    return true;
            }
            return false;
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
            using (var dba = new BetEXDataContainer())
            {
                dba.AddToMembers(entity);
                dba.SaveChanges();
            }
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
