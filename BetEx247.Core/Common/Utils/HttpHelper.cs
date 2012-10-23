using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BetEx247.Core.Common.Extensions;

namespace BetEx247.Core.Common.Utils
{
    public enum ePAGE_VIEW_MODE { VIEW, ADD, INSERT, UPDATE, DELETE, APPROVE }
    public enum eRESULT_STATUS { NONE, SUCCESS, FAILURE }

    public class HttpHelper
    {
        public const string APPLICATION_CONFIGURATIONS = "APPLICATION_CONFIGURATIONS";

        #region HttpApplication
        public static void AppSetValue(string pvName, object pvValue)
        {
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application.Set(pvName, pvValue);
            HttpContext.Current.Application.UnLock();
        }
        public static object AppGetValue(string pvName)
        {
            HttpContext.Current.Application.Lock();
            object value = HttpContext.Current.Application.Get(pvName);
            HttpContext.Current.Application.UnLock();
            return value;
        }          

        public static void AppRemoveValue(string pvName)
        {
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application.Remove(pvName);
            HttpContext.Current.Application.UnLock();
        }
        public static string PhysicalFilePath(string pvVirtualpath)
        {
            return HttpContext.Current.Server.MapPath(pvVirtualpath);
        }
        public static string GetContentType(string extension)
        {
            extension = extension.ToLower();
            string contentType = string.Empty;
            switch (extension)
            {
                // Document - Application
                case "ppt":
                    contentType = "application/vnd.ms-powerpoint";
                    break;
                case "doc":
                    contentType = "application/msword";
                    break;
                case "xls":
                    contentType = "application/msexcel";
                    break;
                case "pdf":
                    contentType = "application/pdf";
                    break;
                // Compression
                case "zip":
                    contentType = "application/x-zip-compressed";
                    break;
                case "rar":
                    contentType = "application/x-zip-compressed";
                    break;
                // Document Text
                case "txt":
                    contentType = "text/plain";
                    break;
                case "rtf":
                    contentType = "text/plain";
                    break;
                case "html":
                    contentType = "text/html";
                    break;
                case "xml":
                    contentType = "text/xml";
                    break;
                case "css":
                    contentType = "text/css";
                    break;
                // Bitmap
                case "jpg":
                    contentType = "image/pjpeg";
                    break;
                case "gif":
                    contentType = "image/gif";
                    break;
                case "ico":
                    contentType = "image/x-icon";
                    break;
                case "png":
                    contentType = "image/x-png";
                    break;
                // Audio
                case "wav":
                    contentType = "audio/wav";
                    break;
                case "wma":
                    contentType = "audio/x-ms-wma";
                    break;
                case "wmv":
                    contentType = "video/x-ms-wmv";
                    break;
                case "mp3":
                    contentType = "audio/mpeg";
                    break;
                case "avi":
                    contentType = "video/avi";
                    break;
                default:
                    contentType = string.Empty;
                    break;
            }
            return contentType;
        }
        #endregion

        #region ServerVariables
        public static string GetAppPhysicalPath(HttpRequest request)
        {
            return request.ServerVariables["APPL_PHYSICAL_PATH"].Trim();
        }
        public static string GetAppPhysicalPath()
        {
            return GetAppPhysicalPath(HttpContext.Current.Request);
        }
        public static string GetPhysicalPath(HttpRequest request)
        {
            return HttpContext.Current.Request.PhysicalPath;
        }
        public static string GetPhysicalPath()
        {
            return GetPhysicalPath(HttpContext.Current.Request);
        }
        public static string GetHttpHost(HttpRequest request)
        {
            return request.ServerVariables["HTTP_HOST"];
        }
        public static string GetHttpHost()
        {
            return GetHttpHost(HttpContext.Current.Request);
        }
        public static string GetRemoteAddr(HttpRequest request)
        {
            string strIpAddress;
            strIpAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (strIpAddress == null)
                strIpAddress = request.ServerVariables["REMOTE_ADDR"];
            return strIpAddress.Split(',')[0].ToString();
        }
        public static string GetRemoteAddr()
        {
            return GetRemoteAddr(HttpContext.Current.Request);
        }
        public static string GetRemoteHost(HttpRequest request)
        {
            return GetRemoteAddr(request);// request.UserHostName;
        }
        public static string GetRemoteHost()
        {
            return GetRemoteHost(HttpContext.Current.Request);
        }
        public static string GetHttpReferer(HttpRequest request)
        {
            return CSafeDataHelper.SafeString(request.ServerVariables["HTTP_REFERER"]);
        }
        public static string GetHttpReferer()
        {
            return GetHttpReferer(HttpContext.Current.Request);
        }
        public static string GetScriptName(HttpRequest request)
        {
            return request.ServerVariables["SCRIPT_NAME"];
        }
        public static string GetScriptName()
        {
            return GetScriptName(HttpContext.Current.Request);
        }
        public static string GetServerProtocol(HttpRequest request)
        {
            if (request.ServerVariables["SERVER_PROTOCOL"].ToUpper().Contains("HTTPS")) return "https://";
            return "http://";
        }
        public static string GetServerProtocol()
        {
            return GetServerProtocol(HttpContext.Current.Request);
        }
        public static string GetPagingUrl(HttpRequest request)
        {
            string query = request.Url.Query.TrimStart("?".ToCharArray());
            query = query.Replace(String.Format("PageSize={0}", request.QueryString["PageSize"]), "");
            query = query.Replace(String.Format("PageNum={0}", request.QueryString["PageNum"]), "");
            query = query.Replace("PageSize=", "");
            query = query.Replace("PageNum=", "");
            query = query.Replace("&&&", "&").Replace("&&&", "&");
            query = query.Replace("&&", "&").Replace("&&", "&").Replace("&&", "&");
            query = query.Trim("&".ToCharArray());

            string url = (request.Url.Query.Trim() != String.Empty ? request.Url.AbsoluteUri.Replace(request.Url.Query, "") : request.Url.AbsoluteUri);
            url = url.TrimEnd("?".ToCharArray());
            return (url + "?" + query);
        }
        public static string GetPageUrl(HttpRequest request, bool excludeQuery)
        {
            if (excludeQuery && request.Url.Query.Trim() != String.Empty)
                return request.Url.AbsoluteUri.Replace(request.Url.Query, "");

            return request.Url.AbsoluteUri;
        }
        public static string GetPageUrl(bool excludeQuery)
        {
            return GetPageUrl(HttpContext.Current.Request, excludeQuery);
        }

