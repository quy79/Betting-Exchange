using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using DatabaseLayer;
namespace EmailServices
{
    class Email
    {
        public static DataTable GetEmails(int iEmailCount = 0)
        {
            if (iEmailCount <= 0)
            {
                iEmailCount = 10;
            }

            ArrayList arrParams = new ArrayList();
            //Email
            SqlParameter paramEmailCount = new SqlParameter("@EmailCount", iEmailCount);
            paramEmailCount.SqlDbType = SqlDbType.Int;
            arrParams.Add(paramEmailCount);

            //SentDate
            SqlParameter paramSentDate = new SqlParameter("@SentDate", DateTime.Now);
            paramSentDate.SqlDbType = SqlDbType.DateTime;
            arrParams.Add(paramSentDate);

            DataTable tblTemp = DBManager.ExecuteQueryStoreProcedure("sp_GetEmails", arrParams);
            if (tblTemp != null && tblTemp.Rows.Count > 0)
            {
                return tblTemp;
            }

            return null;
        }
        public static bool DeleteEmail(Int64 iEmailID)
        {
            ArrayList arrayParams = new ArrayList();
            SqlParameter paramEmailID = new SqlParameter("@EmailID", iEmailID);
            paramEmailID.SqlDbType = SqlDbType.BigInt;
            paramEmailID.Direction = ParameterDirection.Input;
            arrayParams.Add(paramEmailID);

            int intNumRecordAffect = 0;
            intNumRecordAffect = DBManager.ExecuteNonQueryStoreProcedure("sp_DeleteEmail", arrayParams);
            if (intNumRecordAffect > 0)
            {
                return true;
            }

            return false;
        }
    }
}
