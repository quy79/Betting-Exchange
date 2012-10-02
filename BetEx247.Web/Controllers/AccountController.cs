using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BetEx247.Data.Model;
using BetEx247.Web.Models;
using BetEx247.Core.Infrastructure;
using BetEx247.Data.DAL;
using BetEx247.Core.Common.Extensions;
using BetEx247.Core.Common.Utils;
using BetEx247.Core;
using BetEx247.Data;

namespace BetEx247.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Login & Logout
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (IoC.Resolve<ICustomerService>().Authenticate(model.UserName, FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password.Trim(), "sha1")))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                   
                    //insert history login
                    LoginHistory history = new LoginHistory();
                    history.MemberID = SessionManager.USER_ID;
                    history.LoginTime = DateTime.Now;
                    history.Status = Constant.Status.ACTIVENUM;
                    history.IP = Request.UserHostAddress;
                    IoC.Resolve<ICustomerService>().InsertHistory(history);


                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
                  
        public string LoginAjax(string userName, string userPass)
        {
            if (IoC.Resolve<ICustomerService>().Authenticate(userName, FormsAuthentication.HashPasswordForStoringInConfigFile(userPass, "sha1")))
            {
                FormsAuthentication.SetAuthCookie(userName, false);
                var member = IoC.Resolve<ICustomerService>().GetCustomerByUsername(userName);
                //insert history login
                LoginHistory history = new LoginHistory();
                history.MemberID = member.MemberID;
                history.LoginTime = DateTime.Now;
                history.Status = Constant.Status.ACTIVENUM;
                history.IP = Request.UserHostAddress;
                IoC.Resolve<ICustomerService>().InsertHistory(history);        
               
                var memberInfo = IoC.Resolve<ICustomerService>().GetCustomerByUsername(userName);
                return "success|"+memberInfo.FirstName+" " +memberInfo.LastName;
            }
            return null;
        }
                       
        public string LogOutAjax()
        {
            try
            {
                //update history
                //insert history login
                try
                {
                    LoginHistory history = new LoginHistory();
                    history.ID = SessionManager.HISTORY_ID;
                    history.LogoutTime = DateTime.Now;
                    IoC.Resolve<ICustomerService>().UpdateHistory(history);
                }
                catch { }
                SessionManager.Logout();
                return "success";
            }
            catch
            {
                return null;
            }
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Register
        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            ViewBag.ListCountry = IoC.Resolve<ICommonService>().getAllCountry();
            ViewBag.Gender = IoC.Resolve<ICommonService>().MakeSelectListGender();
            ViewBag.CurrencyList = IoC.Resolve<ICommonService>().MakeSelectListCurrency();
            RegisterModel model = new RegisterModel();
            return View(model);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            MembershipCreateStatus createStatus = new MembershipCreateStatus();
            var member = new Member();
            if (IoC.Resolve<ICustomerService>().checkExistEmail(model.Email))
            {
                createStatus = MembershipCreateStatus.DuplicateEmail;
            }
            else if (model.Email2 != null && IoC.Resolve<ICustomerService>().checkExistEmail(model.Email2))
            {
                createStatus = MembershipCreateStatus.DuplicateEmail;
            }
            else if (IoC.Resolve<ICustomerService>().checkExistNickName(model.NickName))
            {
                createStatus = MembershipCreateStatus.DuplicateUserName;
            }
            else
            {
                // Attempt to register the user
                member.NickName = CommonHelper.EnsureNotNull(model.NickName);
                member.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password.Trim(), "sha1");
                member.SecurityQuestion1 = CommonHelper.EnsureNotNull(model.SecurityQuestion1);
                member.SecurityAnswer1 = CommonHelper.EnsureNotNull(model.SecurityAnswer1);
                member.SecurityQuestion2 = CommonHelper.EnsureNotNull(model.SecurityQuestion2);
                member.SecurityAnswer2 = CommonHelper.EnsureNotNull(model.SecurityAnswer2);
                member.Currency = short.Parse(Request.Form["Currency"].ToString());
                member.FirstName = CommonHelper.EnsureNotNull(model.FirstName);
                member.MiddleName = CommonHelper.EnsureNotNull(model.MiddleName);
                member.LastName = CommonHelper.EnsureNotNull(model.LastName);
                member.Address = CommonHelper.EnsureNotNull(model.Address);
                member.City = CommonHelper.EnsureNotNull(model.City);
                member.PostalCode = CommonHelper.EnsureNotNull(model.PostalCode);
                member.Telephone = CommonHelper.EnsureNotNull(model.Telephone);
                member.Cellphone = CommonHelper.EnsureNotNull(model.Cellphone);
                member.Country = Request.Form["Country"].ToInt64();
                member.Email1 = CommonHelper.EnsureNotNull(model.Email);
                member.Email2 = CommonHelper.EnsureNotNull(model.Email2);
                member.Gender = Request.Form["Gender"] == "M" ? true : false;
                member.BettingRegion = CommonHelper.EnsureNotNull(model.BettingRegion);
                member.Timezone = CommonHelper.EnsureNotNull(model.Timezone);
                member.AddedDate = DateTime.UtcNow;
                member.Updatedate = DateTime.UtcNow;
                member.Status = Constant.Status.INACTIVE;
                member.IsActive = false;

                IoC.Resolve<ICustomerService>().Insert(member);
                createStatus = MembershipCreateStatus.Success;
            }

            if (createStatus == MembershipCreateStatus.Success)
            {
                //FormsAuthentication.SetAuthCookie(member.NickName, false /* createPersistentCookie */);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", ErrorCodeToString(createStatus));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region Change & Update Pass
        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }
        #endregion

        #region Account Info
        public ActionResult Balances()
        {
            return View();
        }
        #endregion

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