        public static bool IsSamePage(HttpRequest request)
        {
            string scriptname = GetScriptName(request).ToLower();
            string referer = GetHttpReferer(request);
            if (referer == null || referer == String.Empty) return false;
            if (referer.IndexOf("?") > 0) referer = referer.Substring(0, referer.IndexOf("?")).Trim();

            referer = referer.ToLower();
            if (referer.EndsWith(scriptname) || (referer + "/default.aspx").EndsWith(scriptname) || (referer + "default.aspx").EndsWith(scriptname))
                return true;

            return false;
        }
        #endregion

        #region Browsers
        public static string GetWebBrowser(HttpRequest request)
        {
            return String.Format("{0} {1}", request.Browser.Browser, request.Browser.Version);
        }
        public static string GetWebBrowser()
        {
            return GetWebBrowser(HttpContext.Current.Request);
        }
        public static string GetBrowserName(HttpRequest request)
        {
            return request.Browser.Browser;
        }
        public static string GetBrowserName()
        {
            return GetBrowserName(HttpContext.Current.Request);
        }
        public static string GetBrowserVersion(HttpRequest request)
        {
            return request.Browser.Version;
        }
        public static string GetBrowserVersion()
        {
            return GetBrowserVersion(HttpContext.Current.Request);
        }
        #endregion

        #region Sessions
        public static bool SessionExists(string pvName)
        {
            return (GetSessionValue(pvName) == null ? false : true);
        }
        public static void SetSessionValue(string pvName, object value)
        {
            HttpContext.Current.Session[pvName] = value;
        }
        public static object GetSessionValue(string pvName)
        {
            if (HttpContext.Current.Session != null)
                return HttpContext.Current.Session[pvName];
            return null;
        }
        public static string GetSessionString(string pvName)
        {
            return CSafeDataHelper.SafeString(GetSessionValue(pvName)).Trim();
        }
        public static DateTime GetSessionDatetime(string pvName)
        {
            return CSafeDataHelper.SafeDateTime(GetSessionValue(pvName));
        }
        public static bool GetSessionBoolean(string pvName)
        {
            return CSafeDataHelper.SafeBool(GetSessionValue(pvName));
        }
        public static double GetSessionDouble(string pvName)
        {
            return CSafeDataHelper.SafeDouble(GetSessionValue(pvName));
        }
        public static float GetSessionFloat(string pvName)
        {
            return CSafeDataHelper.SafeFloat(GetSessionValue(pvName));
        }
        public static long GetSessionLong(string pvName)
        {
            return CSafeDataHelper.SafeLong(GetSessionValue(pvName));
        }
        public static int GetSessionInt(string pvName)
        {
            return CSafeDataHelper.SafeInt(GetSessionValue(pvName));
        }
        #endregion

