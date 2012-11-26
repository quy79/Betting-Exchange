using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Core;
using BetEx247.Core.Common.Utils;
using System.Web.Security;
using BetEx247.Core.Infrastructure;
using ChilkatEmail;

namespace BetEx247.Data.DAL
{
    public partial class CustomerService : ICustomerService
    {
        #region Customer
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
                return dba.Members.Where(w => w.Email1 == email || w.Email2 == email && w.Status==Constant.Status.ACTIVE).SingleOrDefault();
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
                var profile = dba.Members.Where(w => w.NickName == nickname && w.Password == password).SingleOrDefault();
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

        public void ForgetPassword(string email)
        {
            Member memProfile = IoC.Resolve<ICustomerService>().GetCustomerByEmail(email);
            if (memProfile != null)
            {
                string newPass= CommonHelper.RandomString(8);
                memProfile.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(newPass, "sha1");
                Update(memProfile);
                //send mail to user
                MailServices mailService = new MailServices();
                List<String> mailTo = new List<string>();
                mailTo.Add(memProfile.Email1);
                List<String> mailCC = new List<string>();
                mailCC.Add("chantinh2204@gmail.com");
                List<String> mailBBC = new List<string>();

                mailService.SendEmail(mailTo, mailCC, mailBBC, "Forget Password", "Your Pass new update pass: " + newPass + ".");
            }
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
        #endregion

        #region Check Data
        /// <summary>
        /// Check email is exist in database
        /// </summary>
        /// <param name="email">email to check</param>
        /// <returns>true, false</returns>
        public bool checkExistEmail(string email)
        {
            using (var dba = new BetEXDataContainer())
            {
                var member = dba.Members.Where(w => w.Email1 == email || w.Email2 == email).SingleOrDefault();
                if (member != null)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check nickname is exist in database
        /// </summary>
        /// <param name="email">nickname to check</param>
        /// <returns>true, false</returns>
        public bool checkExistNickName(string nickName)
        {
            using (var dba = new BetEXDataContainer())
            {
                var member = dba.Members.Where(w => w.NickName == nickName).SingleOrDefault();
                if (member != null)
                    return true;
            }
            return false;
        }
        #endregion

        #region History

        public long InsertHistory(LoginHistory history)
        {
            using (var dba = new BetEXDataContainer())
            {
                dba.AddToLoginHistories(history);
                dba.SaveChanges();
                return history.ID;
            }
        }

        public bool UpdateHistory(LoginHistory history)
        {
            using (var dba = new BetEXDataContainer())
            {
                try
                {
                    var Orghistory = dba.LoginHistories.Where(w => w.ID == history.ID).SingleOrDefault();
                    if (Orghistory != null)
                    {
                        Orghistory.LogoutTime = history.LogoutTime;
                        dba.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public LoginHistory LastLogin(long memberID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var lstHisttory = dba.LoginHistories.Where(w => w.MemberID == memberID).OrderByDescending(k => k.ID).ToList();
                if (lstHisttory != null)
                    return lstHisttory[0];
            }
            return null;
        }
        #endregion

        #region Account Info
        public MyWallet GetAccountWallet(long memberId)
        {
            using (var dba = new BetEXDataContainer())
            {
                return dba.MyWallets.Where(w => w.MemberID == memberId).SingleOrDefault();                
            }
        }

        /// <summary>
        /// Insert new user's wallet
        /// </summary>
        /// <param name="wallet">wallet info</param>
        /// <returns>wallet id</returns>
        public long InsertWallet(MyWallet wallet)
        {
            using (var dba = new BetEXDataContainer())
            {
                dba.AddToMyWallets(wallet);
                return wallet.ID;
            }
        }

        /// <summary>
        /// Update user's wallet
        /// </summary>
        /// <param name="wallet">wallet info</param>
        /// <returns>true:update succefully; false: otherwise</returns>
        public bool UpdateWallet(MyWallet wallet)
        {
            using (var dba = new BetEXDataContainer())
            {
                MyWallet origin= dba.MyWallets.Where(w => w.MemberID == wallet.MemberID).SingleOrDefault();

                if (origin != null)
                {
                    origin.Balance = wallet.Balance;
                    origin.Available = wallet.Available;
                    origin.UpdatedTime = wallet.UpdatedTime;
                    dba.SaveChanges();
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Amount of Net Winnings in all  markets which Bettor has betted 
        /// </summary>
        /// <param name="memberId">user id</param>
        /// <returns>money of net winnings</returns>
        public decimal WinningHeld(long memberId)
        {
            decimal result=0;
            List<PSV_MYBET> lstBet = IoC.Resolve<IBettingService>().GetMyBetByMemberId(memberId);
            if (lstBet != null)
            {
                foreach(PSV_MYBET item in lstBet)
                {
                    if (item.IsWon)
                    {
                        result += item.NetProfit != null ? item.NetProfit.Value : 0;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Amount of Commissions has paid for BetEx247
        /// </summary>
        /// <param name="memberId">user id</param>
        /// <returns>Amount of Commissions has paid for BetEx247</returns>
        public decimal CommissionsHeld(long memberId)
        {
            decimal result = 0;
            List<PSV_MYBET> lstBet = IoC.Resolve<IBettingService>().GetMyBetByMemberId(memberId);
            if (lstBet != null)
            {
                foreach (PSV_MYBET item in lstBet)
                {
                    if (item.IsWon)
                    {
                        result += item.Commission != null ? item.Commission.Value : 0;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Amount of money in Funds
        /// </summary>
        /// <param name="memberId">user id</param>
        /// <returns>Amount of money in Funds</returns>
        public decimal FundsHeld(long memberId)
        {
            return 0;
        }

        /// <summary>
        /// The discount (%) applied to the Maximum Market Rate for each Bettor depending on the number of BetEx247 Points they have when the market is settled.
        /// </summary>
        /// <param name="memberId">user id</param>
        /// <returns>The discount (%) applied to the Maximum Market Rate</returns>
        public decimal LoyaltyDiscountRate(long memberId)
        {
            return 0;
        }

        /// <summary>
        /// Points earned by each Bettor in respect of their settled activity on the Exchange
        /// </summary>
        /// <param name="memberId">user id</param>
        /// <returns>Points earned by each Bettor</returns>
        public decimal TotalPoints(long memberId)
        {
            return 0;
        }

        /// <summary>
        /// The cumulative amount of BetEx247 Points earned by Bettor, irrespective of the BetEx247 Decay Rate.
        /// </summary>
        /// <param name="memberId">user id</param>
        /// <returns>The cumulative amount of BetEx247 Points</returns>
        public decimal LifetimePoints(long memberId)
        {
            return 0;
        }
        #endregion
    }
}
