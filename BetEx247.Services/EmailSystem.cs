using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace EmailServices
{
    public class EmailSystem
    {
        #region Properties

        private const string tabEmailSubjectXML = "subject";
        private const string tabEmailBodyXML = "body";
        private static string _SMTPServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
        private static int _Port = 25;
        private static string _From = ConfigurationManager.AppSettings["EmailFrom"].ToString();

        // default sent email count
        static int _iSentEmailCount = 10;

        #endregion

        public static void SendEmails()
        {
            try
            {
                _iSentEmailCount = int.Parse(ConfigurationManager.AppSettings["SentEmailCount"].ToString());
            }
            catch { }

            //Email email = new Email();
            DataTable dtEmails = Email.GetEmails(_iSentEmailCount);
            if (dtEmails != null && dtEmails.Rows.Count > 0)
            {
                for (int i = 0; i < dtEmails.Rows.Count; i++)
                {
                    try
                    {
                        string strBody = dtEmails.Rows[i]["Content"].ToString();
                        string strSubject = dtEmails.Rows[i]["Subject"].ToString();

                        //Reject character special
                        strBody = strBody.Replace("\r", "");
                        strBody = strBody.Replace("\n", "");
                        strBody = strBody.Replace("\\", "");
                        string strCC = string.Empty;
                        string strBCC = string.Empty;

                        string strEmailTo = dtEmails.Rows[i]["ReceiverMail"].ToString();
                        string strFrom = "\"MarketingEmail\" <" + _From + ">";

                        string strErrorMessage = SendMail(_SMTPServer, strFrom, strEmailTo, strCC, strBCC, strSubject, strBody, "Basic");
                        if (string.IsNullOrEmpty(strErrorMessage))
                        {
                            try
                            {
                                Int64 iEmailID = Int64.Parse(dtEmails.Rows[i]["EmailID"].ToString());
                                Email.DeleteEmail(iEmailID);
                            }
                            catch { }
                        }
                    }
                    catch { }
                }
            }
        }

        private static string SendMail(string smtpServer, string strFrom, string strTo, string strCC,
                                    string strBCC, string strSubject, string strBody, string strAuthType)
        {
            string strErrorMessage = string.Empty;
            try
            {
                System.Net.Mail.MailMessage objMessage = new System.Net.Mail.MailMessage();
                objMessage.IsBodyHtml = true;
                objMessage.From = new System.Net.Mail.MailAddress(strFrom);
                if (strTo.IndexOf(";") >= 0)
                {
                    string[] arrEmail = strTo.Split(';');
                    for (int i = 0; i < arrEmail.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(arrEmail[i].Trim()))
                        {
                            objMessage.To.Add(arrEmail[i].Trim());
                        }
                    }
                }
                else
                {
                    objMessage.To.Add(strTo);
                }

                objMessage.Subject = strSubject;
                objMessage.Body = strBody;
                //Optional
                if (strCC != string.Empty)
                    objMessage.CC.Add(strCC);
                if (strBCC != string.Empty)
                    objMessage.Bcc.Add(strBCC);

                int intSMTPPort = _Port;
                string authType = strAuthType;// "Basic";
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpServer, intSMTPPort);
                //Send Mail From Client Localhost
                string authMailUser = ConfigurationManager.AppSettings["AuthenticationMailUser"].ToString();
                string authMailPassword = ConfigurationManager.AppSettings["AuthenticationMailPassword"].ToString();
                if (!string.IsNullOrEmpty(authMailUser) && !string.IsNullOrEmpty(authMailPassword))
                {
                    System.Net.NetworkCredential nc = new System.Net.NetworkCredential(authMailUser, authMailPassword);
                    smtp.Credentials = (System.Net.ICredentialsByHost)nc.GetCredential(smtp.Host, smtp.Port, authType);
                }

                smtp.Send(objMessage);
            }
            catch (Exception ex)
            {
                strErrorMessage = ex.Message;
            }

            return strErrorMessage;
        }

        public static Hashtable ProcessMailTemplate(string strPathResource, Hashtable replaceParams)
        {
            Hashtable hashReturn = null;

            try
            {
                if (string.IsNullOrEmpty(strPathResource)) return hashReturn;

                if (replaceParams == null || replaceParams.Count <= 0) return hashReturn;

                System.Xml.XmlDataDocument myDoc = new System.Xml.XmlDataDocument();
                myDoc.Load(strPathResource);

                string strBody = myDoc.GetElementsByTagName(tabEmailBodyXML).Item(0).InnerText;
                string strSubject = myDoc.GetElementsByTagName(tabEmailSubjectXML).Item(0).InnerText;

                hashReturn = new Hashtable();

                IDictionaryEnumerator en = replaceParams.GetEnumerator();
                while (en.MoveNext())
                {

                    if (strSubject.IndexOf(en.Key.ToString()) >= 0)
                    {
                        strSubject = strSubject.Replace(en.Key.ToString(), en.Value.ToString());
                    }
                    if (strBody.IndexOf(en.Key.ToString()) >= 0)
                    {
                        strBody = strBody.Replace(en.Key.ToString(), en.Value.ToString());
                    }
                }

                //Reject character special
                strBody = strBody.Replace("\r", "");
                strBody = strBody.Replace("\n", "");
                strBody = strBody.Replace("\\", "");

                hashReturn.Add(tabEmailSubjectXML, strSubject);
                hashReturn.Add(tabEmailBodyXML, strBody);
            }
            catch { }

            return hashReturn;
        }
    }
}
