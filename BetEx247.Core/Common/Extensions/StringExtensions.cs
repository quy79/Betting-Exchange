using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Globalization;

namespace BetEx247.Core.Common.Extensions
{
    /// <summary>
    /// Extend the System.String class
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Check whether the input value is a us zipcode
        /// </summary>
        /// <param name="value">input value</param>
        /// <returns>true or false</returns>
        public static bool IsZipCodeUS(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;


            Regex regex = new Regex(@"^[0-9-]+$");
            return regex.IsMatch(value);
        }


        /// <summary>
        /// Create a copy of this string, upper the first character, and return to the caller
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Capitalize-first-case string or an empty string if the value is null or empty</returns>
        public static string ToCapFirst(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Length == 1)
                return value.ToUpper();

            return string.Concat(char.ToUpper(value[0]), value.Substring(1).ToLower());
        }

        /// <summary>
        /// Create a copy of this string, convert to titlecase, and return to the caller
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Titlecase string or an empty string if the value is null or empty</returns>
        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Length == 1)
                return value.ToUpper();

            TextInfo text = new CultureInfo("en-US", false).TextInfo;
            return text.ToTitleCase(value.ToLower());
        }

        /// <summary>
        /// Check whether the input value is an integer number or not
        /// </summary>
        /// <param name="value">input value</param>
        /// <returns>true or false</returns>
        public static bool IsInteger(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            Regex regex = new Regex(@"^[+-]?\d+$");
            return regex.IsMatch(value);
        }

        /// <summary>
        /// Check whether the input value is a number or not
        /// </summary>
        /// <param name="value">input value</param>
        /// <returns>true or false</returns>
        public static bool IsNumber(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            Regex regex = new Regex(@"^[+-]?\d+(\.\d+)?$");
            return regex.IsMatch(value);
        }

        /// <summary>
        /// Determine whether this string is numeric
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value)
        {
            return Regex.IsMatch(value, "^\\d+$");
        }

        /// <summary>
        /// Parse to 32 bit signed integer value.
        /// Return to int.MinValue if the input value's not integer number.
        /// </summary>
        /// <param name="value">input value</param>
        /// <returns>32 bit signed integer value</returns>
        public static int ToInt32(this string value)
        {
            if (!IsInteger(value))
                return int.MinValue;

            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Parse to 64 bit signed integer value.
        /// Return to long.MinValue if the input value's not integer number.
        /// </summary>
        /// <param name="value">input value</param>
        /// <returns>64 bit signed integer value</returns>
        public static long ToInt64(this string value)
        {
            if (!IsInteger(value))
                return long.MinValue;

            return Convert.ToInt64(value);
        }

        /// <summary>
        /// Parse to float number
        /// Return to defaultValue (if supply) or float.MinValue if the input value's not number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this string value, float defaultValue)
        {
            if (!IsNumber(value))
            {
                return defaultValue;
            }

            return float.Parse(value);
        }

        /// <summary>
        /// Convert to decimal number
        /// Return 0 if the input value's not number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value)
        {
            if (!IsNumber(value))
            {
                return 0;
            }

            return Convert.ToDecimal(value);
        }

        /// <summary>
        /// Not test yet
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return DateTime.MinValue;

            DateTime result = DateTime.MinValue;

            try { result = Convert.ToDateTime(value); }
            catch { result = DateTime.MinValue; }

            return result;
        }


        /// <summary>
        /// Concat all the items have index value contained in the 'indices' paramter
        /// </summary>
        /// <param name="list">source</param>
        /// <param name="indices">separated comma indices string</param>
        /// <returns>string</returns>
        public static string ToString(this List<string> list, string indices)
        {
            if (list == null || list.Count == 0 || string.IsNullOrEmpty(indices))
                return string.Empty;

            string ret = string.Empty;

            var filterlist = from item in list
                             where indices.Contains(list.IndexOf(item).ToString(), ',')
                             select item;

            foreach (string temp in filterlist)
            {
                if (temp.IndexOf("','") == -1 || !temp.Trim().StartsWith("'") || !temp.Trim().EndsWith("'"))
                    temp.Replace("'", "''");

                ret = string.Format("{0}'{1}',", ret, temp);
            }

            return ret.Trim(',');
        }

        /// <summary>
        /// Determines whether a string contains a specified value by using the default equality comparer.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keys"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static bool Contains(this string source, string value, params char[] separator)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value))
                return false;

            var asource = source.Split(separator).Select(s => s.Trim());

            var avalue = value.Split(separator).Select(s => s.Trim());

            foreach (string key in avalue)
            {
                if (asource.Contains(key))
                {
                    return true;
                }
            }

            return false;
        }

        public static string RemoveH2H3(this string input)
        {
            string sResult = input;
            if (input != null && input != "")
            {
                string mPattern = @"(<h2>)+(\w)+(</h2>)?";

                Regex rx = new Regex(mPattern, RegexOptions.IgnoreCase);

                MatchCollection mMatchCollection = rx.Matches(input);

                if (mMatchCollection.Count > 0)
                {
                    for (int i = 0; i < mMatchCollection.Count; i++)
                    {
                        sResult = sResult.Replace(mMatchCollection[i].ToString(), "");
                    }
                }//End if

                mPattern = @"(<h3>)+(\w)+(</h3>)?";

                rx = new Regex(mPattern, RegexOptions.IgnoreCase);

                mMatchCollection = rx.Matches(input);

                if (mMatchCollection.Count > 0)
                {
                    for (int i = 0; i < mMatchCollection.Count; i++)
                    {
                        sResult = sResult.Replace(mMatchCollection[i].ToString(), "");
                    }
                }//End if

            }
            return sResult;
        }


        public static string SubStringAtEnd(this string text, int numOfCharacter, string sAtEnd)
        {
            string returnStr = "";
            text = text.RemoveH2H3();
            text = ReplaceLink(text).Trim();

            if (numOfCharacter >= text.Length) returnStr = text;
            else
            {
                string[] arr = text.Split(new char[] { ' ' });
                for (int i = 0; i < arr.Length; i++)
                {
                    string tmp = returnStr + arr[i];
                    if (tmp.Length <= numOfCharacter) returnStr += arr[i] + " ";
                    else break;
                }
            }

            returnStr = returnStr.Trim().TrimEnd('.') + sAtEnd;

            return returnStr;
        }

        public static string SubStringAtEnd2(this string text, int numOfCharacter, string sAtEnd)
        {
            string returnStr = "";
            text = text.RemoveH2H3();
            text = ReplaceLink(text).Trim();

            if (numOfCharacter >= text.Length) returnStr = text;
            else
            {
                string[] arr = text.Split(new char[] { ' ' });
                for (int i = 0; i < arr.Length; i++)
                {
                    string tmp = returnStr + arr[i];
                    if (tmp.Length <= numOfCharacter) returnStr += arr[i] + " ";
                    else break;
                }

                returnStr = returnStr.Trim().TrimEnd('.') + sAtEnd;
            }

            return returnStr;
        }

        public static string Truncate(this string s, int length, bool atWord, bool addEllipsis)
        {
            // Return if the string is less than or equal to the truncation length
            if (s == null || s.Length <= length)
                return s;

            // Do a simple tuncation at the desired length
            string s2 = s.Substring(0, length);

            // Truncate the string at the word
            if (atWord)
            {
                // List of characters that denote the start or a new word (add to or remove more as necessary)
                List<char> alternativeCutOffs = new List<char>() { ' ', ',', '.', '?', '/', ':', ';', '\'', '\"', '\'', '-' };

                // Get the index of the last space in the truncated string
                int lastSpace = s2.LastIndexOf(' ');

                // If the last space index isn't -1 and also the next character in the original
                // string isn't contained in the alternativeCutOffs List (which means the previous
                // truncation actually truncated at the end of a word),then shorten string to the last space
                if (lastSpace != -1 && (s.Length >= length + 1 && !alternativeCutOffs.Contains(s.ToCharArray()[length])))
                    s2 = s2.Remove(lastSpace);
            }

            // Add Ellipsis if desired
            if (addEllipsis)
                s2 += "...";

            return s2;
        }

        public static string ReplaceLink(string link)
        {
            string mInput = link;
            if (mInput != null && mInput != "")
            {
                //string mPattern = @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?";            

                Regex rx = new Regex("<[^>]*>", RegexOptions.IgnoreCase);

                MatchCollection mMatchCollection = rx.Matches(mInput);
                foreach (Match urlMatch in mMatchCollection)
                {
                    string mTemporaryOutput = "";
                    mInput = mInput.Replace(urlMatch.ToString(), mTemporaryOutput);
                }
            }
            return mInput;
        }

        public static string WrapOverflow(this string text, int maxlength, string trail)
        {
            if (text.IsEmpty() || text.Length < (maxlength - trail.Length)) return text;

            return text.Substring(0, maxlength - trail.Length) + trail;
        }

        public static string WrapOverflow(this  string text)
        {
            string[] arrs = text.Split(' ');

            return string.Join(" ", (from sp in arrs
                                     select BuildWrapTextIf(sp, 40)).ToArray());

        }

        public static string WrapOverflow(this  string text, int limit)
        {
            string[] arrs = text.Split(' ');

            return string.Join(" ", (from sp in arrs
                                     select BuildWrapTextIf(sp, limit)).ToArray());

        }

        private static string BuildWrapTextIf(string s, int limit)
        {
            if (s.Length > limit)
            {
                return string.Join("<wbr/>", (from sp in s select sp.ToString()).ToArray());
            }

            return s;
        }

        public static string LimitWords(this string s, int length, int wordLength)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= length)
                return s;

            string t = s.Substring(0, length - 1);

            if (s.Substring(length - 1, 1) == " ")
                return t;

            string[] arrs = t.Split(' ');

            if (arrs.Last().Length >= wordLength)
                return t;

            return string.Join(" ", arrs.Take(arrs.Length - 1).ToArray());

        }
        public static string BoldFirstWord(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            int idx = s.IndexOf(' ');
            if (idx == -1) idx = s.Length;
            return string.Format("<b>{0}</b>{1}", s.Substring(0, idx), s.Substring(idx));
        }

        public static string SafeString(string s)
        {
            return s.Replace("<", "zv12zaacxc").Replace(">", "zv12zaacxc").Replace("</", "zv12zaacxc").Replace("'", "''");
        }
        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ' || c == '\'')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public static IDictionary<string, string> Map(this string me, string pattern, Func<Match, KeyValuePair<string, string>> action)
        {
            IDictionary<string, string> matches = new Dictionary<string, string>();
            foreach (Match match in Regex.Matches(me, pattern))
            {
                matches.Add(action(match));
            }
            return matches;
        }

        public static List<string> Map(this string me, string pattern, Func<Match, string> action)
        {
            List<string> matches = new List<string>();
            if (me == null) me = string.Empty;
            foreach (Match match in Regex.Matches(me, pattern))
            {
                matches.Add(action(match));
            }
            return matches;
        }

        public static string convertSpToNormal(string vl)
        {
            return vl.Replace("KT@DB", "$").Replace("KT@1DB", "^").Replace("KT@2DB", "&");
        }
    }

    /// <summary>
    /// ObjectExtensions
    /// </summary>
    public static class ObjectExtensions
    {
        public static bool IsEmpty(this object value)
        {
            return value == DBNull.Value || value == null || string.IsNullOrEmpty(value.ToString().Trim());
        }
    }

    /// <summary>
    /// Extend the control class
    /// </summary>
    public static class ControlExtenders
    {
        public static string RenderControl(this Control control)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter textWriter = new HtmlTextWriter(sw))
                {
                    control.RenderControl(textWriter);
                }
            }
            return sb.ToString();
        }
    }
}
