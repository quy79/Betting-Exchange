using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;

namespace BetEx247.Core.Common.Utils
{
    /// <summary>
    /// CookieHelper.GetCookie problem with IE6
    /// a replacement is this class
    /// Summary description for CookieHelper
    /// </summary>
    public class CookieHelper
    {
        public const double DefaultCookieExpired = 30.0;

        public CookieHelper()
        {
        }

        public static string GetCookie(string sName)
        {
            HttpCookieCollection lsCookies = HttpContext.Current.Request.Cookies;

            for (int i = 0; i < lsCookies.Count; i++)
            {
                HttpCookie sCk = lsCookies[i];

                if (sCk.Name == sName)
                {
                    return sCk.Value;
                }
            }

            return string.Empty;
        }

        public static void SetCookie(string sName, string sValue)
        {
            SetCookie(sName, sValue, DefaultCookieExpired);
        }

        public static void SetCookie(string sName, string sValue, double dExpiredDate)
        {
            HttpCookie cookie = new HttpCookie(sName, sValue);
            if (dExpiredDate != 0)
                cookie.Expires = DateTime.Now.AddDays(dExpiredDate);
            cookie.Domain = ConfigurationManager.AppSettings["CurrentDomain"];
            HttpContext.Current.Response.SetCookie(cookie);
        }
    }
}
