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
           /* Globals.ConnectionString = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EmailConnectionString"].ToString());
            ChilkatEmail.Utils.Constants.strSmtpHost = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"].ToString();
            ChilkatEmail.Utils.Constants.strSmtpUser = System.Configuration.ConfigurationManager.AppSettings["AuthenticationMailUser"].ToString();
            ChilkatEmail.Utils.Constants.strSmtpPass = System.Configuration.ConfigurationManager.AppSettings["AuthenticationMailPassword"].ToString();
            ChilkatEmail.Utils.Constants.emailSentPerTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["emailSentPerTime"].ToString());
            ChilkatEmail.Utils.Constants.bounceEmailAddress = System.Configuration.ConfigurationManager.AppSettings["BounceAddress"].ToString();
            ChilkatEmail.Utils.Constants.bounceEmailPassword = System.Configuration.ConfigurationManager.AppSettings["BounceEmailPassword"].ToString();
            Debug.WriteLine("Globals.ConnectionString=" + Globals.ConnectionString);
            Debug.WriteLine("strSmtpHost" + ChilkatEmail.Utils.Constants.strSmtpHost);
            Debug.WriteLine("strSmtpUser=" + ChilkatEmail.Utils.Constants.strSmtpUser);
            Debug.WriteLine("strSmtpPass=" + ChilkatEmail.Utils.Constants.strSmtpPass);
             Debug.WriteLine("emailSentPerTime=" + ChilkatEmail.Utils.Constants.emailSentPerTime);
             Debug.WriteLine("bounceEmailAddress="+ChilkatEmail.Utils.Constants.bounceEmailAddress);
             Debug.WriteLine("bounceEmailPassword="+ChilkatEmail.Utils.Constants.bounceEmailPassword);*/
          //  IoC.Resolve<ICommonService>().getAllCountry();
            Constant.SourceXML.BETCLICKURL = System.Configuration.ConfigurationManager.AppSettings["BETCLICKURL"].ToString();
            Constant.SourceXML.GOALSEVERURL = ""/*System.Configuration.ConfigurationManager.AppSettings["GOALSERVEURL"].ToString()*/;
            Constant.SourceXML.MASTERXMLSOURCE = System.Configuration.ConfigurationManager.AppSettings["MASTERXMLSOURCE"].ToString();
            
           

        }
       
       
    }
}
