using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BetEx247.Data.Model;
//using BetEx247.Web.Models;
using BetEx247.Core.Infrastructure;
using BetEx247.Data.DAL;
using BetEx247.Core.Common.Extensions;
using BetEx247.Core.Common.Utils;
using BetEx247.Core;
namespace BetEx247.Services
{
    /// <summary>
    /// Management connection for database and execute store or sql command
    /// </summary>
     class DBManager
    {
        #region "global variable"
          
        #endregion

        public  void initConnection(){
           // Globals.ConnectionString = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EmailConnectionString"].ToString());
            ChilkatEmail.Utils.Constants.strSmtpHost = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"].ToString();
            ChilkatEmail.Utils.Constants.strSmtpUser = System.Configuration.ConfigurationManager.AppSettings["AuthenticationMailUser"].ToString();
            ChilkatEmail.Utils.Constants.strSmtpPass = System.Configuration.ConfigurationManager.AppSettings["AuthenticationMailPassword"].ToString();
            ChilkatEmail.Utils.Constants.emailSentPerTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["emailSentPerTime"].ToString());
            ChilkatEmail.Utils.Constants.bounceEmailAddress = System.Configuration.ConfigurationManager.AppSettings["BounceAddress"].ToString();
            ChilkatEmail.Utils.Constants.bounceEmailPassword = System.Configuration.ConfigurationManager.AppSettings["BounceEmailPassword"].ToString();
            ChilkatEmail.Utils.Constants.ChilkatEmailUnlock = System.Configuration.ConfigurationManager.AppSettings["ChilkatEmailUnlock"].ToString();
            ChilkatEmail.Utils.Constants.EmailFrom = System.Configuration.ConfigurationManager.AppSettings["EmailFrom"].ToString();
           
            Constant.SourceXML.BETCLICKURL = System.Configuration.ConfigurationManager.AppSettings["BETCLICKURL"].ToString();
            Constant.SourceXML.GOALSEVERURL = ""/*System.Configuration.ConfigurationManager.AppSettings["GOALSERVEURL"].ToString()*/;
            Constant.SourceXML.MASTERXMLSOURCE = System.Configuration.ConfigurationManager.AppSettings["MASTERXMLSOURCE"].ToString();
            
           

        }
       
       
    }
}
