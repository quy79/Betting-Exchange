using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BetEx247.Core.Common.Utils
{
    public class CSafeDataHelper
    {
        #region Safe DataRow Value
        /// <summary>
        /// Get integer value with safe mode
        /// </summary>
        /// <param name="row"></param>
        /// <param name="pvColumnName"></param>
        /// <returns></returns>
        public static int SafeInt(DataRow pvRow, string pvColumnName)
        {
            if (pvRow[pvColumnName] == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return (int)Convert.ToInt32(pvRow[pvColumnName]);
            }
        }

        /// <summary>
        /// Get long value with safe mode
        /// </summary>
        /// <param name="pvRow"></param>
        /// <param name="pvColumnName"></param>
        /// <returns></returns>
        public static long SafeLong(DataRow pvRow, string pvColumnName)
        {
            if (pvRow[pvColumnName] == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return (long)Convert.ToInt64(pvRow[pvColumnName]);
            }
        }

        /// <summary>
        /// Get bool value with safe mode
        /// </summary>
        /// <param name="pvRow"></param>
        /// <param name="pvColumnName"></param>
        /// <returns></returns>
        public static bool SafeBool(DataRow pvRow, string pvColumnName)
        {
            if (pvRow[pvColumnName] == DBNull.Value)
            {
                return false;
            }
            else
            {
                return (bool)Convert.ToBoolean(pvRow[pvColumnName]);
            }
        }

        /// <summary>
        /// Get DateTime value with safe mode
        /// </summary>
        /// <param name="pvRow"></param>
        /// <param name="pvColumnName"></param>
        /// <returns></returns>
        public static DateTime SafeDateTime(DataRow pvRow, string pvColumnName)
        {
            if (pvRow[pvColumnName] == DBNull.Value)
            {
                return DateTime.Today;
            }
            else
            {
                return (DateTime)Convert.ToDateTime(pvRow[pvColumnName]);
            }
        }

        /// <summary>
        /// Get Byte value with safe mode
        /// </summary>
        /// <param name="pvRow"></param>
        /// <param name="pvColumnName"></param>
        /// <returns></returns>
        public static byte SafeByte(DataRow pvRow, string pvColumnName)
        {
            if (pvRow[pvColumnName] == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return (byte)Convert.ToByte(pvRow[pvColumnName]);
            }
        }

        /// <summary>
        /// Get Decimal value with safe mode
        /// </summary>
        /// <param name="pvRow"></param>
        /// <param name="pvColumnName"></param>
        /// <returns></returns>
        public static decimal SafeDecimal(DataRow pvRow, string pvColumnName)
        {
            if (pvRow[pvColumnName] == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return (decimal)Convert.ToDecimal(pvRow[pvColumnName]); ;
            }
        }

        /// <summary>
        /// Get float value with safe mode
        /// </summary>
        /// <param name="pvRow"></param>
        /// <param name="pvColumnName"></param>
        /// <returns></returns>
        public static float SafeFloat(DataRow pvRow, string pvColumnName)
        {
            if (pvRow[pvColumnName] == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToSingle(pvRow[pvColumnName]);
            }
        }

        /// <summary>
        /// Get Double value with safe mode
        /// </summary>
        /// <param name="pvRow"></param>
        /// <param name="pvColumnName"></param>
        /// <returns></returns>
        public static double SafeDouble(DataRow pvRow, string pvColumnName)
        {
            if (pvRow[pvColumnName] == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToDouble(pvRow[pvColumnName]);
            }
        }


        /// <summary>
        /// Get string value with safe mode
        /// </summary>
        /// <param name="pvRow"></param>
        /// <param name="pvColumnName"></param>
        /// <returns></returns>
        public static string SafeString(DataRow pvRow, string pvColumnName)
        {
            if (pvRow[pvColumnName] == DBNull.Value)
            {
                return "";
            }
            else
            {
                return Convert.ToString(pvRow[pvColumnName]);
            }
        }

        /// <summary>
        /// Get GUID value with safe mode
        /// </summary>
        /// <param name="pvRow"></param>
        /// <param name="pvColumnName"></param>
        /// <returns></returns>
        public static Guid SafeGuid(DataRow pvRow, string pvColumnName)
        {
            return (Guid)pvRow[pvColumnName];
        }

        #endregion

        #region Safe Object Value
        /// <summary>
        /// Get int Value from object in safe mode
        /// </summary>
        /// <param name="pvObj"></param>
        /// <returns></returns>
        public static int SafeInt(object pvObj)
        {
            if (pvObj == null || pvObj == DBNull.Value || pvObj.ToString().Trim() == String.Empty )
            {
                return 0;
            }
            else
            {
                return (int)Convert.ToInt32(pvObj);
            }
        }

        /// <summary>
        /// Get long Value from object in safe mode
        /// </summary>
        /// <param name="pvObj"></param>
        /// <returns></returns>
        public static long SafeLong(object pvObj)
        {
            if (pvObj == null || pvObj == DBNull.Value || pvObj.ToString().Trim() == String.Empty )
            {
                return 0;
            }
            else
            {
                return (long)Convert.ToInt64(pvObj);
            }
        }

        /// <summary>
        /// Get boolean Value from object in safe mode
        /// </summary>
        /// <param name="pvObj"></param>
        /// <returns></returns>
        public static bool SafeBool(object pvObj)
        {
            if (pvObj == null || pvObj == DBNull.Value || pvObj.ToString().Trim() == String.Empty)
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(pvObj);
            }
        }

        /// <summary>
        /// Get sbyte Value from object in safe mode
        /// </summary>
        /// <param name="pvObj"></param>
        /// <returns></returns>
        public static sbyte SafeSByte(object pvObj)
        {
            if (pvObj == null || pvObj == DBNull.Value || pvObj.ToString().Trim() == String.Empty || pvObj.ToString().Trim().ToLower() == "false")
            {
                return 0;
            }
            else if (pvObj.ToString().Trim().ToLower() == "true")
            {
                return 1;
            }
            else
            {
                return Convert.ToSByte(pvObj);
            }
        }

        /// <summary>
        /// Get DateTime Value from object in safe mode
        /// </summary>
        /// <param name="pvObj"></param>
        /// <returns></returns>
        public static DateTime SafeDateTime(object pvObj)
        {
            if (pvObj == null || pvObj == DBNull.Value || pvObj.ToString().Trim() == String.Empty)
                return DateTime.MaxValue;
            else if (pvObj is String)
                return DateTime.Parse(pvObj.ToString());
            else
                return (DateTime)pvObj;
        }

        /// <summary>
        /// Get Byte Value from object in safe mode
        /// </summary>
        /// <param name="pvObj"></param>
        /// <returns></returns>
        public static byte SafeByte(object pvObj)
        {
            if (pvObj == null || pvObj == DBNull.Value || pvObj.ToString().Trim() == String.Empty)
            {
                return 0;
            }
            else
            {
                return Convert.ToByte(pvObj);
            }
        }

        /// <summary>
        /// Get Decimal Value from object in safe mode
        /// </summary>
        /// <param name="pvObj"></param>
        /// <returns></returns>
        public static decimal SafeDecimal(object pvObj)
        {
            if (pvObj == null || pvObj == DBNull.Value || pvObj.ToString().Trim() == String.Empty)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(pvObj);
            }
        }

        /// <summary>
        /// Get Float Value from object in safe mode
        /// </summary>
        /// <param name="pvObj"></param>
        /// <returns></returns>
        public static float SafeFloat(object pvObj)
        {
            if (pvObj == null || pvObj == DBNull.Value || pvObj.ToString().Trim() == String.Empty)
            {
                return 0;
            }
            else
            {
                return Convert.ToSingle(pvObj);
            }
        }

        /// <summary>
        /// Get double Value from object in safe mode
        /// </summary>
        /// <param name="pvObj"></param>
        /// <returns></returns>
        public static double SafeDouble(object pvObj)
        {
            if (pvObj == null || pvObj == DBNull.Value || pvObj.ToString().Trim() == String.Empty)
            {
                return 0.0;
            }
            else
            {
                return Convert.ToDouble(pvObj);
            }
        }

        /// <summary>
        /// Get string Value from object in safe mode
        /// </summary>
        /// <param name="pvObj"></param>
        /// <returns></returns>
        public static string SafeString(object pvObj)
        {
            if (pvObj == null || pvObj == DBNull.Value || pvObj.ToString().Trim() == String.Empty)
            {
                return "";
            }
            else
            {
                return Convert.ToString(pvObj);
            }
        }

        /// <summary>
        /// Get GUID Value from object in safe mode
        /// </summary>
        /// <param name="pvObj"></param>
        /// <returns></returns>
        public static Guid SafeGuid(object pvObj)
        {
            return (Guid)pvObj;
        }   
        #endregion
    }
}