        #region Cookies
        public static HttpCookie SetCookie(string pvCookieName, string pvCookieValue, double pvExpireDate)
        {
            HttpCookie cookie = new HttpCookie(pvCookieName, pvCookieValue);
            if (pvExpireDate != 0)
                cookie.Expires = DateTime.Now.AddDays(pvExpireDate);

            HttpContext.Current.Response.SetCookie(cookie);
            return cookie;
        }
        public static HttpCookie SetCookie(string pvCookieName, string pvCookieValue)
        {
            HttpCookie fvCookie = new HttpCookie(pvCookieName, pvCookieValue);
            HttpContext.Current.Response.SetCookie(fvCookie);
            return fvCookie;
        }
        public static bool CookieExists(string pvCookieName)
        {
            return (HttpContext.Current.Request.Cookies[pvCookieName] == null ? false : true);
        }
        public static string GetCookie(string pvCookieName)
        {
            if (CookieExists(pvCookieName))
                return HttpContext.Current.Request.Cookies[pvCookieName].Value.Trim();

            return String.Empty;
        }
        public static int GetCookieInt(string pvCookieName, int pvDefaultValue)
        {
            if (CookieExists(pvCookieName))
                return HttpContext.Current.Request.Cookies[pvCookieName].Value.Trim().ToInt32();

            return pvDefaultValue;
        }
        public static void ClearCookie(string ckName)
        {
            HttpCookie cookie = new HttpCookie(ckName);
            cookie.Expires = DateTime.Now.AddDays(-1d);
            HttpContext.Current.Response.SetCookie(cookie);
        }
        #endregion             

        #region HttpResponse Helpers
        public static void WriteOutput(string text)
        {
            HttpContext.Current.Response.Write(text);
        }
        public static void WriteOutput(string text, params object[] items)
        {
            WriteOutput(String.Format(text, items));
        }
        public static void WriteOutputLine(string text)
        {
            WriteOutput(text + "\n");
        }
        public static void WriteOutputLine(string text, params object[] items)
        {
            WriteOutputLine(String.Format(text, items));
        }
        #endregion

        #region Html/Url Encoding
        public static string HtmlDecode(string html)
        {
            return HttpContext.Current.Server.HtmlDecode(html);
        }
        public static string HtmlEncode(string html)
        {
            return HttpContext.Current.Server.HtmlEncode(html);
        }
        public static string UrlDecode(string url)
        {
            return HttpContext.Current.Server.UrlDecode(url);
        }
        public static string UrlEncode(string url)
        {
            return HttpContext.Current.Server.UrlEncode(url);
        }
        public static string HtmlEntityDecode(string text)
        {
            try
            {
                StringBuilder result = new StringBuilder(text.Length + (int)(text.Length * 0.1));
                char[] chars = text.ToCharArray();

                foreach (char c in chars)
                {
                    int value = Convert.ToInt32(c);
                    if (value > 127)
                    {
                        switch (value)
                        {
                            case 160:
                                result.Append(' '); //.AppendFormat("&#{0};", 32); space
                                break;
                            case 8220:
                                result.Append('\"'); //.AppendFormat("&#{0};", 34); double quote
                                break;
                            case 8221:
                                result.Append('\"'); //.AppendFormat("&#{0};", 34); double quote
                                break;
                            case 8217:
                                result.Append('\''); //.AppendFormat("&#{0};", 39); single quote
                                break;
                            default:
                                result.AppendFormat("&#{0};", value);
                                break;
                        }
                    }
                    else result.Append(c);
                }
                return result.ToString();
            }
            catch (Exception ex)
            {
                //Logger.Error("Error in function HttpHelper.HtmlEntityDecode() method.", ex);
            }
            return string.Empty;
        }
        #endregion

        #region QueryString
        public static string GetQueryStringValue(string pvName)
        {
            return HttpContext.Current.Request.QueryString[pvName];
        }

        public static string GetQueryStringString(string pvName)
        {
            return CSafeDataHelper.SafeString(GetQueryStringValue(pvName)).Trim();
        }

        public static DateTime GetQueryStringDatetime(string pvName)
        {
            return CSafeDataHelper.SafeDateTime(GetQueryStringValue(pvName));
        }

        public static bool GetQueryStringBoolean(string pvName)
        {
            return CSafeDataHelper.SafeBool(GetQueryStringValue(pvName));
        }

        public static double GetQueryStringDouble(string pvName)
        {
            return CSafeDataHelper.SafeDouble(GetQueryStringValue(pvName));
        }

        public static float GetQueryStringFloat(string pvName)
        {
            return CSafeDataHelper.SafeFloat(GetQueryStringValue(pvName));
        }

        public static long GetQueryStringLong(string pvName)
        {
            return CSafeDataHelper.SafeLong(GetQueryStringValue(pvName));
        }

        public static int GetQueryStringInt(string pvName)
        {
            return CSafeDataHelper.SafeInt(GetQueryStringValue(pvName));
        }
        #endregion
    }
}
