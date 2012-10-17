using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ChilkatEmail.Utils
{
    public class MailUtils
    {
        public String mailParse(List<String> list)
        {
            String result = "";
            foreach (String item in list)
            {
                result += ";" + item;
            }
            return result;
        }
        public String ProcessHTMLBody(String body, bool isHTML, String serverName, String autoresponderID, String messageID, String listID, String contactID, String emailTO)
        {
            String result = "";
            if (isHTML)
            {
                List<String> templinks = FetchLinksFromSource(body);
                for (int i = 0; i < templinks.Count; i++)
                {
                    String link = templinks[i].Replace("'","");
                   
                    if (link.IndexOf( "Unsubscribe")>=0)
                    {
                        String patamcode = Encode("CONTACTID=" + contactID /*+ "&LISTID=" + listID*/ + "&EMAIL="+emailTO);
                        String replaceString = "'" + serverName + "/unsubscribe.aspx?paramcode=" + patamcode + "' ";
                       
                        //String replaceString = "'" + serverName + "/redirect.aspx?AUTORESPONDERID=" + autoresponderID + "&MESSAGEID=" + messageID + "&LISTID=" + listID + "&REDIRECTURL=" + link.Replace("\"", "") + "'";
                        //String script = unsubscribeScript(serverName, messageID, listID, contactID);
                        //  String replaceString = "'" + serverName + "/unsubscribe.aspx?CONTACTID=" + contactID + "&LISTID=" + listID + "&REDIRECTURL=" + link.Replace("\"", "") + "'";
                        body = body.Replace(link, replaceString);
                       // body = body.Replace("<html>", "<html>" + script);
                    }
                    else if (link.IndexOf("mailto") >= 0 || link.IndexOf("#") >= 0)
                    {

                    }
                    else
                    {

                        String patamcode = Encode("AUTORESPONDERID=" + autoresponderID + "&MESSAGEID=" + messageID + "&LISTID=" + listID + "&CONTACTID=" + contactID + "&REDIRECTURL=" + link.Replace("\"", "") + "'");
                        String replaceString = "'" + serverName + "/redirect.aspx?paramcode=" + patamcode + "' ";
                        body = body.Replace(link, replaceString);
                    }


                    //unsubscribe.aspx
                }
                // Literal1.Text = a;
                if (autoresponderID == "-1")
                {
                    result = body.Replace("</html>", "<img alt='' src='" + serverName + "/imageHandler.ashx?paramcode=" + Encode("AUTORESPONDERID=" + autoresponderID  +"&MESSAGEID=" + messageID + "&LISTID=" + listID + "&CONTACTID=" + contactID) + "'/>");
                    result += "</html>";
                }
                else
                {
                    result = body.Replace("</html>", "<img alt='' src='" + serverName + "/imageHandler.ashx?paramcode=" + Encode("AUTORESPONDERID=" + autoresponderID + "&MESSAGEID=" + messageID + "&LISTID=" + listID) + "'/>");
                    result += "</html>";
                }

                
            }
            else
            {
                result += "<html>";
                result += "<body>";
                result += body;
                result += "<img alt='' src='" + serverName + "/empty.jpg' />";
                result += "</body>";
                result += "</html>";
            }
            return result;
        }

        public List<String> FetchLinksFromSource(string htmlSource)
        {
            if (String.IsNullOrEmpty(htmlSource)) return null;
            List<String> links = new List<String>();
            string regexImgSrc = "\\s*(?i)href\\s*=\\s*(\"([^\"]*\")|'[^']*'|([^'\">\\s]+))";
            MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match m in matchesImgSrc)
            {
                string href = m.Groups[1].Value;
                links.Add(href);
            }
            return links;
        }
        String unsubscribeScript(String serverName, String messageID, String listID, String contactID)
        {
            String param = "CONTACTID=" + contactID + "&LISTID=" + listID + "&REDIRECTURL=Unsubscribe";
            String scr = "<script>" +
                        "function unsubscribePost() {" +

                              "var iframe = document.createElement('iframe');" +
                              "var uniqueString = 'CHANGE_THIS_TO_SOME_UNIQUE_STRING';" +
                              "document.body.appendChild(iframe);" +
                              "iframe.style.display = 'none';" +
                              "iframe.contentWindow.name = uniqueString;" +

                              // construct a form with hidden inputs, targeting the iframe
                              "var form = document.createElement('form');" +
                              "form.target = uniqueString;" +
                              "form.action = '" + serverName + "/unsubscribe.aspx?paramcode=" + Encode(param) + "';" +
                              "form.method = 'POST';" +


                              "document.body.appendChild(form);" +
                              "form.submit();" +
                              "alert('Unsubscribe successful.');" +
                              "}" +
                             "</script>";
            return scr;
        }
        public string Encode(string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }
       
    }
}
