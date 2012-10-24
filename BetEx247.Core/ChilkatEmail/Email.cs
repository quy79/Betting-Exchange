using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Chilkat;
using ChilkatEmail.Utils;
namespace ChilkatEmail
{
   public class Email
    {
       
        private int iSmtpPort;
        private bool startTLS = true;
        private string strCharSet = "";



        #region contructor
        public  Email(string smtpHost, string smtpUser, string smtpPass, int smtpPort)
        {
           // strSmtpHost = smtpHost;
           // strSmtpUser = smtpUser;
            //strSmtpPass = smtpPass;
            //iSmtpPort = smtpPort;
        }
        #endregion

        public bool StartTLS {
            get { return startTLS; }
            set { startTLS = value; }
        }

        public string Charset
        {
            get
            {
                return strCharSet;
            }
            set
            {
                strCharSet = value;
            }
        }

        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

      
        public bool SendEmailFromWebsite(string url, string subject, string emailFrom, string emailTo)
        {
            //  The mailman object is used for receiving (POP3)
            //  and sending (SMTP) email.
            Chilkat.MailMan mailman = new Chilkat.MailMan();

            //  The MHT component can be used to convert an HTML page
            //  from a URL, file, or in-memory HTML into an email
            //  with embedded images and style sheets.
            Chilkat.Mht mht = new Chilkat.Mht();
            mht.UseCids = true;
            //  Note: This example requires licenses to both "Chilkat Email" and "Chilkat MHT".

            //  Any string argument automatically begins the 30-day trial.
            bool success;

            success = mht.UnlockComponent(Constants. ChilkatEmailUnlock);
            if (success != true) return false;
            success = mailman.UnlockComponent(Constants.ChilkatEmailUnlock);
            if (success != true) return false;

            Chilkat.Email email = null;
            email = mht.GetEmail(url);
            if (email == null) return false;

            email.Subject = subject;

            email.AddTo(emailTo, emailTo);
            email.From = emailFrom;

           // mailman.SmtpHost = strSmtpHost;
            //mailman.SmtpUsername = strSmtpUser;
            //mailman.SmtpPassword = strSmtpPass;
            mailman.SmtpPort = iSmtpPort;
            mailman.StartTLS = startTLS;

            if (Charset.Length != 0) email.Charset = Charset;

            success = mailman.SendEmail(email);
            if (success != true) return false;

            success = mailman.CloseSmtpConnection();

            return true;
        }

       
       

    }
}
