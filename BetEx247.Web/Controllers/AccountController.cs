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
using BetEx247.Core.Payment;
using System.Text;
using CaptchaMvc.Attributes;
using CaptchaMvc.Infrastructure;

namespace BetEx247.Web.Controllers
{
    public class AccountController : BaseController
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
            string result = string.Empty;
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
                result= "success|" + memberInfo.FirstName + " " + memberInfo.LastName;
                MyWallet wallet = IoC.Resolve<ICustomerService>().GetAccountWallet(member.MemberID);
                if (wallet != null)
                {
                    result += wallet.Available != null ? "|" + wallet.Available.ToString() : "|0";
                    result += wallet.Exposure != null ? "|" + wallet.Exposure.ToString() : "|0";
                    result += wallet.Balance != null ? "|" + wallet.Balance.ToString() : "|0";
                }
                else
                {
                    result += "|0|0|0";
                }
                return result;
            }
            return ErrorCodeToString(MembershipCreateStatus.ProviderError);
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
        [CaptchaVerify("Captcha is not valid")]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["Message"] = "Message: captcha is valid.";
                MembershipCreateStatus createStatus = new MembershipCreateStatus();
                var member = new Member();
                if (IoC.Resolve<ICustomerService>().checkExistEmail(model.Email))
                {
                    createStatus = MembershipCreateStatus.DuplicateEmail;
                }
                //else if (model.Email2 != null && IoC.Resolve<ICustomerService>().checkExistEmail(model.Email2))
                //{
                //    createStatus = MembershipCreateStatus.DuplicateEmail;
                //}
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
                    //member.Address = CommonHelper.EnsureNotNull(model.Address);
                    //member.City = CommonHelper.EnsureNotNull(model.City);
                    //member.PostalCode = CommonHelper.EnsureNotNull(model.PostalCode);
                    //member.Telephone = CommonHelper.EnsureNotNull(model.Telephone);
                    //member.Cellphone = CommonHelper.EnsureNotNull(model.Cellphone);
                    member.Country = Request.Form["Country"].ToInt64();
                    member.Email1 = CommonHelper.EnsureNotNull(model.Email);
                    //member.Email2 = CommonHelper.EnsureNotNull(model.Email2);
                    member.Gender = Request.Form["Gender"] == "M" ? true : false;
                    //member.BettingRegion = CommonHelper.EnsureNotNull(model.BettingRegion);
                    //member.Timezone = CommonHelper.EnsureNotNull(model.Timezone);
                    member.AddedDate = DateTime.UtcNow;
                    member.Updatedate = DateTime.UtcNow;
                    member.Status = Constant.Status.INACTIVE;
                    member.IsActive = false;

                    IoC.Resolve<ICustomerService>().Insert(member);
                    createStatus = MembershipCreateStatus.Success;
                }

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(member.NickName, false /* createPersistentCookie */);
                    return RedirectToAction("register-done", "account");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            else
            {
                TempData["ErrorMessage"] = "Error: captcha is not valid.";
                ViewBag.ListCountry = IoC.Resolve<ICommonService>().getAllCountry();
                ViewBag.Gender = IoC.Resolve<ICommonService>().MakeSelectListGender();
                ViewBag.CurrencyList = IoC.Resolve<ICommonService>().MakeSelectListCurrency();                 
                return View(model);
            }
        }

        [ActionName("register-done")]
        public ActionResult RegisterComplete()
        {
            long userId = SessionManager.USER_ID;
            var member = IoC.Resolve<ICustomerService>().GetCustomerById(userId);
            return View(member);
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

        [Authorize]
        public ActionResult Balances()
        {
            long memberId = SessionManager.USER_ID;
            var mywallet = IoC.Resolve<ICustomerService>().GetAccountWallet(memberId);
            return View(mywallet);
        }

        [Authorize]
        public ActionResult Account(int? id)
        {
            long memberId = SessionManager.USER_ID;
            ViewBag.Type = id;
            var mywallet = IoC.Resolve<ICustomerService>().GetAccountWallet(memberId);
            return View(mywallet);
        }

        [Authorize]
        public ActionResult Statement()
        {               
            long memberId = SessionManager.USER_ID;
            var lstSportData = IoC.Resolve<IGuiService>().GetSportData();
            var lstStatement = IoC.Resolve<IBettingService>().GetStatementByMemberId(memberId,"",1,Constant.DefaultRow);
            ViewBag.lstSportData =new SelectList(lstSportData,"ID","SportName");
            ViewBag.lstDateSearchType = IoC.Resolve<ICommonService>().MakeSelectListDateSearch();
            ViewBag.lstDisplaySearch = IoC.Resolve<ICommonService>().MakeSelectListBetDisplay();
            ViewBag.lstStatement = lstStatement;
            return View();
        }

        [Authorize]
        public ActionResult Exposure()
        {
            long memberId = SessionManager.USER_ID;
            var lstExposure = IoC.Resolve<IBettingService>().GetMyBetByType(memberId, (short)Constant.MyBetStatus.EXPOSURE);
            return View(lstExposure);
        }

        [Authorize]
        public ActionResult UnmatchedBets()
        {
            long memberId = SessionManager.USER_ID;
            var lstUnmatchedBet = IoC.Resolve<IBettingService>().GetMyBetByType(memberId, (short)Constant.MyBetStatus.UNMATCHEDBETS);
            return View(lstUnmatchedBet);
        }

        [Authorize]
        public ActionResult UnsettledBets()
        {
            long memberId = SessionManager.USER_ID;
            var lstUnSettledBet = IoC.Resolve<IBettingService>().GetMyBetByType(memberId, (short)Constant.MyBetStatus.UNSETTLEDBETS);
            return View(lstUnSettledBet);
        }

        [Authorize]
        public ActionResult BettingPL()
        {
            long memberId = SessionManager.USER_ID;
            var lstBettingPL = IoC.Resolve<IBettingService>().GetMyBetByType(memberId, (short)Constant.MyBetStatus.BETTINGPL);

            var lstSportData = IoC.Resolve<IGuiService>().GetSportData();
            ViewBag.lstSportData = new SelectList(lstSportData, "ID", "SportName");
            ViewBag.lstDateSearchType = IoC.Resolve<ICommonService>().MakeSelectListDateSearch(); 

            return View(lstBettingPL);
        }

        [Authorize]
        public ActionResult SettledBets()
        {
            long memberId = SessionManager.USER_ID;
            var lstSettledBet = IoC.Resolve<IBettingService>().GetMyBetByType(memberId, (short)Constant.MyBetStatus.SETTLEDBETS);

            var lstSportData = IoC.Resolve<IGuiService>().GetSportData();
            ViewBag.lstSportData = new SelectList(lstSportData, "ID", "SportName");
            ViewBag.lstDateSearchType = IoC.Resolve<ICommonService>().MakeSelectListDateSearch();   

            return View(lstSettledBet);
        }

        [Authorize]
        public ActionResult CancelledBets()
        {
            long memberId = SessionManager.USER_ID;
            var lstCancelBet = IoC.Resolve<IBettingService>().GetMyBetByType(memberId, (short)Constant.MyBetStatus.CANCELLEDBETS);

            var lstSportData = IoC.Resolve<IGuiService>().GetSportData();
            ViewBag.lstSportData = new SelectList(lstSportData, "ID", "SportName");
            ViewBag.lstDateSearchType = IoC.Resolve<ICommonService>().MakeSelectListDateSearch();
            int totalRow = IoC.Resolve<IBettingService>().CountRowBetByMemberId(memberId, "");            

            return View(lstCancelBet);
        }

        [Authorize]
        public ActionResult LapsedBets()
        {
            long memberId = SessionManager.USER_ID;
            var lstLapsedBet = IoC.Resolve<IBettingService>().GetMyBetByType(memberId, (short)Constant.MyBetStatus.LAPSEDBETS);

            var lstSportData = IoC.Resolve<IGuiService>().GetSportData();
            ViewBag.lstSportData = new SelectList(lstSportData, "ID", "SportName");
            ViewBag.lstDateSearchType = IoC.Resolve<ICommonService>().MakeSelectListDateSearch(); 

            return View(lstLapsedBet);
        }

        [Authorize]
        public ActionResult VoidBets()
        {
            long memberId = SessionManager.USER_ID;
            var lstVoidBet = IoC.Resolve<IBettingService>().GetMyBetByType(memberId, (short)Constant.MyBetStatus.VOIDBETS);

            var lstSportData = IoC.Resolve<IGuiService>().GetSportData();
            ViewBag.lstSportData = new SelectList(lstSportData, "ID", "SportName");
            ViewBag.lstDateSearchType = IoC.Resolve<ICommonService>().MakeSelectListDateSearch(); 
            
            return View(lstVoidBet);
        }

        [Authorize]
        public ActionResult UpdateCards()
        {
            return View();
        }

        [Authorize]
        public ActionResult UpdateCreditCard(long id)
        {
            var paymentMenthod = IoC.Resolve<IPaymentService>().GetPaymentMethodById(id);
            ViewBag.CartType = paymentMenthod.CardType;
            ViewBag.MonthFrom = new SelectList(IoC.Resolve<ICommonService>().MakeSelectListMonth(), "Value", "Text", paymentMenthod.ValidFrom.Month);
            ViewBag.MonthTo = new SelectList(IoC.Resolve<ICommonService>().MakeSelectListMonth(), "Value", "Text", paymentMenthod.ValidTo.Month);
            ViewBag.YearFrom = new SelectList(IoC.Resolve<ICommonService>().MakeSelectListYearCard(), "Value", "Text", paymentMenthod.ValidFrom.Year);
            ViewBag.YearTo = new SelectList(IoC.Resolve<ICommonService>().MakeSelectListYearCard(), "Value", "Text", paymentMenthod.ValidTo.Year);
            ViewBag.ListCountry = new SelectList(IoC.Resolve<ICommonService>().getAllCountry(), "Value", "Text", paymentMenthod.Country);
            return View(paymentMenthod);
        }

        [Authorize, HttpPost]
        public ActionResult UpdateCreditCard(long id, FormCollection collection)
        {
            string message = string.Empty;
            var paymentMethod = IoC.Resolve<IPaymentService>().GetPaymentMethodById(id);
            paymentMethod.Name = collection["NameOnCard"];
            paymentMethod.Description = "Payment menthod: " + id;
            paymentMethod.CreditCardNumber = collection["CreditCardNumber"];
            paymentMethod.NameOnCard = collection["NameOnCard"];
            paymentMethod.CardCvv2 = collection["CardCvv2"];
            paymentMethod.MaskedCreditCardNumber = collection["CardCvv2"];
            string monthFrom = collection["MonthFrom"];
            string monthTo = collection["MonthTo"];
            string yearFrom = collection["YearFrom"];
            string yearTo = collection["YearTo"];
            DateTime dateFrom = new DateTime(yearFrom.ToInt32(), monthFrom.ToInt32(), 1);
            DateTime dateTo = new DateTime(yearTo.ToInt32(), monthTo.ToInt32(), 28);
            paymentMethod.ValidFrom = dateFrom;
            paymentMethod.ValidTo = dateTo;
            paymentMethod.CardExpirationMonth = monthTo;
            paymentMethod.CardExpirationYear = yearTo;
            paymentMethod.Address = collection["Address"];
            paymentMethod.Country = collection["Country"];
            paymentMethod.Zipcode = collection["Zipcode"];
            paymentMethod.Bank = collection["Bank"];

            try
            {
                IoC.Resolve<IPaymentService>().UpdatePaymentMethod(paymentMethod);
                message = "Update successfully!";
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
            }
            ViewBag.Message = message;
            //update datemonth
            ViewBag.MonthFrom = new SelectList(IoC.Resolve<ICommonService>().MakeSelectListMonth(), "Value", "Text", paymentMethod.ValidFrom.Month);
            ViewBag.MonthTo = new SelectList(IoC.Resolve<ICommonService>().MakeSelectListMonth(), "Value", "Text", paymentMethod.ValidTo.Month);
            ViewBag.YearFrom = new SelectList(IoC.Resolve<ICommonService>().MakeSelectListYearCard(), "Value", "Text", paymentMethod.ValidFrom.Year);
            ViewBag.YearTo = new SelectList(IoC.Resolve<ICommonService>().MakeSelectListYearCard(), "Value", "Text", paymentMethod.ValidTo.Year);
            ViewBag.ListCountry = new SelectList(IoC.Resolve<ICommonService>().getAllCountry(), "Value", "Text", paymentMethod.Country);
            return View();
        }

        [Authorize]
        public ActionResult AddNewCard(int id)
        {
            ViewBag.CartType = id.ToString();
            ViewBag.ListMonth = IoC.Resolve<ICommonService>().MakeSelectListMonth();
            ViewBag.ListYear = IoC.Resolve<ICommonService>().MakeSelectListYearCard();
            ViewBag.ListCountry = IoC.Resolve<ICommonService>().getAllCountry();
            return View();
        }

        [Authorize, HttpPost]
        public ActionResult AddNewCard(int id, FormCollection collection)
        {
            string message = string.Empty;
            string cardType = string.Empty;
            switch (id)
            {
                case 1:
                    cardType = Constant.CardType.VISACREDIT;
                    break;
                default:
                    cardType = Constant.CardType.VISACREDIT;
                    break;
            }
            PaymentMethod paymentMethod = new PaymentMethod();
            paymentMethod.CardType = cardType;
            paymentMethod.Name = collection["NameOnCard"];
            paymentMethod.Description = "Payment menthod: " + id;
            paymentMethod.CreditCardNumber = collection["CreditCardNumber"];
            paymentMethod.NameOnCard = collection["NameOnCard"];
            paymentMethod.CardCvv2 = collection["CardCvv2"];
            paymentMethod.MaskedCreditCardNumber = collection["CardCvv2"];
            string monthFrom = collection["MonthFrom"];
            string monthTo = collection["MonthTo"];
            string yearFrom = collection["YearFrom"];
            string yearTo = collection["YearTo"];
            DateTime dateFrom = new DateTime(yearFrom.ToInt32(), monthFrom.ToInt32(), 1);
            DateTime dateTo = new DateTime(yearTo.ToInt32(), monthTo.ToInt32(), 28);
            paymentMethod.ValidFrom = dateFrom;
            paymentMethod.ValidTo = dateTo;
            paymentMethod.CardExpirationMonth = monthTo;
            paymentMethod.CardExpirationYear = yearTo;
            paymentMethod.Address = collection["Address"];
            paymentMethod.Country = collection["Country"];
            paymentMethod.Zipcode = collection["Zipcode"];
            paymentMethod.Email = SessionManager.USER_EMAIL;
            paymentMethod.Bank = collection["Bank"];
            paymentMethod.ClassName = Constant.PaymentClass.APCOService;
            paymentMethod.Verified = false;
            paymentMethod.AddedDate = DateTime.Now;
            paymentMethod.ModifyDate = DateTime.Now;

            try
            {
                IoC.Resolve<IPaymentService>().InsertPaymentMethod(paymentMethod);
                message = "Insert card successfully!";
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
            }
            //update load data
            ViewBag.CartType = id.ToString();
            ViewBag.ListMonth = IoC.Resolve<ICommonService>().MakeSelectListMonth();
            ViewBag.ListYear = IoC.Resolve<ICommonService>().MakeSelectListYearCard();
            ViewBag.ListCountry = IoC.Resolve<ICommonService>().getAllCountry();
            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        public ActionResult Deposit()
        {
            return View();
        }

        [Authorize, HttpPost]
        public ActionResult Deposit(FormCollection collection)
        {
            string message = string.Empty;
            long transactionPaymentId = 0;
            long memberId = SessionManager.USER_ID;

            TransactionPayment transactionPayment = new TransactionPayment();
            transactionPayment.TransactionPaymentType = (int)Constant.TransactionType.DEPOSIT;
            transactionPayment.MemberId = memberId;
            transactionPayment.MemberIP = Request.UserHostAddress;
            transactionPayment.MemberEmail = transactionPayment.Customer.Email1;
            transactionPayment.TransactionPaymentTotal = collection["Amount"].ToDecimal();
            transactionPayment.TransactionPaymentStatusId = (int)PaymentStatusEnum.Authorized;
            transactionPayment.PaymentMethodId = 1;

            message = IoC.Resolve<ITransactionPaymentService>().PlaceTransactionPayment(transactionPayment, out transactionPaymentId);

            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        public ActionResult Withdraw()
        {
            return View();
        }

        [Authorize, HttpPost]
        public ActionResult Withdraw(FormCollection collection)
        {
            string message = string.Empty;
            long memberId = SessionManager.USER_ID;
            long transactionPaymentId = 0;
            if (IoC.Resolve<ITransactionPaymentService>().GetTransactionByUserId(memberId) != null)
            {
                message = Constant.Messagage.NOTRANSACTION;
            }
            else
            {
                TransactionPayment transactionPayment = new TransactionPayment();
                transactionPayment = IoC.Resolve<ITransactionPaymentService>().GetTransactionPaymentByUserId(memberId);
                transactionPayment.TransactionPaymentType = (int)Constant.TransactionType.WITHDRAW;
                transactionPayment.MemberIP = Request.UserHostAddress;
                transactionPayment.RecurringTotalCycles = 1;
                transactionPayment.RecurringCycleLength = 7;

                message = IoC.Resolve<ITransactionPaymentService>().PlaceTransactionPayment(transactionPayment, out transactionPaymentId);
            }
            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        public ActionResult MyProfile()
        {
            var memberId = SessionManager.USER_ID;
            var memberProfile = IoC.Resolve<ICustomerService>().GetCustomerById(memberId);
            return View(memberProfile);
        }

        [Authorize, HttpPost]
        public ActionResult MyProfile(Member model)
        {
            IoC.Resolve<ICustomerService>().Update(model);
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

        public ActionResult ChangeContainerProvider()
        {
            if (CaptchaUtils.CaptchaManager.StorageProvider is CookieStorageProvider)
                CaptchaUtils.CaptchaManager.StorageProvider = new SessionStorageProvider();
            else
                CaptchaUtils.CaptchaManager.StorageProvider = new CookieStorageProvider();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
