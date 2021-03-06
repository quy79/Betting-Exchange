﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using BetEx247.Data.Model;
using BetEx247.Core.Common.Utils;
using BetEx247.Core;
using BetEx247.Core.Common.Extensions;
using BetEx247.Core.Infrastructure;
using BetEx247.Data.DAL;

namespace BetEx247.Data
{
    /// <summary>
    /// Summary description for SessionManager
    /// </summary>
    public class SessionManager
    {
        #region Properties

        /// <summary>
        /// Get user info which user logged from Session, get from DB in case session is null at first time
        /// </summary>
        public static Member CurrentUserLogged
        {
            get
            {
                if (!UserLoggedIn())
                {
                    Logout();
                }
                else
                {
                    if (HttpHelper.GetSessionValue("CURRENT_USER_LOGGED") == null)
                    {
                        Member member = GetCurrentUserLoggedIn();
                        HttpHelper.SetSessionValue("CURRENT_USER_LOGGED", member);
                    }
                }
                return HttpHelper.GetSessionValue("CURRENT_USER_LOGGED") as Member;
            }
            set { }
        }

        public static LoginHistory CurrentLoginHistory
        {
            get
            {
                if (!UserLoggedIn())
                {
                    Logout();
                }
                else
                {
                    if (USER_ID > 0)
                    {
                        var loginHistory = IoC.Resolve<ICustomerService>().LastLogin(USER_ID);
                        HttpHelper.SetSessionValue("CURRENT_LOGIN_HISTORY", loginHistory);
                    }                      
                }
                return HttpHelper.GetSessionValue("CURRENT_LOGIN_HISTORY") as LoginHistory;
            }
        }
        
        /// <summary>
        /// Get Current Login User by Member ID
        /// </summary>
        /// <returns>Object Current Login User</returns>
        private static Member GetCurrentUserLoggedIn()
        {
            Member user = null;
            HttpContext Context = HttpContext.Current;
            string sNickName = string.Empty;
            if (UserLoggedIn())
            {
                //in case session is old (email) => check if it is not integer then logout
                if (string.IsNullOrEmpty(Context.User.Identity.Name))
                {
                    Logout();
                    HttpResponse res = Context.Response;
                    res.Redirect(Constant.SEOLinkPage.MEMBER_LOGIN, true);
                }
                else
                {
                    sNickName = Context.User.Identity.Name;
                    user = IoC.Resolve<ICustomerService>().GetCustomerByUsername(sNickName);
                }
            }

            return user;
        }

        public static long USER_ID
        {
            get
            {
                if (UserLoggedIn())
                {
                    if (HttpHelper.GetSessionValue("USER_ID") == null)
                    {
                        HttpHelper.SetSessionValue("USER_ID", CurrentUserLogged.MemberID);
                    }
                    return HttpHelper.GetSessionLong("USER_ID");
                }
                else
                    return 0;
            }
        }

        public static long HISTORY_ID
        {
            get
            {
                if (UserLoggedIn())
                {
                    if (HttpHelper.GetSessionValue("HISTORY_ID") == null)
                    {
                        HttpHelper.SetSessionValue("HISTORY_ID",CurrentLoginHistory.ID);
                    }
                    return HttpHelper.GetSessionLong("HISTORY_ID");
                }
                else
                    return 0;
            }
        }

        public static string USER_NAME
        {
            get
            {
                if (UserLoggedIn())
                {
                    if (HttpHelper.GetSessionValue("USER_NAME") == null)
                    {
                        HttpHelper.SetSessionValue("USER_NAME", CurrentUserLogged.FirstName + " " + CurrentUserLogged.LastName);
                    }
                }
                return HttpHelper.GetSessionString("USER_NAME");
            }
        }

        public static string USER_EMAIL
        {
            get
            {
                if (UserLoggedIn())
                {
                    if (HttpHelper.GetSessionValue("USER_EMAIL") == null)
                    {                           
                        HttpHelper.SetSessionValue("USER_EMAIL", CurrentUserLogged.Email1);
                    }
                }
                return HttpHelper.GetSessionString("USER_EMAIL");
            }
        }
        #endregion

        public SessionManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static bool UserLoggedIn()
        {
            HttpContext Context = HttpContext.Current;
            bool isLogin = Context.User.Identity.IsAuthenticated;   

            return isLogin;
        }

        public static void Logout()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
        }
    }
}
