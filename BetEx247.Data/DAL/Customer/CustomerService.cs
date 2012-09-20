using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Core;
using BetEx247.Core.Common.Utils;
using System.Web.Security;

namespace BetEx247.Data.DAL
{
    public partial class CustomerService : ICustomerService
    {
        /// <summary>
        /// Sets a customer email
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="newEmail">New email</param>
        /// <returns>Customer</returns>
        public Member SetEmail(long customerId, string newEmail)
        {
            using (var dba = new BetEXDataContainer())
            {
                var member = dba.Members.Where(w => w.MemberID == customerId).SingleOrDefault();
                if (member != null)
                {
                    member.Email1 = newEmail;
                    dba.SaveChanges();
                    return member;
                }
                return null;
            }
        }

        /// <summary>
        /// Marks customer as deleted
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        public bool MarkCustomerAsDeleted(long customerId)
        {
            using (var dba = new BetEXDataContainer())
            {
                var member = dba.Members.Where(w => w.MemberID == customerId).SingleOrDefault();
                if (member != null)
                {
                    member.Status = Constant.Status.DELETED;
                    dba.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Marks customer as active the first time throught active email
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        public bool UpdateFirstActive(long customerId)
        {
            using (var dba = new BetEXDataContainer())
            {
                var member = dba.Members.Where(w => w.MemberID == customerId).SingleOrDefault();
                if (member != null)
                {
                    member.Status = Constant.Status.ACTIVE;
                    member.IsActive = true;
                    dba.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets a customer by email
        /// </summary>
        /// <param name="email">Customer Email</param>
        /// <returns>A customer</returns>
        public Member GetCustomerByEmail(string email)
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.Members.Where(w => w.Email1 == email || w.Email2 == email).SingleOrDefault();
            }
        }

        /// <summary>
        /// Gets a customer by username
        /// </summary>
        /// <param name="username">Customer username</param>
        /// <returns>A customer</returns>
        public Member GetCustomerByUsername(string username)
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.Members.Where(w => w.NickName == username).SingleOrDefault();
            }
        }

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>A customer</returns>
        public Member GetCustomerById(long customerId)
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.Members.Where(w => w.MemberID == customerId).SingleOrDefault();
            }
        }

        /// <summary>
        /// Authenticate a customer
        /// </summary>
        /// <param name="email">A customer email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        public bool Authenticate(string nickname, string password)
        {
            using (var dba = new BetEXDataContainer())
            {
                var profile = dba.Members.Where(w => w.NickName == nickname && w.Password == password && w.Status == Constant.Status.ACTIVE && w.IsActive == true).SingleOrDefault();
                if (profile != null)
                {
                    FormsAuthentication.SetAuthCookie(nickname, false);  
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Logout customer
        /// </summary>
        public void Logout()
        {
            SessionManager.Logout();
        }

        public IList<Member> GetAll()
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.Members.Where(w => w.Status == Constant.Status.ACTIVE && w.IsActive == true).ToList();
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

        public bool Update(Member entity)
        {
            using (var dba = new BetEXDataContainer())
            {
                var member = dba.Members.Where(w => w.MemberID == entity.MemberID).SingleOrDefault();
                if (member != null)
                {
                    member.NickName = CommonHelper.EnsureNotNull(entity.NickName);
                    member.FirstName = CommonHelper.EnsureNotNull(entity.FirstName);
                    member.LastName = CommonHelper.EnsureNotNull(entity.LastName);
                    member.MiddleName = CommonHelper.EnsureNotNull(entity.MiddleName);
                    member.SecurityQuestion1 = CommonHelper.EnsureNotNull(entity.SecurityQuestion1) ?? member.SecurityQuestion1;
                    member.SecurityQuestion2 = CommonHelper.EnsureNotNull(entity.SecurityQuestion2) ?? member.SecurityQuestion2;
                    member.SecurityAnswer1 = CommonHelper.EnsureNotNull(entity.SecurityAnswer1) ?? member.SecurityAnswer1;
                    member.SecurityAnswer2 = CommonHelper.EnsureNotNull(entity.SecurityAnswer2) ?? member.SecurityAnswer2;
                    member.Address = CommonHelper.EnsureNotNull(entity.Address) ?? member.Address;
                    member.City = CommonHelper.EnsureNotNull(entity.City) ?? member.City;
                    member.PostalCode = CommonHelper.EnsureNotNull(entity.PostalCode) ?? member.PostalCode;
                    member.Telephone = CommonHelper.EnsureNotNull(entity.Telephone) ?? member.Telephone;
                    member.Cellphone = CommonHelper.EnsureNotNull(entity.Cellphone) ?? member.Cellphone;
                    member.Email1 = CommonHelper.EnsureNotNull(entity.Email1) ?? member.Email1;
                    member.Email2 = CommonHelper.EnsureNotNull(entity.Email2) ?? member.Email2;
                    member.Country = entity.Country;
                    member.Gender = entity.Gender;
                    member.Suffix = CommonHelper.EnsureNotNull(entity.Suffix) ?? member.Suffix;
                    member.TotalPoints = entity.TotalPoints ?? member.TotalPoints;
                    member.DiscountRate = entity.DiscountRate ?? member.DiscountRate;
                    member.LastUpdate = entity.LastUpdate ?? member.LastUpdate;
                    member.Language = CommonHelper.EnsureNotNull(entity.Language) ?? member.Language;
                    member.BettingRegion = CommonHelper.EnsureNotNull(entity.BettingRegion) ?? member.BettingRegion;
                    member.Timezone = CommonHelper.EnsureNotNull(entity.Timezone) ?? member.Timezone;
                    member.Currency = entity.Currency ?? member.Currency;
                    member.AutoLogout = entity.AutoLogout ?? member.AutoLogout;
                    member.EmailFormat = CommonHelper.EnsureNotNull(entity.EmailFormat) ?? member.EmailFormat;                 

                    dba.SaveChanges();
                }
                return true;
            }
        }

        public bool Delete(Member entity)
        {
            using (var dba = new BetEXDataContainer())
            {
                var member = dba.Members.Where(w => w.MemberID == entity.MemberID).SingleOrDefault();
                if (member != null)
                {
                    dba.DeleteObject(member);
                    return true;
                }
                return false;
            }
        }

        public IQueryable<Member> Table
        {
            get { throw new NotImplementedException(); }
        }
    }
}
